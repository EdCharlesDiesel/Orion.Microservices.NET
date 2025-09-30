// using System.Text.Json;
// using Microsoft.AspNetCore.Mvc;
// using Orion.API.TradingEconomics.Interfaces;
// using Orion.DataAccess.Postgres.Entities.Common;
//
// namespace Orion.API.TradingEconomics.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class CalendarController(ICalendarServices service) : ControllerBase
//     {
//         /// <summary>
//         /// Get all calendar events
//         /// </summary>
//         [HttpGet]
//         public async Task<IActionResult> GetAllEvents()
//         {
//             string result = await service.GetCalendarEvents();
//
//             List<TradingEconomicsCalendar>? calendarEvents;
//             try
//             {
//                 calendarEvents = JsonSerializer.Deserialize<List<TradingEconomicsCalendar>>(result, new JsonSerializerOptions
//                 {
//                     PropertyNameCaseInsensitive = true
//                 });
//
//                 if (calendarEvents == null || !calendarEvents.Any())
//                     return BadRequest("No calendar events found in the JSON.");
//             }
//             catch (JsonException ex)
//             {
//                 return BadRequest($"JSON deserialization error: {ex.Message}");
//             }
//
//             await service.Create(calendarEvents);
//             return Ok(result);
//         }
//
//         /// <summary>
//         /// Get calendar events by date range
//         /// </summary>
//         [HttpGet("daterange")]
//         public async Task<IActionResult> GetEventsByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
//         {
//             var result = await service.GetCalendarEventsByDate(startDate, endDate);
//             return Ok(result);
//         }
//
//         /// <summary>
//         /// Get calendar events by country names
//         /// </summary>
//         [HttpGet("countries")]
//         public async Task<IActionResult> GetEventsByCountries([FromQuery] string[] names)
//         {
//             var result = await service.GetCalendarEventsByCountries(names);
//             return Ok(result);
//         }
//
//         /// <summary>
//         /// Get calendar events by countries and date range
//         /// </summary>
//         [HttpGet("countriesdaterange")]
//         public async Task<IActionResult> GetEventsByCountriesAndDates(
//             [FromQuery] DateTime startDate,
//             [FromQuery] DateTime endDate,
//             [FromQuery] string[] names)
//         {
//             var result = await service.GetCalendarEventsByCountriesAndDates(startDate, endDate, names);
//             return Ok(result);
//         }
//
//         /// <summary>
//         /// Get calendar events by indicator names
//         /// </summary>
//         [HttpGet("indicators")]
//         public async Task<IActionResult> GetEventsByIndicators([FromQuery] string[] names)
//         {
//             var result = await service.GetCalendarEventsByIndicators(names);
//             return Ok(result);
//         }
//     }
//
//
// }
