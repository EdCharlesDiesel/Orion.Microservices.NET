using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class MissingNumbersClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { 4, 5, 1, 3 };
            var expected = new[] { 2, 6 };
            var actual = new MissingNumbersClass().MissingNumbers(input);
            Assert.True(expected.Length == actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(expected[i] == actual[i]);
            }
        }
    }
}