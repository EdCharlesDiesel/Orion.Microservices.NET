using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class MergeOverlappingIntervalsClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[][] intervals = new[]
            {
              new[] { 1, 2 },
              new[] { 3, 5 },
              new[] { 4, 7 },
              new[] { 6, 8 },
              new[] { 9, 10 },
            };


            int[][] expected = new[]
            {
              new[] { 1, 2 },
              new[] { 3, 8 },
              new[] { 9, 10 },
            };

            int[][] actual = new MergeOverlappingIntervalsClass().MergeOverlappingIntervals(intervals);
            for (int i = 0; i < actual.Length; i++)
            {
                for (int j = 0; j < actual[i].Length; j++)
                {
                    Assert.True(expected[i][j] == actual[i][j]);
                }
            }
        }
    }
}