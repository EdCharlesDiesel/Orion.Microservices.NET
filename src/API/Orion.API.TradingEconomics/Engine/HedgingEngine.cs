using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;


    namespace Orion.API.TradingEconomics.Engine
    {
        public sealed class HedgingEngine : IHedgingEngine
        {
            public Task<HedgingResult> AnalyzeAsync(HedgingRequest request,CancellationToken cancellationToken = default)
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                if (request.OpenPositions.Count == 0)
                {
                    return Task.FromResult(new HedgingResult
                    {
                        HedgeStatus = "NO_HEDGE_REQUIRED",
                        RiskSummary = "No open positions supplied.",
                        TimestampUtc = DateTime.UtcNow
                    });
                }

                var positions = request.OpenPositions
                    .Where(x => !string.IsNullOrWhiteSpace(x.Pair) && x.NotionalUsd > 0)
                    .ToList();

                if (positions.Count == 0)
                    throw new ArgumentException("No valid open positions supplied.", nameof(request));

                decimal longExposure = 0m;
                decimal shortExposure = 0m;

                foreach (var position in positions)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var direction = NormalizeDirection(position.Direction);

                    if (direction == "LONG")
                        longExposure += position.NotionalUsd;

                    if (direction == "SHORT")
                        shortExposure += position.NotionalUsd;
                }

                var grossExposure = longExposure + shortExposure;
                var netExposure = longExposure - shortExposure;

                if (grossExposure <= 0)
                    throw new ArgumentException("Gross exposure must be greater than zero.", nameof(request));

                var hedgeRequired = CalculateHedgeRequired(
                    netExposure,
                    grossExposure,
                    request.MaxHedgePercent);

                var recommendations = BuildRecommendations(
                    request.HedgeCandidates,
                    netExposure,
                    hedgeRequired,
                    cancellationToken);

                return Task.FromResult(new HedgingResult
                {
                    NetLongExposureUsd = Math.Round(longExposure, 2),
                    NetShortExposureUsd = Math.Round(shortExposure, 2),
                    NetExposureUsd = Math.Round(netExposure, 2),
                    GrossExposureUsd = Math.Round(grossExposure, 2),
                    HedgeRequiredUsd = Math.Round(hedgeRequired, 2),
                    Recommendations = recommendations,
                    HedgeStatus = ResolveHedgeStatus(hedgeRequired, grossExposure),
                    RiskSummary = ResolveRiskSummary(netExposure, grossExposure, recommendations),
                    TimestampUtc = DateTime.UtcNow
                });
            }

            private static List<HedgeRecommendation> BuildRecommendations(
                List<HedgeCandidate>? candidates,
                decimal netExposure,
                decimal hedgeRequired,
                CancellationToken cancellationToken)
            {
                if (candidates == null || candidates.Count == 0 || hedgeRequired <= 0)
                    return new List<HedgeRecommendation>();

                var hedgeDirection = netExposure > 0 ? "SHORT" : "LONG";

                return candidates
                    .Where(x => !string.IsNullOrWhiteSpace(x.Pair))
                    .Where(x => Math.Abs(x.CorrelationToPortfolio) >= 0.30m)
                    .OrderByDescending(x => ScoreCandidate(x))
                    .Take(3)
                    .Select(candidate =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var hedgeRatio = CalculateHedgeRatio(candidate);
                        var hedgeSize = hedgeRequired * hedgeRatio;

                        return new HedgeRecommendation
                        {
                            Pair = candidate.Pair,
                            Direction = hedgeDirection,
                            HedgeSizeUsd = Math.Round(hedgeSize, 2),
                            HedgeRatio = Math.Round(hedgeRatio, 4),
                            CorrelationUsed = Math.Round(candidate.CorrelationToPortfolio, 4),
                            Confidence = Math.Round(ScoreCandidate(candidate), 2),
                            Reason = BuildReason(candidate, hedgeDirection)
                        };
                    })
                    .ToList();
            }

            private static decimal CalculateHedgeRequired(
                decimal netExposure,
                decimal grossExposure,
                decimal maxHedgePercent)
            {
                var absNetExposure = Math.Abs(netExposure);

                if (absNetExposure <= 0)
                    return 0m;

                var maxAllowedHedge = grossExposure * Clamp(maxHedgePercent, 0m, 100m) / 100m;

                return Math.Min(absNetExposure, maxAllowedHedge);
            }

            private static decimal CalculateHedgeRatio(HedgeCandidate candidate)
            {
                var correlationStrength = Math.Abs(candidate.CorrelationToPortfolio);
                var liquidityAdjustment = Clamp(candidate.LiquidityScore / 100m, 0.25m, 1m);
                var volatilityAdjustment = candidate.VolatilityPercent <= 0
                    ? 1m
                    : Clamp(1m / candidate.VolatilityPercent, 0.25m, 1m);

                return Clamp(correlationStrength * liquidityAdjustment * volatilityAdjustment, 0.10m, 1m);
            }

            private static decimal ScoreCandidate(HedgeCandidate candidate)
            {
                var correlationScore = Math.Abs(candidate.CorrelationToPortfolio) * 60m;
                var liquidityScore = Clamp(candidate.LiquidityScore, 0m, 100m) * 0.30m;
                var volatilityPenalty = Clamp(candidate.VolatilityPercent, 0m, 100m) * 0.10m;

                return Clamp(correlationScore + liquidityScore - volatilityPenalty, 0m, 100m);
            }

            private static string BuildReason(HedgeCandidate candidate, string direction)
            {
                return $"{direction} hedge selected because {candidate.Pair} has " +
                       $"{ResolveCorrelationDescription(candidate.CorrelationToPortfolio)} correlation, " +
                       $"liquidity score {candidate.LiquidityScore}, and volatility {candidate.VolatilityPercent}%.";
            }

            private static string ResolveCorrelationDescription(decimal correlation)
            {
                var abs = Math.Abs(correlation);

                return abs switch
                {
                    >= 0.80m => "very strong",
                    >= 0.60m => "strong",
                    >= 0.40m => "moderate",
                    _ => "weak"
                };
            }

            private static string ResolveHedgeStatus(decimal hedgeRequired, decimal grossExposure)
            {
                if (hedgeRequired <= 0)
                    return "NO_HEDGE_REQUIRED";

                var hedgePercent = hedgeRequired / grossExposure * 100m;

                return hedgePercent switch
                {
                    >= 40m => "HEDGE_REQUIRED_HIGH",
                    >= 20m => "HEDGE_REQUIRED_MODERATE",
                    _ => "HEDGE_REQUIRED_LOW"
                };
            }

            private static string ResolveRiskSummary(
                decimal netExposure,
                decimal grossExposure,
                List<HedgeRecommendation> recommendations)
            {
                var netExposurePercent = grossExposure > 0
                    ? Math.Abs(netExposure) / grossExposure * 100m
                    : 0m;

                if (netExposurePercent >= 60m && recommendations.Count == 0)
                    return "HIGH_UNHEDGED_DIRECTIONAL_EXPOSURE";

                if (netExposurePercent >= 60m)
                    return "HIGH_DIRECTIONAL_EXPOSURE_HEDGE_AVAILABLE";

                if (netExposurePercent >= 30m)
                    return "MODERATE_DIRECTIONAL_EXPOSURE";

                return "PORTFOLIO_EXPOSURE_BALANCED";
            }

            private static string NormalizeDirection(string direction)
            {
                if (string.IsNullOrWhiteSpace(direction))
                    return "UNKNOWN";

                return direction.Trim().ToUpperInvariant() switch
                {
                    "BUY" => "LONG",
                    "LONG" => "LONG",
                    "BULLISH" => "LONG",

                    "SELL" => "SHORT",
                    "SHORT" => "SHORT",
                    "BEARISH" => "SHORT",

                    _ => "UNKNOWN"
                };
            }

            private static decimal Clamp(decimal value, decimal min, decimal max)
            {
                if (value < min)
                    return min;

                if (value > max)
                    return max;

                return value;
            }
        }
    }

