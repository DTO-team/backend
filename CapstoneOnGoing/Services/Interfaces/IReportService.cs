using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using Models.Dtos;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface IReportService
	{
		bool CreateWeeklyReport(Guid teamId, string studentEmail,CreateWeeklyReportDTO createWeeklyReportDTO);

		List<GetTeamWeeklyReportResponse> GetTeamWeeklyReport(Guid teamId, int week, GetSemesterDTO currentSemester, string email);
	}
}
