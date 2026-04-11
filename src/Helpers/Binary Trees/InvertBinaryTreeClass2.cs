namespace Orion.Helpers.Binary_Trees
{
    internal class InvertBinaryTreeClass2
    {
        // O(n) time | O(d) space
        public static void InvertBinaryTree(BinaryTree tree)
        {
            if (tree == null)
            {
                return;
            }
            SwapLeftAndRight(tree);
            InvertBinaryTree(tree.Left);
            InvertBinaryTree(tree.Right);
        }
        private static void SwapLeftAndRight(BinaryTree tree)
        {
            (tree.Left, tree.Right) = (tree.Right, tree.Left);
        }
        public class BinaryTree
        {
            public int Value;
            public BinaryTree Left;
            public BinaryTree Right;
            public BinaryTree(int value)
            {
                this.Value = value;
            }
        }
    }
}
