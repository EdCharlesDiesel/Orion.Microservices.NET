using Orion.Helpers.Arrays;
using Orion.Helpers.Binary_Trees;

namespace Orion.Helpers;

public static class GenericClassAlgorithm
{
    // =========================
    // ======== BST ============
    // =========================
    public class BST
    {
        public BST? Left;
        public BST? Right;
        public int Value;

        public BST(int value)
        {
            this.Value = value;
        }

        public void Insert(int i)
        {
            throw new NotImplementedException();
        }

        public void Remove(int i)
        {
            throw new NotImplementedException();
        }

        public bool Contains(int p0)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// For each index i, count how many elements to its right are smaller than array[i].
    /// Fenwick Tree (BIT) + coordinate compression. O(n log n).
    /// </summary>
    public static IEnumerable<int> RightSmallerThan(List<int>? array)
    {
        if (array == null || array.Count == 0) return Array.Empty<int>();

        // Coordinate compression
        var sortedUnique = array.Distinct().OrderBy(x => x).ToArray();
        var rank = new Dictionary<int, int>(sortedUnique.Length);
        for (int i = 0; i < sortedUnique.Length; i++) rank[sortedUnique[i]] = i + 1; // 1-based

        var bit = new Fenwick(sortedUnique.Length);
        var result = new int[array.Count];
        for (int i = array.Count - 1; i >= 0; i--)
        {
            int r = rank[array[i]];
            // elements strictly smaller -> prefix sum of (r - 1)
            result[i] = bit.Sum(r - 1);
            bit.Add(r, 1);
        }
        return result;
    }

    private sealed class Fenwick
    {
        private readonly int[] _tree;
        public Fenwick(int n) => _tree = new int[n + 2];
        public void Add(int i, int delta)
        {
            for (int x = i; x < _tree.Length; x += x & -x) _tree[x] += delta;
        }
        public int Sum(int i)
        {
            int s = 0;
            for (int x = i; x > 0; x -= x & -x) s += _tree[x];
            return s;
        }
    }

    /// <summary>
    /// Build a min-height BST from a sorted array. O(n).
    /// </summary>
    public static BST MinHeightBST(List<int> array)
    {
        if (array == null || array.Count == 0) throw new ArgumentException("array must be non-empty");
        return ConstructMinHeightBST(array, 0, array.Count - 1);
    }

    private static BST? ConstructMinHeightBST(List<int> arr, int lo, int hi)
    {
        if (hi < lo) return null;
        int mid = (lo + hi) / 2;
        var node = new BST(arr[mid])
        {
            Left = ConstructMinHeightBST(arr, lo, mid - 1),
            Right = ConstructMinHeightBST(arr, mid + 1, hi)
        };
        return node;
    }

    /// <summary>
    /// Validate BST property with min/max window. O(n).
    /// </summary>
    public static bool ValidateBST(BST root) =>
        ValidateBSTHelper(root, long.MinValue, long.MaxValue);

    private static bool ValidateBSTHelper(BST? node, long min, long max)
    {
        if (node == null) return true;
        if (node.Value <= min || node.Value > max) return false;
        return ValidateBSTHelper(node.Left, min, node.Value) &&
               ValidateBSTHelper(node.Right, node.Value, max);
    }

    // =========================
    // ======== BinaryTree =====
    // =========================
    public class BinaryTree
    {
        public BinaryTree? Left;
        public BinaryTree? Right;
        public int Value;

        public BinaryTree(int value)
        {
            this.Value = value;
        }
    }

    /// <summary>
    /// AncestralTree node with parent pointer.
    /// </summary>
    public class AncestralTree
    {
        public char Name;
        public AncestralTree? Ancestor; // parent

        public AncestralTree(char name)
        {
            this.Name = name;
        }
    }

    /// <summary>
    /// Strongly-typed YCA (youngest common ancestor) with parent pointers. O(depth).
    /// </summary>
    public static AncestralTree GetYoungestCommonAncestor(AncestralTree topAncestor, AncestralTree descOne, AncestralTree descTwo)
    {
        int d1 = GetDepth(descOne, topAncestor);
        int d2 = GetDepth(descTwo, topAncestor);

        // Elevate deeper one
        if (d1 > d2) descOne = GoUpBy(descOne, d1 - d2);
        else if (d2 > d1) descTwo = GoUpBy(descTwo, d2 - d1);

        // Climb together
        while (descOne != descTwo)
        {
            descOne = descOne.Ancestor!;
            descTwo = descTwo.Ancestor!;
        }
        return descOne;
    }

