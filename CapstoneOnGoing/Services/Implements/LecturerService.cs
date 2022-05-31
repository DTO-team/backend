using CapstoneOnGoing.Services.Interfaces;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace CapstoneOnGoing.Services.Implements
{
    public class LecturerService : ILecturerService
    {

        private readonly IUnitOfWork _unitOfWork;

        public LecturerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Create lecturer
        public void CreateLecturer(Lecturer lecturer)
        {
            _unitOfWork.Lecturer.Insert(lecturer);
        }

        //Delete lecturer
        public void DeleteLecturer(Lecturer lecturerToDelete)
        {
            _unitOfWork.Lecturer.Delete(lecturerToDelete);
        }

        //Delete lecturer by id
        public void DeleteLecturerById(Guid lecturerId)
        {
            _unitOfWork.Lecturer.DeleteById(lecturerId);
        }

        //Get Lecturer list
        public IEnumerable<Lecturer> GetAllLecturers()
        {
            IEnumerable<Lecturer> lecturers = _unitOfWork.Lecturer.Get();
            return lecturers;
        }

        //Get lecturer by id
        public Lecturer GetLecturerById(Guid lecturerId)
        {
            Lecturer lecturer = _unitOfWork.Lecturer.GetById(lecturerId);
            return lecturer;
        }

        //Update lecturer
        public void UpdateLecturer(Lecturer lecturerToUpdate)
        {
            _unitOfWork.Lecturer.Update(lecturerToUpdate);
        }
    }
}
