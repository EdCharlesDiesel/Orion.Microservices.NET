namespace Orion.Helpers.Tries
{
    public class MultiStringSearchClass3
    {
        // O(ns + bs) time | O(ns) space
        public static List<bool> MultistringSearch(string bigstring, string[] smallstrings)
        {
            Trie trie = new Trie();
            foreach (string smallstring in smallstrings)
            {
                trie.Insert(smallstring);
            }
            HashSet<string> containedstrings = new HashSet<string>();
            for (int i = 0; i < bigstring.Length; i++)
            {
                FindSmallstringsIn(bigstring, i, trie, containedstrings);
            }
            List<bool> solution = new List<bool>();
            foreach (string str in smallstrings)
            {
                solution.Add(containedstrings.Contains(str));
            }
            return solution;
        }
        public static void FindSmallstringsIn(string str, int startIdx, Trie trie,
        HashSet<string> containedstrings)
        {
            TrieNode currentNode = trie.Root;
            for (int i = startIdx; i < str.Length; i++)
            {
                char currentChar = str[i];
                if (!currentNode.Children.ContainsKey(currentChar))
                {
                    break;
                }
                currentNode = currentNode.Children[currentChar];
                if (currentNode.Children.ContainsKey(trie.EndSymbol))
                {
                    containedstrings.Add(currentNode.Word);
                }
            }
        }
     
    }

    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();
        public string Word;
    }
    public class Trie
    {
        public TrieNode Root = new TrieNode();
        public char EndSymbol = '*';
        public void Insert(string str)
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
