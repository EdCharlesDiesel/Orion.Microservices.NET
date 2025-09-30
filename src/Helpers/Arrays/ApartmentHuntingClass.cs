namespace Orion.Helpers.Arrays
{
    public static class ApartmentHuntingClass
    {
        // O(b^2*r) time | O(b) space - where b is the number of blocks and r is the number of requirements
        public static int ApartmentHunting(List<Dictionary<string, bool>> blocks, string[] reqs)
        {
            var maxDistancesAtBlocks = new int[blocks.Count];
            Array.Fill(maxDistancesAtBlocks, int.MinValue);
            for (var i = 0; i < blocks.Count; i++)
            {
                foreach (var req in reqs)
                {
                    var closestReqDistance = int.MaxValue;
                    for (var j = 0; j < blocks.Count; j++)
                    {
                        if (blocks[j][req])
                        {
                            closestReqDistance = Math.Min(closestReqDistance, DistanceBetween(
                            i,
                            j));
                        }
                    }
                    maxDistancesAtBlocks[i] = Math.Max(maxDistancesAtBlocks[i],
                    closestReqDistance);
                }
            }
            return GetIdxAtMinValue(maxDistancesAtBlocks);
        }

        private static int GetIdxAtMinValue(int[] array)
        {
            var idxAtMinValue = 0;
            var minValue = int.MaxValue;
            for (var i = 0; i < array.Length; i++)
            {
                var currentValue = array[i];
                if (currentValue >= minValue) continue;
                minValue = currentValue;
                idxAtMinValue = i;
            }
            return idxAtMinValue;
        }

        private static int DistanceBetween(int a, int b)
        {
            return Math.Abs(a - b);
        }
    }
    public static class ApartmentHuntingClass2
    {
        // O(br) time | O(br) space - where b is the number of blocks and r is the number of requirements
        public static int ApartmentHunting(List<Dictionary<string, bool>> blocks, string[] reqs)
        {
            var minDistancesFromBlocks = new int[reqs.Length][];
            for (var i = 0; i < reqs.Length; i++)
            {
                minDistancesFromBlocks[i] = GetMinDistances(blocks, reqs[i]);
            }
            var maxDistancesAtBlocks =
            GetMaxDistancesAtBlocks(blocks, minDistancesFromBlocks);
            return GetIdxAtMinValue(maxDistancesAtBlocks);
        }

        private static int[] GetMinDistances(List<Dictionary<string, bool>> blocks, string req)
        {
            var minDistances = new int[blocks.Count];
            var closestReqIdx = int.MaxValue;
            for (var i = 0; i < blocks.Count; i++)
            {
                if (blocks[i][req]) closestReqIdx = i;
                minDistances[i] = DistanceBetween(i, closestReqIdx);
            }
            for (var i = blocks.Count - 1; i >= 0; i--)
            {
                if (blocks[i][req]) closestReqIdx = i;
                minDistances[i] = Math.Min(minDistances[i], DistanceBetween(i,
                closestReqIdx));
            }
            return minDistances;
        }

        private static int[] GetMaxDistancesAtBlocks(List<Dictionary<string, bool>> blocks,
        int[][] minDistancesFromBlocks)
        {
            var maxDistancesAtBlocks = new int[blocks.Count];
            for (var i = 0; i < blocks.Count; i++)
            {
                var minDistancesAtBlock = new int[minDistancesFromBlocks.Length];
                for (var j = 0; j < minDistancesFromBlocks.Length; j++)
                {
                    minDistancesAtBlock[j] = minDistancesFromBlocks[j][i];
                }
                maxDistancesAtBlocks[i] = ArrayMax(minDistancesAtBlock);
            }
            return maxDistancesAtBlocks;
        }

        private static int GetIdxAtMinValue(int[] array)
        {
            var idxAtMinValue = 0;
            var minValue = Int32.MaxValue;
            for (var i = 0; i < array.Length; i++)
            {
                var currentValue = array[i];
                if (currentValue >= minValue) continue;
                minValue = currentValue;
                idxAtMinValue = i;
            }
            return idxAtMinValue;
        }

        private static int DistanceBetween(int a, int b)
        {
            return Math.Abs(a - b);
        }

        private static int ArrayMax(int[] array)
        {
            var max = array[0];
            foreach (var a in array)
            {
                if (a > max)
                {
                    max = a;
                }
            }
            return max;
        }
    }
}