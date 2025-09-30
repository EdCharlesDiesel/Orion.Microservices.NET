using Orion.Helpers.Strings;

namespace Orion.Helpers.UnitTests.Strings
{
    public class PalindromeCheckClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            Assert.True(PalindromeCheckClass.IsPalindrome("abcdcba"));
        }
    }
}