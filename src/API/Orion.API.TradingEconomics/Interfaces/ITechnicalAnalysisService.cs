using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{

    public interface ITechnicalAnalysisService
    {
        /// <summary>
        /// Calculate all technical indicators for a given OHLCV dataset
        /// </summary>
        TechnicalIndicators CalculateIndicators(List<OhlcvBar> data);

        /// <summary>
        /// Get entry signal based on 15-minute data and trend bias
        /// </summary>
        EntrySignalResult GetEntrySignal(List<OhlcvBar> data15M, string bias);

        /// <summary>
        /// Generate trading ideas across all pairs using multi-timeframe analysis
        /// </summary>
        Task<List<TradingIdea>> GenerateTradingIdeasAsync(Dictionary<string, Dictionary<string, MarketDataResponse>> allData,CancellationToken cancellationToken = default);

        /// <summary>
        /// Generate swing trading ideas based on weekly/daily/4H structure
        /// </summary>
        Task<List<SwingTradingIdea>> GenerateSwingIdeasAsync(Dictionary<string, Dictionary<string, MarketDataResponse>> allData,CancellationToken cancellationToken = default);

        /// <summary>
        /// Generate multi-timeframe bias dashboard for all pairs
        /// </summary>
        Task<List<BiasAnalysisResult>> GenerateBiasDashboardAsync(Dictionary<string, Dictionary<string, MarketDataResponse>> allData,CancellationToken cancellationToken = default);

        /// <summary>
        /// Analyze a single pair across multiple timeframes
        /// </summary>
        Task<TradingIdea?> AnalyzeMultiTimeframeAsync(MarketDataResponse daily,MarketDataResponse fourHour,MarketDataResponse oneHour,MarketDataResponse fifteenMin,string pairName,CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculate stop loss level based on ATR and market structure
        /// </summary>
        StopLossResult CalculateStopLoss(List<OhlcvBar> df,string pair,string bias,decimal currentPrice,decimal atr,int lookback = 20);

        /// <summary>
        /// Calculate take profit levels
        /// </summary>
        TakeProfitResult CalculateTakeProfit(List<OhlcvBar> df,string pair,string bias,decimal currentPrice,decimal atr,decimal stopLoss,int lookback = 20);

        /// <summary>
        /// Calculate pip size for a given currency pair
        /// </summary>
        decimal GetPipSize(string pair);

        /// <summary>
        /// Convert price distance to pips
        /// </summary>
        decimal PriceToPips(string pair, decimal distance);

        /// <summary>
        /// Get swing high/low for stop loss placement
        /// </summary>
        decimal? GetSwingStop(List<OhlcvBar> df, string bias, int lookback = 20);

        /// <summary>
        /// Get swing high/low for take profit target
        /// </summary>
        decimal? GetSwingTarget(List<OhlcvBar> df, string bias, int lookback = 20);

        /// <summary>
        /// Calculate ADX trend strength
        /// </summary>
        decimal CalculateAdx(List<OhlcvBar> data, int window = 14);

        /// <summary>
        /// Determine overall trend bias from multiple indicators
        /// </summary>
        TrendBiasResult DetermineTrendBias(List<OhlcvBar> daily,List<OhlcvBar> fourHour,List<OhlcvBar> oneHour);

        /// <summary>
        /// Check if there's a valid entry setup
        /// </summary>
        bool HasValidEntrySetup(List<OhlcvBar> data15M,string bias,out List<string> reasons);     

        
    }
}