using Orion.Helpers.Searching;

namespace Orion.Helpers.UnitTests.Searching
{
    public class OptimalAssemblyLineClassUnitTest1
    {
        [Fact(Skip = "Not Implemented yet")]
        public void Test1()
        {
            int[] stepDurations = new[] { 15, 15, 30, 30, 45 };
            int numStations = 3;
            int actual = new OptimalAssemblyLineClass().OptimalAssemblyLine(stepDurations, numStations);
            int expected = 60;
            Assert.True(expected == actual);
        }
    }
}