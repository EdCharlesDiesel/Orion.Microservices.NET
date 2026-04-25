using System.Net;
using Microsoft.AspNetCore.Mvc;
using Orion.API.TradingEconomics.ActionFilters;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LatestController : ControllerBase
    {
        private readonly ILatestService _latestService;

        public LatestController(ILatestService latestService)
        {
            _latestService = latestService;
        }

        /// <summary>
        /// Get the latest news from Trading Economics
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLatestUpdates()
        {
            var result = await _latestService.GetLatestUpdatesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get latest updates since a specific date
        /// </summary>
        [HttpGet("{date:datetime}", Name = "GetLatestUpdatesByDate")]
        [CheckShowStatisticsHeader]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetLatestUpdatesByDate(DateTime date)
        {
            var result = await _latestService.GetLatestUpdatesByDateAsync(date);

            if (string.IsNullOrWhiteSpace(result))
                return NotFound();

            return Ok(result);
        }
    }
}