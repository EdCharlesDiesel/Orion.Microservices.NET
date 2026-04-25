namespace Orion.API.TradingEconomics.Entities
{
    public class FxPriceSimulator
    {
        public List<FxPrice> Simulate(
            List<MacroState> states,
            Dictionary<string, decimal> initialPrices,
            CurrencyStrengthModel strengthModel,
            FxRelativePricer pricer)
        {
            var prices = new Dictionary<string, decimal>(initialPrices);
            var path = new List<FxPrice>();

            foreach (var state in states)
            {
                var strength = strengthModel.Compute(state);
                var returns = pricer.ComputeReturns(strength);

                foreach (var r in returns)
                {
                    if (!prices.ContainsKey(r.Pair))
                        prices[r.Pair] = 1.0m;

                    // Apply small scaling factor
                    var scaledReturn = 0.01m * r.Return;

                    prices[r.Pair] *= (1 + scaledReturn);

                    path.Add(new FxPrice
                    {
                        Pair = r.Pair,
                        Price = prices[r.Pair]
                    });
                }
            }

            return path;
        }
    }
}
