﻿using System;
using System.Collections.Generic;
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
using Models.Response;
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

        private bool CreateWeeklyReport(Project currentProject, User user, CreateWeeklyReportDTO createWeeklyReportDTO,
            Week currentWeek, long currentDateTime)
        {

            //check deadline
            if (currentDateTime >= currentWeek.Deadline)
            {
                throw new BadHttpRequestException(
                    $"Deadline of week was: {DateTimeHelper.ConvertLongToDateTime(currentWeek.Deadline)}");
            }

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


        private Report GetCreatedReport(CreateWeeklyReportDTO createWeeklyReportDTO, User user, Week currentWeek)
        {
            Report createdReport = null;
            createdReport =_unitOfWork.Report.Get(report =>
                report.IsTeamReport.Equals(createWeeklyReportDTO.IsTeamReport) &&
                report.ProjectId.Equals(createWeeklyReportDTO.ProjectId) && report.ReporterId.Equals(user.Id) &&
                report.WeekId.Equals(currentWeek.Id)).FirstOrDefault();
            if (createdReport is not null)
            {
                return createdReport;
            }
            else
            {
                throw new BadHttpRequestException("Report is not created !");
            }
        }

        public Guid? CreateWeeklyReport(Guid teamId, string studentEmail, CreateWeeklyReportDTO createWeeklyReportDTO)
        {
            Guid? returnReportId = null;
            //check team is exist
            Semester currentSemester =
                _unitOfWork.Semester.Get(x => x.Status == (int)SemesterStatus.Ongoing).FirstOrDefault();
            Team team = _unitOfWork.Team
                .Get(x => (x.Id == teamId && x.SemesterId == currentSemester.Id), null, "TeamStudents")
                .FirstOrDefault();
            if (team == null)
            {
                throw new BadHttpRequestException("Team does not exist");
            }

            User user = _unitOfWork.User.Get(x => x.Email.Equals(studentEmail)).FirstOrDefault();

            if (team.TeamStudents.Select(x => x.StudentId).Contains(user.Id))
            {
                //Get team project
                Project currentProject = _unitOfWork.Project.Get(x => x.TeamId == team.Id).FirstOrDefault();

                //Get current week
                long currentDateTime = DateTimeHelper.ConvertDateTimeToLong(DateTime.Now);
                Week currentWeek = _unitOfWork.Week
                    .Get(x => x.FromDate <= currentDateTime && x.ToDate >= currentDateTime).FirstOrDefault();

                //Check is any report exist by reporter and currentWeek;
                IEnumerable<Report> reports =
                    _unitOfWork.Report.Get(report =>
                        report.ReporterId.Equals(user.Id) && report.WeekId.Equals(currentWeek.Id));

                //If not have any exist => create new report
                if (reports.Any().Equals(false))
                {
                    bool isCreated = CreateWeeklyReport(currentProject, user, createWeeklyReportDTO, currentWeek, currentDateTime);
                    if (isCreated)
                    {
                        Report createdReport = GetCreatedReport(createWeeklyReportDTO, user, currentWeek);
                        returnReportId = createdReport.Id;
                    }

                    return returnReportId;
                }

                //If have => check is have team report or not (because of team leader can create 2 report)
                //If not have team report => create new team report 
                else if (reports.Any().Equals(true) && user.Id.Equals(team.TeamLeaderId) && createWeeklyReportDTO.IsTeamReport.Equals(true))
                {
                    bool isExistedReport = reports.ToArray().Select(report =>
                            report.IsTeamReport.Equals(true) && report.ProjectId.Equals(currentProject.Id))
                        .FirstOrDefault();
                    if (isExistedReport.Equals(false))
                    {
                        bool isCreated = CreateWeeklyReport(currentProject, user, createWeeklyReportDTO, currentWeek, currentDateTime);
                        if (isCreated)
                        {
                            Report createdReport = GetCreatedReport(createWeeklyReportDTO, user, currentWeek);
                            returnReportId = createdReport.Id;
                        }

                        return returnReportId;
                    }
                    else
                    {
                        throw new BadHttpRequestException("You already update your team report in this week");
                    }
                }
                else
                {
                    throw new BadHttpRequestException("You already update your report in this week");
                }
            }
            else
            {
                throw new BadHttpRequestException("You're not in the team");
            }
        }

        public List<GetTeamWeeklyReportResponse> GetTeamWeeklyReport(Guid teamId, int week, GetSemesterDTO currentSemester, string email)
        {
	        Team team = _unitOfWork.Team.GetTeamWithProject(teamId);
	        User user = _unitOfWork.User.Get(x => x.Email.Equals(email)).FirstOrDefault();
	        Semester semseter = _unitOfWork.Semester.GetById(currentSemester.Id);
	        Week currentWeek = _unitOfWork.Week.Get(x => x.SemesterId == semseter.Id && x.Number == week).FirstOrDefault();
	        if (team == null)
	        {
		        throw new BadHttpRequestException("Team does not exits");

	        }

	        if (user == null)
	        {
		        throw new BadHttpRequestException("User does not exits");
	        }
            //check if user is student in team or mentor of the team
	        if (team.TeamStudents.Select(x => x.StudentId).Contains(user.Id) || team.Project.Mentors.Select(x => x.LecturerId).Contains(user.Id))
	        {
		        List<GetTeamWeeklyReportResponse> teamWeeklyReportsResponse = new List<GetTeamWeeklyReportResponse>(); 
		        switch (user.RoleId)
		        {
                    case (int)RoleEnum.Student:
	                    Report studentWeeklyReport = _unitOfWork.Report
		                    .Get(x => x.WeekId == currentWeek.Id && x.ReporterId.Equals(user.Id) && x.ProjectId == team.Project.Id,null, "Week,ReportEvidences").FirstOrDefault();
	                    Report teamWeeklyReports = _unitOfWork.Report
		                    .Get(x => x.WeekId == currentWeek.Id && x.IsTeamReport == true && x.ProjectId == team.Project.Id, null, "Week,ReportEvidences").FirstOrDefault();
	                    GetTeamWeeklyReportResponse studentWeeklyReportResponse =
		                    _mapper.Map<GetTeamWeeklyReportResponse>(studentWeeklyReport);
	                    GetTeamWeeklyReportResponse teamWeeklyReportResponse =
		                    _mapper.Map<GetTeamWeeklyReportResponse>(teamWeeklyReports);
	                        teamWeeklyReportsResponse.Add(studentWeeklyReportResponse);
	                        teamWeeklyReportsResponse.Add(teamWeeklyReportResponse);
	                    break;
                    case (int)RoleEnum.Lecturer:
                    case (int)RoleEnum.Admin:

	                    IEnumerable<Report> studentWeeklyReports = _unitOfWork.Report.Get(
		                    x => x.WeekId == currentWeek.Id && x.ProjectId == team.Project.Id, null, "Week,ReportEvidences");
                        Report teamWeeklyReport = _unitOfWork.Report
	                        .Get(x => x.WeekId == currentWeek.Id && x.IsTeamReport == true && x.ProjectId == team.Project.Id, null, "Week,ReportEvidences").FirstOrDefault();
	                    IEnumerable<GetTeamWeeklyReportResponse> studentWeeklyReportsResponse =
		                    _mapper.Map<IEnumerable<GetTeamWeeklyReportResponse>>(studentWeeklyReports);
	                    GetTeamWeeklyReportResponse teamsWeeklyReportResponse =
		                    _mapper.Map<GetTeamWeeklyReportResponse>(teamWeeklyReport);
	                    teamWeeklyReportsResponse.AddRange(studentWeeklyReportsResponse);
	                    teamWeeklyReportsResponse.Add(teamsWeeklyReportResponse);
                        break;
		        }
		        return teamWeeklyReportsResponse;

	        }
	        else
	        {
		        throw new BadHttpRequestException("You can view this team report");
	        }
        }

        public GetWeeklyReportDetailResponse GetReportDetail(Guid teamId, Guid reportId, string email)
        {
	        Team team = _unitOfWork.Team.GetTeamWithProject(teamId);
	        User user = _unitOfWork.User
		        .Get(x => x.Email.Equals(email) && x.StatusId == (int)UserStatus.Activated).FirstOrDefault();
	        if (team == null || user == null)
	        {
		        throw new BadHttpRequestException("Team or student does not exist");
	        }

	        if (!team.TeamStudents.Select(x => x.Id).Contains(user.Id) ||
	            !team.Project.Mentors.Select(x => x.Id).Contains(user.Id))
	        {
		        throw new BadHttpRequestException("You don't have permission to view this report");
	        }

	        Report report = _unitOfWork.Report.Get(x => x.Id == reportId, null, "Reporter,ReportEvidences,Week").FirstOrDefault();
	        if (report == null)
	        {
		        return null;
	        }

	        User reporter = _unitOfWork.User.Get(x => x.Id == report.ReporterId, null, "Student").FirstOrDefault();
	        reporter.Student.Semester = _unitOfWork.Semester.GetById(reporter.Student.SemesterId.Value);
	        GetWeeklyReportDetailResponse getWeeklyReportDetailResponse =
		        _mapper.Map<GetWeeklyReportDetailResponse>(report);
            return getWeeklyReportDetailResponse;
        }

        public bool FeedbackReport(Guid teamId, Guid reportId, string email, FeedbackReportRequest feedbackReportRequest)
        {
	        Team team = _unitOfWork.Team.GetTeamWithProject(teamId);
	        User lecturer = _unitOfWork.User.Get(x => x.Email.Equals(email)).FirstOrDefault();
	        if (team == null || lecturer == null)
	        {
		        throw new BadHttpRequestException("Team does not exist or lecturer does not exist");
	        }

	        if (!team.Project.Mentors.Select(x => x.Id).Contains(lecturer.Id))
	        {
		        throw new BadHttpRequestException("You are not mentors of this team");
	        }
			Report report = _unitOfWork.Report.GetById(reportId);
			if (report == null)
			{
				throw new BadHttpRequestException("Report does not exist");
			}
			report.Feedbacks = feedbackReportRequest.Value;
            _unitOfWork.Report.Update(report);
            bool isSuccessful = _unitOfWork.Save() > 0;
            return isSuccessful;
        }
    }
}

