using Orion.Helpers.GreedyAlgorithmns;

namespace Orion.Helpers.UnitTests.GreedyAlgorithmns
{
    public class OptimalFreelancingClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            Dictionary<string, int>[] input = new Dictionary<string, int>[1];
            Dictionary<string, int> job = new Dictionary<string, int>();
            job["deadline"] = 1;
            job["payment"] = 1;
            input[0] = job;
            var expected = 1;
            var actual = new OptimalFreelancingClass().OptimalFreelancing(input);
            Assert.True(expected == actual);
        }
    }
}