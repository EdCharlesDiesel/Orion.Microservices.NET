using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class SubarraySortUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] input = { 1, 2, 4, 7, 10, 11, 7, 12, 6, 7, 16, 18, 19 };
            int[] expected = { 3, 9 };
            int[] actual = SubarraySortClass.SubarraySort(input); // Call static method directly
            Assert.True(Enumerable.SequenceEqual(actual, expected));
        }
    }
}