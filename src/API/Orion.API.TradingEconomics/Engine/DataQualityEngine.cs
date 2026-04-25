using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{

    public sealed class DataQualityEngine: IDataQualityEngine
    {
        public DataQualityResult Validate(ForexMarketInput? input)
        {
            if (input == null)
                return DataQualityResult.Fail("Market input is null.");

            if (string.IsNullOrWhiteSpace(input.Pair))
                return DataQualityResult.Fail("Pair is missing.");

            if (input.Candles.Count == 0)
                return DataQualityResult.Fail("No candle data supplied.");

            if (input.Candles.Count < 50)
                return DataQualityResult.Fail("Not enough candles. Minimum required is 50.");

            var ordered = input.Candles
                .OrderBy(x => x.TimestampUtc)
                .ToList();

            var duplicateTimestamps = ordered
                .GroupBy(x => x.TimestampUtc)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateTimestamps.Count > 0)
                return DataQualityResult.Fail("Duplicate candle timestamps detected.");

            var invalidPrices = ordered.Any(x =>
                x.Open <= 0 ||
                x.High <= 0 ||
                x.Low <= 0 ||
                x.Close <= 0 ||
                x.High < x.Low ||
                x.High < x.Open ||
                x.High < x.Close ||
                x.Low > x.Open ||
                x.Low > x.Close);

            if (invalidPrices)
                return DataQualityResult.Fail("Invalid OHLC candle prices detected.");

            var staleResult = CheckStaleness(ordered);

            if (!staleResult.IsValid)
                return staleResult;

            var gapResult = CheckGaps(ordered);

            if (!gapResult.IsValid)
                return gapResult;

            var spikeResult = CheckPriceSpikes(ordered);

            if (!spikeResult.IsValid)
                return spikeResult;

            return DataQualityResult.Pass(
                $"Data quality passed. Candles={ordered.Count}, From={ordered.First().TimestampUtc:u}, To={ordered.Last().TimestampUtc:u}");
        }

        private static DataQualityResult CheckStaleness(List<OhlcvBar> candles)
        {
            var latest = candles[^1].TimestampUtc;
            var age = DateTime.UtcNow - latest;

            if (age.TotalDays > 7)
                return DataQualityResult.Fail($"Market data is stale. Latest candle is {latest:u}.");

            return DataQualityResult.Pass("Staleness check passed.");
        }

        private static DataQualityResult CheckGaps(List<OhlcvBar> candles)
        {
            var gaps = 0;

            for (var i = 1; i < candles.Count; i++)
            {
                var gap = candles[i].TimestampUtc - candles[i - 1].TimestampUtc;

                if (gap.TotalDays > 5)
                    gaps++;
            }

            if (gaps > 3)
                return DataQualityResult.Fail($"Too many candle gaps detected: {gaps}.");

            return DataQualityResult.Pass("Gap check passed.");
        }

        private static DataQualityResult CheckPriceSpikes(List<OhlcvBar> candles)
        {
            var recent = candles.TakeLast(50).ToList();

            for (var i = 1; i < recent.Count; i++)
            {
                var previousClose = recent[i - 1].Close;
                var currentClose = recent[i].Close;

                if (previousClose <= 0)
                    continue;

                var movePercent = Math.Abs((currentClose - previousClose) / previousClose) * 100m;

                if (movePercent > 15m)
                {
                    return DataQualityResult.Fail(
                        $"Abnormal price spike detected: {movePercent:F2}% at {recent[i].TimestampUtc:u}.");
                }
            }

            return DataQualityResult.Pass("Spike check passed.");
        }

        public DataQualityResult ValidateCandles(IReadOnlyList<OhlcvBar>? candles)
        {
            if (candles == null || candles.Count == 0)
                return DataQualityResult.Fail("No candle data supplied.");

            if (candles.Count < 50)
                return DataQualityResult.Fail("Not enough candles. Minimum required is 50.");

            var ordered = candles
                .OrderBy(x => x.TimestampUtc)
                .ToList();

            var duplicateTimestamps = ordered
                .GroupBy(x => x.TimestampUtc)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateTimestamps.Count > 0)
                return DataQualityResult.Fail("Duplicate candle timestamps detected.");

            var invalidPrices = ordered.Any(x =>
                x.Open <= 0 ||
                x.High <= 0 ||
                x.Low <= 0 ||
                x.Close <= 0 ||
                x.High < x.Low ||
                x.High < x.Open ||
                x.High < x.Close ||
                x.Low > x.Open ||
                x.Low > x.Close);

            if (invalidPrices)
                return DataQualityResult.Fail("Invalid OHLC candle prices detected.");

            var latest = ordered[^1].TimestampUtc;
            var age = DateTime.UtcNow - latest;

            if (age.TotalDays > 7)
                return DataQualityResult.Fail($"Market data is stale. Latest candle is {latest:u}.");

            var gaps = 0;

            for (var i = 1; i < ordered.Count; i++)
            {
                var gap = ordered[i].TimestampUtc - ordered[i - 1].TimestampUtc;

                if (gap.TotalDays > 5)
                    gaps++;
            }

            if (gaps > 3)
                return DataQualityResult.Fail($"Too many candle gaps detected: {gaps}.");

            var recent = ordered.TakeLast(50).ToList();

            for (var i = 1; i < recent.Count; i++)
            {
                var previousClose = recent[i - 1].Close;
                var currentClose = recent[i].Close;

                if (previousClose <= 0)
                    continue;

                var movePercent = Math.Abs((currentClose - previousClose) / previousClose) * 100m;

                if (movePercent > 15m)
                {
                    return DataQualityResult.Fail(
                        $"Abnormal price spike detected: {movePercent:F2}% at {recent[i].TimestampUtc:u}.");
                }
            }

            return DataQualityResult.Pass(
                $"Data quality passed. Candles={ordered.Count}, From={ordered.First().TimestampUtc:u}, To={ordered.Last().TimestampUtc:u}");
        }
        
    }
}
