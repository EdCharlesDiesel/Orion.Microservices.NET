using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Helpers
{
    

    namespace Orion.API.TradingEconomics.Engine
    {
        public class ProbabilisticScenarioGenerator(ScenarioEngine scenarioEngine) : IProbabilisticScenarioGenerator
        {
            private readonly Random _rand = new();

            // -------------------------------
            // Generate probabilistic scenarios
            // -------------------------------
            public List<ProbabilisticScenario> Generate(int simulations)
            {
                var scenarios = new List<ProbabilisticScenario>();

                for (int i = 0; i < simulations; i++)
                {
                    scenarios.Add(new ProbabilisticScenario
                    {
                        SimulationId = i,
                        Shocks = new List<ScenarioShock>
                    {
                        GenerateShock("United States", "CPI", 0.02m),
                        GenerateShock("United States", "Interest Rate", 0.5m),
                        GenerateShock("Euro Area", "CPI", 0.015m)
                    }
                    });
                }

                return scenarios;
            }

            // -------------------------------
            // Run scenarios through pipeline
            // -------------------------------
            public async Task<List<SimulationResult>> RunAsync(List<ProbabilisticScenario> scenarios)
            {
                var results = new List<SimulationResult>();

                foreach (var s in scenarios)
                {
                    var scenario = new Scenario
                    {
                        Name = $"Simulation {s.SimulationId}",
                        Shocks = s.Shocks
                    };

                    var scenarioResult = await scenarioEngine.RunAsync(scenario);

                    var portfolioReturn = EstimateReturn(scenarioResult.Portfolio);
                    var risk = EstimateRisk(scenarioResult.Portfolio);

                    results.Add(new SimulationResult
                    {
                        SimulationId = s.SimulationId,
                        PortfolioReturn = portfolioReturn,
                        Risk = risk,
                        Portfolio = scenarioResult.Portfolio
                    });
                }

                return results;
            }

            // -------------------------------
            // Helpers
            // -------------------------------
            private ScenarioShock GenerateShock(string country, string indicator, decimal volatility)
            {
                var shock = (decimal)NextGaussian(0, (double)volatility);

                return new ScenarioShock
                {
                    Country = country,
                    Indicator = indicator,
                    ShockValue = shock,
                    Type = ShockType.Relative
                };
            }

            private double NextGaussian(double mean, double stdDev)
            {
                var u1 = 1.0 - _rand.NextDouble();
                var u2 = 1.0 - _rand.NextDouble();

                var randStdNormal =
                    Math.Sqrt(-2.0 * Math.Log(u1)) *
                    Math.Sin(2.0 * Math.PI * u2);

                return mean + stdDev * randStdNormal;
            }

            private decimal EstimateReturn(List<PortfolioPosition> portfolio)
            {
                if (portfolio == null || portfolio.Count == 0)
                    return 0;

                return portfolio.Sum(p => p.Weight * p.SignalStrength);
            }

            private decimal EstimateRisk(List<PortfolioPosition> portfolio)
            {
                if (portfolio == null || portfolio.Count == 0)
                    return 0;

                var variance = portfolio.Sum(p => p.Weight * p.Weight * p.Volatility);
                return (decimal)Math.Sqrt((double)variance);
            }
        }
    }
}
