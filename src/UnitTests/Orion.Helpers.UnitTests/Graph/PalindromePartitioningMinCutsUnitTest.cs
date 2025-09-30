using Orion.Helpers.Graphs;

namespace Orion.Helpers.UnitTests.Graph
{
    public class PalindromePartitioningMinCutsClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            Assert.True(PalindromePartitioningMinCutsClass.PalindromePartitioningMinCuts("noonabbad") == 2);
        }
    }
}