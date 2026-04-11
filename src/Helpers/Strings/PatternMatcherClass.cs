using System.Text;

namespace Orion.Helpers.Strings
{
    public class PatternMatcherClass
    {
        // O(n^2 + m) time | O(n + m) space
        public static string[] PatternMatcher(string pattern, string str)
        {
            if (pattern.Length > str.Length)
            {
                return new string[] { };
            }
            char[] newPattern = GetNewPattern(pattern);
            bool didSwitch = newPattern[0] != pattern[0];
            Dictionary<char, int> counts = new Dictionary<char, int>();
            counts['x'] = 0;
            counts['y'] = 0;
            int firstYPos = GetCountsAndFirstYPos(newPattern, counts);
            if (counts['y'] != 0)
            {
                for (int lenOfX = 1; lenOfX < str.Length; lenOfX++)
                {
                    double lenOfY =
                    (str.Length - lenOfX *
                    (double)counts['x']) /
                    counts['y'];
                    if (lenOfY <= 0 || lenOfY % 1 != 0)
                    {
                        continue;
                    }
                    int yIdx = firstYPos * lenOfX;
                    string x = str.Substring(0, lenOfX);
                    string y = str.Substring(yIdx, (int)lenOfY);
                    string potentialMatch = BuildPotentialMatch(newPattern, x, y);
                    if (str.Equals(potentialMatch))
                    {
                        return didSwitch ? new[] { y, x } : new[] {x,y};
                    }
                }
            }
            else
            {
                double lenOfX = str.Length / counts['x'];
                if (lenOfX % 1 == 0)
                {
                    string x = str.Substring(0, (int)lenOfX);
                    string potentialMatch = BuildPotentialMatch(newPattern, x, "");
                    if (str.Equals(potentialMatch))
                    {
                        return didSwitch ? new[] { "", x } : new[] {x,""};
                    }
                }
            }
            return new string[] { };
        }
        public static char[] GetNewPattern(string pattern)
        {
            char[] patternLetters = pattern.ToCharArray();
            if (pattern[0] == 'x')
            {
                return patternLetters;
            }
            for (int i = 0; i < patternLetters.Length; i++)
            {
                if (patternLetters[i] == 'x')
                {
                    patternLetters[i] = 'y';
                }
                else
                {
                    patternLetters[i] = 'x';
                }
            }
            return patternLetters;
        }
        public static int GetCountsAndFirstYPos(char[] pattern, Dictionary<char, int> counts)
        {
            int firstYPos = -1;
            for (int i = 0; i < pattern.Length; i++)
            {
                char c = pattern[i];
                counts[c] = counts[c] + 1;
                if (c == 'y' && firstYPos == -1)
                {
                    firstYPos = i;
                }
            }
            return firstYPos;
        }
        public static string BuildPotentialMatch(char[] pattern, string x, string y)
        {
            StringBuilder potentialMatch = new StringBuilder();
            foreach (char c in pattern)
            {
                if (c == 'x')
                {
                    potentialMatch.Append(x);
                }
                else
                {
                    potentialMatch.Append(y);
                }
            }
            return potentialMatch.ToString();
        }
    }
}