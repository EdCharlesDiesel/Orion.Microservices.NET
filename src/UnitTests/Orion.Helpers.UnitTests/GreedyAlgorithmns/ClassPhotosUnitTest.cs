using Orion.Helpers.GreedyAlgorithmns;

namespace Orion.Helpers.UnitTests.GreedyAlgorithmns
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            List<int> redShirtHeights = new List<int> { 5, 8, 1, 3, 4 };
            List<int> blueShirtHeights = new List<int> { 6, 9, 2, 4, 5 };
            bool expected = true;
            bool actual = new ClassPhotosClass().ClassPhotos(redShirtHeights, blueShirtHeights);
            Assert.True(expected == actual);
        }
    }
}