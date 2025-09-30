using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class ZeroSumSubarrayClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { 4, 2, -1, -1, 3 };
            var expected = true;
            var actual = new ZeroSumSubarrayClass().ZeroSumSubarray(input);
            Assert.True(expected == actual);
        }
    }
}