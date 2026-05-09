using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Engine
{
    public class ProbabilisticScenarioEngine(ScenarioEngine scenarioEngine) : IProbabilisticScenarioEngine
    {
        public async Task<List<SimulationResult>> RunAsync(List<ProbabilisticScenario> scenarios)
        {
            var results = new List<SimulationResult>();

            foreach (var s in scenarios)
            {
                var scenario = new Scenario
                {
                    Name = $"Sim {s.SimulationId}",
                    Shocks = s.Shocks
                };

                var result = await scenarioEngine.RunAsync(scenario);

                var portfolioReturn = EstimateReturn(result.Portfolio);

                results.Add(new SimulationResult
                {
                    SimulationId = s.SimulationId,
                    PortfolioReturn = portfolioReturn,
                    Risk = EstimateRisk(result.Portfolio),
                    Portfolio = result.Portfolio
                });
            }

            return results;
        }

        internal ProbabilisticScenarioResult Calculate(NormalizedIndicator normalized, RegimeResult regime,
            ScenarioResult scenario)
        {
            throw new NotImplementedException();
        }

        private decimal EstimateReturn(List<PortfolioPosition> portfolio)
        {
            return portfolio.Sum(p => p.Weight * p.SignalStrength);
        }

        private decimal EstimateRisk(List<PortfolioPosition> portfolio)
        {
            return (decimal)Math.Sqrt((double)portfolio.Sum(p => p.Weight * p.Weight * p.Volatility));
        }
    }
}