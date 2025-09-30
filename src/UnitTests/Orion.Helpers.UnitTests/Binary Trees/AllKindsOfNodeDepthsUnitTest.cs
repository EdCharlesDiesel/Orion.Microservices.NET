using Orion.Helpers.Binary_Trees;

namespace Orion.Helpers.UnitTests.Binary_Trees
{
    public class AllKindsOfNodeDepthsUnitTest
    {
        [Fact(Skip = "Fix later")]
        public void Test1()
        {
            var root = new AllKindsOfNodeDepths.AllKindsOfNodeDepthsClassBinaryTree(1);
            root.Left = new AllKindsOfNodeDepths.AllKindsOfNodeDepthsClassBinaryTree(2);
            root.Left.Left = new AllKindsOfNodeDepths.AllKindsOfNodeDepthsClassBinaryTree(4);
            root.Left.Left.Left = new AllKindsOfNodeDepths.AllKindsOfNodeDepthsClassBinaryTree(8);
            root.Left.Left.Right = new AllKindsOfNodeDepths.AllKindsOfNodeDepthsClassBinaryTree(9);
            root.Left.Right = new AllKindsOfNodeDepths.AllKindsOfNodeDepthsClassBinaryTree(5);
            root.Right = new AllKindsOfNodeDepths.AllKindsOfNodeDepthsClassBinaryTree(3);
            root.Right.Left = new AllKindsOfNodeDepths.AllKindsOfNodeDepthsClassBinaryTree(6);
            root.Right.Right = new AllKindsOfNodeDepths.AllKindsOfNodeDepthsClassBinaryTree(7);
            var actual = AllKindsOfNodeDepthsClass.AllKindsOfNodeDepths(root);
            Assert.Equal(26, actual);
        }
    }

    public abstract class AllKindsOfNodeDepths
    {
        public class AllKindsOfNodeDepthsClassBinaryTree : Helpers.Binary_Trees.AllKindsOfNodeDepthsClassBinaryTree
        {
            public AllKindsOfNodeDepthsClassBinaryTree(int i) : base()
            {
                throw new NotImplementedException();
            }
        }
    }
}