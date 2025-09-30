using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class MedianOfTwoSortedArraysClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] arrayOne = new[] { 1, 3, 4, 5 };
            int[] arrayTwo = new[] { 2, 3, 6, 7 };
            float actual = new MedianOfTwoSortedArraysClass().MedianOfTwoSortedArrays(arrayOne, arrayTwo);
            float expected = 3.5f;
            Assert.True(expected == actual);
        }
    }
}