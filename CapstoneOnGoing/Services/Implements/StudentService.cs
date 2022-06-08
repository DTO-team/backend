using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using Models.Models;
using Models.Request;
using Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace CapstoneOnGoing.Services.Implements
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        //Create student
        public void CreateStudent(Student newStudent)
        {
            _unitOfWork.Student.Insert(newStudent);
            _unitOfWork.Save();
        }

        //Get list student
        public IEnumerable<User> GetAllStudents(int page, int limit)
        {
            IEnumerable<User> students;
            if (page == 0 || limit == 0 || page < 0 || limit < 0)
            { 
                 students = _unitOfWork.User.Get(x=> (x.Role.Name == "STUDENT" && x.RoleId == 3),null, page:1, limit:10);
                 foreach(User student in students)
                {
                    student.Student = _unitOfWork.Student.GetById(student.Id);
                    if (student.Student != null)
                    {
                        if(student.Student.SemesterId != null)
                        {
                            student.Student.Semester = _unitOfWork.Semester.GetById((Guid)student.Student.SemesterId);
                        }
                        if(student.RoleId != 0)
                        { 
                            student.Role = _unitOfWork.Role.GetRoleById(student.RoleId);
                        }
                    }
                }
            } else
            {
                students = _unitOfWork.User.Get(x => (x.Role.Name == "STUDENT" && x.RoleId == 3), null, page: page, limit: limit);
                foreach (User student in students)
                {
                    student.Student = _unitOfWork.Student.GetById(student.Id);
                    if (student.Student != null)
                    {
                        if (student.Student.SemesterId != null)
                        {
                            student.Student.Semester = _unitOfWork.Semester.GetById((Guid)student.Student.SemesterId);
                        }
                        if (student.RoleId != 0)
                        {
                            student.Role = _unitOfWork.Role.GetRoleById(student.RoleId);
                        }
                    }
                }
            }
            return students;
        }

        //Get student by student ID
        public User GetStudentById(Guid studentId)
        {
            User studentToReturn = _unitOfWork.User.GetById(studentId);
            Student student = _unitOfWork.Student.GetById(studentId);
            if (student != null)
            {
                studentToReturn.Student = student;
                studentToReturn.Student.Code = student.Code;
                studentToReturn.Role = _unitOfWork.Role.GetRoleById(3);
                if (student.SemesterId != null)
                {
                    studentToReturn.Student.Semester = _unitOfWork.Semester.GetById((Guid)student.SemesterId);
                }
            }
            return studentToReturn;
        }

        //Update student
        public User UpdateStudent(StudentUpdateRequest studentToUpate)
        {
            Student student = _unitOfWork.Student.GetById(studentToUpate.Id);
            if (student != null)
            {
                User userToUpdateDTO = _mapper.Map<User>(studentToUpate);
                Student studentToUpdateDTO = _mapper.Map<Student>(studentToUpate);
                studentToUpdateDTO.Code = studentToUpate.Code;
                userToUpdateDTO.Student = studentToUpdateDTO;

                _unitOfWork.User.Update(userToUpdateDTO);
                _unitOfWork.Save();

                User userUpdated = _unitOfWork.User.GetById(studentToUpate.Id);
                return userUpdated;
            } else
            {
                return null;
            }
        }
        
        //Delete student
        public void DeleteStudent(Student studentToDelete)
        {
            _unitOfWork.Student.Delete(studentToDelete);
            _unitOfWork.Save();
        }

        //Delete student by Student id
        public void DeleteStudentById(Guid studentId)
        {
            _unitOfWork.Student.DeleteById(studentId);
            _unitOfWork.Save();
        }
    }
}
