using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using Models.Dtos;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Models.Request;
using System.Net.Mail;
using Models.Response;

namespace CapstoneOnGoing.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

		public User GetUserWithRoleByEmail(string email)
		{
			User user = null;
			if (string.IsNullOrEmpty(email))
			{
				return null;
			}
			else
			{
				user = _unitOfWork.User.Get(x => x.Email == email, null, "Role").First();
				switch (user.RoleId)
				{
					case 2:
						user.Lecturer = _unitOfWork.Lecturer.Get(x => x.Id == user.Id, null, "Department", 0, 0).FirstOrDefault();
						break;
					case 3:
						user.Student = _unitOfWork.Student.Get(x => x.Id == user.Id, null, "Semester", 0, 0).FirstOrDefault();
						break;
					case 4:
						user.Company = _unitOfWork.Companies.Get(x => x.Id == user.Id, null, null, 0, 0).FirstOrDefault();
						break;
					default:
						break;
				}
			}
			return user;
		}



        public User GetUserByEmail(string email)
        {
            User user = null;
            if (!string.IsNullOrEmpty(email))
            {
                user = _unitOfWork.User.Get(x => x.Email == email).FirstOrDefault();
            }
            return user;
        }

		public User GetUserById(Guid id)
		{
			User user = null;
			user = _unitOfWork.User.Get(x=>x.Id == id, null, "Role").FirstOrDefault();
			if (user != null)
			{
				return user;
			}
			return null;
		}

		public IEnumerable<User> GetAllUsers(string name, int page, int limit)
		{
			IEnumerable<User> users;
			//default is page 1 and limit is 10 if not have value of page and limit parameter
			if (page == 0 || limit == 0 || page < 0 || limit < 0)
			{
				page = 1;
				limit = 10;
			}

			if (!string.IsNullOrEmpty(name))
			{
				users = _unitOfWork.User.Get(x => x.UserName.Contains(name), null, "Role", page, limit);
			}
			else
			{
				users = _unitOfWork.User.Get(null, null, "Role", page, limit);
			}

			return users;
		}

        public void CreateUser(CreateNewUserDTO user)
        {
            User userToCreate;
            if (user.RoleId == 0)
            {
                Role studentRole = _unitOfWork.Role.GetRoleByName("STUDENT");
                user.RoleId = studentRole.Id;
                userToCreate = _mapper.Map<User>(user);
                userToCreate.Role = studentRole;
            } else
            {
                Role userRole = _unitOfWork.Role.GetRoleById(user.RoleId);
                user.RoleId = userRole.Id;
                userToCreate = _mapper.Map<User>(user);
                userToCreate.Role = userRole;
            }
            _unitOfWork.User.Insert(userToCreate);
            _unitOfWork.Save();
        }

		public void UpdateUser(User user, string updateRole, int StatusId)
		{
			Role userRole = _unitOfWork.Role.GetRoleByName(updateRole);
			user.RoleId = userRole.Id;
			user.StatusId = StatusId;
			_unitOfWork.User.Update(user);
			_unitOfWork.Save();
		}

		public User CreateUserByEmailAndName(string email, string name)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
			{
				return null;
			}

			User newUser = null;

			if (email.Contains("@fpt.edu.vn"))
			{
				newUser = new User { Email = email, FullName = name, RoleId = 3, StatusId = 1, UserName = email.Substring(0, email.IndexOf('@')) };
				_unitOfWork.User.Insert(newUser);
				_unitOfWork.Save();
			}
			return newUser;
		}

		public bool ImportInProgressStudents(IEnumerable<InProgressStudentsRequest> inProgressStudentsRequests)
		{
			bool isSuccessful = true;
			Semester currentPreparingSemester = _unitOfWork.Semester.Get(x => (x.Status == 1 && x.Year == DateTime.Now.Year)).FirstOrDefault();
			if (currentPreparingSemester != null)
			{
				foreach (var inProgressStudent in inProgressStudentsRequests)
				{
					User newStudent = _mapper.Map<User>(inProgressStudent);
					User isExistedStudent = _unitOfWork.User.Get(x => (x.Email == inProgressStudent.Email && x.RoleId == 3)).FirstOrDefault();

					if (isExistedStudent == null)
					{
						//Create new user for student
						newStudent = new User { Email = inProgressStudent.Email, FullName = inProgressStudent.FullName, RoleId = 3, StatusId = 1, UserName = inProgressStudent.Email.Substring(0, inProgressStudent.Email.IndexOf('@')) };
						newStudent.Student = new Student { Id = newStudent.Id, Code = inProgressStudent.StudentCode, SemesterId = currentPreparingSemester.Id };
						_unitOfWork.User.Insert(newStudent);
					}
					else
					{
						//Check if the student is in-progress of the current semester
						bool isExisted = (_unitOfWork.Student.Get(x => (x.Id == isExistedStudent.Id && x.SemesterId != null)).FirstOrDefault() != null);
						if (!isExisted)
						{
							Student insertedStudent = new Student { Id = isExistedStudent.Id, Code = inProgressStudent.StudentCode, SemesterId = currentPreparingSemester.Id };
							_unitOfWork.Student.Insert(insertedStudent);
						}
						else
						{
							isSuccessful = false;
							break;
						}
					}
				}
			}
			else
			{
				isSuccessful = false;
			}

			if (isSuccessful)
			{
				_unitOfWork.Save();
			}
			return isSuccessful;
		}

        public Guid GetUserIdByUserName(string userName)
        {
			User user = _unitOfWork.User.Get(x => x.UserName == userName).First();
			return user.Id;
        }

        public GenericResponse CreateNewLectuer(LecturerResquest user)
        {
            User userToCreate;
            GenericResponse response;

            if (GetUserByEmail(user.Email) == null)
            {
                Role userRole = _unitOfWork.Role.GetRoleById(user.RoleId);
                user.RoleId = userRole.Id;

				//Set role of user to lecturer
                userToCreate = _mapper.Map<User>(user);
                userToCreate.Role = userRole;

				//Set entity Lecturer in User
				Lecturer lecturerDTO = _mapper.Map<Lecturer>(user);
                lecturerDTO.DepartmentId = user.DepartmentId;
                userToCreate.Lecturer = lecturerDTO;

                _unitOfWork.User.Insert(userToCreate);
                _unitOfWork.Save();

				//Set GenericResponse: Created
				response = new GenericResponse();
                response.HttpStatus = 201;
                response.Message = "Student Created";
                response.TimeStamp = DateTime.Now;
            }
            else
            {
                //Set GenericResponse: Bad request
                response = new GenericResponse();
                response.HttpStatus = 400;
                response.Message = "Lecturer (user) is existed !";
                response.TimeStamp = DateTime.Now;
            }
            return response;
        }

        public GenericResponse CreateNewStudent(StudentRequest user)
        {
            User userToCreate;
            GenericResponse response;

            MailAddress email = new MailAddress(user.Email);
            string username = email.User;
            string domain = email.Host;

            if (domain != "@fpt.edu.vn")
            {
                //Set GenericResponse: Bad request
                response = new GenericResponse();
                response.HttpStatus = 400;
                response.Message = "Wrong email domain format";
                response.TimeStamp = DateTime.Now;

                return response;
            }
            else
            {
                if (GetUserByEmail(user.Email) == null)
                {
                    Role userRole = _unitOfWork.Role.GetRoleById(user.RoleId);

                    userToCreate = _mapper.Map<User>(user);

                    //Set role of user to student
                    userToCreate.RoleId = 3;
                    userToCreate.Role = userRole;

                    //Set entity Student in User
                    Student studentDTO = _mapper.Map<Student>(user);
                    studentDTO.Code = user.Code;
                    studentDTO.SemesterId = user.SemesterId;
                    userToCreate.Student = studentDTO;

                    _unitOfWork.User.Insert(userToCreate);
                    _unitOfWork.Save();

                    //Set GenericResponse: Created
                    response = new GenericResponse();
                    response.HttpStatus = 201;
                    response.Message = "Student Created";
                    response.TimeStamp = DateTime.Now;
                }
                else
                {
                    //Set GenericResponse: Bad request
                    response = new GenericResponse();
                    response.HttpStatus = 400;
                    response.Message = "Student (user) is existed !";
                    response.TimeStamp = DateTime.Now;
                }
            }
            return response;
        }
    }
}
