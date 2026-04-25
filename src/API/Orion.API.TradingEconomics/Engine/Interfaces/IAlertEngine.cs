using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine.Interfaces;

public interface IAlertEngine
{
    List<TradingAlert> Evaluate(TradingDecision? decision);
}