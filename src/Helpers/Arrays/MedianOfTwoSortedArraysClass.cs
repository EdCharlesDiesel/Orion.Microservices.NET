namespace Orion.Helpers.Arrays
{
    /// <summary>
    /// You're given two sorted arrays of integers arrayOne and
    /// arrayTwo. Write a function that returns the median of these
    /// arrays.
    /// 
    /// You can assume both arrays have at least one value. however,
    /// they could be of difference lengths.if the median is a decimal
    /// value, it should not be rounded (beyound the inevitable rounding
    /// of floating rounding of point math)
    /// </summary>
   using System;

public class MedianOfTwoSortedArraysClass
{
    // Optimal O(log(min(m,n))) solution using binary search
    public float MedianOfTwoSortedArrays(int[] nums1, int[] nums2)
    {
        // Ensure nums1 is the smaller array for optimization
        if (nums1.Length > nums2.Length)
        {
            return MedianOfTwoSortedArrays(nums2, nums1);
        }

        int m = nums1.Length;
        int n = nums2.Length;
        int totalLeft = (m + n + 1) / 2;

        int left = 0, right = m;

        while (left <= right)
        {
            int cut1 = (left + right) / 2;
            int cut2 = totalLeft - cut1;

            int left1 = cut1 == 0 ? int.MinValue : nums1[cut1 - 1];
            int left2 = cut2 == 0 ? int.MinValue : nums2[cut2 - 1];

            int right1 = cut1 == m ? int.MaxValue : nums1[cut1];
            int right2 = cut2 == n ? int.MaxValue : nums2[cut2];

            if (left1 <= right2 && left2 <= right1)
            {
                // Found the correct partition
                if ((m + n) % 2 == 0)
                {
                    return (Math.Max(left1, left2) + Math.Min(right1, right2)) / 2.0f;
                }
                else
                {
                    return Math.Max(left1, left2);
                }
            }
            else if (left1 > right2)
            {
                // Move left in nums1
                right = cut1 - 1;
            }
            else
            {
                // Move right in nums1
                left = cut1 + 1;
            }
        }

        throw new ArgumentException("Input arrays are not valid");
    }

    // Simple O(m+n) merge approach - easier to understand
    public float MedianOfTwoSortedArraysSimple(int[] nums1, int[] nums2)
    {
        int m = nums1.Length;
        int n = nums2.Length;
        int[] merged = new int[m + n];
        
        int i = 0, j = 0, k = 0;
        
        // Merge the two arrays
        while (i < m && j < n)
        {
            if (nums1[i] <= nums2[j])
            {
                merged[k++] = nums1[i++];
            }
            else
            {
                merged[k++] = nums2[j++];
            }
        }
        
        // Add remaining elements from nums1
        while (i < m)
        {
            merged[k++] = nums1[i++];
        }
        
        // Add remaining elements from nums2
        while (j < n)
        {
            merged[k++] = nums2[j++];
        }
        
        // Find median
        int totalLength = m + n;
        if (totalLength % 2 == 0)
        {
            // Even length: average of two middle elements
            return (merged[totalLength / 2 - 1] + merged[totalLength / 2]) / 2.0f;
        }
        else
        {
            // Odd length: middle element
            return merged[totalLength / 2];
        }
    }

    // Alternative approach without extra space - O(m+n) time, O(1) space
    public float MedianOfTwoSortedArraysNoExtraSpace(int[] nums1, int[] nums2)
    {
        int m = nums1.Length;
        int n = nums2.Length;
        int totalLength = m + n;
        bool isEven = totalLength % 2 == 0;
        
        int targetIndex1 = totalLength / 2 - 1; // For even length
        int targetIndex2 = totalLength / 2;     // For both even and odd
        
        int i = 0, j = 0, currentIndex = 0;
        int value1 = 0, value2 = 0;
        
        while (currentIndex <= targetIndex2)
        {
            int currentValue;
            
            if (i >= m)
            {
                currentValue = nums2[j++];
            }
            else if (j >= n)
            {
                currentValue = nums1[i++];
            }
            else if (nums1[i] <= nums2[j])
            {
                currentValue = nums1[i++];
            }
            else
            {
                currentValue = nums2[j++];
            }
            
            if (currentIndex == targetIndex1)
                value1 = currentValue;
            if (currentIndex == targetIndex2)
                value2 = currentValue;
                
            currentIndex++;
        }
        
        return isEven ? (value1 + value2) / 2.0f : value2;
    }
}
}