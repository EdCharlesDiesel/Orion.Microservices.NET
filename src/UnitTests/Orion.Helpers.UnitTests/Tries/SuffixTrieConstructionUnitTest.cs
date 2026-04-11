using Orion.Helpers.Tries;

namespace Orion.Helpers.UnitTests.Tries
{
    public class SuffixTrieConstructionClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var trie = new SuffixTrieConstructionClass.SuffixTrie("babc");
            Assert.True(trie.Contains("abc"));
        }
    }
}