using Orion.Helpers.Binary_Search_Tree;

namespace Orion.Helpers.UnitTests.Binary_Search_Tree
{
    public class SameBsTsClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            List<int> arrayOne = new List<int> { 10, 15, 8, 12, 94, 81, 5, 2, 11 };
            List<int> arrayTwo = new List<int> { 10, 8, 5, 15, 2, 12, 11, 94, 81 };
            Assert.True(SameBsTsClass.SameBsts(arrayOne, arrayTwo));
        }
    }
}