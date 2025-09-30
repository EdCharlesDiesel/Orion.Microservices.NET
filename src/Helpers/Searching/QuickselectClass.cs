﻿namespace Orion.Helpers.Searching
{
    public class QuickselectClass
    {
        // Best: O(n) time | O(1) space
        // Average: O(n) time | O(1) space
        // Worst: O(n^2) time | O(1) space
        public static int Quickselect(int[] array, int k)
        {
            int position = k - 1;
            return Quickselect(array, 0, array.Length - 1, position);
        }
        public static int Quickselect(int[] array, int startIdx, int endIdx, int position)
        {
            while (true)
            {
                if (startIdx > endIdx)
                {
                    throw new Exception("Your Algorithm should never arrive here!");
                }
                int pivotIdx = startIdx;
                int leftIdx = startIdx + 1;
                int rightIdx = endIdx;
                while (leftIdx <= rightIdx)
                {
                    if (array[leftIdx] > array[pivotIdx] &&
                    array[rightIdx] < array[pivotIdx])
                    {
                        Swap(leftIdx, rightIdx, array);
                    }
                    if (array[leftIdx] <= array[pivotIdx])
                    {
                        leftIdx++;
                    }
                    if (array[rightIdx] >= array[pivotIdx])
                    {
                        rightIdx--;
                    }
                }
                Swap(pivotIdx, rightIdx, array);
                if (rightIdx == position)
                {
                    return array[rightIdx];
                }

                if (rightIdx < position)
                {
                    startIdx = rightIdx + 1;
                }
                else
                {
                    endIdx = rightIdx - 1;
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