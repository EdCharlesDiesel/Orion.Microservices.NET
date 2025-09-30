namespace Orion.Helpers.Graphs
{
    public static class LongestStringChainClass
    {
   
        // O(n * m^2 + nlog(n)) time | O(nm) space - where n is the number of strings
        // and m is the length of the longest string
        public static List<string> LongestStringChain(List<string> strings)
        {
            // For every string, imagine the longest string chain that starts with it.
            // Set up every string to point to the next string in its respective longest
            // string chain. Also keep track of the lengths of these longest string
            // chains.
            var stringChains = new Dictionary<string, StringChain>();
            foreach (var str in strings)
            {
                stringChains[str] = new StringChain("", 1);
            }
            // Sort the strings based on their length so that whenever we visit a
            // string (as we iterate through them from left to right), we can
            // already have computed the longest string chains of any smaller strings.
            var sortedstrings = new List<string>(strings);
            sortedstrings.Sort((a, b) => a.Length - b.Length);
            foreach (string str in sortedstrings)
            {
                FindLongeststringChain(str, stringChains);
            }
            return BuildLongeststringChain(strings, stringChains);
        }

        private static void FindLongeststringChain(string str, Dictionary<string,
        StringChain> stringChains)
        {
            // Try removing every letter of the current string to see if the
            // remaining strings form a string chain.
            for (int i = 0; i < str.Length; i++)
            {
                var smallerstring = GetSmallerstring(str, i);
                if (!stringChains.ContainsKey(smallerstring)) continue;
                TryUpdateLongeststringChain(str, smallerstring, stringChains);
            }
        }

        private static string GetSmallerstring(string str, int index)
        {
            return str.Substring(0, index) + str.Substring(index + 1);
        }

        private static void TryUpdateLongeststringChain(
        string currentstring,
        string smallerstring,
        Dictionary<string, StringChain> stringChains
        )
        {
            int smallerstringChainLength = stringChains[smallerstring].MaxChainLength;
            int currentstringChainLength = stringChains[currentstring].MaxChainLength;
            // Update the string chain of the current string only if the smaller string
            // leads to a longer string chain.
            if (smallerstringChainLength + 1 > currentstringChainLength)
            {
                stringChains[currentstring].MaxChainLength = smallerstringChainLength + 1;
                stringChains[currentstring].Nextstring = smallerstring;
            }
        }

        private static List<string> BuildLongeststringChain(List<string> strings, Dictionary<string,
        StringChain> stringChains)
        {
            // Find the string that starts the longest string chain.
            var maxChainLength = 0;
            var chainStartingstring = "";
            for (var index = 0; index < strings.Count; index++)
            {
                var str = strings[index];
                if (stringChains[str].MaxChainLength > maxChainLength)
                {
                    maxChainLength = stringChains[str].MaxChainLength;
                    chainStartingstring = str;
                }
            }

            // Starting at the string found above, build the longest string chain.
            var ourLongeststringChain = new List<string>();
            var currentstring = chainStartingstring;
            while (currentstring != "")
            {
                ourLongeststringChain.Add(currentstring);
                currentstring = stringChains[currentstring].Nextstring;
            }
            return ourLongeststringChain.Count ==
            1 ? []
                : ourLongeststringChain;
        }
    }

    public class StringChain(string nextstring, int maxChainLength)
    {
        public string Nextstring = nextstring;
        public int MaxChainLength = maxChainLength;
    }
}