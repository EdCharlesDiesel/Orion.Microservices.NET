// using Microsoft.AspNetCore.Mvc;
// using Orion.API.TradingEconomics.Entities;
// using Orion.API.TradingEconomics.Interfaces;
//
// namespace Orion.API.TradingEconomics.Controllers
// {    
//
//     [ApiController]
//     [Route("api/[controller]")]
//     public class TechnicalAnalysisController : ControllerBase
//     {
//         private readonly ITechnicalAnalysisService _taService;
//         private readonly IYahooFinanceService _yahooService;
//         private readonly ILogger<TechnicalAnalysisController> _logger;
//
//         public TechnicalAnalysisController(
//             ITechnicalAnalysisService taService,
//             IYahooFinanceService yahooService,
//             ILogger<TechnicalAnalysisController> logger)
//         {
//             _taService = taService;
//             _yahooService = yahooService;
//             _logger = logger;
//         }
//
//         [HttpPost("calculate")]
//         public ActionResult<TechnicalIndicators> CalculateIndicators([FromBody] List<OhlcvBar> data)
//         {
//             try
//             {
//                 var indicators = _taService.CalculateIndicators(data);
//                 return Ok(indicators);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error calculating indicators");
//                 return StatusCode(500, new { error = ex.Message });
//             }
//         }
//
//         [HttpGet("entry-signal")]
//         public async Task<ActionResult<EntrySignalResult>> GetEntrySignal([FromQuery] string pair,[FromQuery] string bias,CancellationToken cancellationToken)
//         {
//             throw new NotImplementedException("This endpoint is not implemented yet. In production, it would fetch the latest 15m data for the given pair, calculate the entry signal based on the bias, and return the result.");
//             try
//             {
//                 var data15m = await _yahooService.FetchDataAsync(pair, "15m", "5d", cancellationToken);
//                 var signal = _taService.GetEntrySignal(data15m, bias);
//                 return Ok(signal);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error calculating entry signal");
//                 return StatusCode(500, new { error = ex.Message });
//             }
//         }
//
//         [HttpGet("trading-ideas")]
//         public async Task<ActionResult<List<TradingIdea>>> GetTradingIdeas(CancellationToken cancellationToken)
//         {
//             try
//             {
//                 // In production, fetch all data first
//                 var ideas = await _taService.GenerateTradingIdeasAsync(new(), cancellationToken);
//                 return Ok(ideas);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error generating trading ideas");
//                 return StatusCode(500, new { error = ex.Message });
//             }
//         }
//
//         [HttpGet("swing-ideas")]
//         public async Task<ActionResult<List<SwingTradingIdea>>> GetSwingIdeas(CancellationToken cancellationToken)
//         {
//             try
//             {
//                 var ideas = await _taService.GenerateSwingIdeasAsync(new(), cancellationToken);
//                 return Ok(ideas);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error generating swing ideas");
//                 return StatusCode(500, new { error = ex.Message });
//             }
//         }
//
//         [HttpGet("bias-dashboard")]
//         public async Task<ActionResult<List<BiasAnalysisResult>>> GetBiasDashboard(CancellationToken cancellationToken)
//         {
//             try
//             {
//                 var results = await _taService.GenerateBiasDashboardAsync(new(), cancellationToken);
//                 return Ok(results);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error generating bias dashboard");
//                 return StatusCode(500, new { error = ex.Message });
//             }
//         }
//     }
// }
