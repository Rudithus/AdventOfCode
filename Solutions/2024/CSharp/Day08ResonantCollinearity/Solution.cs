namespace Solutions.Y2024.CSharp.Day08ResonantCollinearity
{
    public partial class Solution : ISolution
    {
        public readonly struct Node(int y, int x)
        {
            public int Y { get; } = y;
            public int X { get; } = x;
            public override string ToString()
            {
                return $"Y:{Y}, X:{X}";
            }

            public static Node operator +(Node first, Node second)
            {
                return new Node(first.Y + second.Y, first.X + second.X);
            }

            public static Node operator -(Node first, Node second)
            {
                return new Node(first.Y - second.Y, first.X - second.X);
            }
        }

        private static Dictionary<char, List<Node>> ParseInput(string[] lines)
        {
            var nodes = new Dictionary<char, List<Node>>();
            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '.') continue;

                    var node = new Node(y, x);

                    if (nodes.TryGetValue(line[x], out var nodeList))
                    {
                        nodeList.Add(node);
                    }
                    else
                    {
                        nodes[line[x]] = [node];
                    }
                }
            }
            return nodes;
        }

        public string SolveFirst(IEnumerable<string> lines)
        {
            var antinodes = new HashSet<Node>();

            var input = lines.ToArray();
            var nodes = ParseInput(lines.ToArray());
            var maxY = input.Length;
            var maxX = input[0].Length;

            foreach (var frequency in nodes.Keys)
            {
                var nodesOfFrequency = nodes[frequency];
                var crossJoinedNodes = nodesOfFrequency.SelectMany(
                    (node, index) => nodesOfFrequency.Skip(index + 1),
                    (node1, node2) => new
                    {
                        node1,
                        node2
                    });

                foreach (var antinode in crossJoinedNodes.SelectMany(pair => LocateAntinodes(pair.node1, pair.node2)))
                {
                    antinodes.Add(antinode);
                }
            }

            return antinodes.Count.ToString();

            IEnumerable<Node> LocateAntinodes(Node first, Node second)
            {
                var diff = second - first;

                var firstAntinode = first - diff;
                if (IsNodeWithinBounds(firstAntinode)) yield return firstAntinode;

                var secondAntinode = second + diff;
                if (IsNodeWithinBounds(secondAntinode)) yield return secondAntinode;
            }

            bool IsNodeWithinBounds(Node node) => node.X < maxX && node.X >= 0 && node.Y < maxY && node.Y >= 0;
        }

        public string SolveSecond(IEnumerable<string> lines)
        {
            var antinodes = new HashSet<Node>();

            var input = lines.ToArray();
            var nodes = ParseInput(lines.ToArray());
            var maxY = input.Length;
            var maxX = input[0].Length;

            foreach (var frequency in nodes.Keys)
            {
                var nodesOfFrequency = nodes[frequency];
                var crossJoinedNodes = nodesOfFrequency.SelectMany(
                    (node, index) => nodesOfFrequency.Skip(index + 1),
                    (node1, node2) => new
                    {
                        node1,
                        node2
                    });

                foreach (var antinode in crossJoinedNodes.SelectMany(pair => LocateAntinodes(pair.node1, pair.node2)))
                {
                    antinodes.Add(antinode);
                }

                nodesOfFrequency.ForEach(node => antinodes.Add(node));
            }

            return antinodes.Count.ToString();

            IEnumerable<Node> LocateAntinodes(Node first, Node second)
            {
                var diff = second - first;
                var antinode = first - diff;

                while (IsNodeWithinBounds(antinode))
                {
                    yield return antinode;
                    antinode = antinode - diff;
                }

                antinode = second + diff;
                while (IsNodeWithinBounds(antinode))
                {
                    yield return antinode;
                    antinode = antinode + diff;
                }
            }

            bool IsNodeWithinBounds(Node node) => node.X < maxX && node.X >= 0 && node.Y < maxY && node.Y >= 0;
        }
    }
}
