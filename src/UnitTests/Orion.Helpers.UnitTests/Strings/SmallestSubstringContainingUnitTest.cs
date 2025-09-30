using Orion.Helpers.Strings;

namespace Orion.Helpers.UnitTests.Strings
{
    public class SmallestSubstringContainingClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            string bigstring = "abcd$ef$axb$c$";
            string smallstring = "$$abf";
            string expected = "f$axb$";
            Assert.Equal(SmallestSubstringContainingClass.SmallestSubstringContaining(bigstring, smallstring)
, expected);
        }
    }
}
