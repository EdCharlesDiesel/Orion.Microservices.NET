namespace Orion.Helpers.Binary_Trees
{
    public class BranchSumsClass
    {
        public class BinaryTree
        {
            public int Value;
            public BinaryTree Left;
            public BinaryTree Right;
            public BinaryTree(int value)
            {
                this.Value = value;
                Left = null;
                Right = null;
            }
        }
        public static List<int> BranchSums(BinaryTree root)
        {
            List<int> sums = new List<int>();
            CalculateBranchSums(root, 0, sums);
            return sums;
        }

        public static void CalculateBranchSums(
            BinaryTree node, int runningSum, List<int> sums)
        {
            if (node == null) return;
            int newRunningSum = runningSum + node.Value;
            if (node.Left == null && node.Right == null)
            {
                sums.Add(newRunningSum);
            }
        }
    }
}