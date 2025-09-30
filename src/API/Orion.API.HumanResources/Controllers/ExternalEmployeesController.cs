
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.API.HumanResources.Business;
using Orion.API.HumanResources.MapperProfiles;
using Orion.API.HumanResources.Models;
using Orion.Domain.DTO;

namespace Orion.API.HumanResources.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalEmployeesController(IEmployeeService employeeService,
        IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        
        [HttpPost]
        public async Task<ActionResult<OrionCalendarEventDto>> CreateCalendar(
            OrionCalendarEventDto calendarForCreation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        
            // create an internal employee entity with default values filled out
            // and the values inputted via the POST request
            var Calendar =
                    await employeeService.CreateCalendarAsync(
                        calendarForCreation.FirstName, calendarForCreation.LastName,calendarForCreation.Company,calendarForCreation.EmployeeNumber);
        
            // persist it
            await employeeService.CreateOrionCalendarEventAsync(Calendar);
        
            // return created employee after mapping to a DTO
            return CreatedAtAction("GetOrionCalenderEvent",
                _mapper.Map<OrionCalendarEventDto>(Calendar),
                new { employeeId = Calendar.Id });
        }
        
        
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Authorize]
        public IActionResult GetProtectedCalendars()
        {
            // depending on the role, redirect to another action
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(
                    "GetCalendars", "ProtectedCalendars");
            }
        
            return RedirectToAction("GetCalendars", "Calendars");
        }

    }
}
