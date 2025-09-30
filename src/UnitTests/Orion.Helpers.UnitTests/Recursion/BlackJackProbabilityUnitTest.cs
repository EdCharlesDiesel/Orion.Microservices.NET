//using NUnit.Framework;

using Orion.Helpers.Recursion;

namespace Orion.Helpers.UnitTests.Recursion
{
    public class BlackJackProbabilityUnitTest
    {
        [Fact]
        public void TestCase1()
        {
            int target = 21;
            int startingHand = 15;
            double expected = 0.45;
            double actual = new BlackJackProbability().BlackjackProbability(target, startingHand);
            Assert.True(expected == actual);
        }
    }
}