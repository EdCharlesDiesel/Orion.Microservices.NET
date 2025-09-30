namespace Orion.Helpers.Arrays
{
    /// <summary>
    /// 
    /// </summary>
    public class KnightConnectionClass
    {
  public int KnightConnection(int[] knightA, int[] knightB)
    {
        // If already at the same position
        if (knightA[0] == knightB[0] && knightA[1] == knightB[1])
            return 0;

        // All possible knight moves (8 directions)
        int[,] moves = new int[,] 
        {
            {2, 1}, {2, -1}, {-2, 1}, {-2, -1},
            {1, 2}, {1, -2}, {-1, 2}, {-1, -2}
        };

        // BFS to find shortest path
        Queue<(int x, int y, int moves)> queue = new Queue<(int, int, int)>();
        HashSet<(int, int)> visited = new HashSet<(int, int)>();

        queue.Enqueue((knightA[0], knightA[1], 0));
        visited.Add((knightA[0], knightA[1]));

        while (queue.Count > 0)
        {
            var (currentX, currentY, currentMoves) = queue.Dequeue();

            // Try all 8 possible knight moves
            for (int i = 0; i < 8; i++)
            {
                int newX = currentX + moves[i, 0];
                int newY = currentY + moves[i, 1];

                // Check if we reached the target
                if (newX == knightB[0] && newY == knightB[1])
                {
                    return currentMoves + 1;
                }

                // If this position hasn't been visited, add it to the queue
                // Note: For infinite board, we don't need boundary checks
                // But we should limit search to reasonable bounds to avoid infinite search
                if (!visited.Contains((newX, newY)) && 
                    Math.Abs(newX) <= 100 && Math.Abs(newY) <= 100) // Reasonable bounds
                {
                    visited.Add((newX, newY));
                    queue.Enqueue((newX, newY, currentMoves + 1));
                }
            }
        }

        // Should never reach here for valid knight positions
        return -1;
    }

    // Alternative implementation for bounded chessboard (8x8)
    public int KnightConnectionChessBoard(int[] knightA, int[] knightB, int boardSize = 8)
    {
        // Validate input
        if (!IsValidPosition(knightA, boardSize) || !IsValidPosition(knightB, boardSize))
            return -1;

        // If already at the same position
        if (knightA[0] == knightB[0] && knightA[1] == knightB[1])
            return 0;

        // All possible knight moves
        int[,] moves = new int[,] 
        {
            {2, 1}, {2, -1}, {-2, 1}, {-2, -1},
            {1, 2}, {1, -2}, {-1, 2}, {-1, -2}
        };

        // BFS
        Queue<(int x, int y, int moves)> queue = new Queue<(int, int, int)>();
        bool[,] visited = new bool[boardSize, boardSize];

        queue.Enqueue((knightA[0], knightA[1], 0));
        visited[knightA[0], knightA[1]] = true;

        while (queue.Count > 0)
        {
            var (currentX, currentY, currentMoves) = queue.Dequeue();

            // Try all 8 possible knight moves
            for (int i = 0; i < 8; i++)
            {
                int newX = currentX + moves[i, 0];
                int newY = currentY + moves[i, 1];

                // Check bounds
                if (newX < 0 || newX >= boardSize || newY < 0 || newY >= boardSize)
                    continue;

                // Check if we reached the target
                if (newX == knightB[0] && newY == knightB[1])
                {
                    return currentMoves + 1;
                }

                // If this position hasn't been visited
                if (!visited[newX, newY])
                {
                    visited[newX, newY] = true;
                    queue.Enqueue((newX, newY, currentMoves + 1));
                }
            }
        }

        return -1; // No path found (shouldn't happen on connected board)
    }

    private bool IsValidPosition(int[] position, int boardSize)
    {
        return position.Length == 2 && 
               position[0] >= 0 && position[0] < boardSize &&
               position[1] >= 0 && position[1] < boardSize;
    }
    }
}