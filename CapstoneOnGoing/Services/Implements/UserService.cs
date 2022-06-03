using AutoMapper;
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

            }
            else
            {
                user = _unitOfWork.User.Get(x => x.Email == email, null, "Role").First();
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
            User user = _unitOfWork.User.GetById(id);
            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            IEnumerable<User> users = _unitOfWork.User.Get(null,null, "Role");
            return users;
        }

        //public void CreateUser(User user)
        public void CreateUser(CreateNewUserDTO user)
        {
            Role studentRole = _unitOfWork.Role.GetRoleByName("STUDENT");
            user.RoleId = studentRole.Id;
            User userToUpdate = _mapper.Map<User>(user);
            _unitOfWork.User.Insert(userToUpdate);
            _unitOfWork.Save();
        }

        public void UpdateUser(User user, string updateRole)
        {
            Role userRole = _unitOfWork.Role.GetRoleByName(updateRole);
            user.RoleId = userRole.Id;
            _unitOfWork.User.Update(user);
            _unitOfWork.Save();
        }


    }
}
