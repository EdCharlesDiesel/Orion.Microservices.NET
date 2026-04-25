using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{    

    public sealed class PositionSizingEngine
    {
        private const decimal DefaultRiskPerTradePercent = 1.0m;
        private const decimal MaxRiskPerTradePercent = 2.0m;
        private const decimal MinPositionSize = 0.01m;

        public PositionSizeResult Calculate(
            SignalResult signal,
            RiskResult risk,
            NormalizedMarketContext market,
            AccountContext account)
        {
            if (signal.Direction == "NO_TRADE")
                return PositionSizeResult.None("No trade signal.");

            if (!risk.IsAllowed)
                return PositionSizeResult.None(risk.Reason);

            if (account.Balance <= 0)
                return PositionSizeResult.None("Invalid account balance.");

            if (market.Candles == null || market.Candles.Count < 14)
                return PositionSizeResult.None("Not enough candles for position sizing.");

            var atr = CalculateAtr(market.Candles, 14);

            if (atr <= 0)
                return PositionSizeResult.None("Invalid ATR.");

            var latestClose = market.Candles[^1].Close;

            var stopDistance = atr * GetAtrStopMultiplier(market.Pair);

            var confidenceMultiplier = GetConfidenceMultiplier(signal.Confidence);
            var riskQualityMultiplier = GetRiskQualityMultiplier(risk.Score);

            var adjustedRiskPercent =
                DefaultRiskPerTradePercent *
                confidenceMultiplier *
                riskQualityMultiplier;

            adjustedRiskPercent = Math.Min(adjustedRiskPercent, MaxRiskPerTradePercent);

            var riskAmount = account.Balance * (adjustedRiskPercent / 100m);

            var rawSize = riskAmount / stopDistance;

            var positionSize = Math.Max(rawSize, MinPositionSize);

            return new PositionSizeResult
            {
                IsAllowed = true,
                Pair = market.Pair,
                Direction = signal.Direction,
                PositionSize = decimal.Round(positionSize, 4),
                RiskAmount = decimal.Round(riskAmount, 2),
                RiskPercent = decimal.Round(adjustedRiskPercent, 2),
                StopDistance = decimal.Round(stopDistance, 5),
                Reason =
                    $"Position accepted. " +
                    $"Risk={adjustedRiskPercent:F2}%, " +
                    $"RiskAmount={riskAmount:F2}, " +
                    $"ATR={atr:F5}, " +
                    $"StopDistance={stopDistance:F5}, " +
                    $"Size={positionSize:F4}"
            };
        }

        private static decimal CalculateAtr(List<OhlcvBar> candles, int period)
        {
            var recent = candles.TakeLast(period + 1).ToList();

            if (recent.Count < period + 1)
                return 0;

            var trueRanges = new List<decimal>();

            for (var i = 1; i < recent.Count; i++)
            {
                var current = recent[i];
                var previous = recent[i - 1];

                var highLow = current.High - current.Low;
                var highClose = Math.Abs(current.High - previous.Close);
                var lowClose = Math.Abs(current.Low - previous.Close);

                trueRanges.Add(Math.Max(highLow, Math.Max(highClose, lowClose)));
            }

            return trueRanges.Average();
        }

        private static decimal GetAtrStopMultiplier(string pair)
        {
            return pair.ToUpperInvariant() switch
            {
                "USD/ZAR" => 2.5m,
                "XAU/USD" => 2.0m,
                "BTC/USD" => 2.0m,
                "GBP/USD" => 1.8m,
                "NZD/USD" => 1.6m,
                _ => 1.5m
            };
        }

        private static decimal GetConfidenceMultiplier(decimal confidence)
        {
            if (confidence >= 85) return 1.30m;
            if (confidence >= 75) return 1.15m;
            if (confidence >= 65) return 1.00m;
            if (confidence >= 55) return 0.75m;

            return 0.0m;
        }

        private static decimal GetRiskQualityMultiplier(decimal riskScore)
        {
            if (riskScore <= 0.20m) return 1.20m;
            if (riskScore <= 0.40m) return 1.00m;
            if (riskScore <= 0.60m) return 0.70m;

            return 0.40m;
        }
    }
}
