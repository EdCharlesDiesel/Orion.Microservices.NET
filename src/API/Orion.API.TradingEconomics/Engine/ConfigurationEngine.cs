using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine
{


    public sealed class ConfigurationEngine(IConfiguration configuration)
    {
        private readonly TradingSystemConfig _config = configuration
            .GetSection("TradingSystem")
            .Get<TradingSystemConfig>() ?? new TradingSystemConfig();

        public TradingSystemConfig GetConfig()
        {
            return _config;
        }

        public PairConfig GetPairConfig(string pair)
        {
            if (_config.Pairs.TryGetValue(pair.ToUpperInvariant(), out var config))
                return config;

            return _config.DefaultPairConfig;
        }

        public bool IsLiveTradingEnabled()
        {
            return _config.LiveTrading.Enabled;
        }

        public bool IsPairEnabled(string pair)
        {
            var pairConfig = GetPairConfig(pair);

            return pairConfig.Enabled;
        }

        public RiskConfig GetRiskConfig()
        {
            return _config.Risk;
        }

        public SignalConfig GetSignalConfig()
        {
            return _config.Signal;
        }

        public ExecutionConfig GetExecutionConfig()
        {
            return _config.Execution;
        }
    }
}
