namespace Orion.API.TradingEconomics.Entities
{
    public class MacroMonteCarloGenerator
    {
        private readonly Random _rand = new();

        public List<ProbabilisticScenario> Generate(int simulations)
        {
            var scenarios = new List<ProbabilisticScenario>();

            for (int i = 0; i < simulations; i++)
            {
                var shocks = new List<ScenarioShock>
            {
                GenerateShock("United States", "CPI", 0.02m),
                GenerateShock("United States", "Interest Rate", 0.5m),
                GenerateShock("Euro Area", "CPI", 0.015m)
            };

                scenarios.Add(new ProbabilisticScenario
                {
                    SimulationId = i,
                    Shocks = shocks
                });
            }

            return scenarios;
        }

        private ScenarioShock GenerateShock(string country, string indicator, decimal volatility)
        {
            // Gaussian shock
            var shock = NextGaussian(0, volatility);

            return new ScenarioShock
            {
                Country = country,
                Indicator = indicator,
                ShockValue = shock,
                Type = ShockType.Relative
            };
        }

        private decimal NextGaussian(decimal mean, decimal stdDev)
        {
            var u1 = 1.0 - _rand.NextDouble();
            var u2 = 1.0 - _rand.NextDouble();

            var randStdNormal =
                Math.Sqrt(-2.0 * Math.Log(u1)) *
                Math.Sin(2.0 * Math.PI * u2);

            return mean + stdDev * (decimal)randStdNormal;
        }
    }
}
