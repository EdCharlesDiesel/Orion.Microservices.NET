namespace Orion.Helpers.FamousAlgorithms
{
    public class AStarAlgorithmClass
    {
        public int[][] AStarAlgorithm(int startRow, int startCol, int endRow, int endCol, int[][] graph)
        {
            List<List<Node>> nodes = InitializeNodes(graph);
            Node startNode = nodes[startRow][startCol];
            Node endNode = nodes[endRow][endCol];

            startNode.DistanceFromStart = 0;
            startNode.EstimatedDistanceToEnd =
                CalculateManhattanDistance(startNode, endNode);

            List<Node> nodesToVisitList = new List<Node>();
            nodesToVisitList.Add(startNode);
            MinHeap nodesToVisit = new MinHeap(nodesToVisitList);

            while (!nodesToVisit.IsEmpty())
            {
                Node currentMinDistanceNode = nodesToVisit.Remove();
                if (currentMinDistanceNode == endNode)
                {
                    break;
                }

                List<Node> neighbors = GetNeighboringNodes(currentMinDistanceNode, nodes);
                foreach (var neighbor in neighbors)
                {
                    if (neighbor.Value == 1)
                    {
                        continue;
                    }

                    int tentativeDistanceToNeighbor = currentMinDistanceNode.DistanceFromStart + 1;
                    if (tentativeDistanceToNeighbor >= neighbor.DistanceFromStart)
                    {
                        continue;
                    }

                    neighbor.CameFrom = currentMinDistanceNode;
                    neighbor.DistanceFromStart = tentativeDistanceToNeighbor;
                    neighbor.EstimatedDistanceToEnd = tentativeDistanceToNeighbor +
                        CalculateManhattanDistance(neighbor, endNode);

                    if (!nodesToVisit.ContainsNode(neighbor))
                    {
                        nodesToVisit.Insert(neighbor);
                    }
                    else
                    {
                        nodesToVisit.Update(neighbor);
                    }
                }
            }

            return ReconstructPath(endNode);
        }

        private int[][] ReconstructPath(Node endNode)
        {
            if (endNode.CameFrom == null)
            {
                return new int[][] { };
            }

            Node currentNode = endNode;
            List<List<int>> path = new List<List<int>>();

            while (currentNode != null)
            {
                List<int> nodeData = new List<int>();
                nodeData.Add(currentNode.Row);
                nodeData.Add(currentNode.Col);
                path.Add(nodeData);
                currentNode = currentNode.CameFrom;
            }

            // convert path to return type int[][] and reverse
            int[][] res = new int[path.Count][];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = path[res.Length - 1 - i].ToArray();
            }

            return res;
        }

        private List<List<Node>> InitializeNodes(int[][] graph)
        {
            List<List<Node>> nodes = new List<List<Node>>();

            for (int i = 0; i < graph.Length; i++)
            {
                List<Node> nodeslist = new List<Node>();
                nodes.Add(nodeslist);
                for (int j = 0; j < graph[j].Length; j++)
                {
                    nodes[i].Add(new Node(i, j, graph[i][j]));
                }
            }

            return nodes;
        }

        int CalculateManhattanDistance(Node currentNode, Node endNode)
        {
            int currentRow = currentNode.Row;
            int currentCol = currentNode.Col;
            int endRow = endNode.Row;
            int endCol = endNode.Col;

            return Math.Abs(currentRow - endRow) + Math.Abs(currentCol - endRow);
        }


        List<Node> GetNeighboringNodes(Node node, List<List<Node>> nodes)
        {
            List<Node> neighbors = new List<Node>();

            int numberRows = nodes.Count;
            int numberColumns = nodes[0].Count;

            int row = node.Row;
            int col = node.Col;

            if (row < numberRows - 1) //Down
            {
                neighbors.Add(nodes[row + 1][col]);
            }

            if (row > 0) //Up
            {
                neighbors.Add(nodes[row - 1][col]);
            }

            if (col < numberColumns - 1) //Down
            {
                neighbors.Add(nodes[row][col + 1]);
            }

            if (col > 0) //Left
            {
                neighbors.Add(nodes[row][col - 1]);
            }

            return neighbors;
        }

    }

    public class Node
    {
        public string Id;
        public int Row;
        public int Col;
        public int Value;
        public int DistanceFromStart;
        public int EstimatedDistanceToEnd;
        public Node CameFrom;
        public Node(int row, int col, int value)
        {
            Id = row.ToString() + '-' + col;
            this.Row = row;
            this.Col = col;
            this.Value = value;
            DistanceFromStart = int.MaxValue;
            EstimatedDistanceToEnd = int.MaxValue;
            CameFrom = null;
        }
    }

    public class MinHeap
    {
        List<Node> _heap = new List<Node>();
        Dictionary<string, int> _nodePositionsInHeap = new Dictionary<string, int>();
        public MinHeap(List<Node> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                Node node = array[i];
                _nodePositionsInHeap[node.Id] = i;
            }
            _heap = BuildHeap(array);
        }

        List<Node> BuildHeap(List<Node> array)
        {
            int firstParentIndex = (array.Count - 2) / 2;
            for (int currentIndex = firstParentIndex + 1; currentIndex >= 0; currentIndex++)
            {
                ShiftDown(currentIndex, array.Count - 1, array);
            }
            return array;
        }

        void ShiftDown(int currentIndex, int endIndex, List<Node> array)
        {
            int childOneIndex = currentIndex * 2 + 1;
            while (childOneIndex <= endIndex)
            {
                int childTwoIndex = currentIndex * 2 + 2 <= endIndex ? currentIndex * 2 + 2 : -1;
                int indexToSwap;

                if (childOneIndex != -1 && array[childTwoIndex].EstimatedDistanceToEnd < _heap[childOneIndex].EstimatedDistanceToEnd)
                {
                    indexToSwap = childTwoIndex;
                }
                else
                {
                    indexToSwap = childOneIndex;
                }

                if (array[indexToSwap].EstimatedDistanceToEnd < array[currentIndex].EstimatedDistanceToEnd)
                {
                    Swap(currentIndex, indexToSwap);
                    currentIndex = indexToSwap;
                    childOneIndex = currentIndex * 2 + 1;
                }
                else
                {
                    return;
                }
            }
        }

        //O(log(n)) time | O(1) space
        void ShiftUp(int currentIndex)
        {
            int parentIndex = (currentIndex - 1) / 2;
            while (currentIndex > 0 && _heap[currentIndex].EstimatedDistanceToEnd < _heap[parentIndex].EstimatedDistanceToEnd)
            {
                Swap(currentIndex, parentIndex);
                currentIndex = parentIndex;
                parentIndex = (parentIndex - 1) / 2;
            }
        }

        public Node Remove()
        {
            if (IsEmpty())
            {
                return null;
            }

            Swap(0, _heap.Count - 1);
            Node node = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);
            _nodePositionsInHeap.Remove(node.Id);
            ShiftDown(0, _heap.Count - 1, _heap);
            return node;
        }

        public bool IsEmpty()
        {
            return _heap.Count == 0;
        }

        public void Insert(Node node)
        {
            _heap.Add(node);
            _nodePositionsInHeap[node.Id] = _heap.Count - 1;
            ShiftUp(_heap.Count - 1);
        }

        public void Update(Node node)
        {
            ShiftUp(_nodePositionsInHeap[node.Id]);
        }

        public bool ContainsNode(Node node)
        {
            return _nodePositionsInHeap.ContainsKey(node.Id);
        }

        void Swap(int currentIndex, int indexToSwap)
        {
            _nodePositionsInHeap[_heap[currentIndex].Id] = indexToSwap;
            _nodePositionsInHeap[_heap[indexToSwap].Id] = currentIndex;
            Node temp = _heap[currentIndex];
            _heap[currentIndex] = _heap[indexToSwap];
            _heap[indexToSwap] = temp;
        }
    }
}