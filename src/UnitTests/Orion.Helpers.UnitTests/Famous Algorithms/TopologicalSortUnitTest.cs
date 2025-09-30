namespace Orion.Helpers.UnitTests.Famous_Algorithms
{
    public class TopologicalSortUnitTest
    {
        [Fact(Skip = "Fix later")]
        public void TopologicalSortTest()
        {
            List<int> jobs = new List<int> { 1, 2, 3, 4 };
            int[,] depsArray =
              new[,] { { 1, 2 }, { 1, 3 }, { 3, 2 }, { 4, 2 }, { 4, 3 } };
            List<int[]> deps = new List<int[]>();
            FillDeps(depsArray, deps);
            List<int> order = TopologicalSortClass.TopologicalSort(jobs, deps);
            Assert.True(IsValidTopologicalOrder(order, jobs, deps));
        }

        void FillDeps(int[,] depsArray, List<int[]> deps)
        {
            for (int x = 0; x < depsArray.GetLength(0); x++)
            {
                var arr = new int[depsArray.GetLength(1)];
                for (int y = 0; y < depsArray.GetLength(1); y++)
                {
                    arr[y] = depsArray[x, y];
                }
                deps.Add(arr);
            }
        }

        private bool IsValidTopologicalOrder(
   List<int> order, List<int> jobs, List<int[]> deps
 )
        {
            Dictionary<int, bool> visited = new Dictionary<int, bool>();
            foreach (int candidate in order)
            {
                foreach (int[] dep in deps)
                {
                    if (candidate == dep[0] && visited.ContainsKey(dep[1])) return false;
                }
                visited.Add(candidate, true);
            }
            foreach (int job in jobs)
            {
                if (!visited.ContainsKey(job)) return false;
            }
            return order.Count == jobs.Count;
        }
    }

    public class TopologicalSortClass
    {
        public static List<int> TopologicalSort(List<int> jobs, List<int[]> deps)
        {
            throw new NotImplementedException();
        }
    }
}