namespace Orion.API.TradingEconomics.Configuration
{
    public class AppConfiguration
    {
        // Version and Cache Settings
        public string Version { get; set; } = "1.0.0";
        public int CacheTTLSeconds { get; set; } = 300;
        public int AutoRefreshIntervalSeconds { get; set; } = 300;

        // Risk Management
        public decimal RiskPerTrade { get; set; } = 0.02m;
        public decimal ATRSLMult { get; set; } = 2.0m;
        public decimal TP1ATRMult { get; set; } = 3.0m;
        public decimal TP2ATRMult { get; set; } = 5.0m;
        public decimal MinRR { get; set; } = 2.0m;

        // Stop Loss Settings
        public decimal DefaultMinStop { get; set; } = 0.0010m;
        public decimal StopBufferPercent { get; set; } = 0.25m;

        // Indicator Thresholds
        public decimal ADXTrendMin { get; set; } = 20.0m;
        public decimal RSI_OS { get; set; } = 40.0m;
        public decimal RSI_OB { get; set; } = 60.0m;
        public decimal StochOS { get; set; } = 25.0m;
        public decimal StochOB { get; set; } = 75.0m;

        // Pair-specific Overrides
        public Dictionary<string, decimal> PairATRMultipliers { get; set; } = new()
        {
            ["EURUSD"] = 1.8m,
            ["GBPUSD"] = 2.0m,
            ["USDJPY"] = 1.2m,
            ["BTCUSD"] = 3.0m,
            ["ETHUSD"] = 3.5m
        };

        public Dictionary<string, decimal> PairMinStop { get; set; } = new()
        {
            ["EURUSD"] = 0.0008m,
            ["GBPUSD"] = 0.0010m,
            ["USDJPY"] = 0.080m,
            ["BTCUSD"] = 50.0m,
            ["ETHUSD"] = 5.0m
        };

        // Asset Mappings
        public Dictionary<string, string> Assets { get; set; } = new()
        {
            ["EUR/USD"] = "EURUSD=X",
            ["GBP/USD"] = "GBPUSD=X",
            ["USD/JPY"] = "JPY=X",
            ["USD/ZAR"] = "ZAR=X",
            ["AUD/USD"] = "AUDUSD=X",
            ["NZD/USD"] = "NZDUSD=X",
            ["USD/CAD"] = "CAD=X",
            ["USD/CHF"] = "CHF=X",
            ["XAU/USD"] = "GC=F",
            ["BTC/USD"] = "BTC-USD"
        };

        // Timeframe Configurations
        public Dictionary<string, TimeframeConfig> Timeframes { get; set; } = new()
        {
            ["Weekly"] = new() { Interval = "1wk", Period = "3mo" },
            ["Daily"] = new() { Interval = "1d", Period = "3mo" },
            ["4 Hour"] = new() { Interval = "1h", Period = "1mo" },
            ["Hourly"] = new() { Interval = "1h", Period = "1mo" },
            ["15 Minute"] = new() { Interval = "15m", Period = "5d" }
        };
        public bool UseMockData { get; internal set; }
        public object ApiBaseUrl { get; internal set; }

        // Helper Methods
        public decimal GetATRMultiplier(string pair)
        {
            // Normalize pair format (remove slash if present)
            var normalizedPair = pair.Replace("/", "");

            return PairATRMultipliers?.GetValueOrDefault(normalizedPair, ATRSLMult) ?? ATRSLMult;
        }

        public decimal GetMinStop(string pair)
        {
            // Normalize pair format (remove slash if present)
            var normalizedPair = pair.Replace("/", "");

            return PairMinStop?.GetValueOrDefault(normalizedPair, DefaultMinStop) ?? DefaultMinStop;
        }

        public string GetYahooSymbol(string asset)
        {
            return Assets?.GetValueOrDefault(asset, asset) ?? asset;
        }

        public TimeframeConfig GetTimeframeConfig(string timeframe)
        {
            return Timeframes?.GetValueOrDefault(timeframe, new TimeframeConfig
            {
                Interval = "1d",
                Period = "1mo"
            }) ?? new TimeframeConfig { Interval = "1d", Period = "1mo" };
        }

        // Validation Method
        public void Validate()
        {
            if (RiskPerTrade <= 0 || RiskPerTrade > 0.05m)
                throw new InvalidOperationException("RiskPerTrade must be between 0 and 5%");

            if (ATRSLMult < 1.0m)
                throw new InvalidOperationException("ATRSLMult should be at least 1.0");

            if (MinRR < 1.0m)
                throw new InvalidOperationException("MinRR should be at least 1.0");

            if (StopBufferPercent < 0 || StopBufferPercent > 1)
                throw new InvalidOperationException("StopBufferPercent must be between 0 and 1");
        }
    }

    public class TimeframeConfig
    {
        public string Interval { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;

        // Helper method to get TimeSpan for the period
        public TimeSpan GetPeriodTimeSpan()
        {
            return Period switch
            {
                "1d" => TimeSpan.FromDays(1),
                "5d" => TimeSpan.FromDays(5),
                "1mo" => TimeSpan.FromDays(30),
                "3mo" => TimeSpan.FromDays(90),
                "6mo" => TimeSpan.FromDays(180),
                "1y" => TimeSpan.FromDays(365),
                _ => TimeSpan.FromDays(30)
            };
        }
    }
}