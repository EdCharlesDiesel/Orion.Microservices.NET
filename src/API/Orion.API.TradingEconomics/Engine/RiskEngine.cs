using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class RiskEngine
    {
        private const decimal MinimumConfidence = 55;
        private const decimal MaximumSpreadPercent = 0.08m;
        private const decimal MaximumVolatilityPercent = 2.5m;
        private const decimal MinimumRiskReward = 1.5m ;

        public RiskResult Evaluate(
            SignalResult signal,
            NormalizedMarketContext? market,
            RegimeResult regime)
        {
            if (market == null)
                return RiskResult.Block("Market context is null.");

            if (signal.Direction == "NO_TRADE")
                return RiskResult.Block(signal.Reason);

            if (signal.Confidence < MinimumConfidence)
                return RiskResult.Block($"Signal confidence too low: {signal.Confidence:F0}%.");

            if (market.Candles.Count < 20)
                return RiskResult.Block("Not enough candles for risk evaluation.");

            var latest = market.Candles[^1];

            if (latest.Close <= 0)
                return RiskResult.Block("Invalid latest close price.");

            var spreadRisk = CalculateSpreadRisk(market);
            var volatilityRisk = CalculateVolatilityRisk(market);
            var regimeRisk = CalculateRegimeRisk(signal, regime);
            var drawdownRisk = CalculateDrawdownRisk(market);

            var totalRisk =
                spreadRisk * 0.25m +
                volatilityRisk * 0.35m +
                regimeRisk * 0.25m +
                drawdownRisk * 0.15m;


            if (spreadRisk >= 0.85m)
                return RiskResult.Block("Spread risk too high.", totalRisk);

            if (volatilityRisk >= 0.90m)
                return RiskResult.Block("Volatility risk too high.", totalRisk);

            if (regimeRisk >= 0.90m )
                return RiskResult.Block("Signal conflicts with market regime.", totalRisk);

            if (totalRisk >= 0.75m)
                return RiskResult.Block($"Total risk too high: {totalRisk:F2}.", totalRisk);

            return RiskResult.Allow(
                score: totalRisk,
                reason:
                    $"Risk accepted. " +
                    $"SpreadRisk={spreadRisk:F2}, " +
                    $"VolatilityRisk={volatilityRisk:F2}, " +
                    $"RegimeRisk={regimeRisk:F2}, " +
                    $"DrawdownRisk={drawdownRisk:F2}, " +
                    $"TotalRisk={totalRisk:F2}");
        }

        private static decimal CalculateSpreadRisk(NormalizedMarketContext market)
        {
            if (market.Spread <= 0)
                return 0.20m;

            var latestClose = market.Candles[^1].Close;

            if (latestClose <= 0)
                return 1.0m;

            var spreadPercent = market.Spread / latestClose * 100.0m;

            return Clamp01(spreadPercent / MaximumSpreadPercent);
        }

        private static decimal CalculateVolatilityRisk(NormalizedMarketContext market)
        {
            var candles = market.Candles.TakeLast(20).ToList();

            var ranges = candles
                .Where(x => x.Close > 0)
                .Select(x => (x.High - x.Low) / x.Close * 100.0m)
                .ToList();

            if (ranges.Count == 0)
                return 1.0m;

            var avgRangePercent = ranges.Average();

            return Clamp01(avgRangePercent / MaximumVolatilityPercent);
        }

        private static decimal CalculateRegimeRisk(
            SignalResult signal,
            RegimeResult regime)
        {
            var regimeName = regime.Name.ToUpperInvariant();
            var direction = signal.Direction.ToUpperInvariant();

            if (regimeName is "NEUTRAL" or "RANGE")
                return 0.35m;

            if (direction == "LONG" &&
                regimeName is "BULLISH" or "RISK_ON" or "UPTREND")
                return 0.15m;
            if (direction == "SHORT" &&
                regimeName is "BEARISH" or "RISK_OFF" or "DOWNTREND")
                return 0.15m;

            if (direction == "LONG" &&
                regimeName is "BEARISH" or "RISK_OFF" or "DOWNTREND")
                return 1.0m;
            if (direction == "SHORT" &&
                regimeName is "BULLISH" or "RISK_ON" or "UPTREND")
                return 1.0m;

            return 0.50m;
        }

        private static decimal CalculateDrawdownRisk(NormalizedMarketContext market)
        {
            var candles = market.Candles.TakeLast(50).ToList();

            if (candles.Count < 10)
                return 0.50m;

            var closes = candles.Select(x => x.Close).ToList();

            var peak = closes.Max();
            var latest = closes[^1];

            if (peak <= 0)
                return 1.0m;

            var drawdownPercent = (peak - latest) / peak * 100.0m;

            return Clamp01(drawdownPercent / 5.0m);
        }

        private static decimal Clamp01(decimal value)
        {
            return Math.Max(0.0m, Math.Min(1.0m, value));
        }
    }
}
