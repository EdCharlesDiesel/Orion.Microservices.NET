using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IProbabilisticScenarioEngine
    {
        Task<List<SimulationResult>> RunAsync(List<ProbabilisticScenario> scenarios);
    }
}
