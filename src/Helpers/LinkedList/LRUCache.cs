namespace Orion.Helpers.LinkedList
{
    public class LruCache
    {
        public Dictionary<string, DoublyLinkedListNode> Cache = new Dictionary<string, DoublyLinkedListNode>();
        public int MaxSize;
        public int CurrentSize;
        public DoublyLinkedList ListOfMostRecent = new DoublyLinkedList();

        public LruCache(int maxSize)
        {
            this.MaxSize = maxSize > 1 ? maxSize : 1;
        }

        // O(1) time | O(1) space
        public void InsertKeyValuePair(string key, int value)
        {
            if (!Cache.ContainsKey(key))
            {
                if (CurrentSize == MaxSize)
                {
                    EvictLeastRecent();
                }
                else
                {
                    CurrentSize++;
                }

                Cache[key] = new DoublyLinkedListNode(key, value);
            }
            else
            {
                ReplaceKey(key, value);
            }

            UpdateMostRecent(Cache[key]);
        }

        // O(1) time | O(1) space
        public LruResult GetValueFromKey(string key)
        {
            if (!Cache.ContainsKey(key))
            {
                return new LruResult(false, -1);
            }

            UpdateMostRecent(Cache[key]);
            return new LruResult(true, Cache[key].Value);
        }

        // O(1) time | O(1) space
        public string GetMostRecentKey()
        {
            return ListOfMostRecent.Head.Key;
        }

        private void EvictLeastRecent()
        {
            string keyToRemove = ListOfMostRecent.Tail.Key;
            ListOfMostRecent.RemoveTail();
            Cache.Remove(keyToRemove);
        }

        private void UpdateMostRecent(DoublyLinkedListNode node)
        {
            ListOfMostRecent.SetHeadTo(node);
        }

        private void ReplaceKey(string key, int value)
        {
            if (!Cache.ContainsKey(key))
            {
                return;
            }

            Cache[key].Value = value;
        }
    }

    public class LruResult
    {
        public bool Found;
        public int Value;

        public LruResult(bool found, int value)
        {
            this.Found = found;
            this.Value = value;
        }
    }

    public class DoublyLinkedListNode
    {
        public string Key;
        public int Value;
        public DoublyLinkedListNode Prev;
        public DoublyLinkedListNode Next;

        public DoublyLinkedListNode(string key, int value)
        {
            this.Key = key;
            this.Value = value;
        }

        public void RemoveBindings()
        {
            if (Prev != null)
            {
                Prev.Next = Next;
            }

            if (Next != null)
            {
                Next.Prev = Prev;
            }

            Prev = null;
            Next = null;
        }
    }

    public class DoublyLinkedList
    {
        public DoublyLinkedListNode Head;
        public DoublyLinkedListNode Tail;

        public void SetHeadTo(DoublyLinkedListNode node)
        {
            if (Head == node)
            {
            }
            else if (Head == null)
            {
                Head = node;
                Tail = node;
            }
            else if (Head == Tail)
            {
                Tail.Prev = node;
                Head = node;
                Head.Next = Tail;
            }
            else
            {
                if (Tail == node)
                {
                    RemoveTail();
                }

                node.RemoveBindings();
                Head.Prev = node;
                node.Next = Head;
                Head = node;
            }
        }

        public void RemoveTail()
        {
            if (Tail == null) return;

            if (Tail == Head)
            {
                Head = null;
                Tail = null;
                return;
            }

            Tail = Tail.Prev;
            if (Tail != null) Tail.Next = null;
        }
    }
}
