namespace Orion.API.TradingEconomics.Entities
{
    public class CorrelatedShockGenerator
    {
        private readonly Random _rand = new();

        // Simplified covariance matrix
        private readonly decimal[,] _cov =
        {
        { 0.02m,  0.01m, -0.005m }, // inflation
        { 0.01m,  0.03m, -0.002m }, // rates
        { -0.005m, -0.002m, 0.025m } // growth
    };

        public (decimal inf, decimal rate, decimal growth) Generate()
        {
            // Simplified correlated draws (approximation)
            var z1 = NextGaussian();
            var z2 = NextGaussian();
            var z3 = NextGaussian();

            var inf = 0.02m * (decimal)z1;
            var rate = 0.01m * (decimal)z1 + 0.02m * (decimal)z2;
            var growth = -0.005m * (decimal)z1 + 0.01m * (decimal)z3;

            return (inf, rate, growth);
        }

        private decimal NextGaussian()
        {
            var u1 = 1.0 - _rand.NextDouble();
            var u2 = 1.0 - _rand.NextDouble();
            return (decimal)(Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2));
        }
    }
}
