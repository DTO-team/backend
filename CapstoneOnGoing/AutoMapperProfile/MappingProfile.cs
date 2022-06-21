using System;
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
                .ForMember(dest => dest.Status, src => src.MapFrom(src => src.StatusId.Equals(1) ? UserStatus.Activated.ToString().ToUpper() : UserStatus.Inactivated.ToString().ToUpper()))
                .ForMember(dest => dest.AvatarUrl, src => src.MapFrom(src => src.AvatarUrl))
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
            CreateMap<UpdateUserInAdminRequest, User>()
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role));
            CreateMap<CreateNewSemesterDTO, Semester>();
            CreateMap<Semester, GetSemesterDTO>();
            CreateMap<User, UserByIdDTO>().ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
            CreateMap<User, LoginUserStudentResponse>()
                .ForMember(dest => dest.StudentCode, src => src.MapFrom(src => src.Student.Code))
                .ForMember(dest => dest.Semester,
                    src => src.MapFrom(src =>
                        (src.Student.Semester.Year.ToString() + " - " + src.Student.Semester.Season.ToString())))
                .ForMember(dest => dest.AccessToken, src => src.Ignore())
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
            CreateMap<User, LoginUserLecturerResponse>()
                .ForMember(dest => dest.AccessToken, src => src.Ignore())
                .ForMember(dest => dest.DepartmentName,
                    src => src.MapFrom(src => src.Lecturer.Department.Name.ToString()))
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
            ;
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
                .ForMember(dest => dest.Department, src => src.MapFrom(src => src.Department.Name));

            CreateMap<User, GetLecturerResponse>()
                .ForMember(dest => dest.Department, src => src.MapFrom(src => src.Lecturer.Department.Name))
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));

            CreateMap<Student, StudentResponse>()
                .ForMember(dest => dest.Semester,
                    src => src.MapFrom(src => $"{src.Semester.Year.ToString()} - {src.Semester.Season.ToString()}"));

            CreateMap<User, StudentResponse>()
                .ForMember(dest => dest.Status, src => src.MapFrom(src => src.StatusId.Equals(1) ? UserStatus.Activated.ToString().ToUpper() : UserStatus.Inactivated.ToString().ToUpper()))
                .ForMember(dest => dest.TeamId, src=> src.MapFrom(src => (src.Student.TeamStudents != null) ? src.Student.TeamStudents.FirstOrDefault().TeamId.ToString() : ""))
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
                .ForMember(dest => dest.Code, src => src.MapFrom(src => src.Student.Code))
                .ForMember(dest => dest.Semester, src => src.MapFrom(src => src.Student.Semester.Season));

            CreateMap<UpdateStudentRequest, Student>()
                .ForMember(dest => dest.Code, src => src.MapFrom(src => src.Code));

            CreateMap<User, LoginUserAdminResponse>()
                .ForMember(dest => dest.AccessToken, src => src.Ignore())
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));

            CreateMap<ImportTopicsRequest, Topic>();

            CreateMap<Team, GetTeamResponse>()
	            .ForMember(dest => dest.TeamId, src => src.MapFrom(src => src.Id))
	            .ForMember(dest => dest.TeamName, src => src.MapFrom(src => src.Name))
	            .ForMember(dest => dest.Leader, src => src.Ignore());
            CreateMap<User, Member>()
	            .ForMember(dest => dest.Email, src => src.MapFrom(src => src.Email))
	            .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FullName))
	            .ForMember(dest => dest.Id, src => src.MapFrom(src => src.Id))
	            .ForMember(dest => dest.AvatarUrl, src => src.MapFrom(src => src.AvatarUrl))
	            .ForMember(dest => dest.Semester, src => src.MapFrom(src => src.Student.Semester.Year + "-" + src.Student.Semester.Season))
	            .ForMember(dest => dest.Status, src => src.MapFrom(src => src.StatusId))
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
            CreateMap<Team, GetTeamDetailResponse>()
	            .IncludeBase<Team,GetTeamResponse>();
            CreateMap<Topic, GetTopicsDTO>()
                .ForMember(dest => dest.TopicId, src => src.MapFrom(src => src.Id))
	            .ForMember(dest => dest.LecturerIds, src => src.MapFrom(src => src.TopicLecturers.Select(src => src.LecturerId)))
	            .ForMember(dest => dest.CompanyId, src => src.MapFrom(src => src.CompanyId));
            CreateMap<User, GetLecturerDTO>()
	            .ForMember(dest => dest.Department,src => src.MapFrom(src => src.Lecturer.Department.Name))
	            .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
            CreateMap<User, GetCompanyDTO>()
	            .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
            CreateMap<GetLecturerDTO, GetLecturerResponse>();
            CreateMap<GetCompanyDTO, GetCompanyResponse>();
            CreateMap<GetTopicsDTO, GetTopicsResponse>()
                .ForMember(dest => dest.TopicId, src => src.MapFrom(src => src.TopicId))
	            .ForMember(dest => dest.LecturersDetails, src => src.MapFrom(src => src.LecturerDtos))
	            .ForMember(dest => dest.CompanyDetail, src => src.MapFrom(src => src.CompanyDto));

            CreateMap<StudentRequest, User>();
        }
    }
}