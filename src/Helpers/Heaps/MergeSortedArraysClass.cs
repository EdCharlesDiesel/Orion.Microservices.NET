namespace Orion.Helpers.Heaps
{
    public class MergeSortedArraysClass
    {
        // O(nk) time | O(n + k) space - where where n is the total
        // number of array elements and k is the number of arrays
        public static List<int> MergeSortedArrays(List<List<int>> arrays)
        {
            List<int> sortedList = new List<int>();
            List<int> elementIdxs = Enumerable.Repeat(0, arrays.Count).ToList();
            while (true)
            {
                List<Item> smallestItems = new List<Item>();
                for (int arrayIdx = 0; arrayIdx < arrays.Count; arrayIdx++)
                {
                    List<int> relevantArray = arrays[arrayIdx];
                    int elementIdx = elementIdxs[arrayIdx];
                    if (elementIdx == relevantArray.Count) continue;
                    smallestItems.Add(new Item(arrayIdx, relevantArray[elementIdx]));
                }
                if (smallestItems.Count == 0) break;
                Item nextItem = GetMinValue(smallestItems);
                sortedList.Add(nextItem.Num);
                elementIdxs[nextItem.ArrayIdx] = elementIdxs[nextItem.ArrayIdx] + 1;
            }
            return sortedList;
        }
        public static Item GetMinValue(List<Item> items)
        {
            int minValueIdx = 0;
            for (int i = 1; i < items.Count; i++)
            {
                if (items[i].Num < items[minValueIdx].Num) minValueIdx = i;
            }
            return items[minValueIdx];
        }
    }

    public class Item
    {
        public int ArrayIdx;
        public int Num;
        public Item(int arrayIdx, int num)
        {
            this.ArrayIdx = arrayIdx;
            this.Num = num;
        }
    }
}