namespace Orion.Helpers.Binary_Trees
{
    public class InvertBinaryTreeClass
    {
        // O(n) time | O(n) space
        public static void InvertBinaryTree(BinaryTree tree)
        {
            List<BinaryTree> queue = new List<BinaryTree>();
            queue.Add(tree);
            var index = 0;
            while (index < queue.Count)
            {
                BinaryTree current = queue[index];
                index += 1;
                if (current == null)
                {
                    continue;
                }
                SwapLeftAndRight(current);
                if (current.Left != null)
                {
                    queue.Add(current.Left);
                }
                if (current.Right != null)
                {
                    queue.Add(current.Right);
                }
            }
        }
        private static void SwapLeftAndRight(BinaryTree tree)
        {
            BinaryTree left = tree.Left;
            tree.Left = tree.Right;
            tree.Right = left;
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