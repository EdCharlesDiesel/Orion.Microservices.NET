// using Microsoft.AspNetCore.Mvc;
// using Orion.DataAccess.Postgres.Repositories;
//
// namespace Orion.API.TradingEconomics.Controllers;
// [ApiController]
// [Route("api/[controller]")]
//
// public class MarketsIntradayController(TradingEconomicsService service) : ControllerBase
// {
//     /// <summary>
//         /// Get intraday prices for a single market symbol.
//         /// </summary>
//         [HttpGet("intraday/{symbol}")]
//         public async Task<IActionResult> GetIntradaySymbol(string symbol)
//         {
//             var result = await service.GetIntradaySymbolAsync(symbol);
//             return Ok(result);
//         }
//
//         /// <summary>
//         /// Get intraday prices by symbol and start date (hour).
//         /// </summary>
//         [HttpGet("intraday-datehour")]
//         public async Task<IActionResult> GetIntradayDateHour([FromQuery] string symbol, [FromQuery] DateTime startDate)
//         {
//             var result = await service.GetIntradayDateHourAsync(symbol, startDate);
//             return Ok(result);
//         }
//
//         /// <summary>
//         /// Get intraday data between two dates.
//         /// </summary>
//         [HttpGet("intraday-dates")]
//         public async Task<IActionResult> GetIntradaySymbolDates([FromQuery] string symbol, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
//         {
//             var result = await service.GetIntradaySymbolDatesAsync(symbol, startDate, endDate);
//             return Ok(result);
//         }
//     }
//
