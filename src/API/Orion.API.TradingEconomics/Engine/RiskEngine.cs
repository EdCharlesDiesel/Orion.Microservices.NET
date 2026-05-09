using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Evaluates signal risk against spread, volatility, regime, and drawdown conditions.
    /// </summary>
    public sealed class RiskEngine : IRiskEngine
    {
        private const decimal MinimumConfidence = 55m;
        private const decimal MaximumSpreadPercent = 0.08m;
        private const decimal MaximumVolatilityPercent = 2.5m;

        /// <inheritdoc />
        public RiskResult Evaluate(
            SignalResult signal,
            NormalizedMarketContext? market,
            RegimeResult regime)
        {
            ArgumentNullException.ThrowIfNull(signal);
            ArgumentNullException.ThrowIfNull(regime);

            if (market == null)
                return RiskResult.Block("Market context is null.");

            if (string.Equals(signal.Direction, "NO_TRADE", StringComparison.OrdinalIgnoreCase))
                return RiskResult.Block(signal.Reason);

            if (signal.Confidence < MinimumConfidence)
                return RiskResult.Block($"Signal confidence too low: {signal.Confidence:F0}%.");

            if (market.Candles == null || market.Candles.Count < 20)
                return RiskResult.Block("Not enough candles for risk evaluation.");

            var latest = market.Candles[^1];

            if (latest.Close <= 0m)
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

            if (regimeRisk >= 0.90m)
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
            if (market.Spread <= 0m)
                return 0.20m;

            var latestClose = market.Candles[^1].Close;

            if (latestClose <= 0m)
                return 1.0m;

            var spreadPercent = market.Spread / latestClose * 100m;

            return Clamp01(spreadPercent / MaximumSpreadPercent);
        }

        private static decimal CalculateVolatilityRisk(NormalizedMarketContext market)
        {
            var ranges = market.Candles
                .TakeLast(20)
                .Where(x => x.Close > 0m)
                .Select(x => (x.High - x.Low) / x.Close * 100m)
                .ToList();

            if (ranges.Count == 0)
                return 1.0m;

            var averageRangePercent = ranges.Average();

            return Clamp01(averageRangePercent / MaximumVolatilityPercent);
        }

        private static decimal CalculateRegimeRisk(
            SignalResult signal,
            RegimeResult regime)
        {
            var regimeName = ResolveRegimeName(regime);
            var direction = signal.Direction.Trim().ToUpperInvariant();

            if (regimeName is "NEUTRAL" or "RANGE")
                return 0.35m;

            if (direction == "LONG" &&
                regimeName is "BULLISH" or "RISK_ON" or "RISKON" or "UPTREND")
                return 0.15m;

            if (direction == "SHORT" &&
                regimeName is "BEARISH" or "RISK_OFF" or "RISKOFF" or "DOWNTREND")
                return 0.15m;

            if (direction == "LONG" &&
                regimeName is "BEARISH" or "RISK_OFF" or "RISKOFF" or "DOWNTREND")
                return 1.0m;

            if (direction == "SHORT" &&
                regimeName is "BULLISH" or "RISK_ON" or "RISKON" or "UPTREND")
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

            if (peak <= 0m)
                return 1.0m;

            var drawdownPercent = (peak - latest) / peak * 100m;

            return Clamp01(drawdownPercent / 5m);
        }

        private static string ResolveRegimeName(RegimeResult regime)
        {
            if (!string.IsNullOrWhiteSpace(regime.Name))
                return regime.Name.Trim().ToUpperInvariant();

            return regime.Regime.ToString().Trim().ToUpperInvariant();
        }

        private static decimal Clamp01(decimal value)
        {
            return Math.Max(0m, Math.Min(1m, value));
        }
    }
}