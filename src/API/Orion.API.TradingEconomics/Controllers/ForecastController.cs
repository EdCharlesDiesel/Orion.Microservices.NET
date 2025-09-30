// using System.Text.Json;
// using Microsoft.AspNetCore.Mvc;
// using Orion.Domain.IRepositories;
//
// namespace Orion.API.TradingEconomics.Controllers;
//
//     [ApiController]
//     [Route("api/[controller]")]
//     public class ForecastController(IForecastServices service) : ControllerBase
//     {
//
//         /// <summary>
//         /// Get all comtrade categories
//         /// </summary>
//         [HttpGet]
//         public async Task<IActionResult> GetAllForecasts()
//         {
//             var result = await service.GetForecasts();
//
//             List<Forecast>? forecasts;
//             try
//             {
//                 forecasts = JsonSerializer.Deserialize<List<Forecast>>(result, new JsonSerializerOptions
//                 {
//                     PropertyNameCaseInsensitive = true
//                 });
//
//                 if (forecasts == null || !forecasts.Any())
//                     return BadRequest("No calendar events found in the JSON.");
//             }
//             catch (JsonException ex)
//             {
//                 return BadRequest($"JSON deserialization error: {ex.Message}");
//             }
//
//             await service.Create(forecasts);
//
//             return Ok(result);
//         }
//
//         // GET: api/calendar/daterange?startDate=2025-07-01&endDate=2025-07-31
//         [HttpGet("daterange")]
//         public async Task<IActionResult> GetEventsByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
//         {
//             var result = await service.GetForecastsByDate(startDate, endDate);
//             return Ok(result);
//         }
//
//         // GET: api/calendar/countries?names=South Africa,USA
//         [HttpGet("countries")]
//         public async Task<IActionResult> GetEventsByCountries([FromQuery] string[] names)
//         {
//             var result = await service.GetForecastsByCountries(names);
//             return Ok(result);
//         }
//
//         // GET: api/calendar/countriesdaterange?startDate=2025-07-01&endDate=2025-07-31&names=USA,Germany
//         [HttpGet("countriesdaterange")]
//         public async Task<IActionResult> GetEventsByCountriesAndDates([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string[] names)
//         {
//             var result = await service.GetForecastsByCountriesAndDates(startDate, endDate, names);
//             return Ok(result);
//         }
//
//         // GET: api/calendar/indicators?names=GDP,Inflation
//         [HttpGet("indicators")]
//         public async Task<IActionResult> GetEventsByIndicators([FromQuery] string[] names)
//         {
//             var result = await service.GetForecastsByIndicator(names);
//             return Ok(result);
//         }
//
//     }
