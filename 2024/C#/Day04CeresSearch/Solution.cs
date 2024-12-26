using Solutions;

namespace Day04CeresSearch
{
    public partial class Solution : BaseSolution
    {
        protected override string ExampleFileName => "Day04_Example.txt";

        protected override string InputFileName => "Day04_Input.txt";

        public class MapSearch(char[][] map)
        {
            public bool Left(int x, int y, string searchString) => Search(i => x - i, i => y, searchString);
            public bool TopLeft(int x, int y, string searchString) => Search(i => x - i, i => y - i, searchString);
            public bool Top(int x, int y, string searchString) => Search(i => x, i => y - i, searchString);
            public bool TopRight(int x, int y, string searchString) => Search(i => x + i, i => y - i, searchString);
            public bool Right(int x, int y, string searchString) => Search(i => x + i, i => y, searchString);
            public bool BottomRight(int x, int y, string searchString) => Search(i => x + i, i => y + i, searchString);
            public bool Bottom(int x, int y, string searchString) => Search(i => x, i => y + i, searchString);
            public bool BottomLeft(int x, int y, string searchString) => Search(i => x - i, i => y + i, searchString);

            private bool Search(Func<int, int> xIncrement, Func<int, int> yIncrement, string searchString)
            {
                var maxMove = searchString.Length;

                if (xIncrement(maxMove) < 0 ||
                    xIncrement(maxMove) >= map.Length ||
                    yIncrement(maxMove) < 0 ||
                    yIncrement(maxMove) >= map.Length) return false;

                return Enumerable.Range(1, maxMove)
                          .All(i => searchString[i - 1] == map[yIncrement(i)][xIncrement(i)]);
            }
        }

        public override string SolveFirst(IEnumerable<string> lines)
        {
            var map = lines.Select(Enumerable.ToArray).ToArray();
            var searchString = "MAS";
            var mapSearch = new MapSearch(map);

            var xmases = 0;

            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] != 'X') continue;

                    if (mapSearch.Left(x, y, searchString)) xmases++;
                    if (mapSearch.TopLeft(x, y, searchString)) xmases++;
                    if (mapSearch.Top(x, y, searchString)) xmases++;
                    if (mapSearch.TopRight(x, y, searchString)) xmases++;
                    if (mapSearch.Right(x, y, searchString)) xmases++;
                    if (mapSearch.BottomRight(x, y, searchString)) xmases++;
                    if (mapSearch.Bottom(x, y, searchString)) xmases++;
                    if (mapSearch.BottomLeft(x, y, searchString)) xmases++;
                }
            }

            return xmases.ToString();
        }

        public override string SolveSecond(IEnumerable<string> lines)
        {
            var map = lines.Select(Enumerable.ToArray).ToArray();
            var mapSearch = new MapSearch(map);
            var xmases = 0;

            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    bool straightClockwiseMas() => mapSearch.TopLeft(x, y, "M") && mapSearch.BottomRight(x, y, "S");
                    bool reverseClockwiseMas() => mapSearch.TopLeft(x, y, "S") && mapSearch.BottomRight(x, y, "M");
                    bool straightAntiClockwiseMas() => mapSearch.BottomLeft(x, y, "M") && mapSearch.TopRight(x, y, "S");
                    bool reverseAntiClockwiseMas() => mapSearch.BottomLeft(x, y, "S") && mapSearch.TopRight(x, y, "M");

                    if (map[y][x] != 'A') continue;

                    if ((straightClockwiseMas() || reverseClockwiseMas()) &&
                        (straightAntiClockwiseMas() || reverseAntiClockwiseMas())) xmases++;
                }
            }

            return xmases.ToString();
        }
    }
}
