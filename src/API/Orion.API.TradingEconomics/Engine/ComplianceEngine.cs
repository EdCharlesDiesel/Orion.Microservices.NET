

using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;

namespace Orion.API.TradingEconomics.Engine
{
    public sealed class ComplianceEngine
    {
        public ComplianceResult Validate(string pair, string direction, decimal requestedSize, AccountSnapshot account, RealTimeRiskResult risk)
        {
            if (string.IsNullOrWhiteSpace(pair))
                throw new ArgumentException("Pair is required.", nameof(pair));

            if (string.IsNullOrWhiteSpace(direction))
                throw new ArgumentException("Direction is required.", nameof(direction));

            if (requestedSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(requestedSize), "Requested size must be greater than zero.");

            if (account == null)
                throw new ArgumentNullException(nameof(account));

            if (risk == null)
                throw new ArgumentNullException(nameof(risk));

            var normalizedDirection = direction.Trim().ToUpperInvariant();

            if (normalizedDirection is not "LONG" and not "SHORT")
                return Fail(pair, normalizedDirection, "Direction must be LONG or SHORT.");

            var violations = new List<string>();

            if (!risk.IsAllowed)
                violations.AddRange(risk.Violations);

            if (account.Balance <= 0)
                violations.Add("Account balance must be greater than zero.");

            if (account.Equity <= 0)
                violations.Add("Account equity must be greater than zero.");

            if (account.FreeMargin < 0)
                violations.Add("Free margin cannot be negative.");

            if (requestedSize > account.Equity)
                violations.Add("Requested size exceeds account equity.");

            var isApproved = violations.Count == 0;

            return new ComplianceResult
            {
                Pair = pair.Trim().ToUpperInvariant(),
                Direction = normalizedDirection,
                RequestedSize = requestedSize,
                IsApproved = isApproved,
                Decision = isApproved
                    ? ComplianceDecision.Approved
                    : ComplianceDecision.Rejected,
                Violations = violations,
                TimestampUtc = DateTime.UtcNow
            };
        }

        private static ComplianceResult Fail(string pair, string direction, string reason)
        {
            return new ComplianceResult
            {
                Pair = pair,
                Direction = direction,
                IsApproved = false,
                Decision = ComplianceDecision.Rejected,
                Violations = new List<string> { reason },
                TimestampUtc = DateTime.UtcNow
            };
        }
    }
}