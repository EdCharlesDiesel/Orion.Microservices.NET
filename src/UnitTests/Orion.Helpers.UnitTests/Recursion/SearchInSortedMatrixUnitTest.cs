namespace Orion.Helpers.UnitTests.Recursion
{
    public class UnitTest1
    {
        [Fact(Skip = "Not Implemented yet")]
        public void Test1()
        {
            int[,] matrix =
            {
                { 1, 4, 7, 12, 15, 1000 },
                { 2, 5, 19, 31, 32, 1001 },
                { 3, 8, 24, 33, 35, 1002 },
                { 40, 41, 42, 44, 45, 1003 },
                { 99, 100, 103, 106, 128, 1004 },
            };

            int[] expected = { 3, 3 };
            int[] output = GenericClassAlgorithm.SearchInSortedMatrix(matrix, 44);
            Assert.True(Compare(output, expected));
        }


        public static bool Compare(int[] arr1, int[] arr2)
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