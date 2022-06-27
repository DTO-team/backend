using System;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Helper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
	public class ReportService : IReportService
	{
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public ReportService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public bool CreateWeeklyReport(Guid teamId, string studentEmail, CreateWeeklyReportDTO createWeeklyReportDTO)
		{
			//check team is exist
			Semester currentSemester = _unitOfWork.Semester.Get(x => x.Status == (int)SemesterStatus.Ongoing).FirstOrDefault();
			Team team = _unitOfWork.Team.Get( x => (x.Id == teamId && x.SemesterId == currentSemester.Id),null, "TeamStudents").FirstOrDefault();
			Project currentProject = _unitOfWork.Project.Get(x => x.TeamId == team.Id).FirstOrDefault();
			User user = _unitOfWork.User.Get(x => x.Email.Equals(studentEmail)).FirstOrDefault();
			if (team == null)
			{
				throw new BadHttpRequestException("Team does not exist");
			}
			if (team.TeamStudents.Select(x => x.StudentId).Contains(user.Id))
			{
				//Get current week
				long currentDateTime = DateTimeHelper.ConvertDateTimeToLong(DateTime.Now);
				Week currentWeek = _unitOfWork.Week
					.Get(x => x.FromDate <= currentDateTime && x.ToDate >= currentDateTime).FirstOrDefault();
				//check deadline
				if (currentDateTime >= currentWeek.Deadline)
				{
					throw new BadHttpRequestException(
						$"Deadline of week was: {DateTimeHelper.ConvertLongToDateTime(currentWeek.Deadline)}");
				}
				//
				Report newReport = new Report()
				{
					ProjectId = currentProject.Id,
					ReporterId = user.Id,
					IsTeamReport = createWeeklyReportDTO.IsTeamReport,
					CompletedTasks = createWeeklyReportDTO.CompletedTasks,
					InProgressTasks = createWeeklyReportDTO.InProgressTasks,
					NextWeekTasks = createWeeklyReportDTO.NextWeekTasks,
					UrgentIssues = createWeeklyReportDTO.UrgentIssues,
					SelfAssessments = createWeeklyReportDTO.SelfAssessment,
					Feedbacks = createWeeklyReportDTO.FeedBack,
					WeekId = currentWeek.Id
				};
				Array.ForEach(createWeeklyReportDTO.ReportEvidences.ToArray(), reportEvidence =>
				{
					newReport.ReportEvidences.Add(new ReportEvidence()
					{
						Url = reportEvidence.Url,
						Name = reportEvidence.Name,
						ReportId = newReport.Id
					});
				});
				_unitOfWork.Report.Insert(newReport);
				return _unitOfWork.Save() > 0;
			}
			else
			{
				throw new BadHttpRequestException("You're not in the team");
			}
		}
	}
}
