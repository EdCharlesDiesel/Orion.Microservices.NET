using Orion.Helpers.Binary_Trees;

namespace Orion.Helpers.UnitTests.Binary_Trees
{
    public class FlattenBinaryTreeClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            FlattenBinaryTreeClass.BinaryTree root = new FlattenBinaryTreeClass.BinaryTree(1);
            insert(root, new[] { 2, 3, 4, 5, 6 });
            root.Left.Right.Left = new FlattenBinaryTreeClass.BinaryTree(7);
            root.Left.Right.Right = new FlattenBinaryTreeClass.BinaryTree(8);
            FlattenBinaryTreeClass.BinaryTree LeftMostNode = FlattenBinaryTreeClass.FlattenBinaryTree(root);
            List<int> LeftToRightToLeft = this.LeftToRightToLeft(LeftMostNode);
            List<int> expected =
              new List<int> { 4, 2, 7, 5, 8, 1, 6, 3, 3, 6, 1, 8, 5, 7, 2, 4 };
            Assert.True(expected.SequenceEqual(LeftToRightToLeft));
        }

        public void insert(FlattenBinaryTreeClass.BinaryTree root, int[] Values)
        {
            Insert(root, Values, 0);
        }

        public static void Insert(FlattenBinaryTreeClass.BinaryTree root, int[] Values, int i)
        {
            if (i >= Values.Length)
            {
                return;
            }
            Queue<FlattenBinaryTreeClass.BinaryTree> queue = new Queue<FlattenBinaryTreeClass.BinaryTree>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                FlattenBinaryTreeClass.BinaryTree current = queue.Dequeue();
                if (current.Left == null)
                {
                    current.Left = new FlattenBinaryTreeClass.BinaryTree(Values[i]);
                    break;
                }
                queue.Enqueue(current.Left);
                if (current.Right == null)
                {
                    current.Right = new FlattenBinaryTreeClass.BinaryTree(Values[i]);
                    break;
                }
                queue.Enqueue(current.Right);
            }
            Insert(root, Values, i + 1);
        }

        public List<int> LeftToRightToLeft(FlattenBinaryTreeClass.BinaryTree leftMost)
        {
            List<int> nodes = new List<int>();
            FlattenBinaryTreeClass.BinaryTree current = leftMost;
            while (current.Right != null)
            {
                nodes.Add(current.Value);
                current = current.Right;
            }
            nodes.Add(current.Value);
            while (current != null)
            {
                nodes.Add(current.Value);
                current = current.Left;
            }
            return nodes;
        }
    }
}