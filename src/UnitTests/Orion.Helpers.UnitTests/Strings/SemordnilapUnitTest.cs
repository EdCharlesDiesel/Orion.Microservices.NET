using Orion.Helpers.Strings;

namespace Orion.Helpers.UnitTests.Strings
{
    public class SemordnilapClassTest 
    {
        [Fact(Skip="Not implemented yet")]
        public void SemordnilapTest1()
        {
            var input = new[] { "desserts", "stressed", "hello" };
            var expected = new List<List<string>>();
            var pair = new List<string> { "desserts", "stressed" };
            expected.Add(pair);
            var actual = new SemordnilapClass().Semordnilap(input);
            Assert.True(expected.Count == actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.True(expected[i].SequenceEqual(actual[i]));
            }
        }
    }
}