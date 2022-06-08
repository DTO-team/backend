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
			CreateMap<CreateNewUserDTO, User>();
			CreateMap<User, UserInAdminDTO>()
					.ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
			CreateMap<UpdateUserInAdminDTO, User>()
					.ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role));
			CreateMap<CreateNewSemesterDTO, Semester>();
			CreateMap<Semester, GetSemesterDTO>();
			CreateMap<User, UserByIdDTO>().ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.Name));
			CreateMap<User, LoginUserStudentResponse>().ForMember(dest => dest.Status,
				src => src.MapFrom(src => src.StatusId))
				.ForMember(dest => dest.StudentCode, src => src.MapFrom(src => src.Student.Code))
				.ForMember(dest => dest.Semester, src => src.MapFrom(src => (src.Student.Semester.Year.ToString() + " - " + src.Student.Semester.Season.ToString())))
				.ForMember(dest => dest.AccessToken, src => src.Ignore());
			CreateMap<User, LoginUserLecturerResponse>()
				.ForMember(dest => dest.AccessToken, src => src.Ignore())
				.ForMember(dest => dest.DepartmentName, src => src.MapFrom(src => src.Lecturer.Department.Name.ToString()));
			CreateMap<User, LoginUserCompanyResponse>()
				.ForMember(dest => dest.AccessToken, src => src.Ignore());
			CreateMap<InProgressStudentsRequest, User>()
				.ForMember(dest => dest.Email, src => src.MapFrom(src => src.Email))
				.ForMember(dest => dest.FullName, src => src.MapFrom(src => src.FullName))
				.ForPath(dest => dest.Student.Code, src => src.MapFrom(src => src.StudentCode));
			CreateMap<User, LoginUserAdminResponse>()
				.ForMember(dest => dest.AccessToken, src => src.Ignore());
		}
	}
}
