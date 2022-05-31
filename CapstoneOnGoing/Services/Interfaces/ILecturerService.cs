using Models.Models;
using System;
using System.Collections.Generic;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface ILecturerService
    {
        void CreateLecturer(Lecturer lecturer);
        IEnumerable<Lecturer> GetAllLecturers();
        Lecturer GetLecturerById(Guid lecturerId);
        void UpdateLecturer(Lecturer lecturerToUpdate);
        void DeleteLecturer(Lecturer lecturerToDelete);
        void DeleteLecturerById(Guid lecturerId);
    }
}
