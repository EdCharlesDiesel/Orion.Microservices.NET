using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;

namespace Orion.API.TradingEconomics.Engine;

/// <summary>
/// Validates trade requests against account state and real-time risk rules.
/// </summary>
public sealed class ComplianceEngine : IComplianceEngine
{
    /// <summary>
    /// Validates whether a trade request is compliant.
    /// </summary>
    public ComplianceResult Validate(
        string pair,
        string direction,
        decimal requestedSize,
        AccountSnapshot account,
        RealTimeRiskResult risk)
    {
        if (string.IsNullOrWhiteSpace(pair))
            throw new ArgumentException("Pair is required.", nameof(pair));

        if (string.IsNullOrWhiteSpace(direction))
            throw new ArgumentException("Direction is required.", nameof(direction));

        if (requestedSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(requestedSize), "Requested size must be greater than zero.");

        ArgumentNullException.ThrowIfNull(account);
        ArgumentNullException.ThrowIfNull(risk);

        var normalizedPair = pair.Trim().ToUpperInvariant();
        var normalizedDirection = direction.Trim().ToUpperInvariant();

        if (normalizedDirection is not "LONG" and not "SHORT")
            return Reject(normalizedPair, normalizedDirection, requestedSize, "Direction must be LONG or SHORT.");

        var violations = new List<string>();

        if (!risk.IsAllowed && risk.Violations is not null)
            violations.AddRange(risk.Violations);

        if (account.Balance <= 0)
            violations.Add("Account balance must be greater than zero.");

        if (account.Equity <= 0)
            violations.Add("Account equity must be greater than zero.");

        if (account.FreeMargin < 0)
            violations.Add("Free margin cannot be negative.");

        if (requestedSize > account.Equity)
            violations.Add("Requested size exceeds account equity.");

        var approved = violations.Count == 0;

        return new ComplianceResult
        {
            Pair = normalizedPair,
            Direction = normalizedDirection,
            RequestedSize = requestedSize,
            IsApproved = approved,
            Decision = approved ? ComplianceDecision.Approved : ComplianceDecision.Rejected,
            Violations = violations,
            TimestampUtc = DateTime.UtcNow
        };
    }

    private static ComplianceResult Reject(
        string pair,
        string direction,
        decimal requestedSize,
        string reason)
    {
        return new ComplianceResult
        {
            Pair = pair,
            Direction = direction,
            RequestedSize = requestedSize,
            IsApproved = false,
            Decision = ComplianceDecision.Rejected,
            Violations = new List<string> { reason },
            TimestampUtc = DateTime.UtcNow
        };
    }
}