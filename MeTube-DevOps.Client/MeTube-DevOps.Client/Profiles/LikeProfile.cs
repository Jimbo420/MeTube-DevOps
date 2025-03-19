using AutoMapper;
using MeTube_DevOps.Client.Models;
using MeTube_DevOps.Client.DTO.LikeDTOs;

namespace MeTube.Client.Profiles
{
    public class LikeProfile : Profile
    {
        public LikeProfile()
        {
            CreateMap<LikeDto, Like>();
            CreateMap<Like, LikeDto>();
        }
    }
}
