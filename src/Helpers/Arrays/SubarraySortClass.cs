using System;
using System.Linq;

public static class SubarraySortClass
{
    // Main method to find the smallest subarray that needs to be sorted
    public static int[] SubarraySort(int[] array)
    {
        if (array == null || array.Length <= 1)
            return new int[] { -1, -1 };

        // Find the leftmost and rightmost out-of-order elements
        int leftIndex = FindLeftmostOutOfOrder(array);
        int rightIndex = FindRightmostOutOfOrder(array);

        // If no out-of-order elements found, array is already sorted
        if (leftIndex == -1)
            return new int[] { -1, -1 };

        // Find min and max values in the out-of-order subarray
        int minOutOfOrder = int.MaxValue;
        int maxOutOfOrder = int.MinValue;

        for (int i = leftIndex; i <= rightIndex; i++)
        {
            minOutOfOrder = Math.Min(minOutOfOrder, array[i]);
            maxOutOfOrder = Math.Max(maxOutOfOrder, array[i]);
        }

        // Extend the left boundary to include all elements > minOutOfOrder
        while (leftIndex > 0 && array[leftIndex - 1] > minOutOfOrder)
        {
            leftIndex--;
        }

        // Extend the right boundary to include all elements < maxOutOfOrder
        while (rightIndex < array.Length - 1 && array[rightIndex + 1] < maxOutOfOrder)
        {
            rightIndex++;
        }

        return new int[] { leftIndex, rightIndex };
    }

    private static int FindLeftmostOutOfOrder(int[] array)
    {
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] < array[i - 1])
                return i - 1;
        }
        return -1;
    }

    private static int FindRightmostOutOfOrder(int[] array)
    {
        for (int i = array.Length - 2; i >= 0; i--)
        {
            if (array[i] > array[i + 1])
                return i + 1;
        }
        return -1;
    }

    // Alternative approach using sorting comparison
    public static int[] SubarraySortAlternative(int[] array)
    {
        if (array == null || array.Length <= 1)
            return new int[] { -1, -1 };

        int[] sortedArray = new int[array.Length];
        Array.Copy(array, sortedArray, array.Length);
        Array.Sort(sortedArray);

        int leftIndex = -1;
        int rightIndex = -1;

        // Find first position where arrays differ
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != sortedArray[i])
            {
                leftIndex = i;
                break;
            }
        }

        // Find last position where arrays differ
        for (int i = array.Length - 1; i >= 0; i--)
        {
            if (array[i] != sortedArray[i])
            {
                rightIndex = i;
                break;
            }
        }

        if (leftIndex == -1)
            return new int[] { -1, -1 };

        return new int[] { leftIndex, rightIndex };
    }

    // Helper method to verify the solution
    public static bool VerifySolution(int[] array, int[] result)
    {
        if (result[0] == -1 && result[1] == -1)
        {
            // Check if array is already sorted
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < array[i - 1])
                    return false;
            }
            return true;
        }

        // Create a copy and sort the subarray
        int[] testArray = new int[array.Length];
        Array.Copy(array, testArray, array.Length);
        
        // Sort only the specified subarray
        Array.Sort(testArray, result[0], result[1] - result[0] + 1);

        // Check if the entire array is now sorted
        for (int i = 1; i < testArray.Length; i++)
        {
            if (testArray[i] < testArray[i - 1])
                return false;
        }

        return true;
    }
}