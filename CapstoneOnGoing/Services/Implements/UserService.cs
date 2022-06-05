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
				return null;
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
			User user = null;
			user = _unitOfWork.User.GetById(id);
			if(user != null)
            {
				return user;
            }
			return null;
		}

		public IEnumerable<User> GetAllUsers(string name, int page, int limit)
		{
			IEnumerable<User> users;
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

		//public void CreateUser(User user)
		public void CreateUser(CreateNewUserDTO user)
		{
			Role studentRole = _unitOfWork.Role.GetRoleByName("STUDENT");
			user.RoleId = studentRole.Id;
			User userToUpdate = _mapper.Map<User>(user);
			_unitOfWork.User.Insert(userToUpdate);
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


	}
}
