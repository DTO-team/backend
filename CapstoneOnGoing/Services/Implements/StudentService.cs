using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Models;
using Models.Request;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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

            students = _unitOfWork.User.Get(x => (x.Role.Name == "STUDENT" && x.RoleId == 3 && x.StatusId == 1), null, page: page, limit: limit);
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
            return students;
        }

        //Get student by student ID
        public User GetStudentById(Guid studentId)
        {
            User returnStudent = _unitOfWork.User.Get(x => x.Id.Equals(studentId), null, "Student").FirstOrDefault();
            Student student = _unitOfWork.Student.Get(x => x.Id.Equals(studentId), null, "TeamStudents").FirstOrDefault();
            if (student != null)
            {
                TeamStudent teamStudent = _unitOfWork.TeamStudent.Get(x => x.StudentId.Equals(student.Id)).FirstOrDefault();
                if (teamStudent is not null)
                {
                    List<TeamStudent> teamStudents = new List<TeamStudent>();
                    teamStudents.Add(teamStudent);
                    returnStudent.Student.TeamStudents = teamStudents;
                }
                returnStudent.Student = student;
                returnStudent.Student.Code = student.Code;
                returnStudent.Role = _unitOfWork.Role.GetRoleById(3);
                if (student.SemesterId != null)
                {
                    returnStudent.Student.Semester = _unitOfWork.Semester.GetById((Guid)student.SemesterId);
                }
            }
            return returnStudent;
        }

        //Get student by student ID
        public User GetStudentByEmail(string userEmail)
        {
            User returnStudent = _unitOfWork.User.Get(x => x.Email.Equals(userEmail), null, "Student").FirstOrDefault();
            Student student = _unitOfWork.Student.Get(x => x.Id.Equals(returnStudent.Id), null, "TeamStudents").FirstOrDefault();
            if (student != null)
            {
                TeamStudent teamStudent = _unitOfWork.TeamStudent.Get(x => x.StudentId.Equals(student.Id)).FirstOrDefault();
                if (teamStudent is not null)
                {
                    List<TeamStudent> teamStudents = new List<TeamStudent>();
                    teamStudents.Add(teamStudent);
                    returnStudent.Student.TeamStudents = teamStudents;
                }
                returnStudent.Student = student;
                returnStudent.Student.Code = student.Code;
                returnStudent.Role = _unitOfWork.Role.GetRoleById(3);
                if (student.SemesterId != null)
                {
                    returnStudent.Student.Semester = _unitOfWork.Semester.GetById((Guid)student.SemesterId);
                }
            }
            return returnStudent;
        }

        //Update student
        public User UpdateStudent(Guid studentId, UpdateStudentRequest updateStudentRequest)
        {
            Student student = _unitOfWork.Student.GetById(studentId);
            if (student != null)
            {
                User studentToUpdate = _unitOfWork.User.GetById(studentId);

                if (!string.IsNullOrEmpty(updateStudentRequest.UserName))
                {
                    studentToUpdate.UserName = updateStudentRequest.UserName;
                }
                if (!string.IsNullOrEmpty(updateStudentRequest.FullName))
                {
                    studentToUpdate.FullName = updateStudentRequest.FullName;
                }
                if (!string.IsNullOrEmpty(updateStudentRequest.AvatarUrl))
                {
                    studentToUpdate.AvatarUrl = updateStudentRequest.AvatarUrl;
                }
                Role studentRole = _unitOfWork.Role.GetRoleByName("STUDENT");
                studentToUpdate.Role = studentRole;

                _unitOfWork.User.Update(studentToUpdate);
                _unitOfWork.Save();

                User userUpdated = _unitOfWork.User.GetById(studentId);
                return userUpdated;
            }
            else
            {
                throw new BadHttpRequestException("Student is not existed");
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
