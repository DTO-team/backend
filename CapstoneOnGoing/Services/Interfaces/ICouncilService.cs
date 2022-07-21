using System;
using System.Collections.Generic;
using Models.Dtos;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ICouncilService
	{
		void CreateCouncil(CreateCouncilRequest createCouncilRequest);

		IEnumerable<GetCouncilResponse> GetAllCouncils(GetSemesterDTO semester);

		GetCouncilResponse GetCouncilById(Guid councilId, GetSemesterDTO semester);

		bool UpdateCouncil(Guid id,UpdateCouncilRequest updateCouncilRequest);

		IEnumerable<GetCouncilOfTeamResponse> GetCouncilOfTeamById(Guid teamId);
	}
}