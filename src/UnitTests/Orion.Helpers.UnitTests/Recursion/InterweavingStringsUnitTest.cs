

using Orion.Helpers.Recursion;

namespace Orion.Helpers.UnitTests.Recursion
{
    public class InterweavingStringsTests
    {
        [Fact]
        public void TestCase1()
        {
            string one = "algoexpert";
            string two = "your-dream-job";
            string three = "your-algodream-expertjob";
           Assert.True(InterweavingStrings.Interweavingstrings(one, two, three) == true);
        }
    }
}