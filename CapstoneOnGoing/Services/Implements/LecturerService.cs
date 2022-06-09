using CapstoneOnGoing.Services.Interfaces;
using Models.Dtos;
using Models.Models;
using Models.Response;
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
        public void CreateLecturer(Lecturer lecturer, Guid userId, Guid departmentId)
        {
            lecturer.Id = userId;
            lecturer.DepartmentId = departmentId;
            _unitOfWork.Lecturer.Insert(lecturer);
            _unitOfWork.Save();
        }

        //Delete lecturer
        public void DeleteLecturer(Lecturer lecturerToDelete)
        {
            _unitOfWork.Lecturer.Delete(lecturerToDelete);
            _unitOfWork.Save();
        }

        //Delete lecturer by id
        public void DeleteLecturerById(Guid lecturerId)
        {
            _unitOfWork.Lecturer.DeleteById(lecturerId);
            _unitOfWork.Save();
        }

        //Get Lecturer list
        public IEnumerable<User> GetAllLecturers(int page, int limit)
        {
            IEnumerable<User> lecturers;
            if (page == 0 || limit == 0 || page < 0 || limit < 0)
            {
                lecturers = _unitOfWork.User.Get(x => (x.Role.Id == 2 && x.Role.Name == "LECTURER" && x.StatusId == 1), null, page: 1, limit: 10);
                foreach (User lecturer in lecturers)
                {
                    lecturer.Lecturer = _unitOfWork.Lecturer.GetById(lecturer.Id);
                    if (lecturer.Lecturer != null)
                    {
                        lecturer.Lecturer.Department = _unitOfWork.Department.GetById(lecturer.Lecturer.DepartmentId);
                        lecturer.Role = _unitOfWork.Role.GetRoleById(2);
                    }
                }
            }
            else
            {
                lecturers = _unitOfWork.User.Get(x => (x.Role.Id == 2 && x.Role.Name == "LECTURER" && x.StatusId == 1), page: page, limit: limit);
                foreach (User lecturer in lecturers)
                {
                    lecturer.Lecturer = _unitOfWork.Lecturer.GetById(lecturer.Id);
                    if (lecturer.Lecturer != null)
                    {
                        lecturer.Lecturer.Department = _unitOfWork.Department.GetById(lecturer.Lecturer.DepartmentId);
                        lecturer.Role = _unitOfWork.Role.GetRoleById(2);
                    }
                }
            }
            return lecturers;
        }

        //Get lecturer by id
        public User GetLecturerById(Guid lecturerId)
        {
            //Lecturer lecturer = _unitOfWork.Lecturer.GetById(lecturerId);
            User lecturerToReturn = _unitOfWork.User.GetById(lecturerId);
            Lecturer lecturer = _unitOfWork.Lecturer.GetById(lecturerId);
            if(lecturer != null)
            {
                lecturerToReturn.Lecturer = lecturer;
                Department department = _unitOfWork.Department.GetById(lecturer.DepartmentId);
                lecturerToReturn.Lecturer.Department = department;
                lecturerToReturn.Role = _unitOfWork.Role.GetRoleById(2);
            }
            return lecturerToReturn;
        }

        //Update lecturer
        public User UpdateLecturer(User lecturerToUpdate)
        {

            User lecturer = _unitOfWork.User.GetById(lecturerToUpdate.Id);
            if(lecturer != null)
            {
                if (string.IsNullOrEmpty(lecturerToUpdate.Email))
                {
                    lecturerToUpdate.Email = lecturer.Email;
                }
                if (string.IsNullOrEmpty(lecturerToUpdate.UserName))
                {
                    lecturerToUpdate.UserName = lecturer.UserName;
                }
                if (string.IsNullOrEmpty(lecturerToUpdate.FullName))
                {
                    lecturerToUpdate.FullName = lecturer.FullName;
                }
                if (string.IsNullOrEmpty(lecturerToUpdate.UserName))
                {
                    lecturerToUpdate.UserName = lecturer.UserName;
                }

                _unitOfWork.User.Update(lecturerToUpdate);
                _unitOfWork.Save();
                return lecturer;
            } else
            {
                return null;
            }
        }
    }
}
