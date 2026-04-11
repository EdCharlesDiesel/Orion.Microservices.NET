
namespace Orion.Helpers.Searching
{
    public class SearchForRangeClass
    {
        // O(log(n)) time | O(log(n)) space
        public static int[] SearchForRange(int[] array, int target)
        {
            int[] finalRange = { -1, -1 };
            AlteredBinarySearch(array, target, 0, array.Length - 1, finalRange, true);
            AlteredBinarySearch(array, target, 0, array.Length - 1, finalRange, false);
            return finalRange;
        }
        public static void AlteredBinarySearch(int[] array, int target, int left, int right,int[] finalRange, bool goLeft)
        {
            if (left > right)
            {
                return;
            }
            int mid = (left + right) / 2;
            if (array[mid] < target)
            {
                AlteredBinarySearch(array, target, mid + 1, right, finalRange, goLeft);
            }
            else if (array[mid] > target)
            {
                AlteredBinarySearch(array, target, left, mid - 1, finalRange, goLeft);
            }
            else
            {
                if (goLeft)
                {
                    if (mid == 0 || array[mid - 1] != target)
                    {
                        finalRange[0] = mid;
                    }
                    else
                    {
                        AlteredBinarySearch(array, target, left, mid - 1,
                        finalRange, goLeft);
                    }
                }
                else
                {
                    if (mid == array.Length - 1 || array[mid + 1] != target)
                    {
                        finalRange[1] = mid;
                    }
                    else
                    {
                        AlteredBinarySearch(array, target, mid + 1, right,
                        finalRange, goLeft);
                    }
                }
            }
        }
    }
}