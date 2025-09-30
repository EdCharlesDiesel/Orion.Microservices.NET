using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Orion.API.HumanResources.Business;
using Orion.API.HumanResources.MapperProfiles;
using Orion.API.HumanResources.Models;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.Domain.DTO;

namespace Orion.API.HumanResources.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InternalEmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeRepository _employee;
        private readonly IMapper _mapper;
        
        public InternalEmployeesController(IEmployeeService employeeService, 
            IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrionCalendarEventDto>>> GetCalendars()
        {
            //TODO: Need to fix this the logic has not yet made sense.
            var employeeID = 1;
            var calendars = await _employeeService.FetchOrionCalendarEventsAsync(1);
            
            // Manual mapping
            var dtos = calendars.Select(e => new OrionCalendarEventDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Salary = e.Salary,
                SuggestedBonus = e.SuggestedBonus,
                YearsInService = e.YearsInService
            });
            
            return Ok(dtos);
        }

        
        [HttpGet("{employeeId}", Name = "GetCalendar")]
        public async Task<ActionResult<OrionCalendarEventDto>> GetCalendar(
            int? employeeId)
        {
            if (!employeeId.HasValue)
            { 
                return NotFound(); 
            }
        
            var Calendar = await _employeeService.FetchOrionCalendarEventAsync(employeeId);
            if (Calendar == null)
            { 
                return NotFound();
            }             
        
            return Ok(_mapper.Map<OrionCalendarEventDto>(Calendar));
        }
        
        
        [HttpPost]
        public async Task<ActionResult<OrionCalendarEventDto>> CreateCalendar(
            CalendarForCreationDto CalendarForCreation)
        { 
            // create an internal employee entity with default values filled out
            // and the values inputted via the POST request
            var Calendar =
                    await _employeeService.CreateCalendarAsync(
                        CalendarForCreation.FirstName, CalendarForCreation.LastName,CalendarForCreation.Company,CalendarForCreation.EmployeeNumber);
        
            // persist it
            await _employeeService.CreateOrionCalendarEventAsync(Calendar);
        
            // return created employee after mapping to a DTO
            return CreatedAtAction("GetCalendar",
                _mapper.Map<OrionCalendarEventDto>(Calendar),
                new { employeeId = Calendar.Id } );
        }
    }

    public class CalendarForCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string EmployeeNumber { get; set; }
    }
}
