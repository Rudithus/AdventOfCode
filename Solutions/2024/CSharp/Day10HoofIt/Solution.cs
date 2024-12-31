namespace Solutions.Y2024.CSharp.Day10HoofIt
{
    public class Solution : ISolution
    {
        public class Node(int y, int x, int height)
        {
            public int Y { get; } = y;
            public int X { get; } = x;
            public int Height { get; } = height;
            public bool IsVisited { get; private set; }
            public List<Node> Connections { get; } = [];
            public HashSet<Node> TrailEnds { get; } = [];
            public HashSet<string> UniquePathsToEnd { get; } = [];

            public void Visit() => IsVisited = true;
            public bool TryAddConnection(Node other)
            {
                switch (other.Height - Height)
                {
                    case 1:
                        Connections.Add(other);
                        break;
                    case -1:
                        other.Connections.Add(this);
                        break;
                    default:
                        return false;
                }
                return true;
            }

            public override string ToString()
            {
                return $"Y: {Y}, X: {X}, Height: {Height}";
            }
        }

        private static (List<Node[]> map, List<Node> trailHeads) ParseMap(IEnumerable<string> lines)
        {
            var map = new List<Node[]>();
            var trailHeads = new List<Node>();

            foreach (var line in lines)
            {
                map.Add(new Node[line.Length]);
                for (var x = 0; x < line.Length; x++)
                {
                    var y = map.Count - 1;
                    var height = int.Parse(line[x].ToString());

                    var node = new Node(y, x, height);
                    map[y][x] = node;

                    if (y != 0) node.TryAddConnection(map[y - 1][x]);

                    if (x != 0) node.TryAddConnection(map[y][x - 1]);

                    if (height == 0) trailHeads.Add(node);
                }
            }

            return (map, trailHeads);
        }
        public string SolveFirst(IEnumerable<string> lines)
        {
            var (map, trailHeads) = ParseMap(lines);

            trailHeads.ForEach(Crawl);

            static void Crawl(Node node)
            {
                node.Visit();

                if (node.Height == 9)
                {
                    node.TrailEnds.Add(node);
                }

                foreach (var connection in node.Connections)
                {
                    if (!connection.IsVisited) Crawl(connection);

                    foreach (var trailEnd in connection.TrailEnds)
                    {
                        node.TrailEnds.Add(trailEnd);
                    }
                }
            }

            return trailHeads.Select(th => th.TrailEnds.Count).Sum().ToString();
        }

        public string SolveSecond(IEnumerable<string> lines)
        {
            var (map, trailHeads) = ParseMap(lines);

            trailHeads.ForEach(Crawl);

            static void Crawl(Node node)
            {
                node.Visit();

                if (node.Height == 9)
                {
                    node.TrailEnds.Add(node);
                    node.UniquePathsToEnd.Add($"{node}");
                }

                foreach (var connection in node.Connections)
                {
                    if (!connection.IsVisited) Crawl(connection);

                    foreach (var trailEnd in connection.TrailEnds)
                    {
                        node.TrailEnds.Add(trailEnd);
                        foreach (var path in connection.UniquePathsToEnd)
                        {
                            node.UniquePathsToEnd.Add($"{node} - {path}");
                        }
                    }
                }
            }

            return trailHeads.Select(th => th.UniquePathsToEnd.Count).Sum().ToString();
        }
    }
}
