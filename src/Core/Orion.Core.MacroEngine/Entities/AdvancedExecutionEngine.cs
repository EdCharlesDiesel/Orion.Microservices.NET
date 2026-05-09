using Orion.Core.MacroEngine.Interfaces;
using Orion.Core.MacroEngine.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Entities
{
    public class AdvancedExecutionEngine
    {
        private readonly IOrderBookProvider _orderBook;
        private readonly ILatencyModel _latency;
        private readonly INewsEventService _news;

        private readonly OrderBookExecutionService _executor = new();

        public AdvancedExecutionEngine(
            IOrderBookProvider orderBook,
            ILatencyModel latency,
            INewsEventService news)
        {
            _orderBook = orderBook;
            _latency = latency;
            _news = news;
        }

        public async Task<ExecutionResult> ExecuteAsync(
            string pair,
            string direction,
            double size)
        {
            // Step 1: Latency delay
            var latencyMs = await _latency.SimulateLatencyMsAsync();
            await Task.Delay(TimeSpan.FromMilliseconds(latencyMs));

            // Step 2: News impact
            var isEvent = await _news.IsHighImpactEventAsync(DateTime.UtcNow);

            if (isEvent)
            {
                // Reduce size or skip entirely
                size *= 0.5;
            }

            // Step 3: Get order book AFTER latency (important)
            var book = await _orderBook.GetOrderBookAsync(pair);

            // Step 4: Execute against depth
            var order = _executor.Execute(book, direction, size);

            var fillRatio = order.FilledSize / (order.RequestedSize + 1e-6);

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
