namespace Orion.API.TradingEconomics.Entities;

public sealed class RiskResult
{
    public bool IsAllowed { get; set; }
    public decimal Score { get; set; }
    public string Reason { get; set; } = "";
    public decimal StopLossDistance { get; set; }
    public decimal TakeProfitDistance { get; set; }

    public static RiskResult Allow(decimal score, string reason)
    {
        return new RiskResult
        {
            IsAllowed = true,
            Score = score,
            Reason = reason
        };
    }

    public static RiskResult Block(string reason, decimal score = 1.0m)
    {
        return new RiskResult
        {
            IsAllowed = false,
            Score = score,
            Reason = reason
        };
    }
}