using Orion.Helpers.Binary_Search_Tree;

namespace Orion.Helpers.UnitTests.Binary_Search_Tree
{
    public class FindClosestValueInBstClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var root = new FindClosestValueInBstClass.Bst(10);
            root.Left = new FindClosestValueInBstClass.Bst(5);
            root.Left.Left = new FindClosestValueInBstClass.Bst(2);
            root.Left.Left = new FindClosestValueInBstClass.Bst(1);
            root.Left.Right = new FindClosestValueInBstClass.Bst(5);
            root.Right = new FindClosestValueInBstClass.Bst(15);
            root.Right.Left = new FindClosestValueInBstClass.Bst(13);
            root.Right.Left.Right = new FindClosestValueInBstClass.Bst(14);
            root.Right.Right = new FindClosestValueInBstClass.Bst(22);

            var expected = 13;
            var actual = FindClosestValueInBstClass.FindClosestValueInBst(root, 12);
            Assert.Equal(expected, actual);
        }
    }
}