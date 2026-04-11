namespace Orion.Helpers.Arrays
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ReverseWordsInStringClass
{
    // Method 1: Using built-in string methods (simplest)
    public string ReverseWordsInString(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        // Split by spaces, reverse the array, then join back
        string[] words = str.Split(' ');
        Array.Reverse(words);
        return string.Join(" ", words);
    }

    // Method 2: Using LINQ (one-liner approach)
    public string ReverseWordsInStringLinq(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        return string.Join(" ", str.Split(' ').Reverse());
    }

    // Method 3: Manual approach with StringBuilder (more control)
    public string ReverseWordsInStringManual(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        List<string> words = new List<string>();
        StringBuilder currentWord = new StringBuilder();

        // Extract words manually
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == ' ')
            {
                if (currentWord.Length > 0)
                {
                    words.Add(currentWord.ToString());
                    currentWord.Clear();
                }
                words.Add(" "); // Preserve spaces as separate elements
            }
            else
            {
                currentWord.Append(str[i]);
            }
        }

        // Add the last word if exists
        if (currentWord.Length > 0)
        {
            words.Add(currentWord.ToString());
        }

        // Filter out spaces and reverse only the actual words
        var actualWords = words.Where(w => w != " ").ToArray();
        Array.Reverse(actualWords);

        return string.Join(" ", actualWords);
    }

    // Method 4: In-place character array approach (most memory efficient)
    public string ReverseWordsInStringInPlace(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        char[] chars = str.ToCharArray();
        
        // Step 1: Reverse the entire string
        ReverseArray(chars, 0, chars.Length - 1);
        
        // Step 2: Reverse each word back to its original form
        int start = 0;
        for (int i = 0; i <= chars.Length; i++)
        {
            if (i == chars.Length || chars[i] == ' ')
            {
                ReverseArray(chars, start, i - 1);
                start = i + 1;
            }
        }
        
        return new string(chars);
    }

    private void ReverseArray(char[] arr, int start, int end)
    {
        while (start < end)
        {
            char temp = arr[start];
            arr[start] = arr[end];
            arr[end] = temp;
            start++;
            end--;
        }
    }

    // Method 5: Stack-based approach
    public string ReverseWordsInStringStack(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        Stack<string> wordStack = new Stack<string>();
        StringBuilder currentWord = new StringBuilder();

        // Push words onto stack
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == ' ')
            {
                if (currentWord.Length > 0)
                {
                    wordStack.Push(currentWord.ToString());
                    currentWord.Clear();
                }
            }
            else
            {
                currentWord.Append(str[i]);
            }
        }

        // Push the last word
        if (currentWord.Length > 0)
        {
            wordStack.Push(currentWord.ToString());
        }

        // Pop from stack to get reversed order
        return string.Join(" ", wordStack.ToArray());
    }

    // Method 6: Handling multiple spaces (if needed)
    public string ReverseWordsInStringPreserveSpaces(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        // Split but keep empty entries to preserve multiple spaces
        string[] parts = str.Split(new char[] { ' ' }, StringSplitOptions.None);
        
        // Filter out empty strings (spaces) and collect actual words
        var words = parts.Where(part => !string.IsNullOrEmpty(part)).ToArray();
        
        // Reverse the words
        Array.Reverse(words);
        
        return string.Join(" ", words);
    }
}
}
