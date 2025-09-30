using Orion.Helpers.Graphs;

namespace Orion.Helpers.UnitTests.Graph
{
    public class MaxProfitWithKTransactionsClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] input = { 5, 11, 3, 50, 60, 90 };
            Assert.Equal(93, MaxProfitWithKTransactionsClass.MaxProfitWithKTransactions(input, 2));
        }
    }
}