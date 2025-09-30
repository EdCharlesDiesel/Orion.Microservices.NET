using System.Globalization;
using AutoMapper;
using Orion.API.HumanResources.Models;
using Orion.DataAccess.Postgres.Entities;
using Orion.Domain.DTO;

namespace Orion.API.HumanResources.MapperProfiles
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
