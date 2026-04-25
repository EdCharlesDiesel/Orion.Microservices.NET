using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IScenarioEngine
    {
        Task<ScenarioResult> RunAsync(Scenario scenario);
    }
}
