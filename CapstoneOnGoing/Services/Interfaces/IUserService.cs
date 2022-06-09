using Models.Dtos;
using Models.Models;
using System;
using System.Collections.Generic;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface IUserService
	{
		User GetUserWithRoleByEmail(string email);
		User GetUserByEmail(string email);
		User GetUserById(Guid id);
		IEnumerable<User> GetAllUsers(string name,int page, int limit);
		//void CreateUser(User user);
		void CreateUser(CreateNewUserDTO user);
		void UpdateUser(User user, string updateRole, int statusId);
		Guid GetUserIdByUserName(string userName);
		User CreateUserByEmailAndName(string email, string name);

		bool ImportInProgressStudents(IEnumerable<InProgressStudentsRequest> inProgressStudentsRequests);
		GenericResponse CreateNewLectuer(LecturerResquest user);
		GenericResponse CreateNewStudent(StudentRequest user);
	}
}
