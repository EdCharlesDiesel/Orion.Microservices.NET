using System;
using System.Collections.Generic;

namespace Orion.Helpers.Binary_Trees
{
    public class IterativeInorderTraversalClass
    {
        public class BinaryTree
        {
            public int Value;
            public BinaryTree Left;
            public BinaryTree Right;
            public BinaryTree Parent;

            public BinaryTree(int value, BinaryTree parent = null)
            {
                Value = value;
                Parent = parent;
                Left = null;
                Right = null;
            }
        }

        public static void IterativeInOrderTraversal(BinaryTree tree, Action<int> callback)
        {
            var stack = new Stack<BinaryTree>();
            var current = tree;

            while (stack.Count > 0 || current != null)
            {
                while (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }

                current = stack.Pop();
                callback(current.Value);

                current = current.Right;
            }
        }
    }
}