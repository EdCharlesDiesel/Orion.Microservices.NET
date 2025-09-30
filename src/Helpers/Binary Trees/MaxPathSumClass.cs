namespace Orion.Helpers.Binary_Trees
{
    public class MaxPathSumClass
    {
        // O(n) time | O(log(n)) space
        public static int MaxPathSum(BinaryTree tree)
        {
            List<int> maxSumArray = FindMaxSum(tree);
            return maxSumArray[1];
        }
        public static List<int> FindMaxSum(BinaryTree tree)
        {
            if (tree == null)
            {
                return new List<int> {
                    0, 0
                    };
            }
            List<int> leftMaxSumArray = FindMaxSum(tree.Left);
            int leftMaxSumAsBranch = leftMaxSumArray[0];
            int leftMaxPathSum = leftMaxSumArray[1];
            List<int> rightMaxSumArray = FindMaxSum(tree.Right);
            int rightMaxSumAsBranch = rightMaxSumArray[0];
            int rightMaxPathSum = rightMaxSumArray[1];
            int maxChildSumAsBranch = Math.Max(leftMaxSumAsBranch, rightMaxSumAsBranch);
            int maxSumAsBranch = Math.Max(maxChildSumAsBranch + tree.Value, tree.Value);
            int maxSumAsRootNode = Math.Max(
            leftMaxSumAsBranch + tree.Value + rightMaxSumAsBranch,
            maxSumAsBranch
            );
            int maxPathSum = Math.Max(leftMaxPathSum, Math.Max(rightMaxPathSum,
            maxSumAsRootNode));
            return new List<int> {
            maxSumAsBranch, maxPathSum
            };
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