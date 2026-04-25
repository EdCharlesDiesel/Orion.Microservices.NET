namespace Orion.API.TradingEconomics.Entities
{
    public class MacroTransitionModel
    {
        public MacroState Next(MacroState prev,
            (decimal inf, decimal rate, decimal growth) shock,
            MarketRegime regime)
        {
            var state = new MacroState
            {
                Inflation = prev.Inflation + shock.inf,
                InterestRate = prev.InterestRate + shock.rate,
                Growth = prev.Growth + shock.growth,
                RiskSentiment = UpdateRisk(prev.RiskSentiment, regime)
            };

            ApplyFeedback(state);

            return state;
        }
     
        private decimal UpdateRisk(decimal prev, MarketRegime regime)
        {
            return regime switch
            {
                MarketRegime.RiskOn => Math.Min(1, prev + 0.1m),
                MarketRegime.RiskOff => Math.Max(-1, prev - 0.1m),
                _ => prev
            };
        }

        private void ApplyFeedback(MacroState state)
        {
            // Inflation → Rates (central bank reaction)
            state.InterestRate += 0.5m * state.Inflation;

            // Rates → Growth (tightening slows growth)
            state.Growth -= 0.3m * state.InterestRate;

            // Risk sentiment → FX (simplified)
            foreach (var ccy in state.CurrencyStrength.Keys.ToList())
            {
                state.CurrencyStrength[ccy] += state.RiskSentiment * 0.05m;
            }
        }
    }
}