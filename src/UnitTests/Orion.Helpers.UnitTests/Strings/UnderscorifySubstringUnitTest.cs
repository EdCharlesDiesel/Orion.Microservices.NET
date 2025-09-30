using Orion.Helpers.Strings;

namespace Orion.Helpers.UnitTests.Strings
{
    public class UnderscorifySubstringClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            string expected =
    "_test_this is a _testtest_ to see if _testestest_ it works";
            string output = UnderscorifySubstringClass.UnderscorifySubstring(
              "testthis is a testtest to see if testestest it works", "test"
            );
            Assert.Equal(expected, output);
        }
    }
}