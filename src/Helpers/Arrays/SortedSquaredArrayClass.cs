namespace Orion.Helpers.Arrays;

public abstract class SortedSquaredArray
{
    public class SortedSquaredArrayClass
    {
        public int[] SortedSquaredArray(int[] input)
        {
            int n = input.Length;
            int[] result = new int[n];
            int left = 0;
            int right = n - 1;
            int idx = n - 1;

            // Two-pointer approach: largest square goes to end
            while (left <= right)
            {
                int leftSquare = input[left] * input[left];
                int rightSquare = input[right] * input[right];

                if (leftSquare > rightSquare)
                {
                    result[idx--] = leftSquare;
                    left++;
                }
                else
                {
                    result[idx--] = rightSquare;
                    right--;
                }
            }

            return result;
        }
    }
}