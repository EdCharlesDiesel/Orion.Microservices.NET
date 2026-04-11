

namespace Orion.Helpers.LinkedList
{
    public class ShiftLinkedListClass
    {
        // O(n) time | O(1) space - where n is the number of nodes in the Linked List
        public static LinkedList ShiftLinkedList(LinkedList head, int k)
        {
            int listLength = 1;
            LinkedList listTail = head;
            while (listTail.Next != null)
            {
                listTail = listTail.Next;
                listLength++;
            }
            int offset = Math.Abs(k) % listLength;
            if (offset == 0) return head;
            int newTailPosition = k > 0 ? listLength - offset : offset;
            LinkedList newTail = head;
            for (int i = 1; i < newTailPosition; i++)
            {
                newTail = newTail.Next;
            }
            LinkedList newHead = newTail.Next;
            newTail.Next = null;
            listTail.Next = head;
            return newHead;


        }

    }


}