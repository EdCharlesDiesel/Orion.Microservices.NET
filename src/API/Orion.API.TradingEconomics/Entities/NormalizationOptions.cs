namespace Orion.API.TradingEconomics.Entities
{
    public sealed class NormalizationOptions
    {
        public int MinimumWindowSize { get; set; } = 6;
        public bool WinsorizeOutliers { get; set; } = true;
        public double WinsorizeZLimit { get; set; } = 4.0;
    }
}
