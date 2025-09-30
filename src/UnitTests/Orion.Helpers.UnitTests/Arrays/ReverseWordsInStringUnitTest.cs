using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class ReverseWordsInStringClassUnitTest
    {
        [Fact(Skip ="")]
        public void Test1()
        {
            string input = "AlgoExpert is the best!";
            string expected = "best! the is AlgoExpert";
            string actual = new ReverseWordsInStringClass().ReverseWordsInString(input);
            Assert.True(expected.Equals(actual));
        }
    }
}