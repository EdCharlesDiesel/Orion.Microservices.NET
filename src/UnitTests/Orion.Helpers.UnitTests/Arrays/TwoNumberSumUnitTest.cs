using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class TwoNumberSumUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] output = new TwoNumberSumClass().TwoNumberSum(new[] { 3, 5, -4, 8, 11, 1, -1, 6 }, 10);
            Assert.True(output.Length == 2);
            Assert.True(Array.Exists(output, e => e == -1));
            Assert.True(Array.Exists(output, e => e == 11));
        }
    }
}