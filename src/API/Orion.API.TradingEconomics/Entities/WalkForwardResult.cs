namespace Orion.API.TradingEconomics.Entities;

public class WalkForwardResult
{
    public DateTime TrainStart { get; set; }
    public DateTime TrainEnd { get; set; }
    public DateTime TestStart { get; set; }
    public DateTime TestEnd { get; set; }
    public int TrainTradeCount { get; set; }
    public int TestTradeCount { get; set; }
    public List<TradeResult> TestTrades { get; set; }
}