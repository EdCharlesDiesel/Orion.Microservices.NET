namespace Orion.Helpers.Binary_Search_Tree
{
  /// <summary>
  /// Write a function that takes in a Binary Tree(BTS) and a target integer value and returns
  /// the closest value to that target value contained in the BTS.
  ///
  /// You can assume that there will only be one closet value.
  ///
  /// Each BTS node has an integer valie, a left child node, and right child node.A node is
  /// said to be a valid BTS node id and only id it satisfies the BTS property: its value is
  /// strictly greater than the values of every node to its value; its value is less than or
  /// equal to the values of every node to its right
  /// tha
  /// </summary>
    public static class FindClosestValueInBstClass
    {
        // Average: O(log(n)) time | O(log(n)) space
        // Worst: O(n) time | O(n) space
        public static int FindClosestValueInBst(Bst tree, int target)
        {
            return FindClosestValueInBst(tree, target, Int32.MaxValue);
        }

        private static int FindClosestValueInBst(Bst tree, int target, double closest)
        {
            if (Math.Abs(target - closest) > Math.Abs(target - tree.Value))
            {
                closest = tree.Value;
            }
            if (target < tree.Value && tree.Left != null)
            {
                return FindClosestValueInBst(tree.Left, target, closest);
            }

            if (target > tree.Value && tree.Right != null)
            {
                return FindClosestValueInBst(tree.Right, target, closest);
            }

            return (int)closest;
        }
        public class Bst
        {
            public int Value;
            public Bst Left;
            public Bst Right;
            public Bst(int value)
            {
                this.Value = value;
            }
        }
    }
}