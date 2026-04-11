
using Microsoft.VisualBasic.CompilerServices;

namespace Orion.Helpers.UnitTests.Graph
{
    public partial class UnitTest1
    {
        [Fact(Skip ="Fix later")]
        public void Test1()
        {
            List<int[]> coords = new List<int[]> {
              new[] { 0, 0 },
              new[] { 0, 1 },
              new[] { 1, 1 },
              new[] { 1, 0 },
              new[] { 2, 1 },
              new[] { 2, 0 },
              new[] { 3, 1 },
              new[] { 3, 0 }
            };
            Assert.True(GenericClassAlgorithm.RectangleMania(coords) == 6);
        }
    }
}