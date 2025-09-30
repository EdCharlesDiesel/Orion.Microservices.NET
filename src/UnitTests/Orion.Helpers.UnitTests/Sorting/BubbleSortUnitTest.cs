using Orion.Helpers.Sorting;

namespace Orion.Helpers.UnitTests.Sorting
{
    public class BubbleSortClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] expected = { 2, 3, 5, 5, 6, 8, 9 };
            int[] input = { 8, 5, 2, 9, 5, 6, 3 };
            Assert.True(Compare(BubbleSortClass.BubbleSort(input), expected));
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