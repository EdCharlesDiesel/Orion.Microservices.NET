namespace Orion.Helpers.Binary_Search_Tree
{
    public class SameBsTsClass
    {
        // O(n^2) time | O(n^2) space - where n is the number of
        // nodes in each array, respectively
        public static bool SameBsts(List<int> arrayOne, List<int> arrayTwo)
        {
            if (arrayOne.Count != arrayTwo.Count) return false;
            if (arrayOne.Count == 0 && arrayTwo.Count == 0) return true;
            if (arrayOne[0] != arrayTwo[0]) return false;
            List<int> leftOne = GetSmaller(arrayOne);
            List<int> leftTwo = GetSmaller(arrayTwo);
            List<int> rightOne = GetBiggerOrEqual(arrayOne);
            List<int> rightTwo = GetBiggerOrEqual(arrayTwo);
            return SameBsts(leftOne, leftTwo) && SameBsts(rightOne, rightTwo);
        }
        public static List<int> GetSmaller(List<int> array)
        {
            List<int> smaller = new List<int>();
            for (int i = 1; i < array.Count; i++)
            {
                if (array[i] < array[0]) smaller.Add(array[i]);
            }
            return smaller;
        }
        public static List<int> GetBiggerOrEqual(List<int> array)
        {
            List<int> biggerOrEqual = new List<int>();
            for (int i = 1; i < array.Count; i++)
            {
                if (array[i] >= array[0]) biggerOrEqual.Add(array[i]);
            }
            return biggerOrEqual;
        }
    }
    
    public class SameBsTsClass2
    {
        // O(n^2) time | O(d) space - where n is the number of
        // nodes in each array, respectively, and d is the depth
        // of the BST that they represent
        public static bool SameBsts(List<int> arrayOne, List<int> arrayTwo)
        {
            return AreSameBsts(arrayOne, arrayTwo, 0, 0, Int32.MinValue, Int32.MaxValue);
        }
        public static bool AreSameBsts(List<int> arrayOne, List<int> arrayTwo, int rootIdxOne,
        int rootIdxTwo, int minVal, int maxVal)
        {
            if (rootIdxOne == -1 || rootIdxTwo == -1) return rootIdxOne == rootIdxTwo;
            if (arrayOne[rootIdxOne] != arrayTwo[rootIdxTwo]) return false;
            int leftRootIdxOne = GetIdxOfFirstSmaller(arrayOne, rootIdxOne, minVal);
            int leftRootIdxTwo = GetIdxOfFirstSmaller(arrayTwo, rootIdxTwo, minVal);
            int rightRootIdxOne = GetIdxOfFirstBiggerOrEqual(arrayOne, rootIdxOne, maxVal);
            int rightRootIdxTwo = GetIdxOfFirstBiggerOrEqual(arrayTwo, rootIdxTwo, maxVal);
            int currentValue = arrayOne[rootIdxOne];
            bool leftAreSame = AreSameBsts(arrayOne, arrayTwo, leftRootIdxOne, leftRootIdxTwo,
            minVal, currentValue);
            bool rightAreSame = AreSameBsts(arrayOne, arrayTwo, rightRootIdxOne,
            rightRootIdxTwo, currentValue, maxVal);
            return leftAreSame && rightAreSame;
        }
        public static int GetIdxOfFirstSmaller(List<int> array, int startingIdx, int minVal)
        {
            // Find the index of the first smaller value after the startingIdx.
            // Make sure that this value is greater than or equal to the minVal,
            // which is the value of the previous parent node in the BST. If it
            // isn't, then that value is located in the left subtree of the
            // previous parent node.
            for (int i = startingIdx + 1; i < array.Count; i++)
            {
                if (array[i] < array[startingIdx] && array[i] >= minVal) return i;
            }
            return -1;
        }
        public static int GetIdxOfFirstBiggerOrEqual(List<int> array, int startingIdx, int maxVal)
        {
            // Find the index of the first bigger/equal value after the startingIdx.
            // Make sure that this value is smaller than maxVal, which is the value
            // of the previous parent node in the BST. If it isn't, then that value
            // is located in the right subtree of the previous parent node.
            for (int i = startingIdx + 1; i < array.Count; i++)
            {
                if (array[i] >= array[startingIdx] && array[i] < maxVal) return i;
            }
            return -1;
        }
    }
}