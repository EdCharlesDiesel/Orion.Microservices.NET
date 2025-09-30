using Orion.Helpers.Dynamic_Programming;

namespace Orion.Helpers.UnitTests.Dynamic_Programming
{
    public class LongestCommonSubsequenceClassUnitTest
    {
        [Fact(Skip ="Fix this")]
        public void Test1()
        {
            char[] expected = { 'X', 'Y', 'Z', 'W' };
            Assert.True(
              Compare(LongestCommonSubsequenceClass.LongestCommonSubsequence("ZXVVYZW", "XKYKZPW"), expected)
            );
        }

        private static bool Compare(List<char> arr1, char[] arr2)
        {
            if (arr1.Count != arr2.Length)
            {
                return false;
            }
            for (int i = 0; i < arr1.Count; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}