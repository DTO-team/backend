using System;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ITeamService
	{
		bool CreateTeam(CreateTeamRequest createTeamRequest,string userEmail, out CreatedTeamResponse createdTeamResponse);
		bool DeleteTeam(Guid deletedTeamId,string userEmail);
	}
}
