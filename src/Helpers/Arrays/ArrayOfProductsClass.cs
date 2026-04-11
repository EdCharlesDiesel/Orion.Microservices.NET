namespace Orion.Helpers.Arrays
{
    /// <summary>
    /// Write a function that takes in a non-empty array of integers and
    /// returns an array of the same length, where each element in the output
    /// array is equal to the product of every other number in the
    /// input array. In other words, the value at output[i] is equal to the product of every 
    /// number in the input array other than input of output[i]. Note that you're expected to
    /// solve this problem without using division.
    /// </summary>
    public static class ArrayOfProductsClass
    {
        public static int[] ArrayOfProducts(int[] array)
        {
            if (array == null || array.Length == 0)
                return new int[0];
        
            int[] result = new int[array.Length];
        
            // Method 1: Two-pass approach (O(n) time, O(1) extra space)
            // First pass: calculate products of all elements to the left
            result[0] = 1;
            for (int i = 1; i < array.Length; i++)
            {
                result[i] = result[i - 1] * array[i - 1];
            }
        
            // Second pass: multiply by products of all elements to the right
            int rightProduct = 1;
            for (int i = array.Length - 1; i >= 0; i--)
            {
                result[i] *= rightProduct;
                rightProduct *= array[i];
            }
        
            return result;
        }
    
        // Alternative simpler approach (O(n²) time)
        public static int[] ArrayOfProductsSimple(int[] array)
        {
            if (array == null || array.Length == 0)
                return new int[0];
        
            int[] result = new int[array.Length];
        
            for (int i = 0; i < array.Length; i++)
            {
                int product = 1;
                for (int j = 0; j < array.Length; j++)
                {
                    if (i != j)
                    {
                        product *= array[j];
                    }
                }
                result[i] = product;
            }
        
            return result;
        }
    }
}