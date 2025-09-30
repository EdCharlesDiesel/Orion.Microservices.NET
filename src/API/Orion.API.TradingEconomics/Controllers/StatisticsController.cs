// using AutoMapper;
// using Microsoft.AspNetCore.Http.Features;
// using Microsoft.AspNetCore.Mvc;
// using Orion.API.TradingEconomics.ActionFilters;
// using Orion.API.TradingEconomics.DTO;
//
// namespace Orion.API.TradingEconomics.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class StatisticsController : ControllerBase
//     {
//         private readonly IMapper _mapper;
//         private readonly HttpClient _httpClient;
//
//         public StatisticsController(IMapper mapper, IHttpClientFactory httpClientFactory)
//         {
//             _mapper = mapper;
//             _httpClient = httpClientFactory.CreateClient();
//         }
//
//         // Example using AutoMapper
//         [HttpGet("connection")]
//         [CheckShowStatisticsHeader]
//         public ActionResult<StatisticsDto> GetStatistics()
//         {
//             var httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();
//             return Ok(_mapper.Map<StatisticsDto>(httpConnectionFeature));
//         }
//
//         // Call TradingEconomics API
//         [HttpGet("trade/country/{country}")]
//         public async Task<ActionResult<string>> GetTradeByCountry(string country = "mexico")
//         {
//             var url = $"https://api.tradingeconomics.com/forecast/country/{country}?c=guest:guest";
//
//             var request = new HttpRequestMessage(HttpMethod.Get, url);
//             request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
//
//             var response = await _httpClient.SendAsync(request);
//
//             if (!response.IsSuccessStatusCode)
//             {
//                 return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
//             }
//
//             var content = await response.Content.ReadAsStringAsync();
//             return Ok(content); // returns JSON string
//         }
//     }
// }