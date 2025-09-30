namespace Orion.Helpers.Binary_Search_Tree
{
    public class MinHeightBstClass
    {
        // O(nlog(n)) time | O(n) space - where n is the length of the array
        public static Bst MinHeightBst(List<int> array)
        {
            return ConstructMinHeightBst(array, null, 0, array.Count - 1);
        }
        public static Bst ConstructMinHeightBst(List<int> array, Bst bst, int startIdx,
        int endIdx)
        {
            if (endIdx < startIdx) return null;
            int midIdx = (startIdx + endIdx) / 2;
            int valueToAdd = array[midIdx];
            if (bst == null)
            {
                bst = new Bst(valueToAdd);
            }
            else
            {
                bst.Insert(valueToAdd);
            }
            ConstructMinHeightBst(array, bst, startIdx, midIdx - 1);
            ConstructMinHeightBst(array, bst, midIdx + 1, endIdx);
            return bst;
        }
        public class Bst
        {
            public int Value;
            public Bst Left;
            public Bst Right;
            public Bst(int value)
            {
                this.Value = value;
                Left = null;
                Right = null;
            }
            public void Insert(int value)
            {
                if (value < this.Value)
                {
                    if (Left == null)
                    {
                        Left = new Bst(value);
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
                        Right = new Bst(value);
                    }
                    else
                    {
                        Right.Insert(value);
                    }
                }
            }
        }
    }
}