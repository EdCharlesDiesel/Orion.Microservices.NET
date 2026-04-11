using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class SmallestDifferenceUnitTest
    {
        [Fact]
        public void TestCase1()
        {
            int[] expected = { 28, 26 };
            Assert.True(SmallestDifferenceClass.SmallestDifference(new[] { -1, 5, 10, 20, 28, 3 }, new[] { 26, 134, 135, 15, 17 }).SequenceEqual(expected));
        }
    }
}