using CapstoneOnGoing.Services.Interfaces;
using Models.Models;
using Newtonsoft.Json.Linq;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapstoneOnGoing.Services.Implements
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		public UserService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public User GetUserWithRoleByEmail(string email)
		{
			User user = null;
			if(string.IsNullOrEmpty(email)){

			}else{
				user = _unitOfWork.User.Get(x => x.Email == email,null,"Role").First();
			}
			return user;
		}

		public User GetUserByEmail(string email)
        {
			User user = null;
            if (!string.IsNullOrEmpty(email))
            {
				user = _unitOfWork.User.Get(x => x.Email == email).First();
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
			IEnumerable<User> users = _unitOfWork.User.Get();
			return users;
        }

		public void CreateUser(User user)
        {
			Role studentRole = _unitOfWork.Role.GetRoleByName("STUDENT");
			user.Role = studentRole;
			_unitOfWork.User.Insert(user);
			_unitOfWork.Save();
        }

		public void UpdateUser(User user)
        {
			_unitOfWork.User.Update(user);
			_unitOfWork.Save();
        }
	}
}
