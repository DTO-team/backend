using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Models.Dtos;

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
        public IEnumerable<User> GetAllLecturers()
        {
            IEnumerable<User> lecturers;
            lecturers = _unitOfWork.User.Get(x => (x.Role.Id == 2 && x.Role.Name == "LECTURER" && x.StatusId == 1));
            foreach (User lecturer in lecturers)
            {
                lecturer.Lecturer = _unitOfWork.Lecturer.GetById(lecturer.Id);
                if (lecturer.Lecturer != null)
                {
                    lecturer.Lecturer.Department = _unitOfWork.Department.GetById(lecturer.Lecturer.DepartmentId);
                    lecturer.Role = _unitOfWork.Role.GetRoleById(2);
                }
            }
            return lecturers;
        }

        //Get lecturer by id
        public User GetLecturerById(Guid lecturerId)
        {
            User lecturerToReturn = _unitOfWork.User.GetById(lecturerId);
            Lecturer lecturer = _unitOfWork.Lecturer.GetById(lecturerId);
            if (lecturer != null)
            {
                lecturerToReturn.Lecturer = lecturer;
                Department department = _unitOfWork.Department.GetById(lecturer.DepartmentId);
                lecturerToReturn.Lecturer.Department = department;
                lecturerToReturn.Role = _unitOfWork.Role.GetRoleById(2);
            }
            return lecturerToReturn;
        } 
        
        //Get lecturer by email
        public User GetLecturerByEmail(string userEmail)
        {
            User lecturerToReturn = _unitOfWork.User.Get( x => x.Email.Equals(userEmail)).FirstOrDefault();
            if (lecturerToReturn is not null)
            {
                Lecturer lecturer = _unitOfWork.Lecturer.GetById(lecturerToReturn.Id);
                if (lecturer != null)
                {
                    lecturerToReturn.Lecturer = lecturer;
                    Department department = _unitOfWork.Department.GetById(lecturer.DepartmentId);
                    lecturerToReturn.Lecturer.Department = department;
                    lecturerToReturn.Role = _unitOfWork.Role.GetRoleById(2);
                }
                return lecturerToReturn;
            }
            else
            {
                throw new BadHttpRequestException($"Lecturer with {userEmail} email is not existed!");
            }
        }

        //Update lecturer
        public User UpdateLecturer(Guid lecturerId, UpdateLecturerRequest lecturerToUpdate)
        {
            User returnLecturer = null;
            Lecturer updateLecturer = _unitOfWork.Lecturer.GetById(lecturerId);
            if (updateLecturer is not null)
            {
                User lecturer = _unitOfWork.User.GetById(lecturerId);
                if (lecturer != null)
                {
                    if (!string.IsNullOrEmpty(lecturerToUpdate.UserName))
                    {
                        lecturer.UserName = lecturerToUpdate.UserName;
                    }

                    if (!string.IsNullOrEmpty(lecturerToUpdate.FullName))
                    {
                        lecturer.FullName = lecturerToUpdate.FullName;
                    }

                    if (!string.IsNullOrEmpty(lecturerToUpdate.UserName))
                    {
                        lecturer.UserName = lecturerToUpdate.UserName;
                    }

                    if (!string.IsNullOrEmpty(lecturerToUpdate.AvatarUrl))
                    {
                        lecturer.AvatarUrl = lecturerToUpdate.AvatarUrl;
                    }

                    _unitOfWork.User.Update(lecturer);

                    if (!lecturerToUpdate.DepartmentId.Equals(Guid.Empty))
                    {
                        Department newDepartment = _unitOfWork.Department.GetById(lecturerToUpdate.DepartmentId);
                        if (newDepartment is not null)
                        {
                            updateLecturer.DepartmentId = newDepartment.Id;
                            _unitOfWork.Lecturer.Update(updateLecturer);
                        }
                        else
                        {
                            throw new BadHttpRequestException(
                                $"Department with {updateLecturer.DepartmentId} id is not existed!");
                        }
                    }
                    _unitOfWork.Save();
                    returnLecturer = GetLecturerById(lecturerId);
                }
                else
                {
                    throw new BadHttpRequestException($"Lecturer with {lecturerId} is not existed!");
                }
            }
            return returnLecturer;
        }
    }
}
