namespace Orion.API.TradingEconomics.Services
{
    public partial class TechnicalAnalysisService
    {
        // Indicator parameters as configuration-driven constants
        private static class IndicatorParams
        {
            public const int RSI_WINDOW = 14;
            public const int MACD_FAST = 12;
            public const int MACD_SLOW = 26;
            public const int MACD_SIGNAL = 9;
            public const int SMA_SHORT = 20;
            public const int SMA_LONG = 50;
            public const int BB_WINDOW = 20;
            public const decimal BB_STD_DEV = 2m;
            public const int ATR_WINDOW = 14;
            public const int STOCH_WINDOW = 14;
            public const int STOCH_SMOOTH = 3;
            public const int ADX_WINDOW = 14;
            public const int MIN_DATA_POINTS = 50;
            public const int SWING_LOOKBACK = 20;
            public const int BIAS_LOOKBACK = 20;
            public const decimal MIN_CONFIDENCE = 0.6m;
        }

        
    }
}
