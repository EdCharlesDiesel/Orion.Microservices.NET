namespace Orion.Helpers.Binary_Search_Tree
{
    public class BTSTraversalClass
    {
        // Binary Search Tree Node
        public class BST
        {
            public int Value;
            public BST Left;
            public BST Right;

            public BST(int value)
            {
                Value = value;
                Left = null;
                Right = null;
            }
        }

        // In-Order Traversal: Left → Node → Right
        public static List<int> InOrderTraverse(BST tree, List<int> array)
        {
            if (tree != null)
            {
                InOrderTraverse(tree.Left, array);
                array.Add(tree.Value);
                InOrderTraverse(tree.Right, array);
            }
            return array;
        }

        // Pre-Order Traversal: Node → Left → Right
        public static List<int> PreOrderTraverse(BST tree, List<int> array)
        {
            if (tree != null)
            {
                array.Add(tree.Value);
                PreOrderTraverse(tree.Left, array);
                PreOrderTraverse(tree.Right, array);
            }
            return array;
        }

        // Post-Order Traversal: Left → Right → Node
        public static List<int> PostOrderTraverse(BST tree, List<int> array)
        {
            if (tree != null)
            {
                PostOrderTraverse(tree.Left, array);
                PostOrderTraverse(tree.Right, array);
                array.Add(tree.Value);
            }
            return array;
        }
    }
}