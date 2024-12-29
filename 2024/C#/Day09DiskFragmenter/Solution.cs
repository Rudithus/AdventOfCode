using Solutions;
using static Solutions.Util;

namespace Day09DiskFragmenter
{
    public partial class Solution : BaseSolution
    {
        protected override string ExampleFileName => "Day09_Example.txt";

        protected override string InputFileName => "Day09_Input.txt";

        public override string SolveFirst(IEnumerable<string> lines)
        {
            long totalSum = 0;
            var line = lines.First();
            var queue = new Queue<long>();
            var i = 0;
            var j = line.Length - 1;
            var actualIndex = 0;

            while (i <= j)
            {
                var isFreeSpace = i % 2 != 0;
                var currDigit = int.Parse(line[i].ToString());

                if (isFreeSpace)
                {
                    while (queue.Count <= currDigit)
                    {
                        var tail = int.Parse(line[j].ToString());
                        long id = j / 2;
                        Repeat(tail, () => queue.Enqueue(id));
                        j -= 2;
                    }

                    totalSum += Enumerable
                        .Range(actualIndex, currDigit)
                        .Select(mul => mul * queue.Dequeue())
                        .Sum();
                }
                else
                {
                    long id = i / 2;
                    totalSum += Enumerable
                        .Range(actualIndex, currDigit)
                        .Select(mul => mul * id)
                        .Sum();
                }

                actualIndex += currDigit;
                i++;
            }

            while (queue.TryDequeue(out var currDigit))
            {
                totalSum += (actualIndex * currDigit);
                actualIndex++;
            }

            return totalSum.ToString();
        }

        public override string SolveSecond(IEnumerable<string> lines)
        {
            long totalSum = 0;
            var line = lines.First();
            var files = new Stack<(int size, int start, long id)>();
            var spaceList = new List<(int size, int start, int end)>();

            var disk = line.Select((ch, index) => new
            {
                size = int.Parse(ch.ToString()),
                id = index / 2,
                index,
                isFile = index % 2 == 0
            });

            var actualIndex = 0;
            foreach (var entry in disk)
            {
                if (entry.size == 0) continue;
                if (entry.isFile)
                {
                    files.Push((entry.size, actualIndex, entry.id));
                }
                else
                {
                    spaceList.Add((entry.size, actualIndex, actualIndex + entry.size));
                }

                actualIndex += entry.size;
            }

            spaceList.Reverse();
            var spaces = new Stack<(int size, int start, int end)>(spaceList);

            while (files.TryPop(out var file))
            {
                var dequeuedSpaces = new Stack<(int size, int start, int end)>();
                var fileMoved = false;
                while (!fileMoved && spaces.TryPop(out var space))
                {
                    if (file.start < space.start) break;

                    if (file.size <= space.size)
                    {
                        space.size -= file.size;

                        totalSum += Enumerable
                            .Range(space.start, file.size)
                            .Select(mul => mul * file.id)
                            .Sum();

                        space.start = space.end - space.size;
                        fileMoved = true;
                    }

                    if (space.size > 0) dequeuedSpaces.Push(space);
                }

                if (!fileMoved)
                {
                    totalSum += Enumerable
                            .Range(file.start, file.size)
                            .Select(mul => mul * file.id)
                            .Sum();
                }

                while (dequeuedSpaces.TryPop(out var space))
                {
                    spaces.Push(space);
                }
            }

            return totalSum.ToString();
        }
    }
}
