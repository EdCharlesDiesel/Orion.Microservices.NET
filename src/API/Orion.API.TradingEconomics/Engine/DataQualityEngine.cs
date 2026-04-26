using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Validates forex market data quality including completeness, staleness, gaps, and price spikes.
    /// </summary>
    public sealed class DataQualityEngine : IDataQualityEngine
    {
        private const int MinCandleCount = 50;
        private const int MaxStaleDays = 7;
        private const int MaxGapDays = 5;
        private const int MaxGapCount = 3;
        private const decimal MaxSpikePercent = 15m;
        private const int SpikeCheckWindow = 50;

        /// <inheritdoc />
        public DataQualityResult Validate(ForexMarketInput? input)
        {
            if (input == null)
                return DataQualityResult.Fail("Market input is null.");

            if (string.IsNullOrWhiteSpace(input.Pair))
                return DataQualityResult.Fail("Pair is missing.");

            return ValidateCandles(input.Candles);
        }

        /// <inheritdoc />
        public DataQualityResult ValidateCandles(IReadOnlyList<OhlcvBar>? candles)
        {
            var basic = ValidateCandleBasics(candles);
            if (!basic.IsValid)
                return basic;

            var ordered = candles!
                .OrderBy(x => x.TimestampUtc)
                .ToList();

            var duplicate = CheckDuplicateTimestamps(ordered);
            if (!duplicate.IsValid)
                return duplicate;

            var price = CheckPriceValidity(ordered);
            if (!price.IsValid)
                return price;

            var stale = CheckStaleness(ordered);
            if (!stale.IsValid)
                return stale;

            var gaps = CheckGaps(ordered);
            if (!gaps.IsValid)
                return gaps;

            var spikes = CheckPriceSpikes(ordered);
            if (!spikes.IsValid)
                return spikes;

            return DataQualityResult.Pass(
                $"Data quality passed. Candles={ordered.Count}, From={ordered.First().TimestampUtc:u}, To={ordered.Last().TimestampUtc:u}");
        }

        private static DataQualityResult ValidateCandleBasics(IReadOnlyList<OhlcvBar>? candles)
        {
            if (candles == null || candles.Count == 0)
                return DataQualityResult.Fail("No candle data supplied.");

            if (candles.Count < MinCandleCount)
                return DataQualityResult.Fail($"Not enough candles. Minimum required is {MinCandleCount}.");

            return DataQualityResult.Pass("Basic validation passed.");
        }

        private static DataQualityResult CheckDuplicateTimestamps(List<OhlcvBar> candles)
        {
            var duplicates = candles
                .GroupBy(x => x.TimestampUtc)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            return duplicates.Count > 0
                ? DataQualityResult.Fail("Duplicate candle timestamps detected.")
                : DataQualityResult.Pass("Duplicate check passed.");
        }

        private static DataQualityResult CheckPriceValidity(List<OhlcvBar> candles)
        {
            var invalid = candles.Any(x =>
                x.Open <= 0m ||
                x.High <= 0m ||
                x.Low <= 0m ||
                x.Close <= 0m ||
                x.High < x.Low ||
                x.High < x.Open ||
                x.High < x.Close ||
                x.Low > x.Open ||
                x.Low > x.Close);

            return invalid
                ? DataQualityResult.Fail("Invalid OHLC candle prices detected.")
                : DataQualityResult.Pass("Price validity check passed.");
        }

        private static DataQualityResult CheckStaleness(List<OhlcvBar> candles)
        {
            var latest = candles[^1].TimestampUtc;
            var age = DateTime.UtcNow - latest;

            return age.TotalDays > MaxStaleDays
                ? DataQualityResult.Fail($"Market data is stale. Latest candle is {latest:u}.")
                : DataQualityResult.Pass("Staleness check passed.");
        }

        private static DataQualityResult CheckGaps(List<OhlcvBar> candles)
        {
            var gapCount = 0;

            for (var i = 1; i < candles.Count; i++)
            {
                var gap = candles[i].TimestampUtc - candles[i - 1].TimestampUtc;

                if (gap.TotalDays > MaxGapDays)
                    gapCount++;
            }

            return gapCount > MaxGapCount
                ? DataQualityResult.Fail($"Too many candle gaps detected: {gapCount}.")
                : DataQualityResult.Pass("Gap check passed.");
        }

        private static DataQualityResult CheckPriceSpikes(List<OhlcvBar> candles)
        {
            var recent = candles.TakeLast(SpikeCheckWindow).ToList();

            for (var i = 1; i < recent.Count; i++)
            {
                var prevClose = recent[i - 1].Close;
                var currClose = recent[i].Close;

                if (prevClose <= 0m)
                    continue;

                var move = Math.Abs((currClose - prevClose) / prevClose) * 100m;

                if (move > MaxSpikePercent)
                {
                    return DataQualityResult.Fail(
                        $"Abnormal price spike detected: {move:F2}% at {recent[i].TimestampUtc:u}.");
                }
            }

            return DataQualityResult.Pass("Spike check passed.");
        }
    }
}