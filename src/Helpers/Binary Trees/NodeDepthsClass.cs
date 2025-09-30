using System;
using System.Collections.Generic;

namespace Orion.Helpers.Binary_Trees
{
    // Average case: when the tree is balanced
    // O(n) time | O(h) space - where n is the number of nodes in
    // the Binary Tree and h is the height of the Binary Tree
    public abstract class NodeDepthsClass
    {
        public static int NodeDepths(BinaryTree root)
        {
            int sumOfDepths = 0;
            Stack<Level> stack = new Stack<Level>();
            stack.Push(new Level(root, 0));

            while (stack.Count > 0)
            {
                Level top = stack.Pop();
                BinaryTree node = top.Root;
                int depth = top.Depth;

                if (node == null) continue;

                sumOfDepths += depth;
                stack.Push(new Level(node.Left, depth + 1));
                stack.Push(new Level(node.Right, depth + 1));
            }

            return sumOfDepths;
        }

        // Binary tree node
        public class BinaryTree
        {

            public int Value { get; set; }
            public BinaryTree Left { get; set; }
            public BinaryTree Right { get; set; }

            public BinaryTree(int value)
            {
                Value = value;
                Left = null;
                Right = null;
            }
        }

        // Helper to store tree node and its depth
        public class Level
        {
            public BinaryTree Root { get; }
            public int Depth { get; }

            public Level(BinaryTree root, int depth)
            {
                Root = root;
                Depth = depth;
            }
        }
    }
}