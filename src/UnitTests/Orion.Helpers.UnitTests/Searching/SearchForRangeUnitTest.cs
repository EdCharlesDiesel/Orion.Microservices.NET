using Orion.Helpers.Searching;

namespace Orion.Helpers.UnitTests.Searching
{
    public class SearchForRangeClassUnitTest1
    {
        [Fact]
        public void Test1()
        {
            int[] expected = { 4, 9 };
            int[] output = SearchForRangeClass.SearchForRange(
              new[] { 0, 1, 21, 33, 45, 45, 45, 45, 45, 45, 61, 71, 73 }, 45
            );
            Assert.True(Compare(output, expected));
        }

        public bool Compare(int[] arr1, int[] arr2)
        {
            if (arr1.Length != arr2.Length)
            {
                return false;
            }
            for (int i = 0; i < arr1.Length; i++)
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