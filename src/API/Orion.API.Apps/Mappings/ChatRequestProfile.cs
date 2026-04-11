using AutoMapper;
using Orion.DataAccess.Postgres.Entities.Common;
using Orion.Domain.DTO;

namespace Orion.API.Apps.Mappings;

public class ChatRequestProfile : Profile
{
    public ChatRequestProfile()
    {
        CreateMap<ChatRequest, ChatRequestDto>().ReverseMap();
    }
}