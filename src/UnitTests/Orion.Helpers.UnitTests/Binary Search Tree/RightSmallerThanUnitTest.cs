

namespace Orion.Helpers.UnitTests.Binary_Search_Tree
{
    public class RightSmallerThanUnitTest
    {
        [Fact]
        public void SequenceEqualUnitTest()
        {
            var array = new List<int> { 8, 5, 11, -1, 3, 4, 2 };
            var expected = new List<int> { 5, 4, 4, 0, 1, 1, 0 };
            var actual = GenericClassAlgorithm.RightSmallerThan(array);
            Assert.True(expected.SequenceEqual(actual));
        }
    }
}