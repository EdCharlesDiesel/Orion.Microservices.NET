

namespace Orion.Helpers.UnitTests.Binary_Search_Tree
{
    public class ValidateBstUnitTest1
    {
        [Fact(Skip = "Fix later")]
        public void Test1()
        {
            var array = new List<int> { 1, 2, 5, 7, 10, 13, 14, 15, 22 };
            GenericClassAlgorithm.BST tree = GenericClassAlgorithm.MinHeightBST(array);

            Assert.True(ValidateBst(tree));
            Assert.Equal(4, GetTreeHeight(tree));

            var inOrder = InOrderTraverse(tree, new List<int>());
            var expected = new List<int> { 1, 2, 5, 7, 10, 13, 14, 15, 22 };
            Assert.True(inOrder.SequenceEqual(expected));
        }

        private static bool ValidateBst(GenericClassAlgorithm.BST tree) =>
            ValidateBst(tree, int.MinValue, int.MaxValue);

        private static bool ValidateBst(GenericClassAlgorithm.BST tree, int minValue, int maxValue)
        {
            if (tree.Value < minValue || tree.Value >= maxValue)
                return false;

            if (tree.Left != null && !ValidateBst(tree.Left, minValue, tree.Value))
                return false;

            return tree.Right == null || ValidateBst(tree.Right, tree.Value, maxValue);
        }

        public static List<int> InOrderTraverse(GenericClassAlgorithm.BST? tree, List<int> array)
        {
            if (tree.Left != null)
                InOrderTraverse(tree.Left, array);

            array.Add(tree.Value);

            InOrderTraverse(tree.Right, array);

            return array;
        }

        static int GetTreeHeight(GenericClassAlgorithm.BST? tree) =>
            GetTreeHeight(tree, 0);

        static int GetTreeHeight(GenericClassAlgorithm.BST? tree, int height)
        {
            if (tree == null) return height;
            int LeftTreeHeight = GetTreeHeight(tree.Left, height + 1);
            int RightTreeHeight = GetTreeHeight(tree.Right, height + 1);
            return Math.Max(LeftTreeHeight, RightTreeHeight);
        }
    }
}
