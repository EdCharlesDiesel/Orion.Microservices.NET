using AutoMapper;
using Orion.Domain.DTO;
using Orion.Domain.DTOs;

namespace Orion.API.AWBuildVersion.Mappings;

public class MappingProfile : Profile
{ public MappingProfile()
    {
        CreateMap<AwBuildVersionDto, DataAccess.Postgres.Entities.AWBuildVersion>().ReverseMap();
    }
}