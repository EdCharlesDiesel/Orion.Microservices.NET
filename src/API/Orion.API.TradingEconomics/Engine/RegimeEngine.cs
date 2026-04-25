using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{
    public class RegimeEngine
    {
        private readonly Random _rand = new();

        private readonly Dictionary<MarketRegime, Dictionary<MarketRegime, decimal>> _transition =
            new()
            {
                [MarketRegime.RiskOn] = new()
                {
                    [MarketRegime.RiskOn] = 0.7m,
                    [MarketRegime.RiskOff] = 0.2m,
                    [MarketRegime.Stagflation] = 0.1m
                },
                [MarketRegime.RiskOff] = new()
                {
                    [MarketRegime.RiskOff] = 0.6m,
                    [MarketRegime.RiskOn] = 0.2m,
                    [MarketRegime.Stagflation] = 0.2m   
                }
            };

        public MarketRegime Next(MarketRegime current)
        {
            var probs = _transition[current];
            var roll = _rand.NextDouble();
            decimal cum = 0;

            foreach (var kv in probs)
            {
                cum += kv.Value;
                if ((decimal)roll <= cum)
                    return kv.Key;
            }

            return current;
        }

        internal RegimeResult Detect(NormalizedIndicator normalized)
        {
            throw new NotImplementedException();
        }
    }
}
