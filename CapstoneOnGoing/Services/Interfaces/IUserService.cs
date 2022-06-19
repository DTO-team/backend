using Models.Dtos;
using Models.Models;
using System;
using System.Collections.Generic;
using Models.Request;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface IUserService
	{
		User GetUserWithRoleByEmail(string email);
		User GetUserById(Guid id);
		IEnumerable<User> GetAllUsers(string name,int page, int limit, out int totalRecords);
		void CreateUser(CreateNewUserRequest user);
		void UpdateUser(UpdateUserInAdminRequest user);
		Guid GetUserIdByUserName(string userName);
		User CreateUserByEmailAndName(string email, string name);

		bool ImportInProgressStudents(IEnumerable<InProgressStudentsRequest> inProgressStudentsRequests);
		bool CreateNewLectuer(LecturerResquest user);
		bool CreateNewStudent(StudentRequest user);
		bool DeleteUserById(Guid userId);
	}
}
