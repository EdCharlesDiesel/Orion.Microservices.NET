using AutoMapper;
using Orion.Domain.DTO;

namespace Orion.API.TransactionHistoryArchive.Mappings;

public class MappingProfile : Profile
{ public MappingProfile()
    {
        CreateMap<TransactionHistoryArchiveDto, DataAccess.Postgres.Entities.TransactionHistoryArchive>().ReverseMap();
    }
}