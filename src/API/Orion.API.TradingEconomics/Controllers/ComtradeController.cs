// using Microsoft.AspNetCore.Mvc;
// using Orion.API.TradingEconomics.Interfaces;
// using Orion.DataAccess.Postgres.Repositories;
// using Orion.Domain.IRepositories;
//
// namespace Orion.API.TradingEconomics.Controllers;
//
// [ApiController]
// [Route("api/[controller]")]
// public class ComtradeController : ControllerBase
// {
//     private readonly TradingEconomicsService _service;
//
//     public ComtradeController(TradingEconomicsService service)
//     {
//         _service = service;
//     }
//
//     public ComtradeController(IComtradeServices serviceMockObject)
//     {
//         throw new NotImplementedException();
//     }
//
//     /// <summary>Get all comtrade categories</summary>
//     [HttpGet("categories")]
//     public async Task<IActionResult> GetComCategories()
//     {
//         var result = await _service.GetCategories();
//         return Ok(result);
//     }
//
//     /// <summary>Get all comtrade countries</summary>
//     [HttpGet("countries")]
//     public async Task<IActionResult> GetComCountries()
//     {
//         var result = await _service.GetCountries();
//         return Ok(result);
//     }
//
//     /// <summary>Get comtrade by country and page</summary>
//     [HttpGet("country/{country}/{pageNumber}")]
//     public async Task<IActionResult> GetComCountryPage(string country, int pageNumber)
//     {
//         var result = await _service.GetByCountryAndPage(country, pageNumber);
//         return Ok(result);
//     }
//
//     /// <summary>Get comtrade between two countries with pagination</summary>
//     [HttpGet("country/{country1}/{country2}/{pageNumber}")]
//     public async Task<IActionResult> GetComBetweenCountries(string country1, string country2, int pageNumber)
//     {
//         var result = await _service.GetBetweenCountries(country1, country2, pageNumber);
//         return Ok(result);
//     }
//
//     /// <summary>Get historical comtrade data by symbol</summary>
//     [HttpGet("historical/{symbol}")]
//     public async Task<IActionResult> GetComHistorical(string symbol)
//     {
//         var result = await _service.GetHistorical(symbol);
//         return Ok(result);
//     }
// }