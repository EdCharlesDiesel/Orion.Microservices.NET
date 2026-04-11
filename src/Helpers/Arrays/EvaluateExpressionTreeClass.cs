namespace Orion.Helpers.Arrays;

public class EvaluateExpressionTreeClass
{
    public int EvaluateExpressionTree(BinaryTree tree)
    {
        if (tree.Value >=0)
        {
            return tree.Value;
        }

        int leftValue = EvaluateExpressionTree(tree.Left);
        int rightValue = EvaluateExpressionTree(tree.Right);

        if (tree.Value ==-1) 
        {
            return leftValue + rightValue;
        }
        if (tree.Value == -2) 
        {
            return leftValue - rightValue;
        }

        if (tree.Value == -3) 
        {
            return leftValue / rightValue;
        }

        return leftValue * rightValue;
    }



    public class BinaryTree
    {
        public int Value;
        public BinaryTree Left = null;
        public BinaryTree Right = null;

        public BinaryTree(int value)
        {
            this.Value = value;
        }
    }
}