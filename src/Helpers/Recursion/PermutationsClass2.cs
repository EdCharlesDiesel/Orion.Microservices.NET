namespace Orion.Helpers.Recursion
{
    public class PermutationsClass2
    {
        // O(n*n!) time | O(n*n!) space
        public static List<List<int>> GetPermutations(List<int> array)
        {
            var permutations = new List<List<int>>();
            GetPermutations(0, array, permutations);
            return permutations;
        }

        private static void GetPermutations(int i, List<int> array, List<List<int>> permutations)
        {
            if (i == array.Count - 1)
            {
                permutations.Add(new List<int>(array));
            }
            else
            {
                for (int j = i; j < array.Count; j++)
                {
                    Swap(array, i, j);
                    GetPermutations(i + 1, array, permutations);
                    Swap(array, i, j);
                }
            }
        }

        private static void Swap(List<int> array, int i, int j)
        {
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
