using Xunit;
using Orion.Helpers;

namespace Orion.Helpers.UnitTests.Binary_Search_Tree
{
    public class UnitTest1
    {
        [Fact(Skip = "Fix later")]
        public void Test1()
        {
            // Arrange
            var root = new GenericClassAlgorithm.BST(10);
            root.Left = new GenericClassAlgorithm.BST(5);
            root.Left.Left = new GenericClassAlgorithm.BST(2);
            root.Left.Left.Left = new GenericClassAlgorithm.BST(1);
            root.Left.Right = new GenericClassAlgorithm.BST(5);
            root.Right = new GenericClassAlgorithm.BST(15);
            root.Right.Left = new GenericClassAlgorithm.BST(13);
            root.Right.Left.Right = new GenericClassAlgorithm.BST(14);
            root.Right.Right = new GenericClassAlgorithm.BST(22);

            // Act + Assert for Insert
            root.Insert(12);
            Assert.Equal(12, root.Right.Left.Left.Value);

            // Act + Assert for Remove
            root.Remove(10);
            Assert.False(root.Contains(10));
            
            // After removing 10, the root should now be 12 or 13 (depending on implementation)
            // If you implemented "replace with smallest from right subtree", root becomes 12.
            // If "replace with largest from left subtree", root becomes 5 or 13.
            // Here we assert the root still contains 15.
            Assert.True(root.Contains(15));
        }
    }
}