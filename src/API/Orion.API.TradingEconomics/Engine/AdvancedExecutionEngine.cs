using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;
namespace Orion.API.TradingEconomics.Engine;

/// <summary>
/// Provides advanced execution logic including:
/// - Latency simulation
/// - News-based risk adjustment
/// - Order book execution
/// </summary>
public abstract class AdvancedExecutionEngine(
    IOrderBookProvider orderBookProvider,
    ILatencyModel latencyModel,
    INewsEventService newsService,
    IOrderBookExecutionService executor): IAdvancedExecutionEngine
{
    private readonly IOrderBookProvider _orderBookProvider = orderBookProvider ?? throw new ArgumentNullException(nameof(orderBookProvider));
    private readonly ILatencyModel _latencyModel = latencyModel ?? throw new ArgumentNullException(nameof(latencyModel));
    private readonly INewsEventService _newsService = newsService ?? throw new ArgumentNullException(nameof(newsService));
    private readonly IOrderBookExecutionService _executor = executor ?? throw new ArgumentNullException(nameof(executor));

    /// <summary>
    /// Executes an order with realistic market conditions:
    /// latency, news impact, and order book depth.
    /// </summary>
    /// <param name="pair">Currency pair (e.g., EUR/USD)</param>
    /// <param name="direction">Trade direction (BUY/SELL)</param>
    /// <param name="size">Requested trade size</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Execution result with fill details and risk flags</returns>
    public async Task<ExecutionResult> ExecuteAsync(string pair, string direction, decimal size, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(pair))
            throw new ArgumentException("Pair is required.", nameof(pair));

        if (string.IsNullOrWhiteSpace(direction))
            throw new ArgumentException("Direction is required.", nameof(direction));

        if (size <= 0)
            throw new ArgumentException("Size must be greater than zero.", nameof(size));

        // 1. Simulate latency
        var latencyMs = await _latencyModel.SimulateLatencyMsAsync();
        await Task.Delay(TimeSpan.FromMilliseconds((long)latencyMs), cancellationToken);

        // 2. News impact adjustment
        var isHighImpactEvent = await _newsService.IsHighImpactEventAsync(DateTime.UtcNow);

        var adjustedSize = isHighImpactEvent ? size * 0.5M : size;

        // 3. Fetch order book AFTER latency
        var orderBook = await _orderBookProvider.GetOrderBookAsync(pair);

        if (orderBook == null)
            throw new InvalidOperationException("Order book not available.");

        // 4. Execute order
        var order = _executor.Execute(orderBook, direction, adjustedSize);

        // Avoid division issues
        var fillRatio = order.RequestedSize == 0
            ? 0
            : order.FilledSize / order.RequestedSize;

        return new ExecutionResult
        {
            Order = order,
            PartialFill = fillRatio < 1m,
            FillRatio = fillRatio,
            LatencyMs = latencyMs,
            HighImpactEvent = isHighImpactEvent
        };
    }
}