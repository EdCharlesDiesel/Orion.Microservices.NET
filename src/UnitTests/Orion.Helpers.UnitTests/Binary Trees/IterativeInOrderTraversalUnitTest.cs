using Orion.Helpers.Binary_Trees;

namespace Orion.Helpers.UnitTests.Binary_Trees
{
    public partial class UnitTest1
    {
        private List<int> testArray = new List<int>();

        private void testCallback(int value)
        {
            testArray.Add(value);
        }

        [Fact]
        public void IterativeInorderTraversalClassTest1()
        {
            // Build tree
            var root = new IterativeInorderTraversalClass.BinaryTree(1);
            root.Left = new IterativeInorderTraversalClass.BinaryTree(2, root);
            root.Left.Left = new IterativeInorderTraversalClass.BinaryTree(4, root.Left);
            root.Left.Left.Right = new IterativeInorderTraversalClass.BinaryTree(9, root.Left.Left);
            root.Right = new IterativeInorderTraversalClass.BinaryTree(3, root);
            root.Right.Left = new IterativeInorderTraversalClass.BinaryTree(6, root.Right);
            root.Right.Right = new IterativeInorderTraversalClass.BinaryTree(7, root.Right);

            // Clear test array
            testArray.Clear();

            // Act
            IterativeInorderTraversalClass.IterativeInOrderTraversal(root, testCallback);

            // Assert
            var expected = new List<int> { 4, 9, 2, 1, 6, 3, 7 };
            Assert.True(Enumerable.SequenceEqual(testArray, expected));
        }
    }
}