    // Backward-compatible wrapper for your original odd signature (casts at runtime).
    public static AncestralTree GetYoungestCommonAncestor(object tree, object o, object tree1)
    {
        if (tree is AncestralTree ta && o is AncestralTree a && tree1 is AncestralTree b)
            return GetYoungestCommonAncestor(ta, a, b);
        throw new ArgumentException("Expected AncestralTree arguments.");
    }

    private static int GetDepth(AncestralTree node, AncestralTree top)
    {
        int d = 0;
        while (node != top)
        {
            node = node.Ancestor!;
            d++;
        }
        return d;
    }

    private static AncestralTree GoUpBy(AncestralTree node, int steps)
    {
        while (steps-- > 0) node = node.Ancestor!;
        return node;
    }

    /// <summary>
    /// Iterative in-order traversal with a generic callback.
    /// Accepts a callback of type Action&lt;int&gt; (node value) or Action&lt;EvaluateExpressionTreeClass.BinaryTree&gt;.
    /// </summary>
    public static void IterativeInOrderTraversal(EvaluateExpressionTreeClass.BinaryTree root, object testCallback)
    {
        if (root == null) return;

        var cbInt = testCallback as Action<int>;
        var cbNode = testCallback as Action<EvaluateExpressionTreeClass.BinaryTree>;
        if (cbInt == null && cbNode == null)
            throw new ArgumentException("Callback must be Action<int> or Action<EvaluateExpressionTreeClass.BinaryTree>.");

        var stack = new Stack<EvaluateExpressionTreeClass.BinaryTree>();
        var curr = root;

        // Assuming EvaluateExpressionTreeClass.BinaryTree has .left, .right, .value
        while (curr != null || stack.Count > 0)
        {
            while (curr != null)
            {
                stack.Push(curr);
                curr = curr.Left;
            }

            curr = stack.Pop();
            if (cbInt != null) cbInt(curr.Value);
            else cbNode!(curr);

            curr = curr.Right;
        }
    }

    /// <summary>
    /// Max path sum in a binary tree (path can start and end at any node).
    /// </summary>
    public static int MaxPathSum(TestBinaryTree test)
    {
        if (test == null || test.Root == null) throw new ArgumentException("TestBinaryTree.Root must be set.");
        int maxSum = int.MinValue;
        DfsMaxPath(test.Root, ref maxSum);
        return maxSum;
    }

    private static int DfsMaxPath(BinaryTree? node, ref int globalMax)
    {
        if (node == null) return 0;
        int leftGain = Math.Max(0, DfsMaxPath(node.Left, ref globalMax));
        int rightGain = Math.Max(0, DfsMaxPath(node.Right, ref globalMax));
        globalMax = Math.Max(globalMax, node.Value + leftGain + rightGain);
        return node.Value + Math.Max(leftGain, rightGain);
    }

    /// <summary>
    /// Convert to a "right sibling tree": for each level, set node.right to its right sibling (or null).
    /// Children links are preserved on .left; the original .right child pointers are lost (by design of this variant).
    /// </summary>
    public static BinaryTree RightSiblingTree(BinaryTree root)
    {
        if (root == null) return root;

        var q = new Queue<BinaryTree>();
        q.Enqueue(root);

        while (q.Count > 0)
        {
            int levelCount = q.Count;
            var levelNodes = new List<BinaryTree>(levelCount);

            // collect level using original children
            for (int i = 0; i < levelCount; i++)
            {
                var node = q.Dequeue();
                levelNodes.Add(node);
                var left = node.Left;
                var right = node.Right;

                if (left != null) q.Enqueue(left);
                if (right != null) q.Enqueue(right);
            }

            // rewire rights to sibling
            for (int i = 0; i < levelNodes.Count; i++)
            {
                var node = levelNodes[i];
                node.Right = (i + 1 < levelNodes.Count) ? levelNodes[i + 1] : null;
            }
        }

        return root;
    }

    /// <summary>
    /// Sum of node depths (distance from root for all nodes). O(n).
    /// Uses your Orion.Helpers.Binary_Trees.NodeDepthsClass.BinaryTree type.
    /// </summary>
    public static int NodeDepths(NodeDepthsClass.BinaryTree root)
    {
        return NodeDepthsDfs(root, 0);
    }

