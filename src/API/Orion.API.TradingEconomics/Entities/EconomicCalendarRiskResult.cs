namespace Orion.API.TradingEconomics.Entities
{
    public sealed class EconomicCalendarRiskResult
    {
        public bool IsBlocked { get; set; }
        public string Reason { get; set; } = "";
        public bool IsClear { get; set; }

        public static EconomicCalendarRiskResult Clear(string reason)
        {
            return new EconomicCalendarRiskResult
            {
                IsBlocked = false,
                Reason = reason
            };
        }

        public static EconomicCalendarRiskResult Block(string reason)
        {
            return new EconomicCalendarRiskResult
            {
                IsBlocked = true,
                Reason = reason
            };
        }

        public bool Message(char obj)
        {
            throw new NotImplementedException();
        }
    }
}
