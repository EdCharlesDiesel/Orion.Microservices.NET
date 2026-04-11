using Orion.Helpers.Binary_Trees;

namespace Orion.Helpers.UnitTests.Binary_Trees
{
    public class NodeDepthsClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var root = new NodeDepthsClass.BinaryTree(1);
            root.Left = new NodeDepthsClass.BinaryTree(2);
            root.Left.Left = new NodeDepthsClass.BinaryTree(4);
            root.Left.Left.Left = new NodeDepthsClass.BinaryTree(8);
            root.Left.Left.Right = new NodeDepthsClass.BinaryTree(9);
            root.Left.Right = new NodeDepthsClass.BinaryTree(5);
            root.Right = new NodeDepthsClass.BinaryTree(3);
            root.Right.Left = new NodeDepthsClass.BinaryTree(6);
            root.Right.Right = new NodeDepthsClass.BinaryTree(7);
            int actual = NodeDepthsClass.NodeDepths(root);
            Assert.Equal(16, actual);
        }
    }
}