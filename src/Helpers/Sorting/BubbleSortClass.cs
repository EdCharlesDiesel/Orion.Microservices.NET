namespace Orion.Helpers.Sorting
{
    public class BubbleSortClass
    {
        // Best: O(n) time | O(1) space
        // Average: O(n^2) time | O(1) space
        // Worst: O(n^2) time | O(1) space
        public static int[] BubbleSort(int[] array)
        {
            if (array.Length == 0)
            {
                return [];
            }
            var isSorted = false;
            var counter = 0;
            while (!isSorted)
            {
                isSorted = true;
                for (var i = 0; i < array.Length - 1 - counter; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        Swap(i, i + 1, array);
                        isSorted = false;
                    }
                }
                counter++;
            }
            return array;
        }

        private static void Swap(int i, int j, int[] array)
        {
            (array[j], array[i]) = (array[i], array[j]);
        }
    }
}