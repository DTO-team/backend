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
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

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
            user = _unitOfWork.User.Get(x => x.Email == email, null, "Role").First();
            if (user != null)
            {
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
        public User GetUserById(Guid id)
        {
            User user = null;
            user = _unitOfWork.User.Get(x => x.Id == id, null, "Role").FirstOrDefault();
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
                users = _unitOfWork.User.Get((x => (x.UserName.Contains(name) && x.StatusId != 2)), null, "Role", page, limit);
            }
            else
            {
                users = _unitOfWork.User.Get(null, null, "Role", page, limit);
            }

            return users;
        }

        public void CreateUser(CreateNewUserRequest user)
        {
            User userToCreate = GetUserWithRoleByEmail(user.Email);
            if (userToCreate != null)
            {
                throw new BadHttpRequestException("User is existed !");
            }
            else
            {
                if (user.RoleId == 0)
                {
                    Role studentRole = _unitOfWork.Role.GetRoleByName("STUDENT");
                    user.RoleId = studentRole.Id;
                    userToCreate = _mapper.Map<User>(user);
                    userToCreate.Role = studentRole;
                }
                else
                {
                    Role userRole = _unitOfWork.Role.GetRoleById(user.RoleId);
                    user.RoleId = userRole.Id;
                    userToCreate = _mapper.Map<User>(user);
                    userToCreate.Role = userRole;
                }
                _unitOfWork.User.Insert(userToCreate);
                _unitOfWork.Save();
            }
        }

        public void UpdateUser(UpdateUserInAdminRequest userInAdminUpdateData)
        {
            //Get user from database base on userInAdminToUpdate id
            User user = GetUserById(userInAdminUpdateData.Id);

            if (user != null)
            {
                //Cannot update user when user is inactivated
                if (user.StatusId != 1)
                {
                    throw new BadHttpRequestException("User is not activated to update");
                }

                //Auto change status id to 1 if user is activated and you want to update user 
                if (!string.IsNullOrEmpty(userInAdminUpdateData.Role) && userInAdminUpdateData.StatusId == 0)
                {
                    userInAdminUpdateData.StatusId = 1;
                }
                Role userRole = _unitOfWork.Role.GetRoleByName(userInAdminUpdateData.Role);
                user.RoleId = userRole.Id;
                user.StatusId = userInAdminUpdateData.StatusId;
                _unitOfWork.User.Update(user);
                _unitOfWork.Save();

            }
            else
            {
                throw new BadHttpRequestException($"User is not existed");
            }
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

        public bool CreateNewLectuer(LecturerResquest user)
        {
            bool isSuccess;
            User userToCreate;

            if (!user.RoleId.Equals(2))
            {
                user.RoleId = 2;
            }
            if (!user.StatusId.Equals(1))
            {
                user.StatusId = 1;
            }

            if (GetUserWithRoleByEmail(user.Email) == null)
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

                isSuccess = true;
            }
            else
            {
                //throw: Bad request exception
                throw new BadHttpRequestException("Lecturer (user) is existed !");
            }
            return isSuccess;
        }

        public bool CreateNewStudent(StudentRequest user)
        {
            User userToCreate;
            bool isSuccess;

            if (!user.RoleId.Equals(3))
            {
                user.RoleId = 3;
            }
            if (!user.StatusId.Equals(1))
            {
                user.StatusId = 1;
            }

            MailAddress email = new MailAddress(user.Email);
            string userName = email.User;

            Regex regex = new Regex("(se|ss|sa|ca){1}[0-9]{6}");
            Match studentCode = regex.Match(userName);


            if (GetUserWithRoleByEmail(user.Email) == null)
            {
                Role userRole = _unitOfWork.Role.GetRoleById(user.RoleId);

                userToCreate = _mapper.Map<User>(user);

                //Set role of user to student
                userToCreate.RoleId = 3;
                userToCreate.Role = userRole;

                //Set entity Student in User
                Student studentDTO = _mapper.Map<Student>(user);
                studentDTO.Code = studentCode.ToString();
                studentDTO.SemesterId = user.SemesterId;
                userToCreate.Student = studentDTO;

                _unitOfWork.User.Insert(userToCreate);
                _unitOfWork.Save();

                isSuccess = true;
                return isSuccess;
            }
            else
            {
                //Set GenericResponse: Bad request
                throw new BadHttpRequestException("Student (user) is existed !");
            }
        }

        public bool DeleteUserById(Guid userId)
        {
            bool isDeleted = false;
            User user = _unitOfWork.User.Get(x => x.Id == userId, null).FirstOrDefault();

            if (user != null)
            {
                user.StatusId = 2;
                _unitOfWork.User.Update(user);
                _unitOfWork.Save();
                isDeleted = true;
            }
            else
            {
                return isDeleted;
            }
            return isDeleted;
        }
    }
}
