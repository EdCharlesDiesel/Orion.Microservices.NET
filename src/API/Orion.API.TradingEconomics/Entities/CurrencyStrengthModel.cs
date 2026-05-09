namespace Orion.API.TradingEconomics.Entities
{
    public class CurrencyStrengthModel(List<CurrencyModel> models)
    {
        public Dictionary<string, decimal> Compute(MacroState state)
        {
            var result = new Dictionary<string, decimal>();

            foreach (var m in models)
            {
                var strength =
                    m.CarryWeight * state.InterestRate +
                    m.GrowthWeight * state.Growth -
                    m.InflationWeight * state.Inflation +
                    m.RiskWeight * state.RiskSentiment;

                result[m.Currency] = strength;
            }

            return result;
        }
    }
}