    private static int NodeDepthsDfs(NodeDepthsClass.BinaryTree? node, int depth)
    {
        if (node == null) return 0;
        return depth + NodeDepthsDfs(node.Left, depth + 1) + NodeDepthsDfs(node.Left, depth + 1);
    }

    // =========================
    // ======== DP/Graphs ======
    // =========================

    /// <summary>
    /// Minimum number of coins for change to sum to n; returns -1 if impossible. Unbounded knapsack. O(n * d).
    /// </summary>
    public static int MinNumberOfCoinsForChange(int n, int[] denoms)
    {
        if (n < 0) return -1;
        if (n == 0) return 0;
        if (denoms == null || denoms.Length == 0) return -1;

        var dp = new int[n + 1];
        Array.Fill(dp, int.MaxValue);
        dp[0] = 0;

        foreach (var coin in denoms.Distinct().Where(x => x > 0))
        {
            for (int amount = coin; amount <= n; amount++)
            {
                if (dp[amount - coin] != int.MaxValue)
                    dp[amount] = Math.Min(dp[amount], dp[amount - coin] + 1);
            }
        }

        return dp[n] == int.MaxValue ? -1 : dp[n];
    }

    /// <summary>
    /// Count axis-aligned rectangles given integer coordinate points.
    /// O(C * Y^2) with hashmap of (y1,y2) pairs seen so far.
    /// </summary>
    public static int RectangleMania(List<int[]> coords)
    {
        if (coords == null || coords.Count < 4) return 0;

        // Group by x: x -> list of y's at that column
        var byX = coords.GroupBy(p => p[0])
                        .ToDictionary(g => g.Key, g => g.Select(p => p[1]).OrderBy(y => y).ToList());

        // For each x from left to right, count pairs of y's and how many previous columns had the same pair.
        var pairCount = new Dictionary<(int y1, int y2), int>();
        int rectangles = 0;

        foreach (var x in byX.Keys.OrderBy(x => x))
        {
            var ys = byX[x];
            for (int i = 0; i < ys.Count; i++)
            {
                for (int j = i + 1; j < ys.Count; j++)
                {
                    var key = (ys[i], ys[j]);
                    if (pairCount.TryGetValue(key, out int seen))
                        rectangles += seen;

                    pairCount[key] = seen + 1;
                }
            }
        }

        return rectangles;
    }

    /// <summary>
    /// Return sizes of all rivers (connected components of 1s) in a 0/1 matrix (4-directional). O(rc).
    /// </summary>
    public static List<int> RiverSizes(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        var sizes = new List<int>();
        var visited = new bool[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (visited[r, c] || matrix[r, c] == 0) continue;
                sizes.Add(ExploreRiver(matrix, visited, r, c));
            }
        }
        return sizes;
    }

    private static int ExploreRiver(int[,] m, bool[,] vis, int sr, int sc)
    {
        int rows = m.GetLength(0), cols = m.GetLength(1);
        int size = 0;
        var q = new Queue<(int r, int c)>();
        q.Enqueue((sr, sc));
        vis[sr, sc] = true;

        int[] dr = { 1, -1, 0, 0 };
        int[] dc = { 0, 0, 1, -1 };

        while (q.Count > 0)
        {
            var (r, c) = q.Dequeue();
            size++;

            for (int k = 0; k < 4; k++)
            {
                int nr = r + dr[k], nc = c + dc[k];
                if (nr < 0 || nr >= rows || nc < 0 || nc >= cols) continue;
                if (vis[nr, nc] || m[nr, nc] == 0) continue;
                vis[nr, nc] = true;
                q.Enqueue((nr, nc));
            }
        }
        return size;
    }

