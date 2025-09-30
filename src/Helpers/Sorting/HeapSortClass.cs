

namespace Orion.Helpers.Sorting
{
    public class HeapSortClass
    {
        // Best: O(nlog(n)) time | O(1) space
        // Average: O(nlog(n)) time | O(1) space
        // Worst: O(nlog(n)) time | O(1) space
        public static int[] HeapSort(int[] array)
        {
            BuildMaxHeap(array);
            for (int endIdx = array.Length - 1; endIdx > 0; endIdx--)
            {
                Swap(0, endIdx, array);
                SiftDown(0, endIdx - 1, array);
            }
            return array;
        }
        public static void BuildMaxHeap(int[] array)
        {
            int firstParentIdx = (array.Length - 2) / 2;
            for (int currentIdx = firstParentIdx; currentIdx >= 0; currentIdx--)
            {
                SiftDown(currentIdx, array.Length - 1, array);
            }
        }
        public static void SiftDown(int currentIdx, int endIdx, int[] heap)
        {
            int childOneIdx = currentIdx * 2 + 1;
            while (childOneIdx <= endIdx)
            {
                int childTwoIdx = currentIdx * 2 + 2 <= endIdx ? currentIdx * 2 + 2 : -1;
                int idxToSwap;
                if (childTwoIdx != -1 && heap[childTwoIdx] > heap[childOneIdx])
                {
                    idxToSwap = childTwoIdx;
                }
                else
                {
                    idxToSwap = childOneIdx;
                }
                if (heap[idxToSwap] > heap[currentIdx])
                {
                    Swap(currentIdx, idxToSwap, heap);
                    currentIdx = idxToSwap;
                    childOneIdx = currentIdx * 2 + 1;
                }
                else
                {
                    return;
                }
            }
        }
        public static void Swap(int i, int j, int[] array)
        {
            int temp = array[j];
            array[j] = array[i];
            array[i] = temp;
        }
    }
}