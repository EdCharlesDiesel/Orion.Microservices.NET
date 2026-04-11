namespace Orion.Helpers.Recursion
{
    public class InterweavingStringsClass
    {
        // O(2^(n + m)) time | O(n + m) space - where n is the length
        // of the first string and m is the length of the second string
        public static bool Interweavingstrings(string one, string two, string three)
        {
            if (three.Length != one.Length + two.Length)
            {
                return false;
            }
            return AreInterwoven(one, two, three, 0, 0);
        }
        public static bool AreInterwoven(string one, string two, string three, int i, int j)
        {
            int k = i + j;
            if (k == three.Length) return true;
            if (i < one.Length && one[i] == three[k])
            {
                if (AreInterwoven(one, two, three, i + 1, j)) return true;
            }
            if (j < two.Length && two[j] == three[k])
            {
                return AreInterwoven(one, two, three, i, j + 1);
            }
            return false;
        }
    }
}