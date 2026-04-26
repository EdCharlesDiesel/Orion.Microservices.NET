using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    /// <summary>
    /// Provides liquidity analysis for bid/ask order book data.
    /// </summary>
    public sealed class LiquidityEngine : ILiquidityEngine
    {
        /// <inheritdoc />
        public Task<LiquidityResult> AnalyzeAsync(
            LiquidityRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.Pair))
                throw new ArgumentException("Pair is required.", nameof(request));

            if (request.Bids == null || request.Bids.Count == 0)
                throw new ArgumentException("Bid levels are required.", nameof(request));

            if (request.Asks == null || request.Asks.Count == 0)
                throw new ArgumentException("Ask levels are required.", nameof(request));

            cancellationToken.ThrowIfCancellationRequested();

            var bids = request.Bids
                .Where(x => x.Price > 0 && x.Volume > 0)
                .OrderByDescending(x => x.Price)
                .ToList();

            var asks = request.Asks
                .Where(x => x.Price > 0 && x.Volume > 0)
                .OrderBy(x => x.Price)
                .ToList();

            if (bids.Count == 0)
                throw new ArgumentException("No valid bid levels supplied.", nameof(request));

            if (asks.Count == 0)
                throw new ArgumentException("No valid ask levels supplied.", nameof(request));

            var bestBid = bids[0].Price;
            var bestAsk = asks[0].Price;

            if (bestAsk <= bestBid)
                throw new ArgumentException("Invalid order book: best ask must be greater than best bid.", nameof(request));

            var midPrice = (bestBid + bestAsk) / 2m;
            var spread = bestAsk - bestBid;
            var spreadPercent = spread / midPrice * 100m;

            var bidDepth = bids.Sum(x => x.Volume);
            var askDepth = asks.Sum(x => x.Volume);
            var totalDepth = bidDepth + askDepth;

            var imbalance = totalDepth > 0
                ? (bidDepth - askDepth) / totalDepth
                : 0m;

            var estimatedSlippagePercent = EstimateSlippagePercent(
                asks,
                request.RequestedSize,
                midPrice);

            return Task.FromResult(new LiquidityResult
            {
                Pair = request.Pair.Trim().ToUpperInvariant(),
                BestBid = bestBid,
                BestAsk = bestAsk,
                Spread = Math.Round(spread, 6),
                SpreadPercent = Math.Round(spreadPercent, 4),
                BidDepth = Math.Round(bidDepth, 4),
                AskDepth = Math.Round(askDepth, 4),
                TotalDepth = Math.Round(totalDepth, 4),
                Imbalance = Math.Round(imbalance, 4),
                EstimatedSlippagePercent = Math.Round(estimatedSlippagePercent, 4),
                LiquidityGrade = ResolveLiquidityGrade(spreadPercent, totalDepth, estimatedSlippagePercent),
                RiskSummary = ResolveRiskSummary(spreadPercent, estimatedSlippagePercent, imbalance),
                TimestampUtc = DateTime.UtcNow
            });
        }

        private static decimal EstimateSlippagePercent(
            IReadOnlyCollection<OrderBookLevel> asks,
            decimal requestedSize,
            decimal midPrice)
        {
            if (requestedSize <= 0 || midPrice <= 0)
                return 0m;

            var remaining = requestedSize;
            var totalCost = 0m;
            var filled = 0m;

            foreach (var level in asks)
            {
                if (remaining <= 0)
                    break;

                var take = Math.Min(remaining, level.Volume);

                totalCost += take * level.Price;
                filled += take;
                remaining -= take;
            }

            if (filled <= 0)
                return 0m;

            var averageFillPrice = totalCost / filled;

            return Math.Max(0m, (averageFillPrice - midPrice) / midPrice * 100m);
        }

        private static string ResolveLiquidityGrade(
            decimal spreadPercent,
            decimal totalDepth,
            decimal slippagePercent)
        {
            if (spreadPercent <= 0.02m && totalDepth >= 1_000_000m && slippagePercent <= 0.02m)
                return "INSTITUTIONAL";

            if (spreadPercent <= 0.05m && totalDepth >= 250_000m && slippagePercent <= 0.05m)
                return "HIGH";

            if (spreadPercent <= 0.15m && totalDepth >= 50_000m && slippagePercent <= 0.15m)
                return "MODERATE";

            if (spreadPercent <= 0.30m && totalDepth >= 10_000m && slippagePercent <= 0.30m)
                return "LOW";

            return "THIN";
        }

        private static string ResolveRiskSummary(
            decimal spreadPercent,
            decimal slippagePercent,
            decimal imbalance)
        {
            if (spreadPercent > 0.30m || slippagePercent > 0.30m)
                return "HIGH_LIQUIDITY_RISK";

            if (Math.Abs(imbalance) >= 0.60m)
                return "ORDER_BOOK_IMBALANCE_RISK";

            if (spreadPercent > 0.15m || slippagePercent > 0.15m)
                return "MODERATE_LIQUIDITY_RISK";

            return "LIQUIDITY_ACCEPTABLE";
        }
    }
}