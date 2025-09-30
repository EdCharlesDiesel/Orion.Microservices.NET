using Orion.Helpers.Dynamic_Programming;

namespace Orion.Helpers.UnitTests.Dynamic_Programming
{
    public class LevenshteinDistanceClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            Assert.True(LevenshteinDistanceClass.LevenshteinDistance("abc", "yabd") == 2);
        }
    }
}