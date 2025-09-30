namespace Orion.Helpers.FamousAlgorithms
{
    // -------------------------------
    // Algorithm 1: DFS-based Topological Sort
    // -------------------------------
    public abstract class TopologicalSortDfs
    {
        // O(j + d) time | O(j + d) space
        public static List<int> TopologicalSort(List<int> jobs, List<int[]> deps)
        {
            var jobGraph = CreateJobGraph(jobs, deps);
            return GetOrderedJobs(jobGraph);
        }

        private static JobGraphDfs CreateJobGraph(List<int> jobs, List<int[]> deps)
        {
            var graph = new JobGraphDfs(jobs);
            foreach (var dep in deps)
            {
                // dep[0] -> prerequisite, dep[1] -> job
                graph.AddPrereq(dep[1], dep[0]);
            }
            return graph;
        }

        private static List<int> GetOrderedJobs(JobGraphDfs graph)
        {
            var orderedJobs = new List<int>();
            var nodes = new List<JobNodeDfs>(graph.Nodes);

            while (nodes.Count > 0)
            {
                var node = nodes[nodes.Count - 1];
                nodes.RemoveAt(nodes.Count - 1);

                var containsCycle = DepthFirstTraverse(node, orderedJobs);
                if (containsCycle)
                    return new List<int>(); // return empty if cycle detected
            }
            return orderedJobs;
        }

        private static bool DepthFirstTraverse(JobNodeDfs node, List<int> orderedJobs)
        {
            if (node.Visited) return false;
            if (node.Visiting) return true;

            node.Visiting = true;
            foreach (JobNodeDfs prereqNode in node.Prereqs)
            {
                bool containsCycle = DepthFirstTraverse(prereqNode, orderedJobs);
                if (containsCycle) return true;
            }

            node.Visited = true;
            node.Visiting = false;
            orderedJobs.Add(node.Job);

            return false;
        }
    }

    internal class JobGraphDfs
    {
        public List<JobNodeDfs> Nodes { get; }
        private Dictionary<int, JobNodeDfs> Graph { get; }

        public JobGraphDfs(List<int> jobs)
        {
            Nodes = new List<JobNodeDfs>();
            Graph = new Dictionary<int, JobNodeDfs>();
            foreach (int job in jobs)
            {
                AddNode(job);
            }
        }

        public void AddPrereq(int job, int prereq)
        {
            JobNodeDfs jobNode = GetNode(job);
            JobNodeDfs prereqNode = GetNode(prereq);
            jobNode.Prereqs.Add(prereqNode);
        }

        private void AddNode(int job)
        {
            Graph[job] = new JobNodeDfs(job);
            Nodes.Add(Graph[job]);
        }

        private JobNodeDfs GetNode(int job)
        {
            if (!Graph.ContainsKey(job)) AddNode(job);
            return Graph[job];
        }
    }

    internal class JobNodeDfs(int job)
    {
        public int Job { get; } = job;
        public List<JobNodeDfs> Prereqs { get; } = new();
        public bool Visited { get; set; }
        public bool Visiting { get; set; }
    }

    // -------------------------------
    // Algorithm 2: Kahn’s Algorithm (BFS-based)
    // -------------------------------
    public abstract class TopologicalSortKahn
    {
        public static List<int> TopologicalSort(List<int> jobs, List<int[]> deps)
        {
            JobGraphKahn graph = CreateJobGraph(jobs, deps);
            return GetOrderedJobs(graph);
        }

        private static JobGraphKahn CreateJobGraph(List<int> jobs, List<int[]> deps)
        {
            JobGraphKahn graph = new JobGraphKahn(jobs);
            foreach (int[] dep in deps)
            {
                // dep[0] -> prerequisite, dep[1] -> job
                graph.AddDep(dep[0], dep[1]);
            }
            return graph;
        }

        private static List<int> GetOrderedJobs(JobGraphKahn graph)
        {
            List<int> orderedJobs = new List<int>();
            List<JobNodeKahn> nodesWithNoPrereqs = new List<JobNodeKahn>();

            foreach (JobNodeKahn node in graph.Nodes)
            {
                if (node.NumOfPrereqs == 0)
                    nodesWithNoPrereqs.Add(node);
            }

            while (nodesWithNoPrereqs.Count > 0)
            {
                JobNodeKahn node = nodesWithNoPrereqs[nodesWithNoPrereqs.Count - 1];
                nodesWithNoPrereqs.RemoveAt(nodesWithNoPrereqs.Count - 1);

                orderedJobs.Add(node.Job);
                RemoveDeps(node, nodesWithNoPrereqs);
            }

            // if cycle exists -> return empty list
            foreach (JobNodeKahn node in graph.Nodes)
            {
                if (node.NumOfPrereqs > 0)
                    return new List<int>();
            }

            return orderedJobs;
        }

        private static void RemoveDeps(JobNodeKahn node, List<JobNodeKahn> nodesWithNoPrereqs)
        {
            while (node.Deps.Count > 0)
            {
                JobNodeKahn dep = node.Deps[node.Deps.Count - 1];
                node.Deps.RemoveAt(node.Deps.Count - 1);

                dep.NumOfPrereqs--;
                if (dep.NumOfPrereqs == 0)
                    nodesWithNoPrereqs.Add(dep);
            }
        }
    }

    internal class JobGraphKahn
    {
        public List<JobNodeKahn> Nodes { get; }
        private Dictionary<int, JobNodeKahn> Graph { get; }

        public JobGraphKahn(List<int> jobs)
        {
            Nodes = new List<JobNodeKahn>();
            Graph = new Dictionary<int, JobNodeKahn>();
            foreach (int job in jobs)
            {
                AddNode(job);
            }
        }

        public void AddDep(int prereq, int job)
        {
            JobNodeKahn jobNode = GetNode(job);
            JobNodeKahn prereqNode = GetNode(prereq);

            prereqNode.Deps.Add(jobNode);
            jobNode.NumOfPrereqs++;
        }

        private void AddNode(int job)
        {
            Graph[job] = new JobNodeKahn(job);
            Nodes.Add(Graph[job]);
        }

        private JobNodeKahn GetNode(int job)
        {
            if (!Graph.ContainsKey(job)) AddNode(job);
            return Graph[job];
        }
    }

    internal class JobNodeKahn
    {
        public int Job { get; }
        public List<JobNodeKahn> Deps { get; }
        public int NumOfPrereqs { get; set; }

        public JobNodeKahn(int job)
        {
            Job = job;
            Deps = new List<JobNodeKahn>();
            NumOfPrereqs = 0;
        }
    }
}
