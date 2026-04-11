

namespace Orion.Helpers.UnitTests.Graph
{
    public class SquareOfZeroesUnitTest
    {
        [Fact]
        public void Test1()
        {
            List<List<int>> test = new List<List<int>>();
            test.Add(new List<int> { 1, 1, 1, 0, 1, 0 });
            test.Add(new List<int> { 0, 0, 0, 0, 0, 1 });
            test.Add(new List<int> { 0, 1, 1, 1, 0, 1 });
            test.Add(new List<int> { 0, 0, 0, 1, 0, 1 });
            test.Add(new List<int> { 0, 1, 1, 1, 0, 1 });
            test.Add(new List<int> { 0, 0, 0, 0, 0, 1 });
            Assert.True(GenericClassAlgorithm.SquareOfZeroes(test));
        }
    }
}