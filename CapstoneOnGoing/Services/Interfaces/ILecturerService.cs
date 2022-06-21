using Models.Dtos;
using Models.Models;
using Models.Response;
using System;
using System.Collections.Generic;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface ILecturerService
    {
        void CreateLecturer(Lecturer lecturer, Guid userId, Guid departmentId);
        IEnumerable<User> GetAllLecturers(int page, int limit);
        User GetLecturerById(Guid lecturerId);
        User UpdateLecturer(User lecturerToUpdate);
        User GetLecturerByEmail(string userEmail);
        void DeleteLecturer(Lecturer lecturerToDelete);
        void DeleteLecturerById(Guid lecturerId);
    }
}
