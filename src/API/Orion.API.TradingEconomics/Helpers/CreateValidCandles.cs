using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Helpers;

public static  class CreateValid
{
    private static List<OhlcvBar> CreateValidCandles(int count = 60)
    {
        var candles = new List<OhlcvBar>();
        var start = DateTime.UtcNow.AddDays(-count);

        decimal price = 1.1000m;

        for (int i = 0; i < count; i++)
        {
            var open = price;
            var close = price + 0.0005m;
            var high = Math.Max(open, close) + 0.0005m;
            var low = Math.Min(open, close) - 0.0005m;

            candles.Add(new OhlcvBar
            {
                TimestampUtc = start.AddDays(i), // ✅ no gaps
                Open = open,
                High = high,
                Low = low,
                Close = close,
                Volume = 1000
            });

            price = close;
        }

        return candles;
    }
}