﻿namespace Orion.Helpers.Arrays
{
    public class LongestSubarrayWithSumClass
    {
        public int[] LongestSubarrayWithSum(int[] array, int targetSum)
        {
            int[] indices = new int[] { };

            for (int startingIndex = 0; startingIndex < array.Length; startingIndex++)
            {
                int currentSubarraySum = 0;
                for (int endingIndex = startingIndex; endingIndex < array.Length; endingIndex++)
                {
                    currentSubarraySum += array[endingIndex];

                    if (currentSubarraySum == targetSum)
                    {
                        if (indices.Length ==0 || indices[1] - indices[0] < endingIndex -startingIndex)
                        {
                            indices= new[] {startingIndex,endingIndex};
                        }
                    }
                }
            }

            return indices;
        }
    }
}