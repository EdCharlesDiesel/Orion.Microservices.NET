namespace Orion.Helpers.LinkedList
{
    public class FindLoopClass
    {
        // O(n) time | O(1) space
        public static LinkedList FindLoop(LinkedList head)
        {
            LinkedList first = head.Next;
            LinkedList second = head.Next.Next;
            while (first != second)
            {
                first = first.Next;
                second = second.Next.Next;
            }
            first = head;
            while (first != second)
            {
                first = first.Next;
                second = second.Next;
            }
            return first;
        }
        public class LinkedList
        {
            public int Value;
            public LinkedList Next = null!;
            public LinkedList(int value)
            {
                this.Value = value;
            }
        }
    }
}