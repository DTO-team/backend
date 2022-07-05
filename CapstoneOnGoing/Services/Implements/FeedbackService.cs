using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
    public class FeedbackService: IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<GetFeedbackDTO> GetFeedbacksByReportId(Guid reportId)
        {
            IEnumerable<GetFeedbackDTO> feedbackDtos = null;
            IEnumerable<Feedback> feedbacks = _unitOfWork.Feedback.Get(feedback => feedback.ReportId.Equals(reportId));
            if (feedbacks.Any())
            {
                feedbackDtos = _mapper.Map<IEnumerable<GetFeedbackDTO>>(feedbacks);
                return feedbackDtos;
            }
            return feedbackDtos;
        }

        public IEnumerable<GetFeedbackDTO> GetAllFeedback(GetFeedbackRequest getFeedbackRequest)
        {
            IEnumerable<GetFeedbackDTO> feedbackDtos = null;
            Guid weekId = getFeedbackRequest.WeekId;
            Guid projectId = getFeedbackRequest.ProjectId;

            //Get week by request
            Week week = _unitOfWork.Week.GetById(weekId);
            if (week is not null)
            {
                //Get report by project Id and WeekId
                IEnumerable<Report> teamInProjectReport =
                    _unitOfWork.Report.Get(report =>
                        report.IsTeamReport.Equals(false)
                        && report.ProjectId.Equals(projectId)
                        && report.WeekId.Equals(weekId));

                if (teamInProjectReport.Any())
                {
                    long weekStartDate = week.FromDate;
                    long weekEndDate = week.ToDate;

                    //Get list feedback by reportId and CurrentDateTime
                    List<Feedback> feedbacks = new List<Feedback>();
                    Array.ForEach(teamInProjectReport.ToArray(), teamReport =>
                    {
                        Feedback feedback = _unitOfWork.Feedback.Get(
                            feedback => (feedback.ReportId.Equals(teamReport.Id) &&
                                         (feedback.CreatedDateTime >= weekStartDate &&
                                          feedback.CreatedDateTime <= weekEndDate)), null, "Author").FirstOrDefault();
                        if (feedback is not null)
                        {
                            feedbacks.Add(feedback);
                        }
                    });
                    if (feedbacks.Any())
                    {
                        feedbackDtos = _mapper.Map<IEnumerable<GetFeedbackDTO>>(feedbacks);
                        return feedbackDtos;
                    }
                }
                return feedbackDtos;
            }
            else
            {
                throw new BadHttpRequestException(
                    $"Week with {getFeedbackRequest.WeekId} id is not started or existed!");
            }
        }

        public IEnumerable<GetFeedbackDTO> GetAllStudentFeedback(GetFeedbackRequest studentsOfATeamFeedbackRequest)
        {
            IEnumerable<GetFeedbackDTO> feedbackDtos = null;
            Guid weekId = studentsOfATeamFeedbackRequest.WeekId;
            Guid projectId = studentsOfATeamFeedbackRequest.ProjectId;

            //Get week by request
            Week week = _unitOfWork.Week.GetById(weekId);
            if (week is not null)
            {
                //Get report by project Id and WeekId
                IEnumerable<Report> teamInProjectReport =
                    _unitOfWork.Report.Get(report =>
                        report.IsTeamReport.Equals(false)
                        && report.ProjectId.Equals(projectId)
                        && report.WeekId.Equals(weekId)
                        && report.IsTeamReport.Equals(false));

                if (teamInProjectReport.Any())
                {
                    long weekStartDate = week.FromDate;
                    long weekEndDate = week.ToDate;

                    //Get list feedback by reportId and CurrentDateTime
                    List<Feedback> feedbacks = new List<Feedback>();
                    Array.ForEach(teamInProjectReport.ToArray(), teamReport =>
                    {
                        Feedback feedback = _unitOfWork.Feedback.Get(
                            feedback => (feedback.ReportId.Equals(teamReport.Id) &&
                                         (feedback.CreatedDateTime >= weekStartDate &&
                                          feedback.CreatedDateTime <= weekEndDate)), null, "Author").FirstOrDefault();
                        if (feedback is not null)
                        {
                            feedbacks.Add(feedback);
                        }
                    });
                    if (feedbacks.Any())
                    {
                        feedbackDtos = _mapper.Map<IEnumerable<GetFeedbackDTO>>(feedbacks);
                        return feedbackDtos;
                    }
                }
                return feedbackDtos;
            }
            else
            {
                throw new BadHttpRequestException(
                    $"Week with {studentsOfATeamFeedbackRequest.WeekId} id is not started or existed!");
            }
        }

        public GetFeedbackDTO GetFeedbackOfStudentReport(GetFeedbackOfStudentReport feedbackOfStudentOfStudentReport)
        {
            GetFeedbackDTO feedbackDto = null;
            Guid weekId = feedbackOfStudentOfStudentReport.WeekId;
            Guid projectId = feedbackOfStudentOfStudentReport.ProjectId;
            Guid studentId = feedbackOfStudentOfStudentReport.StudentId;

            //Get week by request
            Week week = _unitOfWork.Week.GetById(weekId);
            if (week is not null)
            {
                Project project = _unitOfWork.Project.GetById(projectId);
                TeamStudent teamStudent = _unitOfWork.TeamStudent
                    .Get(x => x.TeamId.Equals(project.TeamId) && x.StudentId.Equals(studentId)).FirstOrDefault();

                if (teamStudent is not null)
                {
                    //Get report by project Id and WeekId
                    Report studentReport =
                        _unitOfWork.Report.Get(report =>
                            report.IsTeamReport.Equals(false)
                            && report.ProjectId.Equals(projectId)
                            && report.WeekId.Equals(weekId)
                            && report.ReporterId.Equals(studentId)
                            && report.IsTeamReport.Equals(false)).FirstOrDefault();

                    if (studentReport is not null)
                    {
                        long weekStartDate = week.FromDate;
                        long weekEndDate = week.ToDate;

                        //Get feedback by reportId and CurrentDateTime
                        Feedback feedback = _unitOfWork.Feedback.Get(
                            feedback => (feedback.ReportId.Equals(studentReport.Id) &&
                                         (feedback.CreatedDateTime >= weekStartDate &&
                                          feedback.CreatedDateTime <= weekEndDate)), null, "Author").FirstOrDefault();
                        if (feedback is not null)
                        {
                            feedbackDto = _mapper.Map<GetFeedbackDTO>(feedback);
                            return feedbackDto;
                        }
                    }
                    return feedbackDto;
                }
                else
                {
                    throw new BadHttpRequestException($"Student with {studentId} id is not in team of the project");
                }
            }
            else
            {
                throw new BadHttpRequestException(
                    $"Week with {feedbackOfStudentOfStudentReport.WeekId} id is not started or existed!");
            }
        }

        public GetFeedbackDTO GetFeedbackOfTeamReport(GetFeedbackRequest feedbackRequest)
        {
            GetFeedbackDTO feedbackDto = null;
            Guid weekId = feedbackRequest.WeekId;
            Guid projectId = feedbackRequest.ProjectId;

            //Get week by request
            Week week = _unitOfWork.Week.GetById(weekId);
            if (week is not null)
            {
                //Get report by project Id and WeekId
                Report teamReport =
                    _unitOfWork.Report.Get(report =>
                        report.IsTeamReport.Equals(false)
                        && report.ProjectId.Equals(projectId)
                        && report.WeekId.Equals(weekId)
                        && report.IsTeamReport.Equals(true))
                        .FirstOrDefault();

                if (teamReport is not null)
                {
                    long weekStartDate = week.FromDate;
                    long weekEndDate = week.ToDate;

                    //Get list feedback by reportId and CurrentDateTime
                    Feedback feedback = _unitOfWork.Feedback.Get(
                        feedback => (feedback.ReportId.Equals(teamReport.Id) &&
                                     (feedback.CreatedDateTime >= weekStartDate &&
                                      feedback.CreatedDateTime <= weekEndDate)), null, "Author").FirstOrDefault();
                    if (feedback is not null)
                    {
                        feedbackDto = _mapper.Map<GetFeedbackDTO>(feedback);
                        return feedbackDto;
                    }
                }
                return feedbackDto;
            }
            else
            {
                throw new BadHttpRequestException(
                    $"Week with {feedbackRequest.WeekId} id is not started or existed!");
            }
        }
    }
}
