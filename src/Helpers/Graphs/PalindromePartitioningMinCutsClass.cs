using static System.Int32;

namespace Orion.Helpers.Graphs
{
    public abstract class PalindromePartitioningMinCutsClass
    {
        // O(n^3) time | O(n^2) space
        public static int PalindromePartitioningMinCuts(string str)
        {
            var palindromes = new bool[str.Length, str.Length];
            for (var i = 0; i < str.Length; i++)
            {
                for (var j = i; j < str.Length; j++)
                {
                    palindromes[i, j] = IsPalindrome(str.Substring(i, j + 1 - i));
                }
            }
            var cuts = new int[str.Length];
            Array.Fill(cuts, MaxValue);
            for (var i = 0; i < str.Length; i++)
            {
                if (palindromes[0, i])
                {
                    cuts[i] = 0;
                }
                else
                {
                    cuts[i] = cuts[i - 1] + 1;
                    for (var j = 1; j < i; j++)
                    {
                        if (palindromes[j, i] && cuts[j - 1] + 1 < cuts[i])
                        {
                            cuts[i] = cuts[j - 1] + 1;
                        }
                    }
                }
            }
            return cuts[str.Length - 1];
        }

        private static bool IsPalindrome(string str)
        {
            var leftIdx = 0;
            var rightIdx = str.Length - 1;
            while (leftIdx < rightIdx)
            {
                if (str[leftIdx] != str[rightIdx])
                {
                    return false;
                }
                leftIdx++;
                rightIdx--;
            }
            return true;
        }
    }
}