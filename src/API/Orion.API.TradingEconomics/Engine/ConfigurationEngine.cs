using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Engine;

/// <summary>
/// Provides typed access to trading system configuration.
/// </summary>
public sealed class ConfigurationEngine : IConfigurationEngine
{
    private readonly TradingSystemConfig _config;

    public ConfigurationEngine(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _config = configuration
            .GetSection("TradingSystem")
            .Get<TradingSystemConfig>() ?? new TradingSystemConfig();

        _config.Pairs ??= new Dictionary<string, PairConfig>();
        _config.DefaultPairConfig ??= new PairConfig();
        _config.LiveTrading ??= new LiveTradingConfig();
        _config.Risk ??= new RiskConfig();
        _config.Signal ??= new SignalConfig();
        _config.Execution ??= new ExecutionConfig();
    }

    /// <summary>
    /// Returns the full trading system configuration.
    /// </summary>
    public TradingSystemConfig GetConfig() => _config;

    /// <summary>
    /// Returns pair-specific configuration or the default pair configuration.
    /// </summary>
    public PairConfig GetPairConfig(string pair)
    {
        if (string.IsNullOrWhiteSpace(pair))
            throw new ArgumentException("Pair is required.", nameof(pair));

        var normalizedPair = pair.Trim().ToUpperInvariant();

        return _config.Pairs.TryGetValue(normalizedPair, out var pairConfig)
            ? pairConfig
            : _config.DefaultPairConfig;
    }

    /// <summary>
    /// Returns whether live trading is enabled.
    /// </summary>
    public bool IsLiveTradingEnabled() => _config.LiveTrading.Enabled;

    /// <summary>
    /// Returns whether a pair is enabled for trading.
    /// </summary>
    public bool IsPairEnabled(string pair) => GetPairConfig(pair).Enabled;

    /// <summary>
    /// Returns risk configuration.
    /// </summary>
    public RiskConfig GetRiskConfig() => _config.Risk;

    /// <summary>
    /// Returns signal configuration.
    /// </summary>
    public SignalConfig GetSignalConfig() => _config.Signal;

    /// <summary>
    /// Returns execution configuration.
    /// </summary>
    public ExecutionConfig GetExecutionConfig() => _config.Execution;
}