using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Enum;

namespace Orion.API.TradingEconomics.Helpers
{
    public static class FrequencyResolver
    {
        public static IndicatorFrequency Resolve(string? frequency)
        {
            if (string.IsNullOrWhiteSpace(frequency))
                return IndicatorFrequency.Monthly;

            return frequency.Trim().ToLowerInvariant() switch
            {
                "daily" => IndicatorFrequency.Daily,
                "weekly" => IndicatorFrequency.Weekly,
                "monthly" => IndicatorFrequency.Monthly,
                "quarterly" => IndicatorFrequency.Quarterly,
                "yearly" or "annual" => IndicatorFrequency.Yearly,
                _ => IndicatorFrequency.Monthly
            };
        }

        public static int YoYLag(IndicatorFrequency frequency)
        {
            return frequency switch
            {
                IndicatorFrequency.Daily => 252,
                IndicatorFrequency.Weekly => 52,
                IndicatorFrequency.Monthly => 12,
                IndicatorFrequency.Quarterly => 4,
                IndicatorFrequency.Yearly => 1,
                _ => 12
            };
        }

        public static int DefaultRollingWindow(IndicatorFrequency frequency)
        {
            return frequency switch
            {
                IndicatorFrequency.Daily => 252,
                IndicatorFrequency.Weekly => 52,
                IndicatorFrequency.Monthly => 36,
                IndicatorFrequency.Quarterly => 20,
                IndicatorFrequency.Yearly => 10,
                _ => 36
            };
        }
    }
}
