using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;
using Orion.API.TradingEconomics.Services;

namespace Orion.API.TradingEconomics.Engine
{
    public abstract class AdvancedExecutionEngine(
        IOrderBookProvider orderBook,
        ILatencyModel latency,
        INewsEventService news)
    {
        private readonly OrderBookExecutionService _executor = new();

        public async Task<ExecutionResult> ExecuteAsync(string pair,string direction,decimal size)
        {
            // Step 1: Latency delay
            var latencyMs = await latency.SimulateLatencyMsAsync();
            await Task.Delay(TimeSpan.FromMilliseconds((long)latencyMs));

            // Step 2: News impact
            var isEvent = await news.IsHighImpactEventAsync(DateTime.UtcNow);

            if (isEvent)
            {
                // Reduce size or skip entirely
                size *= 0.5M;
            }

            // Step 3: Get order book AFTER latency (important)
            var book = await orderBook.GetOrderBookAsync(pair);

            // Step 4: Execute against depth
            var order = _executor.Execute(book, direction, size);

            var fillRatio = order.FilledSize / (order.RequestedSize + 1e-6M);

            return new ExecutionResult
            {
                Order = order,
                PartialFill = fillRatio < 1,
                FillRatio = fillRatio,
                LatencyMs = latencyMs,
                HighImpactEvent = isEvent
            };
        }
    }
}
