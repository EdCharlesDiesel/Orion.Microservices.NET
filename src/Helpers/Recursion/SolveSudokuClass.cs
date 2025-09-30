﻿namespace Orion.Helpers.Recursion
{
    /// <summary>
    /// 
    /// </summary>
    public class SolveSudokuClass
    {
        public List<List<int>> SolveSudoku(List<List<int>> board)
        {
            SolvePartialSudoku(0, 0, board);
            return board;
        }

        private bool SolvePartialSudoku(int row, int col, List<List<int>> board)
        {
            int currentRow = row;
            int currentCol = col;

            if (currentCol == board[currentRow].Count)
            {
                currentRow += 1;
                currentCol = 0;
                if (currentCol == board.Count)
                {
                    return true;
                }
            }

            if (board[currentRow][currentCol] == 0)
            {
                return TryDigitsAtPosition(currentRow, currentCol,board);
            }

            return SolvePartialSudoku(currentRow, currentCol +1, board);
        }

        private bool TryDigitsAtPosition(int row, int col, List<List<int>> board)
        {
            for (int digit = 1; digit < 10; digit++)
            {
                if (IsValidAtPosition(digit, row, col, board))
                {
                    board[row][col] = digit;
                    if (SolvePartialSudoku(row,col+1,board))
                    {
                        return true;
                    }
                }
            }
            board[row][col] = 0;
            return false;
        }

        private bool IsValidAtPosition(int value, int row, int col, List<List<int>> board)
        {
            bool rowIsValid = !board[row].Contains(value);
            bool columnIsValid = true;

            for (int r = 0; r < board.Count; r++)
            {
                if (board[r][col]==value)
                {
                    columnIsValid = false;
                }
            }

            if (!rowIsValid || !columnIsValid)
            {
                return false;
            }

            int subgridRowStart = (row / 3) * 3;
            int subgridColStart = (col / 3) * 3;

            for (int rowIndex = 0; rowIndex < 3; rowIndex++)
            {
                for (int colIndex = 0; colIndex < 3; colIndex++)
                {
                    int rowToCheck = subgridRowStart + rowIndex;
                    int colToCheck = subgridColStart + colIndex;
                    int exitingValue = board[rowToCheck][colToCheck];

                    if (exitingValue == value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}