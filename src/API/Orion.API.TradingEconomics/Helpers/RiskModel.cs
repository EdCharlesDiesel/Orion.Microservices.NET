namespace Orion.API.TradingEconomics.Helpers
{
    public static class RiskModel
    {
        public static (decimal sl, decimal tp) Calculate(
            decimal close,
            decimal atr,
            string direction)
        {
            // Safety checks
            if (atr <= 0)
                atr = close * 0.005m; // fallback 0.5% volatility

            if (string.IsNullOrWhiteSpace(direction))
                throw new ArgumentException("Direction must be provided");

            direction = direction.ToUpperInvariant();

            // Risk parameters (tunable)
            const decimal slMultiplier = 1.5m;
            const decimal tpMultiplier = 3.0m;

            decimal stopLoss;
            decimal takeProfit;

            if (direction == "LONG")
            {
                stopLoss = close - (slMultiplier * atr);
                takeProfit = close + (tpMultiplier * atr);
            }
            else if (direction == "SHORT")
            {
                stopLoss = close + (slMultiplier * atr);
                takeProfit = close - (tpMultiplier * atr);
            }
            else
            {
                throw new ArgumentException($"Invalid direction: {direction}");
            }

            // Prevent invalid pricing
            stopLoss = Math.Max(0.00001m, stopLoss);
            takeProfit = Math.Max(0.00001m, takeProfit);

            return (stopLoss, takeProfit);
        }
    }
}