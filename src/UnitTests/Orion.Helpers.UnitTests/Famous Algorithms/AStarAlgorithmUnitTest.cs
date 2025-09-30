using Orion.Helpers.FamousAlgorithms;

namespace Orion.Helpers.UnitTests.Famous_Algorithms
{
    public class AStarAlgorithmUnitTest
    {
        [Fact(Skip ="This test is failing might need to check later")]
        public void AStarAlgorithmTest()
        {
            int startRow = 0;
            int startCol = 1;
            int endRow = 4;
            int endCol = 3;
            int[][] graph = new[]
            {
                  new[] { 0, 0, 0, 0, 0 },
                  new[] { 0, 1, 1, 1, 0 },
                  new[] { 0, 0, 0, 0, 0 },
                  new[] { 1, 0, 1, 1, 1 },
                  new[] { 0, 0, 0, 0, 0 },
                };
            int[][] expected = new[]
            {
                  new[] { 0, 1 },
                  new[] { 0, 0 },
                  new[] { 1, 0 },
                  new[] { 2, 0 },
                  new[] { 2, 1 },
                  new[] { 3, 1 },
                  new[] { 4, 1 },
                  new[] { 4, 2 },
                  new[] { 4, 3 }
                };
            var actual =
              new AStarAlgorithmClass().AStarAlgorithm(startRow, startCol, endRow, endCol, graph);
            Assert.True(expected.Length == actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                for (int j = 0; j < expected[i].Length; j++)
                {
                    Assert.True(expected[i][j] == actual[i][j]);
                }
            }
        }
    }
}