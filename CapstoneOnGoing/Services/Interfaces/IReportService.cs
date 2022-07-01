using System;
using Microsoft.Extensions.Primitives;
using Models.Dtos;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface IReportService
	{
		bool CreateWeeklyReport(Guid teamId, string studentEmail,CreateWeeklyReportDTO createWeeklyReportDTO);

		GetTeamWeeklyReport GetTeamWeeklyReport(string teamId, int week, StringValues currentSemester, string role);
	}
}
