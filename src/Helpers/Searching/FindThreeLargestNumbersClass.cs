namespace Orion.Helpers.Searching
{
    public class FindThreeLargestNumbersClass
    {
        // O(n) time | O(1) space
        public static int[] FindThreeLargestNumbers(int[] array)
        {
            int[] threeLargest = { Int32.MinValue, Int32.MinValue, Int32.MinValue  };
            foreach (int num in array)
            {
                UpdateLargest(threeLargest, num);
            }
            return threeLargest;
        }
        public static void UpdateLargest(int[] threeLargest, int num)
        {
            if (num > threeLargest[2])
            {
                ShiftAndUpdate(threeLargest, num, 2);
            }
            else if (num > threeLargest[1])
            {
                ShiftAndUpdate(threeLargest, num, 1);
            }
            else if (num > threeLargest[0])
            {
                ShiftAndUpdate(threeLargest, num, 0);
            }
        }
        public static void ShiftAndUpdate(int[] array, int num, int idx)
        {
            for (int i = 0; i <= idx; i++)
            {
                if (i == idx)
                {
                    array[i] = num;
                }
                else
                {
                    array[i] = array[i + 1];
                }
            }
        }
    }
}