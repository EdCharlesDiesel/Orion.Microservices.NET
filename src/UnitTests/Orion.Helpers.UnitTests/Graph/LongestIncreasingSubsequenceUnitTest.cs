using Orion.Helpers.Graphs;

namespace Orion.Helpers.UnitTests.Graph
{
    public class LongestIncreasingSubsequenceClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] expected = { -24, 2, 3, 5, 6, 35 };
            Assert.True(compare(
              LongestIncreasingSubsequenceClass.LongestIncreasingSubsequence(
                new[] { 5, 7, -24, 12, 10, 2, 3, 12, 5, 6, 35 }
              ),
              expected
            ));
        }
        public static bool compare(List<int> arr1, int[] arr2)
        {
            if (arr1.Count != arr2.Length)
            {
                return false;
            }
            for (int i = 0; i < arr1.Count; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
