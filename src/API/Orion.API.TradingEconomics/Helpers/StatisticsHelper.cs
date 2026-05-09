namespace Orion.API.TradingEconomics.Helpers
{
    public static class StatisticsHelper
    {
        public static double ZScore(IEnumerable<double> values, double current)
        {
            var list = values.ToList();
            var mean = list.Average();
            var std = Math.Sqrt(list.Sum(v => Math.Pow(v - mean, 2)) / list.Count);

            return std == 0 ? 0 : (current - mean) / std;
        }
    }
}
