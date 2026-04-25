using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Orion.API.TradingEconomics.Configuration;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    public class TradingIdeasController : ControllerBase
    {
        private readonly ITechnicalAnalysisService _taService;
        private readonly IYahooFinanceService _yahooService;
        private readonly ILogger<TradingIdeasController> _logger;
        private readonly AppConfiguration _config;

        // Store generated ideas in memory for the session
        private static List<TradingIdea>? _cachedIdeas;
        private static List<SwingTradingIdea>? _cachedSwingIdeas;
        private static DateTime _ideasGeneratedAt = DateTime.MinValue;
        private static DateTime _swingIdeasGeneratedAt = DateTime.MinValue;

        public TradingIdeasController(
            ITechnicalAnalysisService taService,
            IYahooFinanceService yahooService,
            ILogger<TradingIdeasController> logger,
            IOptions<AppConfiguration> config)
        {
            _taService = taService;
            _yahooService = yahooService;
            _logger = logger;
            _config = config.Value;
        }

        /// <summary>
        /// Generate trading ideas based on multi-timeframe analysis
        /// </summary>
        [HttpPost("generate")]
        public async Task<ActionResult<TradingIdeasResponse>> GenerateTradingIdeas(
            [FromBody] GenerateIdeasRequest? request,
            CancellationToken cancellationToken)
        {
            try
            {
                var pairs = request?.Pairs ?? _config.Assets.Keys.ToList();

                _logger.LogInformation("Generating trading ideas for {Count} pairs", pairs.Count);

                // Fetch all required timeframe data for each pair
                var allData = await FetchAllTimeframeDataAsync(pairs, cancellationToken);

                // Generate trading ideas
                var ideas = await _taService.GenerateTradingIdeasAsync(allData, cancellationToken);

                // Generate swing ideas
                var swingIdeas = await _taService.GenerateSwingIdeasAsync(allData, cancellationToken);

                // Cache the results
                _cachedIdeas = ideas;
                _cachedSwingIdeas = swingIdeas;
                _ideasGeneratedAt = DateTime.UtcNow;
                _swingIdeasGeneratedAt = DateTime.UtcNow;

                // Check for new high-conviction ideas and trigger notifications if configured
                var highConvictionIdeas = ideas.Where(i => i.Conviction == "High").ToList();
                var highConvictionSwing = swingIdeas.Where(i => i.Conviction == "High").ToList();

                // Log generation stats
                _logger.LogInformation(
                    "Generated {IdeaCount} trading ideas ({HighCount} high conviction) and {SwingCount} swing ideas ({SwingHighCount} high conviction)",
                    ideas.Count, highConvictionIdeas.Count,
                    swingIdeas.Count, highConvictionSwing.Count);

                return Ok(new TradingIdeasResponse
                {
                    TradingIdeas = ideas,
                    SwingIdeas = swingIdeas,
                    GeneratedAt = DateTime.UtcNow,
                    TotalPairsAnalyzed = pairs.Count,
                    SkippedPairs = GetSkippedPairs(pairs, allData),
                    Summary = new IdeasSummary
                    {
                        TotalTradingIdeas = ideas.Count,
                        LongIdeas = ideas.Count(i => i.Bias == "Long"),
                        ShortIdeas = ideas.Count(i => i.Bias == "Short"),
                        HighConviction = highConvictionIdeas.Count,
                        MediumConviction = ideas.Count(i => i.Conviction == "Medium"),
                        TotalSwingIdeas = swingIdeas.Count,
                        SwingHighConviction = highConvictionSwing.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating trading ideas");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get previously generated trading ideas (cached)
        /// </summary>
        [HttpGet]
        public ActionResult<TradingIdeasResponse> GetTradingIdeas()
        {
            if (_cachedIdeas == null)
            {
                return Ok(new TradingIdeasResponse
                {
                    TradingIdeas = new List<TradingIdea>(),
                    SwingIdeas = new List<SwingTradingIdea>(),
                    GeneratedAt = null,
                    Message = "No trading ideas generated yet. Call POST /api/tradingideas/generate first."
                });
            }

            return Ok(new TradingIdeasResponse
            {
                TradingIdeas = _cachedIdeas,
                SwingIdeas = _cachedSwingIdeas ?? new List<SwingTradingIdea>(),
                GeneratedAt = _ideasGeneratedAt,
                Summary = new IdeasSummary
                {
                    TotalTradingIdeas = _cachedIdeas.Count,
                    LongIdeas = _cachedIdeas.Count(i => i.Bias == "Long"),
                    ShortIdeas = _cachedIdeas.Count(i => i.Bias == "Short"),
                    HighConviction = _cachedIdeas.Count(i => i.Conviction == "High"),
                    MediumConviction = _cachedIdeas.Count(i => i.Conviction == "Medium"),
                    TotalSwingIdeas = _cachedSwingIdeas?.Count ?? 0,
                    SwingHighConviction = _cachedSwingIdeas?.Count(i => i.Conviction == "High") ?? 0
                }
            });
        }

        /// <summary>
        /// Get only trading ideas (not swing)
        /// </summary>
        [HttpGet("ideas")]
        public ActionResult<List<TradingIdea>> GetIdeasOnly(
            [FromQuery] string? conviction = null,
            [FromQuery] string? bias = null)
        {
            var ideas = _cachedIdeas ?? new List<TradingIdea>();

            if (!string.IsNullOrEmpty(conviction))
            {
                ideas = ideas.Where(i => i.Conviction.Equals(conviction, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(bias))
            {
                ideas = ideas.Where(i => i.Bias.Equals(bias, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(ideas);
        }

        /// <summary>
        /// Get only swing trading ideas
        /// </summary>
        [HttpGet("swing")]
        public ActionResult<List<SwingTradingIdea>> GetSwingIdeasOnly(
            [FromQuery] string? conviction = null,
            [FromQuery] string? bias = null)
        {
            var ideas = _cachedSwingIdeas ?? new List<SwingTradingIdea>();

            if (!string.IsNullOrEmpty(conviction))
            {
                ideas = ideas.Where(i => i.Conviction.Equals(conviction, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(bias))
            {
                ideas = ideas.Where(i => i.Bias.Equals(bias, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(ideas);
        }

        /// <summary>
        /// Get trading idea for a specific pair
        /// </summary>
        [HttpGet("pair/{pair}")]
        public ActionResult<TradingIdea> GetIdeaForPair(string pair)
        {
            if (_cachedIdeas == null)
            {
                return NotFound(new { error = "No trading ideas generated yet" });
            }

            var idea = _cachedIdeas.FirstOrDefault(i =>
                i.Pair.Equals(pair, StringComparison.OrdinalIgnoreCase));

            if (idea == null)
            {
                return NotFound(new { error = $"No trading idea found for {pair}" });
            }

            return Ok(idea);
        }

        /// <summary>
        /// Get swing idea for a specific pair
        /// </summary>
        [HttpGet("swing/pair/{pair}")]
        public ActionResult<SwingTradingIdea> GetSwingIdeaForPair(string pair)
        {
            if (_cachedSwingIdeas == null)
            {
                return NotFound(new { error = "No swing ideas generated yet" });
            }

            var idea = _cachedSwingIdeas.FirstOrDefault(i =>
                i.Pair.Equals(pair, StringComparison.OrdinalIgnoreCase));

            if (idea == null)
            {
                return NotFound(new { error = $"No swing idea found for {pair}" });
            }

            return Ok(idea);
        }

        /// <summary>
        /// Get high-conviction ideas only
        /// </summary>
        [HttpGet("high-conviction")]
        public ActionResult<HighConvictionResponse> GetHighConvictionIdeas()
        {
            var tradingHigh = _cachedIdeas?.Where(i => i.Conviction == "High").ToList() ?? new List<TradingIdea>();
            var swingHigh = _cachedSwingIdeas?.Where(i => i.Conviction == "High").ToList() ?? new List<SwingTradingIdea>();

            return Ok(new HighConvictionResponse
            {
                TradingIdeas = tradingHigh,
                SwingIdeas = swingHigh,
                TotalHighConviction = tradingHigh.Count + swingHigh.Count,
                GeneratedAt = _ideasGeneratedAt
            });
        }

        /// <summary>
        /// Export trading ideas as CSV
        /// </summary>
        [HttpGet("export/csv")]
        public ActionResult ExportIdeasAsCsv([FromQuery] bool includeSwing = true)
        {
            if (_cachedIdeas == null)
            {
                return NotFound("No trading ideas generated yet");
            }

            var csv = GenerateCsvExport(includeSwing);
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);

            return File(
                bytes,
                "text/csv",
                $"trading_ideas_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv");
        }

        /// <summary>
        /// Export trading ideas as JSON
        /// </summary>
        [HttpGet("export/json")]
        public ActionResult ExportIdeasAsJson([FromQuery] bool includeSwing = true)
        {
            if (_cachedIdeas == null)
            {
                return NotFound("No trading ideas generated yet");
            }

            var export = new
            {
                GeneratedAt = _ideasGeneratedAt,
                TradingIdeas = _cachedIdeas,
                SwingIdeas = includeSwing ? _cachedSwingIdeas : null,
                Config = new
                {
                    _config.RiskPerTrade,
                    _config.ATRSLMult,
                    _config.TP1ATRMult,
                    _config.TP2ATRMult,
                    _config.MinRR,
                    _config.ADXTrendMin
                }
            };

            return Ok(export);
        }

        /// <summary>
        /// Clear cached ideas
        /// </summary>
        [HttpDelete("cache")]
        public ActionResult ClearCache()
        {
            _cachedIdeas = null;
            _cachedSwingIdeas = null;
            _ideasGeneratedAt = DateTime.MinValue;
            _swingIdeasGeneratedAt = DateTime.MinValue;

            return Ok(new { message = "Trading ideas cache cleared" });
        }

        /// <summary>
        /// Get bias analysis for all pairs
        /// </summary>
        [HttpGet("bias")]
        public async Task<ActionResult<List<BiasAnalysisResult>>> GetBiasAnalysis(
            [FromQuery] string? pairs = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var pairList = string.IsNullOrEmpty(pairs)
                    ? _config.Assets.Keys.ToList()
                    : pairs.Split(',').Select(p => p.Trim()).ToList();

                var allData = await FetchAllTimeframeDataAsync(pairList, cancellationToken);
                var biasResults = await _taService.GenerateBiasDashboardAsync(allData, cancellationToken);

                return Ok(biasResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating bias analysis");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get entry signal for a specific pair
        /// </summary>
        [HttpGet("entry-signal/{pair}")]
        public async Task<ActionResult<EntrySignalResult>> GetEntrySignal(
            string pair,
            [FromQuery] string? bias = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                throw new NotImplementedException("Entry signal endpoint is not implemented yet");
                //// Fetch 15-min data
                //var data15m = await _yahooService.FetchDataAsync(pair, "15m", "5d", cancellationToken);

                //// Determine bias if not provided
                //var resolvedBias = bias;
                //if (string.IsNullOrEmpty(resolvedBias))
                //{
                //    var dailyData = await _yahooService.FetchDataAsync(pair, "1d", "3mo", cancellationToken);
                //    resolvedBias = DetermineBiasFromDaily(dailyData);
                //}

                //var signal = _taService.GetEntrySignal(data15m.OHLCVData, resolvedBias);

                //return Ok(new
                //{
                //    Pair = pair,
                //    Bias = resolvedBias,
                //    Signal = signal
                //});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting entry signal for {Pair}", pair);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get multi-timeframe analysis for a single pair
        /// </summary>
        [HttpGet("analysis/{pair}")]
        public async Task<ActionResult<PairAnalysisResponse>> GetPairAnalysis(
            string pair,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var allTimeframes = await _yahooService.FetchAllTimeframesAsync(pair, cancellationToken);

                var analysis = new PairAnalysisResponse
                {
                    Pair = pair,
                    Timeframes = new Dictionary<string, TimeframeAnalysis>(),
                    TradingIdea = _cachedIdeas?.FirstOrDefault(i => i.Pair == pair),
                    SwingIdea = _cachedSwingIdeas?.FirstOrDefault(i => i.Pair == pair)
                };

                foreach (var (tfName, data) in allTimeframes)
                {
                    if (data.OhlcvBar.Count > 0)
                    {
                        var indicators = _taService.CalculateIndicators(data.OhlcvBar);
                        var latest = data.OhlcvBar.Last();

                        analysis.Timeframes[tfName] = new TimeframeAnalysis
                        {
                            LatestPrice = latest.Close,
                            LatestClose = latest.Close,
                            RSI = indicators.RSI.LastOrDefault() ?? 0,
                            ADX = indicators.ADX.LastOrDefault() ?? 0,
                            Trend = DetermineTrend(indicators, latest),
                            DataPoints = data.OhlcvBar.Count
                        };
                    }
                }

                // Get entry signal for 15m
                if (allTimeframes.TryGetValue("15 Minute", out var data15m))
                {
                    var bias = analysis.TradingIdea?.Bias ?? "Neutral";
                    analysis.EntrySignal = _taService.GetEntrySignal(data15m.OhlcvBar, bias);
                }

                return Ok(analysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing pair {Pair}", pair);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        #region Private Helpers

        private async Task<Dictionary<string, Dictionary<string, MarketDataResponse>>> FetchAllTimeframeDataAsync(
            List<string> pairs,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //var result = new Dictionary<string, Dictionary<string, MarketDataResponse>>();

            //// Use parallel fetching with throttling
            //var semaphore = new SemaphoreSlim(5);
            //var tasks = new List<Task>();

            //foreach (var pair in pairs)
            //{
            //    await semaphore.WaitAsync(cancellationToken);

            //    var task = Task.Run(async () =>
            //    {
            //        try
            //        {
            //            var pairData = new Dictionary<string, MarketDataResponse>();

            //            foreach (var tf in _config.Timeframes.Keys)
            //            {
            //                var tfConfig = _config.Timeframes[tf];
            //                var data = await _yahooService.FetchDataAsync(
            //                    pair,
            //                    tfConfig.Interval,
            //                    tfConfig.Period,
            //                    cancellationToken);

            //                if (data.OHLCVData.Count > 0)
            //                {
            //                    pairData[tf] = data;
            //                }
            //            }

            //            lock (result)
            //            {
            //                if (pairData.Count > 0)
            //                {
            //                    result[pair] = pairData;
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            _logger.LogWarning(ex, "Failed to fetch data for {Pair}", pair);
            //        }
            //        finally
            //        {
            //            semaphore.Release();
            //        }
            //    }, cancellationToken);

            //    tasks.Add(task);
            //}

            //await Task.WhenAll(tasks);
            //return result;
        }

        private List<string> GetSkippedPairs(
            List<string> requestedPairs,
            Dictionary<string, Dictionary<string, MarketDataResponse>> fetchedData)
        {
            return requestedPairs.Where(p => !fetchedData.ContainsKey(p)).ToList();
        }

        private string DetermineBiasFromDaily(MarketDataResponse dailyData)
        {
            if (dailyData.OhlcvBar.Count < 20) return "Neutral";

            var indicators = _taService.CalculateIndicators(dailyData.OhlcvBar);
            var latest = dailyData.OhlcvBar.Last();
            var ema20 = indicators.EMA20.LastOrDefault() ?? latest.Close;
            var adx = indicators.ADX.LastOrDefault() ?? 0;

            if (adx > _config.ADXTrendMin)
            {
                return latest.Close > ema20 ? "Long" : "Short";
            }

            return "Neutral";
        }

        private string DetermineTrend(TechnicalIndicators indicators, OhlcvBar latest)
        {
            var ema20 = indicators.EMA20.LastOrDefault() ?? latest.Close;
            var ema50 = indicators.EMA50.LastOrDefault() ?? latest.Close;
            var adx = indicators.ADX.LastOrDefault() ?? 0;

            if (adx > _config.ADXTrendMin)
            {
                if (latest.Close > ema20 && ema20 > ema50)
                    return "Strong Bullish";
                if (latest.Close < ema20 && ema20 < ema50)
                    return "Strong Bearish";
            }

            if (latest.Close > ema20)
                return "Bullish";
            if (latest.Close < ema20)
                return "Bearish";

            return "Neutral";
        }

        private string GenerateCsvExport(bool includeSwing)
        {
            var sb = new System.Text.StringBuilder();

            // Header
            sb.AppendLine("Type,Pair,Bias,Conviction,Strength,Entry,StopLoss,StopPips,TP1,TP1_RR,TP2,TP2_RR,ATR,Thesis");

            // Trading ideas
            if (_cachedIdeas != null)
            {
                foreach (var idea in _cachedIdeas)
                {
                    sb.AppendLine($"Trading,{EscapeCsv(idea.Pair)},{idea.Bias},{idea.Conviction},{idea.StrengthScore}," +
                        $"{idea.Entry},{idea.StopLoss},{idea.StopLossPips},{idea.TakeProfit1},{idea.RiskReward1}," +
                        $"{idea.TakeProfit2},{idea.RiskReward2},{idea.ATR},{EscapeCsv(idea.Thesis)}");
                }
            }

            // Swing ideas
            if (includeSwing && _cachedSwingIdeas != null)
            {
                foreach (var idea in _cachedSwingIdeas)
                {
                    sb.AppendLine($"Swing,{EscapeCsv(idea.Pair)},{idea.Bias},{idea.Conviction},{idea.StrengthScore}," +
                        $"{idea.Entry},{idea.StopLoss},{idea.StopLossPips},{idea.TakeProfit1},{idea.RiskReward1}," +
                        $"{idea.TakeProfit2},{idea.RiskReward2},{idea.ATR},{EscapeCsv(idea.Thesis)}");
                }
            }

            return sb.ToString();
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }
            return value;
        }

        #endregion
    }

    #region Request/Response Models

    public class GenerateIdeasRequest
    {
        public List<string>? Pairs { get; set; }
        public bool IncludeSwing { get; set; } = true;
        public bool ForceRefresh { get; set; } = false;
    }

    public class TradingIdeasResponse
    {
        public List<TradingIdea> TradingIdeas { get; set; } = new();
        public List<SwingTradingIdea> SwingIdeas { get; set; } = new();
        public DateTime? GeneratedAt { get; set; }
        public int TotalPairsAnalyzed { get; set; }
        public List<string> SkippedPairs { get; set; } = new();
        public IdeasSummary Summary { get; set; } = new();
        public string? Message { get; set; }
    }

    public class IdeasSummary
    {
        public int TotalTradingIdeas { get; set; }
        public int LongIdeas { get; set; }
        public int ShortIdeas { get; set; }
        public int HighConviction { get; set; }
        public int MediumConviction { get; set; }
        public int TotalSwingIdeas { get; set; }
        public int SwingHighConviction { get; set; }
    }

    public class HighConvictionResponse
    {
        public List<TradingIdea> TradingIdeas { get; set; } = new();
        public List<SwingTradingIdea> SwingIdeas { get; set; } = new();
        public int TotalHighConviction { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class PairAnalysisResponse
    {
        public string Pair { get; set; } = string.Empty;
        public Dictionary<string, TimeframeAnalysis> Timeframes { get; set; } = new();
        public TradingIdea? TradingIdea { get; set; }
        public SwingTradingIdea? SwingIdea { get; set; }
        public EntrySignalResult? EntrySignal { get; set; }
    }

    public class TimeframeAnalysis
    {
        public decimal LatestPrice { get; set; }
        public decimal LatestClose { get; set; }
        public decimal RSI { get; set; }
        public decimal ADX { get; set; }
        public string Trend { get; set; } = string.Empty;
        public int DataPoints { get; set; }
    }

    #endregion
}
