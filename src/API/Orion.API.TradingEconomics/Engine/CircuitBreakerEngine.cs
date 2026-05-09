using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine;

/// <summary>
/// Evaluates trading constraints such as risk limits, open exposure, and data validity.
/// </summary>
public sealed class CircuitBreakerEngine(ConfigurationEngine config) : ICircuitBreakerEngine
{
    private readonly ConfigurationEngine _config = config ?? throw new ArgumentNullException(nameof(config));

    /// <summary>
    /// Evaluates whether trading should continue or be halted.
    /// </summary>
    public CircuitBreakerResult Evaluate(
        AccountContext account,
        List<TradePlan>? todayTrades,
        List<TradePlan>? openTrades,
        DataQualityResult? dataQuality)
    {
        ArgumentNullException.ThrowIfNull(account);

        todayTrades ??= [];
        openTrades ??= [];

        if (account.Equity <= 0)
            return CircuitBreakerResult.Tripped("Invalid account equity.");

        if (dataQuality is { IsValid: false })
            return CircuitBreakerResult.Tripped($"Data quality failed: {dataQuality.Reason}");

        var cfg = _config.GetConfig().LiveTrading;

        var dailyPnL = todayTrades
            .Where(x => string.Equals(x.Status, "CLOSED", StringComparison.OrdinalIgnoreCase))
            .Sum(x => x.ProfitLoss);

        var dailyLossPercent = dailyPnL < 0
            ? Math.Abs(dailyPnL) / account.Equity * 100m
            : 0m;

        if (dailyLossPercent >= cfg.MaxDailyLossPercent)
            return CircuitBreakerResult.Tripped($"Max daily loss reached: {dailyLossPercent:F2}%.");

        var openCount = openTrades.Count(x =>
            string.Equals(x.Status, "OPEN", StringComparison.OrdinalIgnoreCase));

        if (openCount >= cfg.MaxOpenTrades)
            return CircuitBreakerResult.Tripped($"Max open trades reached: {openCount}.");

        var openRisk = openTrades.Sum(CalculateOpenRisk);
        var openRiskPercent = account.Equity == 0 ? 0 : openRisk / account.Equity * 100m;

        if (openRiskPercent >= cfg.MaxDailyLossPercent)
            return CircuitBreakerResult.Tripped($"Open portfolio risk too high: {openRiskPercent:F2}%.");

        return CircuitBreakerResult.Clear(
            $"Circuit clear. DailyLoss={dailyLossPercent:F2}%, OpenRisk={openRiskPercent:F2}%.");
    }

    private static decimal CalculateOpenRisk(TradePlan trade)
    {
        if (!string.Equals(trade.Status, "OPEN", StringComparison.OrdinalIgnoreCase))
            return 0;

        if (trade.PositionSize <= 0 || trade.EntryPrice <= 0 || trade.StopLoss <= 0)
            return 0;

        return trade.Direction?.ToUpperInvariant() switch
        {
            "LONG" => Math.Abs(trade.EntryPrice - trade.StopLoss) * trade.PositionSize,
            "SHORT" => Math.Abs(trade.StopLoss - trade.EntryPrice) * trade.PositionSize,
            _ => 0
        };
    }
}