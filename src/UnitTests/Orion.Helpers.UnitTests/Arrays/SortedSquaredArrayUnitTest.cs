

using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class SortedSquaredArrayUnitTest
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { 1, 2, 3, 5, 6, 8, 9 };
            var expected = new[] { 1, 4, 9, 25, 36, 64, 81 };

            var actual = new SortedSquaredArray.SortedSquaredArrayClass().SortedSquaredArray(input);

            Assert.Equal(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }

        [Fact]
        public void HandlesNegativeNumbers()
        {
            var input = new[] { -7, -3, 1, 9, 22, 30 };
            var expected = new[] { 1, 9, 49, 81, 484, 900 };

            var actual = new SortedSquaredArray.SortedSquaredArrayClass().SortedSquaredArray(input);

            Assert.Equal(expected, actual);
        }
    }
}