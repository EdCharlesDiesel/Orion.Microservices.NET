using Orion.Helpers.Graphs;

namespace Orion.Helpers.UnitTests.Graph
{
    public class LongestStringChainClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            List<string> strings = new List<string>(
     new[] { "abde", "abc", "abd", "abcde", "ade", "ae", "1abde", "abcdef" }
   );
            List<string> expected =
              new List<string> { "abcdef", "abcde", "abde", "ade", "ae" };
            Assert.True(LongestStringChainClass.LongestStringChain(strings).SequenceEqual(expected)
            );
        }
    }
}