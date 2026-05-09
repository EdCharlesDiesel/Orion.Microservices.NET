
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces
{
    /// <summary>
    /// Executes orders using market data or order book.
    /// </summary>
    public interface IExecutionEngine
    {
        Task<ExecutionOrder> ExecuteAsync(string pair, string direction, decimal size, CancellationToken cancellationToken = default);

        ExecutionOrder Execute(OrderBook orderBook, string direction, decimal size);
    }
}