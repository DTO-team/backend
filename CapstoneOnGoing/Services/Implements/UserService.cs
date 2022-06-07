﻿using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using Models.Dtos;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapstoneOnGoing.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public User GetUserWithRoleByEmail(string email)
        {
            User user = null;
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            else
            {
                user = _unitOfWork.User.Get(x => x.Email == email, null, "Role").First();
                switch (user.RoleId)
                {
                    case 2:
                        user.Lecturer = _unitOfWork.Lecturer.Get(x => x.Id == user.Id, null, "Department", 0, 0).FirstOrDefault();
                        break;
                    case 3:
                        user.Student = _unitOfWork.Student.Get(x => x.Id == user.Id, null, "Semester", 0, 0).FirstOrDefault();
                        break;
                    case 4:
                        user.Company = _unitOfWork.Companies.Get(x => x.Id == user.Id, null, null, 0, 0).FirstOrDefault();
                        break;
                    default:
                        break;
                }
            }
            return user;
        }

        public User GetUserByEmail(string email)
        {
            User user = null;
            if (!string.IsNullOrEmpty(email))
            {
                //user = _unitOfWork.User.Get(x => x.Email == email).First();
                user = _unitOfWork.User.Get(x => x.Email == email).FirstOrDefault();
            }
            return user;
        }

        public User GetUserById(Guid id)
        {
            User user = null;
            user = _unitOfWork.User.GetById(id);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public IEnumerable<User> GetAllUsers(string name, int page, int limit)
        {
            IEnumerable<User> users;
            //default is page 1 and limit is 10 if not have value of page and limit parameter
            if (page == 0 || limit == 0 || page < 0 || limit < 0)
            {
                page = 1;
                limit = 10;
            }

            if (!string.IsNullOrEmpty(name))
            {
                users = _unitOfWork.User.Get(x => x.UserName.Contains(name), null, "Role", page, limit);
            }
            else
            {
                users = _unitOfWork.User.Get(null, null, "Role", page, limit);
            }

            return users;
        }

        public void CreateUser(CreateNewUserDTO user)
        {
            User userToCreate;
            if (user.RoleId == 0)
            {
                Role studentRole = _unitOfWork.Role.GetRoleByName("STUDENT");
                user.RoleId = studentRole.Id;
                userToCreate = _mapper.Map<User>(user);
                userToCreate.Role = studentRole;
            } else
            {
                Role userRole = _unitOfWork.Role.GetRoleById(user.RoleId);
                user.RoleId = userRole.Id;
                userToCreate = _mapper.Map<User>(user);
                userToCreate.Role = userRole;
            }
            _unitOfWork.User.Insert(userToCreate);
            _unitOfWork.Save();
        }

        public void UpdateUser(User user, string updateRole, int StatusId)
        {
            Role userRole = _unitOfWork.Role.GetRoleByName(updateRole);
            user.RoleId = userRole.Id;
            user.StatusId = StatusId;
            _unitOfWork.User.Update(user);
            _unitOfWork.Save();
        }

        public User CreateUserByEmailAndName(string email, string name)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
            {
                return null;
            }

            User newUser = null;

            if (email.Contains("@fpt.edu.vn"))
            {
                newUser = new User { Email = email, FullName = name, RoleId = 3, StatusId = 1, UserName = email.Substring(0, email.IndexOf('@')) };
                _unitOfWork.User.Insert(newUser);
                _unitOfWork.Save();
            }
            return newUser;
        }
        public Guid GetUserIdByUserName(string userName)
        {
            User user = null;
            if (!string.IsNullOrEmpty(userName))
            {
                //user = _unitOfWork.User.Get(x => x.Email == email).First();
                user = _unitOfWork.User.Get(x => x.UserName == userName).FirstOrDefault();
            }
            return user.Id;
        }
    }
}
