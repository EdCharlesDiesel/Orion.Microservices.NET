namespace Orion.API.TradingEconomics.Entities;

public enum RiskAction
{
    AllowTrade = 0,
    BlockTrade = 1,
    ReducePosition = 2,
    ClosePosition = 3,
    EmergencyFlatten = 4
}