using Orion.Helpers.Strings;

namespace Orion.Helpers.UnitTests.Strings
{
    public class CommonCharactersClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            string[] input =  { "abc", "bcd", "cbad" };
            string[] expected = { "b", "c" };
            var actual = new CommonCharactersClass().CommonCharacters(input);
            Array.Sort(actual);
            Assert.True(expected.Length == actual.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }
    }
}