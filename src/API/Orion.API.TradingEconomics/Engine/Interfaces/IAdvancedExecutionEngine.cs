using Orion.API.TradingEconomics.Entities;
namespace Orion.API.TradingEconomics.Engine.Interfaces;

public interface IAdvancedExecutionEngine
{
    Task<ExecutionResult> ExecuteAsync(string pair, string direction, decimal size, CancellationToken cancellationToken = default);
}