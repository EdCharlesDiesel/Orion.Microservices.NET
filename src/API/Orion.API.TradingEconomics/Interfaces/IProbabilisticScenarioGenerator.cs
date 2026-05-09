using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IProbabilisticScenarioGenerator
    {
        List<ProbabilisticScenario> Generate(int simulations);
        Task<List<SimulationResult>> RunAsync(List<ProbabilisticScenario> scenarios);
    }
}
