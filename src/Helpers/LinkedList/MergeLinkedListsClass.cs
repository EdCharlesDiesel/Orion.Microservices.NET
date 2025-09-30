
namespace Orion.Helpers.LinkedList
{
    public class LinkedListClass
    {
        // O(n + m) time | O(1) space - where n is the number of nodes in the first
        // Linked List and m is the number of nodes in the second Linked List
        public static LinkedList MergeLinkedLists(LinkedList headOne, LinkedList headTwo)
        {
            LinkedList p1 = headOne;
            LinkedList p1Prev = null;
            LinkedList p2 = headTwo;
            while (p1 != null && p2 != null)
            {
                if (p1.Value < p2.Value)
                {
                    p1Prev = p1;
                    p1 = p1.Next;
                }
                else
                {
                    if (p1Prev != null)
                        p1Prev.Next = p2;
                    p1Prev = p2;
                    p2 = p2.Next;
                    p1Prev.Next = p1;
                }
            }
            if (p1 == null)
                p1Prev.Next = p2!;
            return headOne.Value < headTwo.Value ? headOne : headTwo;
        }
    }

    public abstract class LinkedList
    {
        public int Value;
        public LinkedList Next;

        protected LinkedList(int value)
        {
            this.Value = value;
            Next = null;
        }
    }
}
