using Orion.Helpers.Binary_Search_Tree;

namespace Orion.Helpers.UnitTests.Binary_Search_Tree
{
    public class BtsTraversalClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var root = new BTSTraversalClass.BST(10);
            root.Left = new BTSTraversalClass.BST(5);
            root.Left.Left = new BTSTraversalClass.BST(2);
            root.Left.Left.Left = new BTSTraversalClass.BST(1);
            root.Left.Right = new BTSTraversalClass.BST(5);
            root.Right = new BTSTraversalClass.BST(15);
            root.Right.Right = new BTSTraversalClass.BST(22);

            List<int> inOrder = new List<int> { 1, 2, 5, 5, 10, 15, 22 };
            List<int> preOrder = new List<int> { 10, 5, 2, 1, 5, 15, 22 };
            List<int> postOrder = new List<int> { 1, 2, 5, 5, 22, 15, 10 };

            Assert.True(BTSTraversalClass.InOrderTraverse(root, new List<int>()).SequenceEqual(inOrder
            ));
            Assert.True(BTSTraversalClass.PreOrderTraverse(root, new List<int>()).SequenceEqual(preOrder
            ));
            Assert.True(BTSTraversalClass.PostOrderTraverse(root, new List<int>()).SequenceEqual(postOrder
            ));
        }
    }
}