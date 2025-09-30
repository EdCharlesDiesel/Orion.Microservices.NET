﻿namespace Orion.Helpers.Sorting
{
    public class InsertionSortClass
    {
        // Best: O(n) time | O(1) space
        // Average: O(n^2) time | O(1) space
        // Worst: O(n^2) time | O(1) space
        public static int[] InsertionSort(int[] array)
        {
            if (array.Length == 0)
            {
                return new int[] { };
            }
            for (int i = 1; i < array.Length; i++)
            {
                int j = i;
                while (j > 0 && array[j] < array[j - 1])
                {
                    Swap(j, j - 1, array);
                    j -= 1;
                }
            }
            return array;
        }
        public static void Swap(int i, int j, int[] array)
        {
            int temp = array[j];
            array[j] = array[i];
            array[i] = temp;
        }
    }
}