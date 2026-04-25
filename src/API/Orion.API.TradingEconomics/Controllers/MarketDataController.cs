// using Microsoft.AspNetCore.Mvc;
// using Orion.API.TradingEconomics.DTO;
// using Orion.API.TradingEconomics.Entities;
// using Orion.API.TradingEconomics.Helpers;
// using Orion.API.TradingEconomics.Interfaces;
//
// namespace Orion.API.TradingEconomics.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public sealed class MarketDataController(MarketPipeline marketPipeline,
//         IFredService fredService,
//         ILogger<MarketDataController> logger) : ControllerBase
//     {
//         [HttpGet("macro-data")]
//         public async Task<ActionResult<MacroData>> GetMacroData([FromQuery] string? apiKey = null, CancellationToken cancellationToken = default)
//         {
//             try
//             {
//                 var data = await fredService.GetMacroDataAsync( cancellationToken);
//                 return Ok(data);
//             }
//             catch (Exception ex)
//             {
//                 logger.LogError(ex, "Error fetching FRED macro data");
//                 return StatusCode(500, new { error = ex.Message });
//             }
//         }
//
//         [HttpPost("refresh")]
//         public async Task<ActionResult<MacroData>> RefreshMacroData([FromQuery] string? apiKey = null, CancellationToken cancellationToken = default)
//         {
//             try
//             {
//                 var data = await fredService.RefreshMacroDataAsync( cancellationToken);
//                 return Ok(data);
//             }
//             catch (Exception ex)
//             {
//                 logger.LogError(ex, "Error refreshing FRED macro data");
//                 return StatusCode(500, new { error = ex.Message });
//             }
//         }
//
//         [HttpGet("status")]
//         public async Task<ActionResult<FredStatusResponse>> CheckStatus([FromQuery] string? apiKey = null, CancellationToken cancellationToken = default)
//         {
//             try
//             {
//                 var status = await fredService.CheckStatusAsync( cancellationToken);
//                 return Ok(status);
//             }
//             catch (Exception ex)
//             {
//                 logger.LogError(ex, "Error checking FRED status");
//                 return StatusCode(500, new { error = ex.Message });
//             }
//         }
//
//         [HttpGet("series-mappings")]
//         public ActionResult<Dictionary<string, Dictionary<string, string>>> GetSeriesMappings()
//         {
//             return Ok(fredService.GetFredSeriesMappings());
//         }
//
//
//         
//         /// <summary>
//     /// Load validated market data for a pair
//     /// </summary>
//     [HttpGet("data/{pair}")]
//     public async Task<IActionResult> GetMarketData(
//         string pair,
//         CancellationToken cancellationToken)
//     {
//         try
//         {
//             var input = await _marketPipeline.LoadValidatedMarketAsync(
//                 pair, cancellationToken);
//             
//             return Ok(input);
//         }
//         catch (MarketDataRejectedException ex)
//         {
//             return BadRequest(new 
//             { 
//                 Error = ex.Message, 
//                 Quality = ex.QualityResult 
//             });
//         }
//     }
//
//     /// <summary>
//     /// Load data for multiple pairs
//     /// </summary>
//     [HttpPost("data/bulk")]
//     public async Task<IActionResult> GetBulkMarketData(
//         [FromBody] List<string> pairs,
//         CancellationToken cancellationToken)
//     {
//         var results = await _marketPipeline.LoadMultiplePairsAsync(
//             pairs, cancellationToken);
//         
//         return Ok(new 
//         {
//             Pairs = results.Keys,
//             Data = results.Values,
//             FailedPairs = pairs.Except(results.Keys).ToList()
//         });
//     }
//
//     /// <summary>
//     /// Get quick snapshot (less validation)
//     /// </summary>
//     [HttpGet("snapshot/{pair}")]
//     public async Task<IActionResult> GetQuickSnapshot(
//         string pair,
//         CancellationToken cancellationToken)
//     {
//         var snapshot = await _marketPipeline.GetQuickSnapshotAsync(
//             pair, cancellationToken);
//         
//         return Ok(snapshot);
//     }
//
//     /// <summary>
//     /// Force refresh cache for a pair
//     /// </summary>
//     [HttpPost("refresh/{pair}")]
//     public async Task<IActionResult> RefreshData(
//         string pair,
//         CancellationToken cancellationToken)
//     {
//         var data = await _marketPipeline.RefreshMarketDataAsync(
//             pair, cancellationToken);
//         
//         return Ok(data);
//     }
//
//     /// <summary>
//     /// Get data quality report
//     /// </summary>
//     [HttpGet("quality/{pair}")]
//     public async Task<IActionResult> GetDataQuality(
//         string pair,
//         CancellationToken cancellationToken)
//     {
//         var report = await _marketPipeline.ValidateExistingDataAsync(
//             pair, cancellationToken);
//         
//         return Ok(report);
//     }
//
//     /// <summary>
//     /// Get available pairs
//     /// </summary>
//     [HttpGet("pairs")]
//     public async Task<IActionResult> GetAvailablePairs(
//         CancellationToken cancellationToken)
//     {
//         var pairs = await _marketPipeline.GetAvailablePairsAsync(cancellationToken);
//         return Ok(pairs);
//     }
//
//     /// <summary>
//     /// Get pipeline statistics
//     /// </summary>
//     [HttpGet("stats")]
//     public IActionResult GetStats()
//     {
//         var stats = _marketPipeline.GetStats();
//         return Ok(stats);
//     }
//
//     /// <summary>
//     /// Invalidate cache
//     /// </summary>
//     [HttpDelete("cache")]
//     public IActionResult InvalidateCache([FromQuery] string pair = null)
//     {
//         _marketPipeline.InvalidateCache(pair);
//         return Ok(new { Message = pair != null 
//             ? $"Cache invalidated for {pair}" 
//             : "All cache invalidated" });
//     }
//
//     /// <summary>
//     /// Warm up cache with common pairs
//     /// </summary>
//     [HttpPost("warmup")]
//     public async Task<IActionResult> WarmUpCache(
//         [FromBody] List<string> pairs,
//         CancellationToken cancellationToken)
//     {
//         await _marketPipeline.WarmUpCacheAsync(pairs, cancellationToken);
//         return Ok(new { Message = $"Cache warmed up for {pairs.Count} pairs" });
//     }
//     }
// }