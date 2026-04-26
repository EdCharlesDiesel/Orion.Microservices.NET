using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orion.API.TradingEconomics.Engine;
using Xunit;
namespace Orion.API.TradingEconomics.UnitTests.Engine;
public sealed class ConfigurationEngineTests
{
    [Fact]
    public void Constructor_WhenConfigurationIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ConfigurationEngine(null!));
    }

    [Fact]
    public void GetConfig_WhenSectionMissing_ReturnsDefaultConfig()
    {
        var engine = new ConfigurationEngine(CreateConfiguration(new Dictionary<string, string?>()));

        var config = engine.GetConfig();

        Assert.NotNull(config);
        Assert.NotNull(config.Pairs);
        Assert.NotNull(config.DefaultPairConfig);
        Assert.NotNull(config.LiveTrading);
        Assert.NotNull(config.Risk);
        Assert.NotNull(config.Signal);
        Assert.NotNull(config.Execution);
    }

    [Fact]
    public void GetPairConfig_WhenPairExists_ReturnsPairConfig()
    {
        var config = CreateConfiguration(new Dictionary<string, string?>
        {
            ["TradingSystem:Pairs:EUR/USD:Enabled"] = "true",
            ["TradingSystem:Pairs:USD/ZAR:Enabled"] = "false"
        });

        var engine = new ConfigurationEngine(config);

        var pairConfig = engine.GetPairConfig("eur/usd");

        Assert.True(pairConfig.Enabled);
    }

    [Fact]
    public void GetPairConfig_WhenPairMissing_ReturnsDefaultPairConfig()
    {
        var config = CreateConfiguration(new Dictionary<string, string?>
        {
            ["TradingSystem:DefaultPairConfig:Enabled"] = "true"
        });

        var engine = new ConfigurationEngine(config);

        var pairConfig = engine.GetPairConfig("GBP/USD");

        Assert.True(pairConfig.Enabled);
    }

    [Fact]
    public void GetPairConfig_WhenPairMissing_Throws()
    {
        var engine = new ConfigurationEngine(CreateConfiguration(new Dictionary<string, string?>()));

        Assert.Throws<ArgumentException>(() =>
            engine.GetPairConfig(""));
    }

    [Fact]
    public void IsLiveTradingEnabled_WhenEnabled_ReturnsTrue()
    {
        var config = CreateConfiguration(new Dictionary<string, string?>
        {
            ["TradingSystem:LiveTrading:Enabled"] = "true"
        });

        var engine = new ConfigurationEngine(config);

        Assert.True(engine.IsLiveTradingEnabled());
    }

    [Fact]
    public void IsPairEnabled_WhenPairDisabled_ReturnsFalse()
    {
        var config = CreateConfiguration(new Dictionary<string, string?>
        {
            ["TradingSystem:Pairs:USD/ZAR:Enabled"] = "false"
        });

        var engine = new ConfigurationEngine(config);

        Assert.False(engine.IsPairEnabled("USD/ZAR"));
    }

    private static IConfiguration CreateConfiguration(Dictionary<string, string?> values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }
}