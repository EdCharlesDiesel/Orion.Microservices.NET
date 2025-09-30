using AutoMapper;
using Orion.DataAccess.Postgres.Entities;
using Orion.Domain.DTO;

namespace Orion.API.Sales.MapperProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        { 
            CreateMap<OrionCalendarEvent, OrionCalendarEventDto>(); 
        }
    }


}
