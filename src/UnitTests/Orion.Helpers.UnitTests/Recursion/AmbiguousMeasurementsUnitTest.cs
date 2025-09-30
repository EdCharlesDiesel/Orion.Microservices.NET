

using Orion.Helpers.Recursion;

namespace Orion.Helpers.UnitTests.Recursion
{
    public class AmbiguousMeasurementsUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[][] cups = new[]
            {
            new[] { 200, 210 }, new[] { 450, 465 }, new[] { 800, 850 }
        };
            int low = 2100;
            int high = 2300;
            bool expected = true;
            var actual = new AmbiguousMeasurementsClass().AmbiguousMeasurements(cups, low, high);
            Assert.True(expected == actual);
        }
    }
}