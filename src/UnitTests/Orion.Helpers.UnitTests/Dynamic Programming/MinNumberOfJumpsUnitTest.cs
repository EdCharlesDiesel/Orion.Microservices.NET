using Orion.Helpers.LinkedList;

namespace Orion.Helpers.UnitTests.Dynamic_Programming
{
    public class MinNumberOfJumpsClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] input = { 3, 4, 2, 1, 2, 3, 7, 1, 1, 1, 3 };
            Assert.True(MinNumberOfJumpsClass.MinNumberOfJumps(input) == 4);
        }
    }
}