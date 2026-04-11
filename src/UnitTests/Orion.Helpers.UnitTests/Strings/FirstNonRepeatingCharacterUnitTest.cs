using Orion.Helpers.Strings;

namespace Orion.Helpers.UnitTests.Strings
{
    public class FirstNonRepeatingCharacterClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            string input = "abcdcaf";
            int expected = 1;
            var actual = new FirstNonRepeatingCharacterClass().FirstNonRepeatingCharacter(input);
            Assert.True(expected == actual);
        }
    }
}