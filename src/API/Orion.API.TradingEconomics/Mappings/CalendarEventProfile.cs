using AutoMapper;
using Orion.API.TradingEconomics.DTO;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Entities.TradingEconomics;

namespace Orion.API.TradingEconomics.Mappings;

public class CalendarEventProfile : Profile
{

        public CalendarEventProfile()
        {
            CreateMap<TradingEconomicsCalendar, TradingEconomicsCalendarDto>().ReverseMap();
        }
}