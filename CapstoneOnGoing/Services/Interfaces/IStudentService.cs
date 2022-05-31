using Models.Models;
using System;
using System.Collections.Generic;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAllStudents();
        void CreateStudent(Student newStudent);
        Student GetStudentById(Guid studentId);
        void UpdateStudent(Student studentToUpate);
        void DeleteStudent(Student studentToDelete);
        void DeleteStudentById(Guid studentId);
    }
}
