using Orion.Helpers.Binary_Trees;

namespace Orion.Helpers.UnitTests.Binary_Trees
{
    public class InvertBinaryTreeClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            TestBinaryTree tree = new TestBinaryTree(1);
            tree.insert(new[] { 2, 3, 4, 5, 6, 7, 8, 9 }, 0);
            InvertBinaryTreeClass.InvertBinaryTree(tree);
            InvertedBinaryTree invertedTree = new InvertedBinaryTree(1);
            invertedTree.insert(new[] { 2, 3, 4, 5, 6, 7, 8, 9 }, 0);
            Assert.True(CompareBt(tree, invertedTree));
        }

        private bool CompareBt(InvertBinaryTreeClass.BinaryTree tree1, InvertedBinaryTree tree2)
        {
            if (tree1 == null && tree2 == null)
            {
                return true;
            }
            if (tree1 != null && tree2 != null)
            {
                return tree1.Value == tree2.value && CompareBt(tree1.Left, tree2.Left) &&
                       CompareBt(tree1.Right, tree2.Right);
            }
            return false;
        }

        class InvertedBinaryTree
        {
            public int value;
            public InvertedBinaryTree Left;
            public InvertedBinaryTree Right;

            public InvertedBinaryTree(int value)
            {
                this.value = value;
            }

            public void insert(int[] values, int i)
            {
                if (i >= values.Length)
                {
                    return;
                }
                List<InvertedBinaryTree> queue = new List<InvertedBinaryTree>();
                queue.Add(this);
                var index = 0;
                while (index < queue.Count)
                {
                    InvertedBinaryTree current = queue[index];
                    index += 1;
                    if (current.Right == null)
                    {
                        current.Right = new InvertedBinaryTree(values[i]);
                        break;
                    }
                    queue.Add(current.Right);
                    if (current.Left == null)
                    {
                        current.Left = new InvertedBinaryTree(values[i]);
                        break;
                    }
                    queue.Add(current.Left);
                }
                insert(values, i + 1);
            }
        }

        public class TestBinaryTree : InvertBinaryTreeClass.BinaryTree
        {
            public TestBinaryTree(int value) : base(value) { }

            public void insert(int[] values, int i)
            {
                if (i >= values.Length)
                {
                    return;
                }
                List<InvertBinaryTreeClass.BinaryTree> queue = new List<InvertBinaryTreeClass.BinaryTree>();
                queue.Add(this);
                var index = 0;
                while (index < queue.Count)
                {
                    InvertBinaryTreeClass.BinaryTree current = queue[index];
                    index += 1;
                    if (current.Left == null)
                    {
                        current.Left = new InvertBinaryTreeClass.BinaryTree(values[i]);
                        break;
                    }
                    queue.Add(current.Left);
                    if (current.Right == null)
                    {
                        current.Right = new InvertBinaryTreeClass.BinaryTree(values[i]);
                        break;
                    }
                    queue.Add(current.Right);
                }
                insert(values, i + 1);
            }
        }
    }
}