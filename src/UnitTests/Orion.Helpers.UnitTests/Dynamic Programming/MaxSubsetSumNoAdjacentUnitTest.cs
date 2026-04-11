using Orion.Helpers.Dynamic_Programming;

namespace Orion.Helpers.UnitTests.Dynamic_Programming
{
    public class MaxSubsetSumNoAdjacentClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] input = { 75, 105, 120, 75, 90, 135 };
            Assert.True(MaxSubsetSumNoAdjacentClass.MaxSubsetSumNoAdjacent(input) == 330);
        }
    }
}