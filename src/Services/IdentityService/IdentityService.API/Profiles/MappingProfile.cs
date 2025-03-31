using AutoMapper;
using IdentityService.API.DTOs;
using IdentityService.API.Models;

namespace IdentityService.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            // reverse mapping
            CreateMap<UserDto, ApplicationUser>();
        }
    }
}
