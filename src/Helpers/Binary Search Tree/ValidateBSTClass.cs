namespace Orion.Helpers.Binary_Search_Tree
{
    public abstract class ValidateBstClass
    {
       public class BST
    {
        public int Value { get; set; }
        public BST Left { get; set; }
        public BST Right { get; set; }

        public BST(int value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }

    // Method 1: Using min/max bounds (most efficient)
    public static bool ValidateBST(BST tree)
    {
        return ValidateBSTHelper(tree, int.MinValue, int.MaxValue);
    }

    private static bool ValidateBSTHelper(BST node, int minValue, int maxValue)
    {
        if (node == null)
            return true;

        if (node.Value < minValue || node.Value >= maxValue)
            return false;

        return ValidateBSTHelper(node.Left, minValue, node.Value) &&
               ValidateBSTHelper(node.Right, node.Value, maxValue);
    }

    // Method 2: Using in-order traversal
    public static bool ValidateBSTInOrder(BST tree)
    {
        var values = new List<int>();
        InOrderTraversal(tree, values);
        
        for (int i = 1; i < values.Count; i++)
        {
            if (values[i] <= values[i - 1])
                return false;
        }
        return true;
    }

    private static void InOrderTraversal(BST node, List<int> values)
    {
        if (node == null)
            return;
        
        InOrderTraversal(node.Left, values);
        values.Add(node.Value);
        InOrderTraversal(node.Right, values);
    }

    // Method 3: If duplicates are allowed (use <= for right subtree)
    public static bool ValidateBSTWithDuplicates(BST tree)
    {
        return ValidateBSTWithDuplicatesHelper(tree, int.MinValue, int.MaxValue);
    }

    private static bool ValidateBSTWithDuplicatesHelper(BST node, int minValue, int maxValue)
    {
        if (node == null)
            return true;

        if (node.Value < minValue || node.Value > maxValue)
            return false;

        return ValidateBSTWithDuplicatesHelper(node.Left, minValue, node.Value) &&
               ValidateBSTWithDuplicatesHelper(node.Right, node.Value, maxValue);
    }
    }
}