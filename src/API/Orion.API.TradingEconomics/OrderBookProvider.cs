using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

public class OrderBookProvider: IOrderBookProvider
{
    public Task<OrderBook> GetOrderBookAsync(string pair)
    {
        throw new NotImplementedException();
    }
}