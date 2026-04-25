namespace Orion.API.TradingEconomics.Entities
{
    public sealed class TradingSystemConfig
    {
        public LiveTradingConfig LiveTrading { get; set; } = new();
        public SignalConfig Signal { get; set; } = new();
        public RiskConfig Risk { get; set; } = new();
        public ExecutionConfig Execution { get; set; } = new();

        public PairConfig DefaultPairConfig { get; set; } = new();

        public Dictionary<string, PairConfig> Pairs { get; set; } = new();
    }
}
