﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.AutoMapperProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Config Mapping in here
            CreateMap<CreateNewUserRequest, User>();
            CreateMap<User, UserInAdminDTO>()
                .ForMember(dest => dest.Status, src => src.MapFrom(src => new UserStatusResponse() { StatusId = src.StatusId, StatusName = src.StatusId.Equals(1) ? UserStatus.Activated.ToString().ToUpper() : UserStatus.Inactivated.ToString().ToUpper() }))
                .ForMember(dest => dest.AvatarUrl, src => src.MapFrom(src => src.AvatarUrl))
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
            CreateMap<UpdateUserInAdminRequest, User>()
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role));
            CreateMap<CreateNewSemesterDTO, Semester>();
            CreateMap<Semester, GetSemesterDTO>();
            CreateMap<User, UserByIdDTO>()
                .ForMember(dest => dest.AvatarUrl, src => src.MapFrom(src=> src.AvatarUrl))
                .ForMember(dest => dest.Status, src => src.MapFrom(src => new UserStatusResponse() { StatusId = src.StatusId, StatusName = src.StatusId.Equals(1) ? UserStatus.Activated.ToString().ToUpper() : UserStatus.Inactivated.ToString().ToUpper() }))
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
           
            CreateMap<User, LoginUserStudentResponse>()
                .ForMember(dest => dest.StudentCode, src => src.MapFrom(src => src.Student.Code))
                .ForMember(dest => dest.Semester,
                    src => src.MapFrom(src =>
                        (src.Student.Semester.Year.ToString() + " - " + src.Student.Semester.Season.ToString())))
                .ForMember(dest => dest.AccessToken, src => src.Ignore())
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));

            CreateMap<User, LoginUserLecturerResponse>()
                .ForMember(dest => dest.AccessToken, src => src.Ignore())
                .ForMember(dest => dest.Department, src => src.MapFrom(src => src.Lecturer.Department))
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));

            CreateMap<User, LoginUserCompanyResponse>()
                .ForMember(dest => dest.AccessToken, src => src.Ignore());

            CreateMap<InProgressStudentsRequest, User>()
                .ForMember(dest => dest.Email, src => src.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FullName))
                .ForPath(dest => dest.Student.Code, src => src.MapFrom(src => src.StudentCode));

            CreateMap<User, LoginUserAdminResponse>()
                .ForMember(dest => dest.AccessToken, src => src.Ignore())
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));

            //===============================================================================
            CreateMap<LecturerResquest, CreateNewUserRequest>();

            CreateMap<LecturerResquest, Lecturer>();

            CreateMap<UpdateLecturerRequest, User>();

            CreateMap<Lecturer, GetLecturerResponse>()
                .ForMember(dest => dest.Department, src => src.MapFrom(src => src.Department));

            CreateMap<User, GetLecturerResponse>()
                .ForMember(dest => dest.Status, src => src.MapFrom(src => new UserStatusResponse() { StatusId = src.StatusId, StatusName = src.StatusId.Equals(1) ? UserStatus.Activated.ToString().ToUpper() : UserStatus.Inactivated.ToString().ToUpper() }))
                .ForMember(dest => dest.Department, src => src.MapFrom(src => src.Lecturer.Department))
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));

            CreateMap<Student, StudentResponse>()
                .ForMember(dest => dest.Semester,
                    src => src.MapFrom(src => $"{src.Semester.Year.ToString()} - {src.Semester.Season.ToString()}"));

            CreateMap<User, StudentResponse>()
                .ForMember(dest => dest.Status, src => src.MapFrom(src => new UserStatusResponse() { StatusId = src.StatusId, StatusName = src.StatusId.Equals(1) ? UserStatus.Activated.ToString().ToUpper() : UserStatus.Inactivated.ToString().ToUpper() }))
                .ForMember(dest => dest.TeamId, src=> src.MapFrom(src => (src.Student.TeamStudents != null) ? src.Student.TeamStudents.FirstOrDefault().TeamId : Guid.Empty))
                .ForMember(dest => dest.Semester, src => src.MapFrom(src => $"{src.Student.Semester.Year.ToString()} - {src.Student.Semester.Season.ToString()}"))
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.Code, src => src.MapFrom(src => src.Student.Code));
            CreateMap<LecturerResquest, User>()
                .ForMember(dest => dest.Email, src => src.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, src => src.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FullName))
                .ForMember(dest => dest.RoleId, src => src.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.StatusId, src => src.MapFrom(src => src.StatusId));

            CreateMap<StudentRequest, Student>()
                .ForMember(dest => dest.SemesterId, src => src.MapFrom(src => src.SemesterId));

            CreateMap<UpdateStudentRequest, User>();

            CreateMap<User, StudentUpdateResponse>()
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.Code, src => src.MapFrom(src => src.Student.Code))
                .ForMember(dest => dest.Semester, src => src.MapFrom(src => src.Student.Semester.Season))
                .ForMember(dest => dest.Status,
                    src => src.MapFrom(src => new UserStatusResponse()
                    {
                        StatusId = src.StatusId,
                        StatusName = src.StatusId.Equals((int)UserStatus.Activated)
                            ? UserStatus.Activated.ToString().ToUpper()
                            : UserStatus.Inactivated.ToString().ToUpper()
                    }));

            CreateMap<UpdateStudentRequest, Student>();

            CreateMap<User, LoginUserAdminResponse>()
                .ForMember(dest => dest.AccessToken, src => src.Ignore())
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));

            CreateMap<ImportTopicsRequest, Topic>();

            CreateMap<Team, GetTeamResponse>()
	            .ForMember(dest => dest.TeamId, src => src.MapFrom(src => src.Id))
	            .ForMember(dest => dest.TeamName, src => src.MapFrom(src => src.Name))
	            .ForMember(dest => dest.Leader, src => src.Ignore());
            CreateMap<User, Member>()
                .ForMember(dest => dest.Status, src => src.MapFrom(src => new UserStatusResponse() { StatusId = src.StatusId, StatusName = src.StatusId.Equals(1) ? UserStatus.Activated.ToString().ToUpper() : UserStatus.Inactivated.ToString().ToUpper() }))
                .ForMember(dest => dest.Email, src => src.MapFrom(src => src.Email))
	            .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FullName))
	            .ForMember(dest => dest.Id, src => src.MapFrom(src => src.Id))
	            .ForMember(dest => dest.AvatarUrl, src => src.MapFrom(src => src.AvatarUrl))
	            .ForMember(dest => dest.Semester, src => src.MapFrom(src => src.Student.Semester.Year + "-" + src.Student.Semester.Season))
	            .ForMember(dest => dest.Code, src => src.MapFrom(src => src.Student.Code))
	            .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));

            CreateMap<Application, GetApplicationDTO>()
                .ForMember(dest => dest.ApplicationId, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.TeamInformation,
                    src => src.MapFrom(src => new ApplicationFields() {TeamId = src.TeamId ,TeamName = src.Team.Name, TeamLeaderId = src.Team.TeamLeaderId, TeamSemesterId = (Guid)src.Team.SemesterId }))
                .ForMember(dest => dest.Topic,
                    src => src.MapFrom(src => new TopicFields() { TopicId = src.TopicId, Description = src.Topic.Description }))
                .ForMember(dest => dest.Status,
                    src => src.MapFrom(src => src.StatusId));
            CreateMap<User, CreatedTeamResponse>()
                .ForMember(dest => dest.TeamLeaderEmail, src => src.MapFrom(src => src.Email))
                .ForMember(dest => dest.TeamLeaderName, src => src.MapFrom(src => src.FullName));

            CreateMap<Team, CreatedTeamResponse>()
                .ForMember(dest => dest.TeamId, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.JoinCode, src => src.MapFrom(src => src.JoinCode));

            CreateMap<Team, GetTeamDetailResponse>()
	            .IncludeBase<Team,GetTeamResponse>();
            CreateMap<Topic, GetTopicsDTO>()
                .ForMember(dest => dest.TopicId, src => src.MapFrom(src => src.Id))
	            .ForMember(dest => dest.LecturerIds, src => src.MapFrom(src => src.TopicLecturers.Select(src => src.LecturerId)))
	            .ForMember(dest => dest.CompanyId, src => src.MapFrom(src => src.CompanyId));
            CreateMap<Department,GetDepartmentDTO>();
            CreateMap<GetDepartmentDTO, GetDepartmentResponse>();
            CreateMap<User, GetLecturerDTO>()
                .ForMember(dest => dest.Department, src => src.MapFrom(src => src.Lecturer.Department))
	            .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
            CreateMap<User, GetCompanyDTO>()
	            .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
            CreateMap<GetLecturerDTO, GetLecturerResponse>()
                .ForMember(dest => dest.Department, src => src.MapFrom(src => src.Department))
                .ForMember(dest => dest.Status,
                    src => src.MapFrom(src => new UserStatusResponse()
                    {
                        StatusId = src.StatusId,
                        StatusName = src.StatusId.Equals((int)UserStatus.Activated)
                            ? UserStatus.Activated.ToString().ToUpper()
                            : UserStatus.Inactivated.ToString().ToUpper()
                    }));


            CreateMap<GetCompanyDTO, GetCompanyResponse>()
                .ForMember(dest => dest.Status,
                    src => src.MapFrom(src => new UserStatusResponse()
                    {
                        StatusId = src.StatusId,
                        StatusName = src.StatusId.Equals((int)UserStatus.Activated)
                            ? UserStatus.Activated.ToString().ToUpper()
                            : UserStatus.Inactivated.ToString().ToUpper()
                    })); ;

            CreateMap<User, GetUserResponse>()
                .ForMember(dest => dest.Status,
                    src => src.MapFrom(src => new UserStatusResponse()
                    {
                        StatusId = src.StatusId,
                        StatusName = src.StatusId.Equals((int)UserStatus.Activated)
                            ? UserStatus.Activated.ToString().ToUpper()
                            : UserStatus.Inactivated.ToString().ToUpper()
                    }));
            CreateMap<GetTeamDetailResponse, Member>();
            CreateMap<GetTopicsDTO, GetTopicsResponse>()
                .ForMember(dest => dest.TopicName, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.TopicId, src => src.MapFrom(src => src.TopicId))
	            .ForMember(dest => dest.LecturersDetails, src => src.MapFrom(src => src.LecturerDtos))
	            .ForMember(dest => dest.CompanyDetail, src => src.MapFrom(src => src.CompanyDto));

            CreateMap<StudentRequest, User>();
            CreateMap<Department,GetDepartmentResponse>();
            CreateMap<CreateWeeklyReportRequest, CreateWeeklyReportDTO>();
            CreateMap<ReportEvidenceRequest, ReportEvidenceDTO>();
            CreateMap<Report, GetTeamWeeklyReportResponse>()
	            .ForMember(dest => dest.Week, src => src.MapFrom(src => src.Week))
	            .ForMember(dest => dest.ReportEvidences, src => src.MapFrom(src => src.ReportEvidences))
                .ForMember(dest => dest.Feedback, src => src.Ignore())
                .ForMember(dest => dest.Reporter, src => src.Ignore());
            CreateMap<Week, GetWeekResponse>();
            CreateMap<ReportEvidence, GetTeamWeeklyReportsEvidenceResponse>();
            CreateMap<Report, GetWeeklyReportDetailResponse>()
	            .ForMember(dest => dest.Feedbacks, src => src.MapFrom(src => src.Feedbacks))
	            .ForMember(dest => dest.ReportsEvidences, src => src.MapFrom(src => src.ReportEvidences));
            CreateMap<Feedback, GetFeedbackDTO>();

            CreateMap<Grade, GradeDTO>();
            CreateMap<Question, QuestionDTO>();

            CreateMap<Criterion, CriteriaDTO>();

            CreateMap<CriterionGradeRequest, Grade>()
                .ForMember(dest => dest.Level, src => src.MapFrom(src => src.Level.ToUpper()));
            CreateMap<CriterionQuestionRequest, Question>()
                .ForMember(dest => dest.Priority, src => src.MapFrom(src => src.Priority.ToUpper()));

            CreateMap<CreateCriteriaRequest, Criterion>()
                .ForMember(dest => dest.Code, src => src.MapFrom(src => src.Code.ToUpper()))
                .ForMember(dest => dest.Grades, src => src.Ignore())
                .ForMember(dest => dest.Questions, src => src.Ignore());

            CreateMap<Grade, UpdateCriteriaGradeRequest>();

            CreateMap<UpdateCriteriaGradeRequest, Grade>()
                .ForMember(dest => dest.Level, src => src.MapFrom(src=> src.Level.ToUpper())); 
            CreateMap<UpdateCriteriaQuestionRequest, Question>()
                .ForMember(dest => dest.Priority, src => src.MapFrom(src => src.Priority.ToUpper()));
            CreateMap<UpdateCriteriaRequest, Criterion>()
                .ForMember(dest => dest.Grades, src => src.Ignore())
                .ForMember(dest => dest.Questions, src => src.Ignore());

            CreateMap<CreateNewCriteriaGradeRequest, Grade>()
                .ForMember(dest=> dest.Level, src => src.MapFrom(src=> src.Level.ToUpper()))
                .ForMember(dest => dest.Id, src => src.Ignore())
                .ForMember(dest => dest.Criteria, src => src.Ignore());

            CreateMap<CreateNewCriteriaQuestionRequest, Question>()
                .ForMember(dest=> dest.Priority, src=>src.MapFrom(src => src.Priority.ToUpper()))
                .ForMember(dest => dest.Id, src => src.Ignore())
                .ForMember(dest => dest.Criteria, src => src.Ignore());

            CreateMap<SemesterCriterion, GetSemesterCriteriaResponse>();
            CreateMap<EvaluationSession, GetEvaluationSessionResponse>();
            CreateMap<Council, GetCouncilResponse>();

            CreateMap<GetTopicsDTO, GetTopicAllProjectResponse>()
                .ForMember(dest => dest.TopicName, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.TopicId, src => src.MapFrom(src => src.TopicId))
                .ForMember(dest => dest.LecturersDetails, src => src.MapFrom(src => src.LecturerDtos))
                .ForMember(dest => dest.CompanyDetail, src => src.MapFrom(src => src.CompanyDto));

            CreateMap<ReviewGradeRequest, ReviewGrade>()
                .ForMember(dest => dest.Id, src => src.Ignore());
            CreateMap<ReviewQuestionRequest, ReviewQuestion>()
                .ForMember(dest => dest.Id, src => src.Ignore()); ;
            CreateMap<CreateNewReviewRequest, Review>()
                .ForMember(dest => dest.ReviewGrades, src => src.Ignore())
                .ForMember(dest => dest.ReviewQuestions, src => src.Ignore());

            CreateMap<EvaluationReportDetailRequest, EvaluationReportDetail>();
            CreateMap<Council, GetCouncilOfTeamResponse>();
            CreateMap<EvaluationReportDetail, EvaluationReportDetailResponse>();

            CreateMap<GradeCopy, GetGradeCopyResponse>();
            CreateMap<QuestionCopy, GetQuestionCopyResponse>();
            CreateMap<Review, GetCouncilReviewOnProjectResponse>()
                .ForMember(dest => dest.GradeCopyResponses, src=> src.Ignore())
                .ForMember(dest => dest.QuestionCopyResponses, src => src.Ignore());
        }
    }
}