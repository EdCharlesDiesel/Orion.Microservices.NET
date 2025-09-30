

namespace Orion.Helpers.UnitTests.Graph
{
    public class RiverSizesUnitTest1
    {
        [Fact]
        public void Test1()
        {
                    int[,] input = {
              { 1, 0, 0, 1, 0 },
              { 1, 0, 1, 0, 0 },
              { 0, 0, 1, 0, 1 },
              { 1, 0, 1, 0, 1 },
              { 1, 0, 1, 1, 0 },
            };
                    int[] expected = { 1, 2, 2, 2, 5 };
                    List<int> output = GenericClassAlgorithm.RiverSizes(input);
                    output.Sort();
                    Assert.True(Compare(output, expected));
                }
        

        public static bool Compare(List<int> arr1, int[] arr2)
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