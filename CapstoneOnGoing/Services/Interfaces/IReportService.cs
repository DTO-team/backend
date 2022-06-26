using System;
using Models.Dtos;
using Models.Request;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface IReportService
	{
		bool CreateWeeklyReport(Guid teamId, string studentEmail,CreateWeeklyReportDTO createWeeklyReportDTO);
	}
}
