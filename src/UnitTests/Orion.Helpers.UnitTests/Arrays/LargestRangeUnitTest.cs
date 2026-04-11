using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class LargestRangeClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] expected = { 0, 7 };
            Assert.True(LargestRangeClass.LargestRange(new[] { 1, 11, 3, 0, 15, 5, 2, 4, 10, 7, 12, 6 }
            ).SequenceEqual(expected
            ));
        }
    }
}