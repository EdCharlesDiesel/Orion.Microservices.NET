namespace Orion.Helpers.Binary_Search_Tree
{
    public class BstConstructionClass
    {

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
        // Average: O(log(n)) time | O(log(n)) space
        // Worst: O(n) time | O(n) space
        public Bst Insert(int value)
        {
            if (value < this.Value)
            {
                if (Left == null)
                {
                    Bst newBst = new Bst(value);
                    Left = newBst;
                }
                else
                {
                    Left.Insert(value);
                }
            }
            else
            {
                if (Right == null)
                {
                    Bst newBst = new Bst(value);
                    Right = newBst;
                }
                else
                {
                    Right.Insert(value);
                }
            }
            return this;
        }
        // Average: O(log(n)) time | O(log(n)) space
        // Worst: O(n) time | O(n) space
        public bool Contains(int value)
        {
            if (value < this.Value)
            {
                if (Left == null)
                {
                    return false;
                }

                return Left.Contains(value);
            }

            if (value > this.Value)
            {
                if (Right == null)
                {
                    return false;
                }

                return Right.Contains(value);
            }

            return true;
        }
        // Average: O(log(n)) time | O(log(n)) space
        // Worst: O(n) time | O(n) space
        public Bst Remove(int value)
        {
            Remove(value, null);
            return this;
        }
        public void Remove(int value, Bst parent)
        {
            if (value < this.Value)
            {
                if (Left != null)
                {
                    Left.Remove(value, this);
                }
            }
            else if (value > this.Value)
            {
                if (Right != null)
                {
                    Right.Remove(value, this);
                }
            }
            else
            {
                if (Left != null && Right != null)
                {
                    this.Value = Right.GetMinValue();
                    Right.Remove(this.Value, this);
                }
                else if (parent == null)
                {
                    if (Left != null)
                    {
                        this.Value = Left.Value;
                        Right = Left.Right;
                        Left = Left.Left;
                    }
                    else if (Right != null)
                    {
                        this.Value = Right.Value;
                        Left = Right.Left;
                        Right = Right.Right;
                    }
                    // This is a single-node tree; do nothing.
                }
                else if (parent.Left == this)
                {
                    parent.Left = Left != null ? Left : Right;
                }
                else if (parent.Right == this)
                {
                    parent.Right = Left != null ? Left : Right;
                }
            }
        }

        public int GetMinValue()
        {
            if (Left == null)
            {
                return Value;
            }

            return Left.GetMinValue();
        }
    }
}