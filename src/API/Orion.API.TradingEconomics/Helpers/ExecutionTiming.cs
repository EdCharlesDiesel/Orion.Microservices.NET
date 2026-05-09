namespace Orion.API.TradingEconomics.Helpers
{
    public static class ExecutionTiming
    {
        public static bool IsGoodLiquidityWindow(DateTime utcNow)
        {
            var hour = utcNow.Hour;

            // London + NY overlap
            return hour >= 12 && hour <= 16;
        }
    }
}
