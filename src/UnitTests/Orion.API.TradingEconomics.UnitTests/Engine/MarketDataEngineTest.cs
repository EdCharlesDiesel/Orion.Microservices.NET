using Microsoft.Extensions.Logging;
using Moq;
using Orion.API.TradingEconomics.DTO;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public sealed class MarketDataEngineTests
    {
        private readonly Mock<IFredService> _fredService = new();
        private readonly Mock<ILogger<MarketDataEngine>> _logger = new();

        private MarketDataEngine CreateEngine()
        {
            return new MarketDataEngine(_fredService.Object, _logger.Object);
        }

        [Fact]
        public async Task GetMacroDataAsync_ShouldReturnMacroData()
        {
            var expected = new MacroData();

            _fredService
                .Setup(x => x.GetMacroDataAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var engine = CreateEngine();

            var result = await engine.GetMacroDataAsync();

            Assert.Same(expected, result);
        }

        [Fact]
        public async Task RefreshMacroDataAsync_ShouldReturnMacroData()
        {
            var expected = new MacroData();

            _fredService
                .Setup(x => x.RefreshMacroDataAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var engine = CreateEngine();

            var result = await engine.RefreshMacroDataAsync();

            Assert.Same(expected, result);
        }

        [Fact]
        public void GetFredSeriesMappings_ShouldReturnMappings()
        {
            var mappings = new Dictionary<string, Dictionary<string, string>>
            {
                ["USD"] = new()
                {
                    ["InterestRate"] = "FEDFUNDS"
                }
            };

            _fredService
                .Setup(x => x.GetFredSeriesMappings())
                .Returns(mappings);

            var engine = CreateEngine();

            var result = engine.GetFredSeriesMappings();

            Assert.Same(mappings, result);
        }

        [Fact]
        public async Task CheckStatusAsync_ShouldReturnFredStatus()
        {
            var expected = new FredStatusResponse();

            _fredService
                .Setup(x => x.CheckStatusAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var engine = CreateEngine();

            var result = await engine.CheckStatusAsync();

            Assert.Same(expected, result);
        }

        [Fact]
        public async Task GetHistoricalCandlesAsync_ShouldReturnEmptyList()
        {
            var engine = CreateEngine();

            var request = new MarketDataRequest
            {
                Pair = "EURUSD"
            };

            var result = await engine.GetHistoricalCandlesAsync(request);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetHistoricalCandlesAsync_ShouldThrow_WhenRequestIsNull()
        {
            var engine = CreateEngine();

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                engine.GetHistoricalCandlesAsync(null!));
        }

        [Fact]
        public async Task GetHistoricalCandlesAsync_ShouldThrow_WhenPairIsMissing()
        {
            var engine = CreateEngine();

            var request = new MarketDataRequest
            {
                Pair = ""
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                engine.GetHistoricalCandlesAsync(request));
        }

        [Fact]
        public async Task GetLatestQuoteAsync_ShouldReturnNull()
        {
            var engine = CreateEngine();

            var result = await engine.GetLatestQuoteAsync("EURUSD");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetLatestQuoteAsync_ShouldThrow_WhenPairIsMissing()
        {
            var engine = CreateEngine();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                engine.GetLatestQuoteAsync(""));
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnHealthyResult()
        {
            var engine = CreateEngine();

            var result = await engine.CheckHealthAsync("eurusd");

            Assert.Equal("EURUSD", result.Pair);
            Assert.True(result.IsHealthy);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldThrow_WhenPairIsMissing()
        {
            var engine = CreateEngine();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                engine.CheckHealthAsync(""));
        }
    }
}