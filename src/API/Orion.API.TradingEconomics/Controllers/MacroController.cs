using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orion.API.TradingEconomics.Commands;
using Orion.API.TradingEconomics.DTO;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Helpers;
using Orion.API.TradingEconomics.Interfaces;
using Orion.Core.MacroEngine.Application;
using System.Reflection.Emit;

namespace Orion.API.TradingEconomics.Controllers
{
    [ApiController]
    [Route("api/macro")]
    public class MacroController(IMediator _mediator, IFredService _fredService, ILogger _logger,IScenarioEngine _scenarioEngine, IProbabilisticScenarioGenerator _generator, IProbabilisticScenarioEngine _engine) : ControllerBase
    {


        [HttpPost("ingest/{country}")]
        public async Task<IActionResult> Ingest(string country)
        {
            await _mediator.Send(new IngestMacroDataCommand(country));
            return Ok();
        }

        [HttpPost("normalize")]
        public async Task<IActionResult> Normalize(bool forceRefresh = false)
        {
            var count = await _mediator.Send(
                new NormalizeMacroDataCommand(forceRefresh));

            return Ok(new
            {
                normalized = count,
                forceRefresh
            });
        }
        [HttpGet("factors")]
        public async Task<IActionResult> GetFactors()
        {
            var result = await _mediator.Send(new CalculateCurrencyFactorsCommand());
            return Ok(result);
        }

        [HttpGet("signals")]
        public async Task<IActionResult> GetSignals()
        {
            var result = await _mediator.Send(new GenerateFxSignalsCommand());
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capital"></param>
        /// <returns></returns>
        [HttpGet("portfolio")]
        public async Task<IActionResult> GetPortfolio(decimal capital = 100000)
        {
            var result = await _mediator.Send(new BuildPortfolioCommand(capital));
            return Ok(result);
        }


        /// <summary>
        /// Get macroeconomic data (GDP, Inflation, Rates, Unemployment) for all currencies
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<MacroData>> GetMacroData( CancellationToken cancellationToken = default)
        {
            try
            {
                var macroData = await _fredService.GetMacroDataAsync( cancellationToken);
                return Ok(macroData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching macro data from FRED");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get macro data for a specific currency
        /// </summary>
        [HttpGet("{currency}")]
        public async Task<ActionResult<CurrencyMacroData>> GetMacroDataForCurrency(string currency, [FromQuery] string? apiKey = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var macroData = await _fredService.GetMacroDataAsync( cancellationToken);

                if (macroData.Data.TryGetValue(currency.ToUpper(), out var currencyData))
                {
                    return Ok(currencyData);
                }

                return NotFound(new { error = $"Currency '{currency}' not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching macro data for {Currency}", currency);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get available FRED series mappings
        /// </summary>
        [HttpGet("series")]
        public ActionResult<Dictionary<string, Dictionary<string, string>>> GetFredSeries()
        {
            try
            {
                var series = _fredService.GetFredSeriesMappings();
                return Ok(series);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching FRED series mappings");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // /// <summary>
        // /// Get fallback/static macro data
        // /// </summary>
        // [HttpGet("fallback")]
        // public ActionResult<Dictionary<string, CurrencyMacroData>> GetFallbackData()
        // {
        //     try
        //     {
        //         var fallback = _fredService.GetFallbackMacroData();
        //         return Ok(fallback);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error fetching fallback macro data");
        //         return StatusCode(500, new { error = ex.Message });
        //     }
        // }

        /// <summary>
        /// Check if FRED API is configured and working
        /// </summary>
        [HttpGet("status")]
        public async Task<ActionResult<FredStatusResponse>> GetStatus(
            [FromQuery] string? apiKey = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var status = await _fredService.CheckStatusAsync( cancellationToken);
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking FRED status");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Refresh macro data cache
        /// </summary>
        [HttpPost("refresh")]
        public async Task<ActionResult<MacroData>> RefreshMacroData([FromQuery] string? apiKey = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var macroData = await _fredService.RefreshMacroDataAsync(cancellationToken);
                return Ok(macroData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing macro data");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("scenario/run")]
        public async Task<IActionResult> RunScenario([FromBody] Scenario scenario)
        {
            var result = await _scenarioEngine.RunAsync(scenario);
            return Ok(result);
        }

        [HttpGet("scenario/probabilistic")]
        public async Task<IActionResult> RunProbabilistic(int simulations = 500)
        {
            var scenarios = _generator.Generate(simulations);

            var results = await _engine.RunAsync(scenarios);

            var aggregated = ProbabilisticAggregator.Aggregate(results);

            return Ok(aggregated);
        }
    }
}
