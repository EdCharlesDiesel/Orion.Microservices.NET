namespace Orion.Helpers.Graphs
{ 
    public class RectangleManiaClass
    {
        private static readonly string Up = "up";
        private static readonly string Right = "right";
        private static readonly string Down = "down";

        private static readonly string Left = "left";
        // O(n^2) time | O(n^2) space - where n is the number of coordinates
        public static int RectangleMania(Point[] coords)
        {
            var coordsTable = GetCoordsTable(
            coords);
            return GetRectangleCount(coords, coordsTable);
        }

        private static Dictionary<string, Dictionary<string, List<Point>>> GetCoordsTable(
        Point[] coords)
        {
            Dictionary<string, Dictionary<string,
            List<Point>>> coordsTable = new Dictionary<string,
            Dictionary<string,
            List<Point>>>();
            foreach (Point coord1 in coords)
            {
                Dictionary<string, List<Point>> coord1Directions = new Dictionary<string,
                List<Point>>();
                coord1Directions.Add(Up, new List<Point>());
                coord1Directions.Add(Right, new List<Point>());
                coord1Directions.Add(Down, new List<Point>());
                coord1Directions.Add(Left, new List<Point>());
                foreach (Point coord2 in coords)
                {
                    string coord2Direction = GetCoordDirection(coord1, coord2);
                    if (coord1Directions.ContainsKey(coord2Direction)) coord1Directions[
                    coord2Direction].Add(coord2);
                }
                string coord1String = CoordTostring(coord1);
                coordsTable.Add(coord1String, coord1Directions);
            }
            return coordsTable;
        }


        private static string GetCoordDirection(Point coord1, Point coord2)
        {
            if (coord2.Y == coord1.Y)
            {
                if (coord2.X > coord1.X)
                {
                    return Right;
                }

                if (coord2.X < coord1.X)
                {
                    return Left;
                }
            }
            else if (coord2.X == coord1.X)
            {
                if (coord2.Y > coord1.Y)
                {
                    return Up;
                }

                if (coord2.Y < coord1.Y)
                {
                    return Down;
                }
            }
            return "";
        }

        private static int GetRectangleCount(Point[] coords, Dictionary<string, Dictionary<string,
        List<Point>>> coordsTable)
        {
            var rectangleCount = 0;
            foreach (var coord in coords)
            {
                rectangleCount += ClockwiseCountRectangles(coord, coordsTable, Up, coord);
            }
            return rectangleCount;
        }

        private static int ClockwiseCountRectangles(
        Point coord,
        Dictionary<string, Dictionary<string, List<Point>>> coordsTable,
        string direction,
        Point origin
        )
        {
            var coordstring = CoordTostring(coord);
            if (direction == Left)
            {
                bool rectangleFound = coordsTable[coordstring][Left].Contains(origin);
                return rectangleFound ? 1 : 0;
            }

            var rectangleCount = 0;
            var nextDirection = GetNextClockwiseDirection(direction);
            foreach (var nextCoord in coordsTable[coordstring][direction])
            {
                rectangleCount += ClockwiseCountRectangles(nextCoord, coordsTable,
                    nextDirection, origin);
            }
            return rectangleCount;
        }
        public static string GetNextClockwiseDirection(string direction)
        {
            if (direction == Up) return Right;
            if (direction == Right) return Down;
            if (direction == Down) return Left;
            return "";
        }
        public static string CoordTostring(Point coord)
        {
            return coord.X + "-" + coord.Y;
        }
    }
    public class Point(int x, int y)
    {
        public readonly int X = x;
        public readonly int Y = y;
    }
}