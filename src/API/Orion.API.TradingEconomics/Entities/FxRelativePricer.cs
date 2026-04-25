namespace Orion.API.TradingEconomics.Entities
{
    public class FxRelativePricer
    {
        public List<FxReturn> ComputeReturns(Dictionary<string, decimal> strength)
        {
            var result = new List<FxReturn>();
            var currencies = strength.Keys.ToList();

            for (int i = 0; i < currencies.Count; i++)
            {
                for (int j = i + 1; j < currencies.Count; j++)
                {
                    var a = currencies[i];
                    var b = currencies[j];

                    var diff = strength[a] - strength[b];

                    result.Add(new FxReturn
                    {
                        Pair = $"{a}/{b}",
                        Return = diff
                    });
                }
            }

            return result;
        }
    }
}
