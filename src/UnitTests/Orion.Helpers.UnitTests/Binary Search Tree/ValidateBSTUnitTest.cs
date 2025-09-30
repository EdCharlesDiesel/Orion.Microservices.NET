

using Orion.Helpers.Binary_Search_Tree;

namespace Orion.Helpers.UnitTests.Binary_Search_Tree
{
    public class ValidateBstClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var root = new ValidateBstClass.BST(10);
            root.Left = new ValidateBstClass.BST(5);
            root.Left.Left = new ValidateBstClass.BST(2);
            root.Left.Left.Left = new ValidateBstClass.BST(1);
            root.Left.Right = new ValidateBstClass.BST(5);
            root.Right = new ValidateBstClass.BST(15);
            root.Right.Left = new ValidateBstClass.BST(13);
            root.Right.Left.Right = new ValidateBstClass.BST(14);
            root.Right.Right = new ValidateBstClass.BST(22);

            Assert.True(ValidateBstClass.ValidateBST(root));
        }
    }
}