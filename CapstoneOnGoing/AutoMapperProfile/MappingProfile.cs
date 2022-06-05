using AutoMapper;
using Models.Dtos;
using Models.Models;

namespace CapstoneOnGoing.AutoMapperProfile
{
    public class MappingProfile :Profile
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
        }
    }
}
