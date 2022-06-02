﻿using Models.Models;
using System;
using System.Collections.Generic;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface IUserService
	{
		User GetUserWithRoleByEmail(string email);
		User GetUserByEmail(string email);
		User GetUserById(Guid id);
		IEnumerable<User> GetAllUsers();
		void CreateUser(User user);
		void UpdateUser(User user);
	}
}
