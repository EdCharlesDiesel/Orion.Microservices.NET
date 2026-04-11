
using Orion.Helpers.Recursion;

namespace Orion.Helpers.UnitTests.Recursion
{
    public class RevealMinesweeperUnitTests
    {
        [Fact]
        public void Test1()
        {
            string[][] board = new[]
            {
            new[] {"H", "H", "H", "H", "M"},
            new[] {"H", "H", "M", "H", "H"},
            new[] {"H", "H", "H", "H", "H"},
            new[] {"H", "H", "H", "H", "H"}
        };
            int row = 3;
            int column = 4;
            string[][] expected = new[]
            {
            new[] {"0", "1", "H", "H", "M"},
            new[] {"0", "1", "M", "2", "1"},
            new[] {"0", "1", "1", "1", "0"},
            new[] {"0", "0", "0", "0", "0"}
        };
            string[][] actual = new RevealMineSweeper().RevealMineSweeperMethod(board, row, column);

            Assert.True(expected.Length == actual.Length);
            Assert.True(expected[0].Length == actual[0].Length);

            for (int currRow = 0; currRow < expected.Length; currRow++)
            {
                for (int currColumn = 0; currColumn < expected[0].Length; currColumn++)
                {
                    Assert.Equal(expected[currRow][currColumn], actual[currRow][currColumn]);
                }
            }
        }
    }
}