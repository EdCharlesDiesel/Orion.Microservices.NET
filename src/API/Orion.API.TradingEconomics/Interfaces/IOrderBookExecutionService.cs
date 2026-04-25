using Orion.API.TradingEconomics.Entities;
namespace Orion.API.TradingEconomics.Interfaces;

public interface IOrderBookExecutionService
{
     ExecutionOrder Execute(OrderBook book, string direction, decimal size);
}