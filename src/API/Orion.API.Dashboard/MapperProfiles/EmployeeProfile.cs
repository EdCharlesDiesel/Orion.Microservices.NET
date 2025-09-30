using AutoMapper;
using Orion.API.Dashboard.Models;
using Orion.DataAccess.Postgres.Entities;
using Orion.Domain.DTO;

namespace Orion.API.Dashboard.MapperProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        { 
            CreateMap<OrionCalendarEvent, OrionCalendarEventDto>(); 
            CreateMap<Employee, InternalEmployeeDto>(); 
        }
    }


}