    /// <summary>
    /// Determine if the matrix contains a square whose border is all zeros.
    /// Uses precomputed counts of consecutive zeros to the right and down. O(n^3) worst-case but fast in practice.
    /// </summary>
    public static bool SquareOfZeroes(List<List<int>> matrix)
    {
        int n = matrix.Count;
        if (n == 0) return false;
        int m = matrix[0].Count;
        if (m == 0) return false;

        var right = new int[n, m];
        var down = new int[n, m];

        // Precompute consecutive zeros to the right and down for each cell
        for (int r = n - 1; r >= 0; r--)
        {
            for (int c = m - 1; c >= 0; c--)
            {
                if (matrix[r][c] == 0)
                {
                    right[r, c] = 1 + (c + 1 < m ? right[r, c + 1] : 0);
                    down[r, c] = 1 + (r + 1 < n ? down[r + 1, c] : 0);
                }
                else
                {
                    right[r, c] = 0;
                    down[r, c] = 0;
                }
            }
        }

        // Try all top-left corners and sizes
        for (int r = 0; r < n; r++)
        {
            for (int c = 0; c < m; c++)
            {
                // max possible size from here
                int maxSize = Math.Min(n - r, m - c);
                for (int size = 2; size <= maxSize; size++)
                {
                    int br = r + size - 1;
                    int bc = c + size - 1;

                    bool topOk = right[r, c] >= size;
                    bool leftOk = down[r, c] >= size;
                    bool bottomOk = right[br, c] >= size;
                    bool rightOk = down[r, bc] >= size;

                    if (topOk && leftOk && bottomOk && rightOk)
                        return true;
                }
            }
        }
        return false;
    }

    // =========================
    // ======== LinkedList =====
    // =========================
    public class LinkedList
    {
        public LinkedList? Next;
        public int Value;

        public LinkedList(int value)
        {
            this.Value = value;
        }
    }

    /// <summary>
    /// Remove k-th node from end, in-place, using two-pointer technique. O(n).
    /// </summary>
    public static void RemoveKthNodeFromEnd(TestLinkedList test, int k)
    {
        if (test?.Head == null || k <= 0) return;

        var head = test.Head;
        var first = head;
        var second = head;

        // advance second by k
        for (int i = 0; i < k; i++)
        {
            if (second == null) return; // k > length
            second = second.Next!;
        }

        // If second null => remove head
        if (second == null)
        {
            // Copy next into head (constraint: we must mutate head, not reassign reference)
            if (head.Next != null)
            {
                head.Value = head.Next.Value;
                head.Next = head.Next.Next;
            }
            else
            {
                // single node list and k==1 -> make it "empty" by convention (no-op otherwise)
                test.Head = null;
            }
            return;
        }

        // Move together
        while (second.Next != null)
        {
            first = first.Next!;
            second = second.Next!;
        }

        // delete first.next
        first.Next = first.Next?.Next;
    }

    /// <summary>
    /// Reverse singly linked list iteratively. O(n).
    /// </summary>
    public static LinkedList ReverseLinkedList(LinkedList head)
    {
        LinkedList? prev = null;
        var curr = head;
        while (curr != null)
        {
            var nxt = curr.Next;
            curr.Next = prev;
            prev = curr;
            curr = nxt!;
        }
        return prev!;
    }

    /// <summary>
    /// Shift list by k (positive: right, negative: left). O(n).
    /// </summary>
    public static LinkedList ShiftLinkedList(LinkedList head, int k)
    {
        if (head == null || head.Next == null || k == 0) return head;

        // compute length and tail
        int length = 1;
        var tail = head;
        while (tail.Next != null)
        {
            tail = tail.Next;
            length++;
        }

        k %= length;
        if (k == 0) return head;
        if (k < 0) k += length; // convert left shift to right

        // make it circular
        tail.Next = head;

        // new tail at length - k - 1 steps from head
        int stepsToNewTail = length - k - 1;
        var newTail = head;
        for (int i = 0; i < stepsToNewTail; i++) newTail = newTail.Next!;
        var newHead = newTail.Next!;

        // break circle
        newTail.Next = null;
        return newHead;
    }

    public class MinMaxStack
    {
        public void Push(int p0)
        {
            throw new NotImplementedException();
        }

        public int Pop()
        {
            throw new NotImplementedException();
        }

        public int GetMin()
        {
            throw new NotImplementedException();
        }

        public int GetMax()
        {
            throw new NotImplementedException();
        }

        public int Peek()
        {
            throw new NotImplementedException();
        }
    }

    public static object ProductSum(List<object> test)
    {
        throw new NotImplementedException();
    }

    public static int[] SearchInSortedMatrix(int[,] matrix, int i)
    {
        throw new NotImplementedException();
    }

    public class MinHeap
    {
        public MinHeap(List<int> ints)
        {
            throw new NotImplementedException();
        }
    }
}

// ======================================
// ========= TEST CLASSES ===============
// ======================================
public class TestLinkedList
{
    public GenericClassAlgorithm.LinkedList? Head { get; set; }
}