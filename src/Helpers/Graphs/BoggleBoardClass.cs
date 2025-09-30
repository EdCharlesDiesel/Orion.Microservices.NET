namespace Orion.Helpers.Graphs
{
    public abstract class BoggleBoardClass
    {
        public static void Explore(int i, int j, char[,] board, TrieNode trieNode, bool[,] visited,HashSet<string> finalWords)
        {
            if (visited[i, j])
            {
                return;
            }
            char letter = board[i, j];
            if (!trieNode.Children.ContainsKey(letter))
            {
                return;
            }
            visited[i, j] = true;
            trieNode = trieNode.Children[letter];
            if (trieNode.Children.ContainsKey('*'))
            {
                finalWords.Add(trieNode.Word);
            }
            List<int[]> neighbors = GetNeighbors(i, j, board);
            foreach (int[] neighbor in neighbors)
            {
                Explore(neighbor[0], neighbor[1], board, trieNode, visited, finalWords);
            }
            visited[i, j] = false;
        }
        public static List<int[]> GetNeighbors(int i, int j, char[,] board)
        {
            List<int[]> neighbors = new List<int[]>();
            if (i > 0 && j > 0)
            {
                neighbors.Add(new[] { i - 1, j - 1 });
            }
            if (i > 0 && j < board.GetLength(1) - 1)
            {
                neighbors.Add(new[] { i - 1, j + 1 });
            }
            if (i < board.GetLength(0) - 1 && j < board.GetLength(1) - 1)
            {
                neighbors.Add(new[] { i + 1, j + 1 });
            }
            if (i < board.GetLength(0) - 1 && j > 0)
            {
                neighbors.Add(new[] { i + 1, j - 1 });
            }
            if (i > 0)
            {
                neighbors.Add(new[] { i - 1, j });
            }
            if (i < board.GetLength(0) - 1)
            {
                neighbors.Add(new[] { i + 1, j });
            }
            if (j > 0)
            {
                neighbors.Add(new[] { i, j - 1 });
            }
            if (j < board.GetLength(1) - 1)
            {
                neighbors.Add(new[] { i, j + 1 });
            }
            return neighbors;
        }

        public static List<string> BoggleBoard(char[,] board, string[] words)
        {
            throw new NotImplementedException();
        }
    }

    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();
        public string Word = "";
    }
    public class Trie
    {
        public TrieNode Root;
        public char EndSymbol;
        public Trie()
        {
            Root = new TrieNode();
            EndSymbol = '*';
        }
        public void Add(string str)
        {
            TrieNode node = Root;
            for (int i = 0; i < str.Length; i++)
            {
                char letter = str[i];
                if (!node.Children.ContainsKey(letter))
                {
                    TrieNode newNode = new TrieNode();
                    node.Children.Add(letter, newNode);
                }
                node = node.Children[letter];
            }
            node.Children[EndSymbol] = null;
            node.Word = str;
        }
    }
}