namespace Orion.Helpers.Graphs
{
    internal class RectangleManiaClass2
    {
        static string _up = "up";
        static string _right = "right";
        static string _down = "down";
        // O(n^2) time | O(n) space - where n is the number of coordinates
        public static int RectangleMania(Point[] coords)
        {
            Dictionary<string, Dictionary<int, List<Point>>> coordsTable = GetCoordsTable(
            coords);
            return GetRectangleCount(coords, coordsTable);
        }
        public static Dictionary<string, Dictionary<int, List<Point>>> GetCoordsTable(
        Point[] coords)
        {
            Dictionary<string, Dictionary<int,
            List<Point>>> coordsTable = new Dictionary<string,
            Dictionary<int,
            List<Point>>>();
            coordsTable.Add("x", new Dictionary<int, List<Point>>());
            coordsTable.Add("y", new Dictionary<int, List<Point>>());
            foreach (Point coord in coords)
            {
                if (!coordsTable["x"].ContainsKey(coord.X))
                {
                    coordsTable["x"].Add(coord.X, new List<Point>());
                }
                if (!coordsTable["y"].ContainsKey(coord.Y))
                {
                    coordsTable["y"].Add(coord.Y, new List<Point>());
                }
                coordsTable["x"][coord.X].Add(coord);
                coordsTable["y"][coord.Y].Add(coord);
            }
            return coordsTable;
        }
        public static int GetRectangleCount(Point[] coords, Dictionary<string, Dictionary<int,
        List<Point>>> coordsTable)
        {
            int rectangleCount = 0;
            foreach (Point coord in coords)
            {
                int lowerLeftY = coord.Y;
                rectangleCount += ClockwiseCountRectangles(coord, coordsTable, _up,
                lowerLeftY);
            }
            return rectangleCount;
        }
        public static int ClockwiseCountRectangles(
        Point coord1,
        Dictionary<string, Dictionary<int, List<Point>>> coordsTable,
        string direction,
        int lowerLeftY
        )
        {
            if (direction == _down)
            {
                List<Point> relevantCoords = coordsTable["x"][coord1.X];
                foreach (Point coord2 in relevantCoords)
                {
                    int lowerRightY = coord2.Y;
                    if (lowerRightY == lowerLeftY) return 1;
                }
                return 0;
            }

            int rectangleCount = 0;
            if (direction == _up)
            {
                List<Point> relevantCoords = coordsTable["x"][coord1.X];
                foreach (Point coord2 in relevantCoords)
                {
                    bool isAbove = coord2.Y > coord1.Y;
                    if (isAbove) rectangleCount += ClockwiseCountRectangles(
                        coord2, coordsTable, _right, lowerLeftY);
                }
            }
            else if (direction == _right)
            {
                List<Point> relevantCoords = coordsTable["y"][coord1.Y];
                foreach (Point coord2 in relevantCoords)
                {
                    bool isRight = coord2.X > coord1.X;
                    if (isRight) rectangleCount += ClockwiseCountRectangles(
                        coord2, coordsTable, _down, lowerLeftY);
                }
            }
            return rectangleCount;
        }
      
    }
    //public class Point
    //{
    //    public int x;
    //    public int y;
    //    public Point(int x, int y)
    //    {
    //        this.x = x;
    //        this.y = y;
    //    }
    //}
}
