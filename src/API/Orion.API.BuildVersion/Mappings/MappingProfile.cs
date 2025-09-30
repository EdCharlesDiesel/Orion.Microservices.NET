using AutoMapper;
using Orion.Domain.DTO;
using Orion.Domain.DTOs;

namespace Orion.API.BuildVersion.Mappings;

public class MappingProfile : Profile
{ public MappingProfile()
    {
        CreateMap<BuildVersionDto, DataAccess.Postgres.Entities.BuildVersion>().ReverseMap();
    }
}