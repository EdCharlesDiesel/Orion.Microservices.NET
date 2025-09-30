namespace Orion.Helpers.Binary_Trees
{
    internal class FlattenBinaryTreeClass2
    {
        // O(n) time | O(d) space - where n is the number of nodes in the Binary
        // Tree and d is the depth (height) of the Binary Tree
        public static BinaryTree FlattenBinaryTree(BinaryTree root)
        {
            FlattenTree(root);
            return GetLeftMost(root);
        }
        public static BinaryTree[] FlattenTree(BinaryTree node)
        {
            BinaryTree leftMost;
            BinaryTree rightMost;
            if (node.Left == null)
            {
                leftMost = node;
            }
            else
            {
                BinaryTree[] leftAndRightMostNodes = FlattenTree(node.Left);
                ConnectNodes(leftAndRightMostNodes[1], node);
                leftMost = leftAndRightMostNodes[0];
            }
            if (node.Right == null)
            {
                rightMost = node;
            }
            else
            {
                BinaryTree[] leftAndRightMostNodes = FlattenTree(node.Right);
                ConnectNodes(node, leftAndRightMostNodes[0]);
                rightMost = leftAndRightMostNodes[1];
            }
            return new[] { leftMost, rightMost };
        }
        public static void ConnectNodes(BinaryTree left, BinaryTree right)
        {
            left.Right = right;
            right.Left = left;
        }
        public static BinaryTree GetLeftMost(BinaryTree node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }
            return node;
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
