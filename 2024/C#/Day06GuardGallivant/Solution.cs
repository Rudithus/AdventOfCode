using Solutions;
using System.Xml.Linq;

namespace Day06GuardGallivant
{
    public partial class Solution : BaseSolution
    {
        protected override string ExampleFileName => "Day06_Example.txt";

        protected override string InputFileName => "Day06_Input.txt";

        private static (bool[][] map, Guard guard) GenerateMapAndStartingPoint(IEnumerable<string> lines)
        {
            var map = new List<bool[]>();
            var startingPoint = new Position(0, 0);
            foreach (var line in lines)
            {
                var horizontal = new bool[line.Length];

                for (int i = 0; i < line.Length; i++)
                {
                    char c = line[i];
                    switch (c)
                    {
                        case '.':
                            horizontal[i] = false;
                            break;
                        case '#':
                            horizontal[i] = true;
                            break;
                        case '^':
                            horizontal[i] = false;
                            startingPoint = new Position(i, map.Count);
                            break;
                        default:
                            break;
                    }
                }
                map.Add(horizontal);
            }

            return (map.ToArray(), new Guard(startingPoint, [.. map], Direction.Up));
        }

        public class Direction
        {
            public static readonly Direction Right = new(1, 0, nameof(Right));
            public static readonly Direction Up = new(0, -1, nameof(Up));
            public static readonly Direction Down = new(0, 1, nameof(Down));
            public static readonly Direction Left = new(-1, 0, nameof(Left));

            public readonly int X;
            public readonly int Y;
            private readonly string _name;

            public Direction GetRight()
            {
                if (this == Right) return Down;
                if (this == Down) return Left;
                if (this == Left) return Up;
                if (this == Up) return Right;
                throw new ArgumentOutOfRangeException();
            }

            private Direction(int x, int y, string name)
            {
                X = x;
                Y = y;
                _name = name;
            }

            public override string ToString()
            {
                return _name;
            }
        }

        public readonly struct Position(int X, int Y)
        {
            public int X { get; } = X;
            public int Y { get; } = Y;

            internal void Deconstruct(out int x, out int y)
            {
                x = X;
                y = Y;
            }

            public override string ToString()
            {
                return $"{X},{Y}";
            }
        }

        public class Guard(Position position, bool[][] map, Direction direction)
        {
            public Position Position = position;
            private readonly bool[][] _map = map;
            public Direction Direction = direction;
            public bool IsOutOfBounds { get; private set; }

            public bool TryMove()
            {
                var (x, y) = Position;
                var wantToMove = new Position(x + Direction.X, y + Direction.Y);
                IsOutOfBounds = CheckOutOfBounds(wantToMove);

                if (!IsOutOfBounds)
                {
                    var isOccupied = _map[wantToMove.Y][wantToMove.X];
                    if (isOccupied)
                    {
                        return false;
                    }
                }

                Position = wantToMove;
                return true;
            }

            public bool CheckMove(out Position wantToMove)
            {
                var (x, y) = Position;
                wantToMove = new Position(x + Direction.X, y + Direction.Y);

                return !CheckOutOfBounds(wantToMove) && !_map[wantToMove.Y][wantToMove.X];
            }

            public void TurnRight()
            {
                Direction = Direction.GetRight();
            }

            private bool CheckOutOfBounds(Position position)
            {
                return position.X < 0 ||
                    position.X >= _map[0].Length ||
                    position.Y < 0 ||
                    position.Y >= _map.Length;
            }
        }

        public override int SolveFirst(IEnumerable<string> lines)
        {
            var (_, guard) = GenerateMapAndStartingPoint(lines);
            var guardPositions = new HashSet<Position>();

            while (!guard.IsOutOfBounds)
            {
                guardPositions.Add(guard.Position);

                if (!guard.TryMove()) guard.TurnRight();
            }

            return guardPositions.Count;

        }


        public override int SolveSecond(IEnumerable<string> lines)
        {
            var (map, guard) = GenerateMapAndStartingPoint(lines);
            var guardPath = new HashSet<Position>();
            var newObstacles = new HashSet<Position>();

            while (!guard.IsOutOfBounds)
            {
                if (guard.CheckMove(out var nextPosition)
                    && !guardPath.Contains(nextPosition)
                    && !newObstacles.Contains(nextPosition))
                {
                    map[nextPosition.Y][nextPosition.X] = true;
                    var newGuard = new Guard(guard.Position, map, guard.Direction.GetRight());

                    if (TestForLoop(newGuard))
                    {
                        newObstacles.Add(nextPosition);
                    }

                    map[nextPosition.Y][nextPosition.X] = false;
                }

                guardPath.Add(guard.Position);
                if (!guard.TryMove()) guard.TurnRight();
            }

            return newObstacles.Count;
        }

        private static bool TestForLoop(Guard guard)
        {
            var positions = new HashSet<(Direction, Position)>();

            while (!guard.IsOutOfBounds)
            {
                if (!positions.Add((guard.Direction, guard.Position)))
                {
                    return true;
                }
                if (!guard.TryMove()) guard.TurnRight();
            }

            return false;
        }
    }
}
