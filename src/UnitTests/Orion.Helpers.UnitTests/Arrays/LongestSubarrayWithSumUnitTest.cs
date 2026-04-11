using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class LongestSubarrayWithSumClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] array = new[] { 1, 2, 3, 4, 3, 3, 1, 2, 1 };
            int targetSum = 10;
            int[] expected = new[] { 4, 8 };
            var actual = new LongestSubarrayWithSumClass().LongestSubarrayWithSum(array, targetSum);
            Assert.True(expected.Length == actual.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                Assert.True(actual[i] == expected[i]);
            }
        }
    }
}