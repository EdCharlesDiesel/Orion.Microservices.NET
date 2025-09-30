using Orion.Helpers.GreedyAlgorithmns;

namespace Orion.Helpers.UnitTests.GreedyAlgorithmns
{
    public class MinimumWaitingTimeClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] queries = new[] { 3, 2, 1, 2, 6 };
            int expected = 17;
            var actual = new MinimumWaitingTimeClass().MinimumWaitingTime(queries);
            Assert.True(actual == expected);
        }
    }
}