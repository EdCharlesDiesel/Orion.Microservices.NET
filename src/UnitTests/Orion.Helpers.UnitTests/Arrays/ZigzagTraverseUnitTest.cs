using System;
using System.Collections.Generic;

public class ZigzagTraverseClass
{
    public static List<int> ZigzagTraverse(List<List<int>> array)
    {
        if (array == null || array.Count == 0 || array[0].Count == 0)
            return new List<int>();

        List<int> result = new List<int>();
        int rows = array.Count;
        int cols = array[0].Count;
        
        int row = 0, col = 0;
        bool goingDown = true;

        while (row < rows && col < cols)
        {
            result.Add(array[row][col]);

            if (goingDown)
            {
                // Going down-left diagonally
                if (col == 0 || row == rows - 1)
                {
                    // Change direction
                    goingDown = false;
                    
                    if (row == rows - 1)
                    {
                        col++; // Move right along bottom edge
                    }
                    else
                    {
                        row++; // Move down along left edge
                    }
                }
                else
                {
                    // Continue diagonally down-left
                    row++;
                    col--;
                }
            }
            else
            {
                // Going up-right diagonally
                if (row == 0 || col == cols - 1)
                {
                    // Change direction
                    goingDown = true;
                    
                    if (col == cols - 1)
                    {
                        row++; // Move down along right edge
                    }
                    else
                    {
                        col++; // Move right along top edge
                    }
                }
                else
                {
                    // Continue diagonally up-right
                    row--;
                    col++;
                }
            }
        }

        return result;
    }

    // Alternative implementation using diagonal enumeration
    public static List<int> ZigzagTraverseAlternative(List<List<int>> array)
    {
        if (array == null || array.Count == 0 || array[0].Count == 0)
            return new List<int>();

        List<int> result = new List<int>();
        int rows = array.Count;
        int cols = array[0].Count;

        // Process each diagonal
        for (int diag = 0; diag < rows + cols - 1; diag++)
        {
            List<int> diagonal = new List<int>();
            
            // Determine starting position for this diagonal
            int startRow = Math.Max(0, diag - cols + 1);
            int startCol = Math.Max(0, diag - rows + 1);
            
            // Collect all elements in this diagonal
            int r = startRow;
            int c = diag - startRow;
            
            while (r < rows && c >= 0 && c < cols)
            {
                diagonal.Add(array[r][c]);
                r++;
                c--;
            }
            
            // Reverse every other diagonal to create zigzag pattern
            if (diag % 2 == 1)
            {
                diagonal.Reverse();
            }
            
            result.AddRange(diagonal);
        }

        return result;
    }

    // Simplified approach with clear direction tracking
    public static List<int> ZigzagTraverseSimple(List<List<int>> array)
    {
        if (array == null || array.Count == 0 || array[0].Count == 0)
            return new List<int>();

        List<int> result = new List<int>();
        int height = array.Count;
        int width = array[0].Count;
        
        int row = 0, col = 0;
        bool goingDown = true;

        for (int i = 0; i < height * width; i++)
        {
            result.Add(array[row][col]);

            if (goingDown)
            {
                if (col == 0 || row == height - 1)
                {
                    goingDown = false;
                    if (row == height - 1)
                        col++;
                    else
                        row++;
                }
                else
                {
                    row++;
                    col--;
                }
            }
            else
            {
                if (row == 0 || col == width - 1)
                {
                    goingDown = true;
                    if (col == width - 1)
                        row++;
                    else
                        col++;
                }
                else
                {
                    row--;
                    col++;
                }
            }
        }

        return result;
    }
}