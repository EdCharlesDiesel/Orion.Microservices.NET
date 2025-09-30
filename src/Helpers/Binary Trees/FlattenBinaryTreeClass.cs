namespace Orion.Helpers.Binary_Trees
{
    public class FlattenBinaryTreeClass
    {
        // O(n) time | O(n) space - where n is the number of nodes in the Binary Tree
        public static BinaryTree FlattenBinaryTree(BinaryTree root)
        {
            List<BinaryTree> inOrderNodes = GetNodesInOrder(root, new List<BinaryTree>());
            for (int i = 0; i < inOrderNodes.Count - 1; i++)
            {
                BinaryTree leftNode = inOrderNodes[i];
                BinaryTree rightNode = inOrderNodes[i + 1];
                leftNode.Right = rightNode;
                rightNode.Left = leftNode;
            }
            return inOrderNodes[0];
        }
        public static List<BinaryTree> GetNodesInOrder(BinaryTree tree, List<BinaryTree> array)
        {
            if (tree != null)
            {
                GetNodesInOrder(tree.Left, array);
                array.Add(tree);
                GetNodesInOrder(tree.Right, array);
            }
            return array;
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