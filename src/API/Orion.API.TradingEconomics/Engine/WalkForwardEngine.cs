namespace Orion.API.TradingEconomics.Engine
{
    public class WalkForwardEngine(BacktestEngine engine)
    {
        public async Task RunAsync(DateTime start, DateTime end)
        {
            var window = TimeSpan.FromDays(90);

            for (var t = start; t < end - window; t += window)
            {
                var trainStart = t;
                var trainEnd = t.AddDays(60);

                var testStart = trainEnd;
                var testEnd = trainEnd.AddDays(30);

                // Train (optimize parameters - placeholder)
                var trainTrades = await engine.RunAsync(trainStart, trainEnd, 100000);

                // Test (out-of-sample)
                var testTrades = await engine.RunAsync(testStart, testEnd, 100000);

                Console.WriteLine($"WF Window: {testStart} → {testEnd}");
                Console.WriteLine($"Trades: {testTrades.Count}");
            }
        }
    }
}
