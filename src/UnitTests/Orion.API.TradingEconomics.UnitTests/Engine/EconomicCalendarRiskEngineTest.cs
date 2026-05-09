using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;
using Xunit;

namespace Orion.API.TradingEconomics.UnitTests.Engine
{
    public class EconomicCalendarRiskEngineTests
    {
        private readonly EconomicCalendarRiskEngine _engine;

        public EconomicCalendarRiskEngineTests()
        {
            _engine = new EconomicCalendarRiskEngine();
        }

        [Fact]
        public void Evaluate_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _engine.Evaluate(null!, DateTime.UtcNow));
        }

        [Fact]
        public void Evaluate_EmptyPair_ThrowsArgumentException()
        {
            var input = new ForexMarketInput { Pair = "", MacroEvents = new List<MacroEvent>() };
            Assert.Throws<ArgumentException>(() =>
                _engine.Evaluate(input, DateTime.UtcNow));
        }

        [Fact]
        public void Evaluate_NullMacroEvents_ReturnsClearResult()
        {
            var input = new ForexMarketInput { Pair = "EUR/USD", MacroEvents = null };
            var result = _engine.Evaluate(input, DateTime.UtcNow);
            Assert.True(result.IsClear);
            Assert.Contains("No macro events", result.Message);
        }

        [Fact]
        public void Evaluate_EmptyMacroEvents_ReturnsClearResult()
        {
            var input = new ForexMarketInput { Pair = "EUR/USD", MacroEvents = new List<MacroEvent>() };
            var result = _engine.Evaluate(input, DateTime.UtcNow);
            Assert.True(result.IsClear);
            Assert.Contains("No macro events", result.Message);
        }

        [Fact]
        public void Evaluate_NoMatchingCurrency_ReturnsClearResult()
        {
            var events = new List<MacroEvent>
            {
                CreateMacroEvent("GBP", "CPI", "HIGH", DateTime.UtcNow.AddHours(1))
            };
            var input = new ForexMarketInput { Pair = "EUR/USD", MacroEvents = events };
            var result = _engine.Evaluate(input, DateTime.UtcNow);
            Assert.True(result.IsClear);
            Assert.Contains("No high-impact", result.Message);
        }

        [Fact]
        public void Evaluate_LowImpactEvent_ReturnsClearResult()
        {
            var events = new List<MacroEvent>
            {
                CreateMacroEvent("USD", "Retail Sales", "LOW", DateTime.UtcNow.AddHours(1))
            };
            var input = new ForexMarketInput { Pair = "EUR/USD", MacroEvents = events };
            var result = _engine.Evaluate(input, DateTime.UtcNow);
            Assert.True(result.IsBlocked);
        }

        [Fact]
        public void Evaluate_HighImpactByFlag_ReturnsBlockResult()
        {
            var eventTime = DateTime.UtcNow.AddHours(1);
            var events = new List<MacroEvent>
            {
                CreateMacroEvent("USD", "FOMC Statement", "HIGH", eventTime)
            };
            var input = new ForexMarketInput { Pair = "EUR/USD", MacroEvents = events };
            var result = _engine.Evaluate(input, DateTime.UtcNow);
            Assert.True(result.IsBlocked);
            Assert.Contains("USD", result.Message);
            Assert.Contains("FOMC", result.Message);
        }

        [Theory]
        [InlineData("CPI")]
        [InlineData("NFP")]
        [InlineData("Nonfarm Payrolls")]
        [InlineData("Interest Rate")]
        [InlineData("GDP")]
        [InlineData("Unemployment")]
        [InlineData("PMI")]
        [InlineData("Fed")]
        [InlineData("ECB")]
        public void Evaluate_HighImpactByKeyword_ReturnsBlockResult(string keyword)
        {
            var eventTime = DateTime.UtcNow.AddHours(1);
            var events = new List<MacroEvent>
            {
                CreateMacroEvent("EUR", keyword, "MEDIUM", eventTime)
            };
            var input = new ForexMarketInput { Pair = "EUR/USD", MacroEvents = events };
            var result = _engine.Evaluate(input, DateTime.UtcNow);
            Assert.True(result.IsBlocked);
        }

        [Theory]
        [InlineData(-31, true)]  // 31 min before - outside window
        [InlineData(-30, false)] // 30 min before - inside window
        [InlineData(0, false)]   // at event time - inside
        [InlineData(45, false)]  // after - inside
        [InlineData(90, false)]  // exactly at limit - inside
        [InlineData(91, true)]   // outside window
        public void Evaluate_RespectsRiskWindow(int minutesOffset, bool shouldBeClear)
        {
            var eventTime = DateTime.UtcNow.AddMinutes(minutesOffset);
            var events = new List<MacroEvent>
            {
                CreateMacroEvent("USD", "CPI", "HIGH", eventTime)
            };
            var input = new ForexMarketInput { Pair = "EUR/USD", MacroEvents = events };
            var result = _engine.Evaluate(input, DateTime.UtcNow);
            Assert.Equal(shouldBeClear, result.IsClear);
        }

        [Fact]
        public void Evaluate_SelectsNearestEvent()
        {
            var now = DateTime.UtcNow;
            var events = new List<MacroEvent>
            {
                CreateMacroEvent("USD", "CPI", "HIGH", now.AddMinutes(60)),
                CreateMacroEvent("USD", "NFP", "HIGH", now.AddMinutes(30)),
                CreateMacroEvent("USD", "FOMC", "HIGH", now.AddMinutes(90))
            };
            var input = new ForexMarketInput { Pair = "EUR/USD", MacroEvents = events };
            var result = _engine.Evaluate(input, now);
            Assert.True(result.IsBlocked);
            Assert.Contains("NFP", result.Message);
        }

        [Fact]
        public void Evaluate_HandlesForwardSlashPairFormat()
        {
            var events = new List<MacroEvent>
            {
                CreateMacroEvent("JPY", "BoJ", "HIGH", DateTime.UtcNow.AddHours(1))
            };
            var input = new ForexMarketInput { Pair = "USD/JPY", MacroEvents = events };
            var result = _engine.Evaluate(input, DateTime.UtcNow);
            Assert.True(result.IsBlocked);
        }

        [Fact]
        public void Evaluate_CaseInsensitiveCurrencyMatching()
        {
            var events = new List<MacroEvent>
            {
                CreateMacroEvent("usd", "CPI", "HIGH", DateTime.UtcNow.AddHours(1))
            };
            var input = new ForexMarketInput { Pair = "EUR/USD", MacroEvents = events };
            var result = _engine.Evaluate(input, DateTime.UtcNow);
            Assert.True(result.IsBlocked);
        }

        [Fact]
        public void Evaluate_MultipleCurrencies_BothCurrenciesChecked()
        {
            var events = new List<MacroEvent>
            {
                CreateMacroEvent("EUR", "ECB", "HIGH", DateTime.UtcNow.AddHours(1)),
                CreateMacroEvent("GBP", "BoE", "HIGH", DateTime.UtcNow.AddHours(2))
            };
            var input = new ForexMarketInput { Pair = "EUR/GBP", MacroEvents = events };
            var result = _engine.Evaluate(input, DateTime.UtcNow);
            Assert.True(result.IsBlocked);
        }

        [Fact]
        public void Evaluate_EventInPastWithinWindow_StillDetected()
        {
            var eventTime = DateTime.UtcNow.AddMinutes(-15); // 15 minutes ago
            var events = new List<MacroEvent>
            {
                CreateMacroEvent("USD", "CPI", "HIGH", eventTime)
            };
            var input = new ForexMarketInput { Pair = "EUR/USD", MacroEvents = events };
            var result = _engine.Evaluate(input, DateTime.UtcNow);
            Assert.True(result.IsBlocked);
        }

        private static MacroEvent CreateMacroEvent(string currency, string name, string impact, DateTime eventTime)
        {
            return new MacroEvent
            {
                Currency = currency,
                Name = name,
                Impact = impact,
                EventTimeUtc = eventTime
            };
        }
    }
}