using CapstoneOnGoing.Services.Interfaces;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace CapstoneOnGoing.Services.Implements
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Create student
        public void CreateStudent(Student newStudent)
        {
            _unitOfWork.Student.Insert(newStudent);
            _unitOfWork.Save();
        }

        //Get list student
        public IEnumerable<Student> GetAllStudents()
        {
            IEnumerable<Student> students = _unitOfWork.Student.Get();
            return students;
        }

        //Get student by student ID
        public Student GetStudentById(Guid studentId)
        {
            Student student = _unitOfWork.Student.GetById(studentId);
            return student;
        }

        //Update student
        public void UpdateStudent(Student studentToUpate)
        {
            _unitOfWork.Student.Update(studentToUpate);
            _unitOfWork.Save();
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
