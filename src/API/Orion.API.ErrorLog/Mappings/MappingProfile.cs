using AutoMapper;
using Orion.Domain.DTO;

namespace Orion.API.ErrorLog.Mappings;

public class MappingProfile : Profile
{ public MappingProfile()
    {
        CreateMap<ErrorLogDto, DataAccess.Postgres.Entities.ErrorLog>().ReverseMap();
    }
}