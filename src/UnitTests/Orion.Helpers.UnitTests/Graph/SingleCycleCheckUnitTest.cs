using Orion.Helpers.Graphs;

namespace Orion.Helpers.UnitTests.Graph
{
    public class SingleCycleCheckClassUnitTest
    {
        [Fact]
        public void Test1()
        {
           Assert.True(SingleCycleCheckClass.HasSingleCycle(new[] { 2, 3, 1, -4, -4, 2 }));
        }
    }
}