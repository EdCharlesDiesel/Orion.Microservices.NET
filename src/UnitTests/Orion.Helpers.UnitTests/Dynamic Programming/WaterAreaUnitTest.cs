using Orion.Helpers.Dynamic_Programming;

namespace Orion.Helpers.UnitTests.Dynamic_Programming
{
    public class WaterAreaClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] input = { 0, 8, 0, 0, 5, 0, 0, 10, 0, 0, 1, 1, 0, 3 };
            Assert.True(WaterAreaClass.WaterArea(input) == 48);
        }
    }
}