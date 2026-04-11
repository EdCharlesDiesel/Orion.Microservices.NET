using AutoMapper;
using Orion.API.UserProfile.DTO;

namespace Orion.API.UserProfile.Mappings;

public class UserProfileProfile : Profile
{

        public UserProfileProfile()
        {
            CreateMap<Domain.IRepositories.UserProfile, UserProfileDto>().ReverseMap();
        }
}