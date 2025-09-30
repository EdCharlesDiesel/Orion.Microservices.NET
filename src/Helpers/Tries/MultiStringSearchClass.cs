﻿namespace Orion.Helpers.Tries
{
    public static class MultiStringSearchClass
    {
        // O(bns) time | O(n) space
        public static List<bool> MultistringSearch(string bigstring, string[] smallstrings)
        {
            List<bool> solution = new List<bool>();
            foreach (string smallstring in smallstrings)
            {
                solution.Add(IsInBigstring(bigstring, smallstring));
            }
            return solution;
        }
        public static bool IsInBigstring(string bigstring, string smallstring)
        {
            for (int i = 0; i < bigstring.Length; i++)
            {
                if (i + smallstring.Length > bigstring.Length)
                {
                    break;
                }
                if (IsInBigstring(bigstring, smallstring, i))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsInBigstring(string bigstring, string smallstring, int startIdx)
        {
            int leftBigIdx = startIdx;
            int rightBigIdx = startIdx + smallstring.Length - 1;
            int leftSmallIdx = 0;
            int rightSmallIdx = smallstring.Length - 1;
            while (leftBigIdx <= rightBigIdx)
            {
                if (
                bigstring[leftBigIdx] != smallstring[leftSmallIdx] ||
                bigstring[rightBigIdx] != smallstring[rightSmallIdx]
                )
                {
                    return false;
                }
                leftBigIdx++;
                rightBigIdx--;
                leftSmallIdx++;
                rightSmallIdx--;
            }
            return true;
        }
    }
}