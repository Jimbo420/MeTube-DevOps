using System;
using MeTube_DevOps.UserService.DTO;
using MeTube_DevOps.UserService.Entities;
using AutoMapper;


namespace MeTube_DevOps.UserService.UserProfile
{

  public class UserProfile : Profile
  {
    public UserProfile()
    {
      // Map from CreateUserDto to User
      CreateMap<CreateUserDto, User>()
    .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));


      // Map DTO to Entity
      CreateMap<User, CreateUserDto>()
          .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
          .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

      CreateMap<UserDto, User>()
      .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
      .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
      .ForMember(dest => dest.Password, opt => opt.Ignore());

    }

  }
}