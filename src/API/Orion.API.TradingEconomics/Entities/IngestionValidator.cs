using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Entities
{
    public class IngestionValidator : IIngestionValidator
    {
        public bool IsValid(EconomicIndicator i)
        {
            if (i == null) return false;
            if (string.IsNullOrWhiteSpace(i.Country)) return false;
            if (string.IsNullOrWhiteSpace(i.Indicator)) return false;
            if (i.Date == default) return false;

            // remove junk/empty economic prints
            if (i.Value == null && i.Forecast == null && i.Previous == null)
                return false;

            return true;
        }
    }
}
