using System;
using Models.Request;
using Models.Response;
using System.Collections.Generic;
using Models.Models;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ITeamService
	{
		bool CreateTeam(CreateTeamRequest createTeamRequest, string userEmail, out CreatedTeamResponse createdTeamResponse);
		bool DeleteTeam(Guid deletedTeamId, string userEmail);
		IEnumerable<GetTeamResponse> GetAllTeams(string teamName = null, int page = 1, int limit = 10);
		bool JoinTeam(string studentEmail,string joinCode, out GetTeamDetailResponse getTeamDetailResponse);
		GetTeamDetailResponse GetTeamDetail(Guid teamId);
        bool IsTeamLeader(Guid userId);

        //Return team Id for controller to get team detail
        //and return response of team detail
		Guid UpdateTeamMentor(UpdateMentorRequest updateMentorRequest);
    }
}
