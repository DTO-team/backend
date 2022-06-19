using System;
using Models.Request;
using Models.Response;
using System.Collections.Generic;
namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ITeamService
	{
		bool CreateTeam(CreateTeamRequest createTeamRequest, string userEmail, out CreatedTeamResponse createdTeamResponse);
		bool DeleteTeam(Guid deletedTeamId, string userEmail);
		IEnumerable<GetTeamResponse> GetAllTeams(string teamName = null, int page = 1, int limit = 10);
		bool JoinTeam(Guid teamId, string studentEmail, out GetTeamDetailResponse getTeamDetailResponse);

		GetTeamDetailResponse GetTeamDetail(Guid teamId);
	}
}
