using System;
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
    public class EvaluationSessionService : IEvaluationSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICouncilService _councilService;
        private readonly IMapper _mapper;
        private readonly ILecturerService _lecturerService;
        private readonly IProjectService _projectService;
        private readonly ITeamService _teamService;

        public EvaluationSessionService(IUnitOfWork unitOfWork, ICouncilService councilService, IMapper mapper, ILecturerService lecturerService, IProjectService projectService, ITeamService teamService)
        {
            _unitOfWork = unitOfWork;
            _councilService = councilService;
            _mapper = mapper;
            _lecturerService = lecturerService;
            _projectService = projectService;
            _teamService = teamService;
        }

        public bool UpdateEvaluationSessionStatus(Guid id, UpdateEvaluationSessionRequest updateEvaluationSessionRequest)
        {
            bool isSuccessful = false;
            if (updateEvaluationSessionRequest != null)
            {
                EvaluationSession updatedEvaluationSession = _unitOfWork.EvaluationSession.GetById(id);
                if (updatedEvaluationSession == null) throw new BadHttpRequestException("No Evaluation session found");
                switch (updateEvaluationSessionRequest.Status)
                {
                    case (int)EvaluationSessionStatus.ONGOING:
                        updatedEvaluationSession.Status = updateEvaluationSessionRequest.Status;
                        _unitOfWork.EvaluationSession.Update(updatedEvaluationSession);
                        _unitOfWork.Save();
                        //Create council when evaluation session status is OnGoing
                        _councilService.CreateCouncil(updateEvaluationSessionRequest.CreateCouncilRequest);
                        isSuccessful = _unitOfWork.Save() > 0;
                        break;
                    case (int)EvaluationSessionStatus.ENDED:
                        updatedEvaluationSession.Status = updateEvaluationSessionRequest.Status;
                        _unitOfWork.EvaluationSession.Update(updatedEvaluationSession);
                        isSuccessful = _unitOfWork.Save() > 0;
                        break;
                }
            }
            return isSuccessful;
        }

        public IEnumerable<GetEvaluationSessionResponse> GetAllEvaluationSession(Guid semesterGuid)
        {
            List<GetEvaluationSessionResponse> getEvaluationSessionResponses = new List<GetEvaluationSessionResponse>();
            IEnumerable<EvaluationSession> evaluationSessions = _unitOfWork.EvaluationSession.Get(x => x.SemesterId == semesterGuid);
            Array.ForEach(evaluationSessions.ToArray(), evaluationSession =>
            {
                GetEvaluationSessionResponse getEvaluationSessionResponse =
                    GetEvaluationSessionById(evaluationSession.Id, semesterGuid);
                getEvaluationSessionResponses.Add(getEvaluationSessionResponse);
            });
            return getEvaluationSessionResponses.OrderBy(x => x.DeadLine);
        }

        public GetEvaluationSessionResponse GetEvaluationSessionById(Guid evaluationId, Guid semesterId)
        {
            EvaluationSession evaluationSession = _unitOfWork.EvaluationSession
                .Get(x => x.Id == evaluationId && x.SemesterId == semesterId, null, "Semester,EvaluationSessionCriteria,Councils").FirstOrDefault();
            if (evaluationSession == null)
            {
                throw new BadHttpRequestException("No evaluation session found");
            }

            GetEvaluationSessionResponse evaluationSessionResponse =
                _mapper.Map<GetEvaluationSessionResponse>(evaluationSession);
            if (evaluationSession.EvaluationSessionCriteria.Any())
            {
                evaluationSessionResponse.SemesterCriterias = new List<GetSemesterCriteriaResponse>();
                Array.ForEach(evaluationSession.EvaluationSessionCriteria.ToArray(), evaluationSessionCriteria =>
                {
                    IEnumerable<SemesterCriterion> semesterCriterion =
                        _unitOfWork.SememesterCriterion.Get(x => x.Id == evaluationSessionCriteria.CriteriaId);
                    IEnumerable<GetSemesterCriteriaResponse> semesterCriteriaResponses =
                        _mapper.Map<IEnumerable<GetSemesterCriteriaResponse>>(semesterCriterion);
                    evaluationSessionResponse.SemesterCriterias.AddRange(semesterCriteriaResponses);
                });
            }

            if (evaluationSession.Councils.Any())
            {
                evaluationSessionResponse.LecturerInCouncils = new List<GetLecturerInCouncil>();
                evaluationSessionResponse.Projects = new List<GetProjectDetailDTO>();
                Array.ForEach(evaluationSession.Councils.ToArray(), council =>
                {
                    GetLecturerInCouncil getLecturerInCouncil = new GetLecturerInCouncil();
                    getLecturerInCouncil.Lecturers = new List<GetLecturerResponse>();
                    IEnumerable<Council> councils = _unitOfWork.Councils.Get(x => x.Id == council.Id, null, "CouncilLecturers,CouncilProjects");
                    Array.ForEach(councils.ToArray(), council =>
                    {
                        Array.ForEach(council.CouncilLecturers.ToArray(), councilLecturer =>
                        {
                            User lecturer = _lecturerService.GetLecturerById(councilLecturer.LecturerId);
                            GetLecturerResponse lecturerResponse = _mapper.Map<GetLecturerResponse>(lecturer);
                            getLecturerInCouncil.Lecturers.Add(lecturerResponse);
                        });
                        Array.ForEach(council.CouncilProjects.ToArray(), councilProject =>
                        {
	                        GetProjectDetailDTO project = _projectService.GetProjectDetailById(councilProject.ProjectId,
		                        new GetSemesterDTO() {Id = semesterId});
                            if(project != null) evaluationSessionResponse.Projects.Add(project);
                        });
                    });
                    evaluationSessionResponse.LecturerInCouncils.Add(getLecturerInCouncil);
                });
            }
            return evaluationSessionResponse;
        }

        public bool CreateNewReviewOfCouncilEvaluationSession(CreateNewReviewRequest newReviewRequest)
        {
            Council evaluationCouncil = _unitOfWork.Councils.GetById(newReviewRequest.CouncilId);
            Project evaluationProject = _unitOfWork.Project.GetById(newReviewRequest.ProjectId);
            ICollection<ReviewGrade> newGrades = new List<ReviewGrade>();
            ICollection<ReviewQuestion> newQuestions = new List<ReviewQuestion>();

            if (evaluationCouncil is null)
            {
                throw new BadHttpRequestException($"Council with {newReviewRequest.CouncilId} id is not existed!");
            }
            if (evaluationProject is null)
            {
                throw new BadHttpRequestException($"Project with {newReviewRequest.ProjectId} id is not existed!");
            }
            Review newReview = new Review();
            newReview.CouncilId = newReviewRequest.CouncilId;
            newReview.ProjectId = newReviewRequest.ProjectId;
            newReview.IsFinal = newReviewRequest.IsFinal;
            if (newReview.Date > 0)
            {
                newReview.Date = newReviewRequest.Date;
            }
            else
            {
                newReview.Date = DateTimeHelper.ConvertDateTimeToLong(DateTime.Now);
            }
            if (newReviewRequest.IsFinal)
            {
                foreach (ReviewGradeRequest reviewGradeRequest in newReviewRequest.ReviewGrade)
                {
                    Grade requestGrade = _unitOfWork.Grade.GetById(reviewGradeRequest.GradeId);
                    if (requestGrade is not null)
                    {
                        ReviewGrade grade = _mapper.Map<ReviewGrade>(reviewGradeRequest);
                        grade.Id = Guid.NewGuid();
                        newGrades.Add(grade);
                    }
                    else
                    {
                        throw new BadHttpRequestException(
                            $"Grade with {reviewGradeRequest.GradeId} id is not existed!");
                    }
                }
                newReview.ReviewGrades = newGrades;
            }
            else
            {
                foreach (ReviewQuestionRequest reviewQuestionRequest in newReviewRequest.ReviewQuestion)
                {
                    Question requestQuestion = _unitOfWork.Question.GetById(reviewQuestionRequest.QuestionId);
                    if (reviewQuestionRequest.Answer.ToLower().Contains("no") || reviewQuestionRequest.Answer.ToLower().Contains("yes"))
                    {
                        if (requestQuestion is not null)
                        {
                            ReviewQuestion question = _mapper.Map<ReviewQuestion>(reviewQuestionRequest);
                            question.Id = Guid.NewGuid();
                            newQuestions.Add(question);
                        }
                        else
                        {
                            throw new BadHttpRequestException(
                                $"Question with {reviewQuestionRequest.QuestionId} id is not existed!");
                        }
                    }
                    else
                    {
                        throw new BadHttpRequestException($"Question answer is only yes/no");
                    }
                }
                newReview.ReviewQuestions = newQuestions;
            }
            _unitOfWork.Review.Insert(newReview);
            return _unitOfWork.Save() > 0;
        }

        public bool CreateNewEvaluationSessionReport(Guid evaluationSessionId, CreateNewEvaluationReportRequest newEvaluationReportRequest)
        {
            EvaluationSession evaluationSession = _unitOfWork.EvaluationSession.GetById(evaluationSessionId);
            Team team = _unitOfWork.Team.GetById(newEvaluationReportRequest.TeamId);

            if (evaluationSession is null)
            {
                throw new BadHttpRequestException($"Evaluation Session with {evaluationSessionId} id is not existed!");
            }

            if (team is null)
            {
                throw new BadHttpRequestException($"Team with {newEvaluationReportRequest.TeamId} id is not existed!");
            }

            if (!newEvaluationReportRequest.EvaluationReportDetailsRequest.Any())
            {
                throw new BadHttpRequestException($"Evaluation report detail is required!");
            }

            EvaluationReport newEvaluationReport = new EvaluationReport();
            newEvaluationReport.EvaluationSessionId = evaluationSessionId;
            newEvaluationReport.TeamId = newEvaluationReportRequest.TeamId;

            ICollection<EvaluationReportDetail> evaluationReportDetailsRequest =
                _mapper.Map<ICollection<EvaluationReportDetail>>(newEvaluationReportRequest
                    .EvaluationReportDetailsRequest);

            newEvaluationReport.EvaluationReportDetails = evaluationReportDetailsRequest;

            _unitOfWork.EvaluationReport.Insert(newEvaluationReport);
            return _unitOfWork.Save() > 0;
        }

        public bool UpdateEvaluationSessionReport(Guid evaluationReportId,
            UpdateEvaluationReportDetailRequest newEvaluationReportDetailRequest)
        {
            EvaluationReport existedEvaluationReport = _unitOfWork.EvaluationReport.Get(report =>
                    report.Id.Equals(evaluationReportId))
                .FirstOrDefault();
            if (existedEvaluationReport is null)
            {
                throw new BadHttpRequestException(
                    $"Evaluation Report with {evaluationReportId} evaluation report id is not existed!");
            }

            EvaluationReportDetail evaluationReportDetail =
                _unitOfWork.EvaluationReportDetail.Get(reportDetail => reportDetail.Id.Equals(newEvaluationReportDetailRequest.evaluationReportDetailId) && reportDetail.EvaluationReportId.Equals(evaluationReportId)).FirstOrDefault();
            if (evaluationReportDetail is not null)
            {
                if (!string.IsNullOrEmpty(newEvaluationReportDetailRequest.updateNewEvaluationReportDetail.Name))
                { 
                    evaluationReportDetail.Name = newEvaluationReportDetailRequest.updateNewEvaluationReportDetail.Name;
                }

                if (!string.IsNullOrEmpty(newEvaluationReportDetailRequest.updateNewEvaluationReportDetail.Url))
                {
                    evaluationReportDetail.Url = newEvaluationReportDetailRequest.updateNewEvaluationReportDetail.Url;
                }
                _unitOfWork.EvaluationReportDetail.Update(evaluationReportDetail);
                return _unitOfWork.Save() > 0;
            }
            else
            {
                throw new BadHttpRequestException(
                    $"Evaluation Report Detail with {newEvaluationReportDetailRequest.evaluationReportDetailId} id is not existed!");
            }
        }

        public GetAllEvaluationReportResponse GetAllEvaluationReportById(Guid evaluationReportId, GetSemesterDTO semesterDto)
        {
            GetAllEvaluationReportResponse reportsResponse = new GetAllEvaluationReportResponse();
            EvaluationReport evaluationReport = _unitOfWork.EvaluationReport.GetById(evaluationReportId);
            if (evaluationReport is null)
            {
                throw new BadHttpRequestException($"Evaluation report with {evaluationReportId} id is not existed!");
            }

            GetTeamDetailResponse teamDetailResponse = _teamService.GetTeamDetail(evaluationReport.TeamId);
            GetEvaluationSessionResponse evaluationSessionResponse =
                GetEvaluationSessionById(evaluationReport.EvaluationSessionId, semesterDto.Id);

            IEnumerable<EvaluationReportDetail> reportsDetail =
                _unitOfWork.EvaluationReportDetail.Get(report => report.EvaluationReportId.Equals(evaluationReportId));

            if (reportsDetail.Any())
            {
                reportsResponse.TeamDetail = teamDetailResponse;
                reportsResponse.EvaluationSession = evaluationSessionResponse;
                reportsResponse.EvaluationReportsDetail = _mapper.Map<IEnumerable<EvaluationReportDetailResponse>>(reportsDetail);
                return reportsResponse;
            }
            else
            {
                throw new BadHttpRequestException("Get all evaluation report detail failed!");
            }
        }

        public GetEvaluationReportDetailResponse GetEvaluationReportDetailById(Guid evaluationReportId, Guid evaluationReportDetailId,
            GetSemesterDTO semesterDto)
        {
            GetEvaluationReportDetailResponse reportResponse = new GetEvaluationReportDetailResponse();
            EvaluationReport evaluationReport = _unitOfWork.EvaluationReport.GetById(evaluationReportId);
            if (evaluationReport is null)
            {
                throw new BadHttpRequestException($"Evaluation report with {evaluationReportId} id is not existed!");
            }

            GetTeamDetailResponse teamDetailResponse = _teamService.GetTeamDetail(evaluationReport.TeamId);
            GetEvaluationSessionResponse evaluationSessionResponse =
                GetEvaluationSessionById(evaluationReport.EvaluationSessionId, semesterDto.Id);

            EvaluationReportDetail reportDetail =
                _unitOfWork.EvaluationReportDetail.Get(report => 
                    report.EvaluationReportId.Equals(evaluationReportId) && report.Id.Equals(evaluationReportDetailId)).FirstOrDefault();

            if (reportDetail is not null)
            {
                reportResponse.TeamDetail = teamDetailResponse;
                reportResponse.EvaluationSession = evaluationSessionResponse;
                reportResponse.EvaluationReportDetail = _mapper.Map<EvaluationReportDetailResponse>(reportDetail);
                return reportResponse;
            }
            else
            {
                throw new BadHttpRequestException("Get all evaluation report detail failed!");
            }
        }
    }
}