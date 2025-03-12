using AutoMapper;
using MeTube_DevOps.UserService.DTO;
using MeTube_DevOps.UserService.Entities;

namespace MeTube_DevOps.UserService.UserProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Map from CreateUserDto to User
            CreateMap<CreateUserDto, User>();

            // Map from User to CreateUserDto
            CreateMap<User, CreateUserDto>();

            // Map from UserDto to User
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            // Map from User to UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}