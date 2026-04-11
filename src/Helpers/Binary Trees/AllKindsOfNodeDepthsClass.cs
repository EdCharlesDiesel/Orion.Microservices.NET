
namespace Orion.Helpers.Binary_Trees
{
    public class AllKindsOfNodeDepthsClass
    {
        // Average case: when the tree is balanced
        // O(nlog(n)) time | O(h) space - where n is the number of nodes in
        // the Binary Tree and h is the height of the Binary Tree
        public static int AllKindsOfNodeDepths(AllKindsOfNodeDepthsClassBinaryTree root)
        {
            int sumOfAllDepths = 0;
            Stack<AllKindsOfNodeDepthsClassBinaryTree> stack = new Stack<AllKindsOfNodeDepthsClassBinaryTree>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                AllKindsOfNodeDepthsClassBinaryTree node = stack.Pop();
                if (node == null) continue;
                sumOfAllDepths += NodeDepths(node, 0);
                stack.Push(node.Left);
                stack.Push(node.Right);
            }
            return sumOfAllDepths;
        }
        public static int NodeDepths(AllKindsOfNodeDepthsClassBinaryTree node, int depth)
        {
            if (node == null) return 0;
            return depth + NodeDepths(node.Left, depth + 1) + NodeDepths(node.Right, depth + 1);
        }


    }
    public class AllKindsOfNodeDepthsClassBinaryTree
    {
        public int Value;
        public AllKindsOfNodeDepthsClassBinaryTree Left;
        public AllKindsOfNodeDepthsClassBinaryTree Right;
        public AllKindsOfNodeDepthsClassBinaryTree(int value)
        {
            this.Value = value;
            Left = null;
            Right = null;
        }

        protected AllKindsOfNodeDepthsClassBinaryTree()
        {
            throw new NotImplementedException();
        }
    }

}