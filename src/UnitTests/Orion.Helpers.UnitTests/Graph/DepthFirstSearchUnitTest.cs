using Orion.Helpers.Graphs;

namespace Orion.Helpers.UnitTests.Graph
{
    public class DepthFirstSearchClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            DepthFirstSearchClass.Node graph = new DepthFirstSearchClass.Node("A");
            graph.AddChild("B").AddChild("C").AddChild("D");
            graph.Children[0].AddChild("E").AddChild("F");
            graph.Children[2].AddChild("G").AddChild("H");
            graph.Children[0].Children[1].AddChild("I").AddChild("J");
            graph.Children[2].Children[0].AddChild("K");
            string[] expected = {
              "A", "B", "E", "F", "I", "J", "C", "D", "G", "K", "H"
            };
            List<string> inputArray = new List<string>();
            Assert.True(Compare(graph.DepthFirstSearch(inputArray), expected));
        }

        private static bool Compare(List<string> arr1, string[] arr2) {
            if (arr1.Count != arr2.Length) {
                return false;
            }
            for (int i = 0; i < arr1.Count; i++) {
                if (!arr1[i].Equals(arr2[i])) {
                    return false;
                }
            }
            return true;
        }
    }
}