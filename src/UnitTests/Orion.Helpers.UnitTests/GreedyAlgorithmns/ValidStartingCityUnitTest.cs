using Orion.Helpers.GreedyAlgorithmns;

namespace Orion.Helpers.UnitTests.GreedyAlgorithmns
{
    public class ValidStartingCityClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] distances = new[] { 5, 25, 15, 10, 15 };
            int[] fuel = new[] { 1, 2, 1, 0, 3 };
            int mpg = 10;
            int expected = 4;
            var actual = new ValidStartingCityClass().ValidStartingCity(distances, fuel, mpg);
          Assert.True(expected == actual);
        }
    }
}