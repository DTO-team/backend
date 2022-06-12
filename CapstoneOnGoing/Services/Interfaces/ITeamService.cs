using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ITeamService
	{
		bool CreateTeam(CreateTeamRequest createTeamRequest, out CreatedTeamResponse createdTeamResponse);
		bool DeleteTeam(DeleteTeamRequest deleteTeamRequest);
	}
}
