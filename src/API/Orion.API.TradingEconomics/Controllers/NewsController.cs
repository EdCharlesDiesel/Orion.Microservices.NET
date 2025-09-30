// using Microsoft.AspNetCore.Mvc;
// using Orion.DataAccess.Postgres.Repositories;
//
// namespace Orion.API.TradingEconomics.Controllers;
// [ApiController]
// [Route("api/[controller]")]
// public class NewsController(TradingEconomicsService service) : ControllerBase
// {
//     [HttpGet("latest")]
//     public async Task<IActionResult> GetLatestNews()
//     {
//         var result = await service.GetLatestNewsAsync();
//         return Ok(result);
//     }
//
//     [HttpGet("by-country")]
//     public async Task<IActionResult> GetNewsByCountry([FromQuery] string[] countries)
//     {
//         var result = await service.GetNewsByCountryAsync(countries);
//         return Ok(result);
//     }
//
//     [HttpGet("by-indicator")]
//     public async Task<IActionResult> GetNewsByIndicator([FromQuery] string[] indicators)
//     {
//         var result = await service.GetNewsByIndicatorAsync(indicators);
//         return Ok(result);
//     }
// }