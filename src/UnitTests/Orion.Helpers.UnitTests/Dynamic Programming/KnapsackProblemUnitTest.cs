using Orion.Helpers.Dynamic_Programming;

namespace Orion.Helpers.UnitTests.Dynamic_Programming
{
    public class KnapsackProblemClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[,] input = { { 1, 2 }, { 4, 3 }, { 5, 6 }, { 6, 7 } };
            Tuple<int, int[]> expected = Tuple.Create(10, new[] { 1, 3 });
            Assert.True(Compare(KnapsackProblemClass.KnapsackProblem(input, 10), expected));
        }

        private static bool Compare(List<List<int>> arr1, Tuple<int, int[]> arr2)
        {
            if (arr1[0][0] != arr2.Item1)
            {
                return false;
            }
            if (arr1[1].Count != arr2.Item2.Length)
            {
                return false;
            }
            for (int i = 0; i < arr1[1].Count; i++)
            {
                if (arr1[1][i] != arr2.Item2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}