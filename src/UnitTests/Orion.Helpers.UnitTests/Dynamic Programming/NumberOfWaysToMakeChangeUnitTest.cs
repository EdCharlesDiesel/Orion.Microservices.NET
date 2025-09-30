using Orion.Helpers.Dynamic_Programming;

namespace Orion.Helpers.UnitTests.Dynamic_Programming
{
    public class NumberOfWaysToMakeChangeClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] input = { 1, 5 };
            Assert.True(NumberOfWaysToMakeChangeClass.NumberOfWaysToMakeChange(6, input) == 2);
        }
    }
}