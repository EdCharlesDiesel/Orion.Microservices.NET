using Orion.Helpers.Strings;

namespace Orion.Helpers.UnitTests.Strings
{
    public class LongestPalindromicSubstringClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("xyzzyx", LongestPalindromicSubstringClass.LongestPalindromicSubstring("abaxyzzyxf"));
        }
    }
}