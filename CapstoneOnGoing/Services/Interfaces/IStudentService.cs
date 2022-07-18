using Models.Models;
using Models.Request;
using System;
using System.Collections.Generic;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface IStudentService
    {
        IEnumerable<StudentResponse> GetAllStudents();
        void CreateStudent(Student newStudent);
        User GetStudentById(Guid studentId);
        User UpdateStudent(Guid studentId,UpdateStudentRequest studentDataToUpate);
        User GetStudentByEmail(string userEmail);
        void DeleteStudent(Student studentToDelete);
        void DeleteStudentById(Guid studentId);
    }
}
