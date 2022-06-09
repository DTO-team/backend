using AutoMapper;
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
					.ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
			CreateMap<UpdateUserInAdminRequest, User>()
					.ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role));
			CreateMap<CreateNewSemesterDTO, Semester>();
			CreateMap<Semester, GetSemesterDTO>();
			CreateMap<User, UserByIdDTO>().ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
			CreateMap<User, LoginUserStudentResponse>()
				.ForMember(dest => dest.StudentCode, src => src.MapFrom(src => src.Student.Code))
				.ForMember(dest => dest.Semester, src => src.MapFrom(src => (src.Student.Semester.Year.ToString() + " - " + src.Student.Semester.Season.ToString())))
				.ForMember(dest => dest.AccessToken, src => src.Ignore())
				.ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
			CreateMap<User, LoginUserLecturerResponse>()
				.ForMember(dest => dest.AccessToken, src => src.Ignore())
				.ForMember(dest => dest.DepartmentName, src => src.MapFrom(src => src.Lecturer.Department.Name.ToString()))
				.ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name)); ;
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

			CreateMap<Lecturer, LecturerResponse>()
				.ForMember(dest => dest.Department, src => src.MapFrom(src => src.Department.Name));

			CreateMap<User, LecturerResponse>()
				.ForMember(dest => dest.Department, src => src.MapFrom(src => src.Lecturer.Department.Name))
				.ForMember(dest => dest.Role, src => src.MapFrom(src=>src.Role.Name));

			CreateMap<Student, StudentResponse>();

			CreateMap<User, StudentResponse>()
				.ForMember(dest => dest.Semester, src => src.MapFrom(src => src.Student.Semester.Season))
				.ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name))
				.ForMember(dest => dest.Code, src => src.MapFrom(src => src.Student.Code));
			CreateMap<LecturerResquest, User>()
				.ForMember(dest => dest.Email, src => src.MapFrom(src => src.Email))
				.ForMember(dest => dest.UserName, src => src.MapFrom(src => src.UserName))
				.ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FullName))
				.ForMember(dest => dest.RoleId, src => src.MapFrom(src => src.RoleId))
				.ForMember(dest => dest.StatusId, src => src.MapFrom(src => src.StatusId));

			CreateMap<StudentRequest, Student>()
				.ForMember(dest=>dest.SemesterId, src => src.MapFrom(src => src.SemesterId));

			CreateMap<UpdateStudentRequest, User>();

			CreateMap<User, StudentUpdateResponse>()
				.ForMember(dest => dest.Code, src => src.MapFrom(src => src.Student.Code))
				.ForMember(dest => dest.Semester, src => src.MapFrom(src => src.Student.Semester.Season));

			CreateMap<UpdateStudentRequest, Student>()
				.ForMember(dest => dest.Code, src => src.MapFrom(src => src.Code));

			CreateMap<User, LoginUserAdminResponse>()
				.ForMember(dest => dest.AccessToken, src => src.Ignore())
				.ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
		}
	}
}
