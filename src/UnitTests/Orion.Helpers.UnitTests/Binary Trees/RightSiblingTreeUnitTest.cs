namespace Orion.Helpers.UnitTests.Binary_Trees
{
    public partial class UnitTest1
    {
        [Fact(Skip = "Fix later")]
        public void Test1()
        {
            // Arrange
            var root = new GenericClassAlgorithm.BinaryTree(1);
            Insert(root, new int[] { 2, 3, 4, 5, 6, 7, 8, 9 });

            root.Left.Right.Right = new GenericClassAlgorithm.BinaryTree(10);
            root.Right.Left.Left = new GenericClassAlgorithm.BinaryTree(11);
            root.Right.Right.Left = new GenericClassAlgorithm.BinaryTree(12);
            root.Right.Right.Right = new GenericClassAlgorithm.BinaryTree(13);
            root.Right.Left.Left.Left = new GenericClassAlgorithm.BinaryTree(14);

            // Act
            GenericClassAlgorithm.BinaryTree mutatedRoot = GenericClassAlgorithm.RightSiblingTree(root);
            List<int> actual = GetDfsOrder(mutatedRoot);

            // Expected DFS order after mutation
            var expected = new List<int>
            {
                1, 2, 4, 8, 9, 5, 6, 11, 14, 7, 12, 13, 3, 6, 11, 14, 7, 12, 13
            };

            // Assert
            Assert.True(expected.SequenceEqual(actual));
        }

        // BFS Insert (level-order)
        public void Insert(GenericClassAlgorithm.BinaryTree root, int[] values)
        {
            insert(root, values, 0);
        }

        private void insert(GenericClassAlgorithm.BinaryTree root, int[] values, int i)
        {
            if (i >= values.Length) return;

            Queue<GenericClassAlgorithm.BinaryTree> queue = new Queue<GenericClassAlgorithm.BinaryTree>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.Left == null)
                {
                    current.Left = new GenericClassAlgorithm.BinaryTree(values[i]);
                    break;
                }
                else
                {
                    queue.Enqueue(current.Left);
                }

                if (current.Right == null)
                {
                    current.Right = new GenericClassAlgorithm.BinaryTree(values[i]);
                    break;
                }
                else
                {
                    queue.Enqueue(current.Right);
                }
            }

            insert(root, values, i + 1);
        }

        // Depth-first traversal (PreOrder)
        private List<int> GetDfsOrder(GenericClassAlgorithm.BinaryTree tree)
        {
            var values = new List<int>();
            values.Add(tree.Value);

            if (tree.Left != null)
                values.AddRange(GetDfsOrder(tree.Left));

            if (tree.Right != null)
                values.AddRange(GetDfsOrder(tree.Right));

            return values;
        }
    }
}
