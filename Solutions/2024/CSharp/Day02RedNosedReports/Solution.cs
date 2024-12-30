namespace Solutions.Y2024.CSharp.Day02RedNosedReports
{
    public class Solution : ISolution
    {
        public string SolveFirst(IEnumerable<string> lines)
        {
            var safetyCount = 0;

            foreach (var line in lines)
            {
                var levels = line.Split(' ').Select(int.Parse);
                var report = new Report(levels.First());
                var success = true;

                foreach (var level in levels.Skip(1))
                {
                    if (!report.TryAdd(level))
                    {
                        success = false;
                        break;
                    }
                }

                if (success) safetyCount++;
            }

            return safetyCount.ToString();
        }


        public string SolveSecond(IEnumerable<string> lines)
        {
            var safetyCount = 0;

            foreach (var line in lines)
            {
                var levels = line.Split(' ').Select(int.Parse).ToArray();
                var report = new Report(levels.First());

                if (Test(levels) || Test(levels.Reverse().ToArray())) safetyCount++;
            }

            return safetyCount.ToString();
        }

        private bool Test(int[] levels)
        {
            var report = new Report(levels.First());
            var success = true;
            var firstFail = true;

            for (int i = 1; i < levels.Length; i++)
            {
                var level = levels[i];
                if (!report.TryAdd(level))
                {
                    if (firstFail)
                    {
                        firstFail = false;

                        if (i + 1 == levels.Length)
                        {
                            continue;
                        }

                        var nextLevel = levels[i + 1];

                        if (report.TryAdd(nextLevel))
                        {
                            i++;
                            continue;
                        }

                        if (report.Count == 1)
                        {
                            report = new Report(level);
                            if (report.TryAdd(nextLevel))
                            {
                                i++;
                                continue;
                            }
                        }
                    }
                    success = false;
                    break;
                }
            }
            return success;
        }

        public class Report
        {
            private readonly Stack<int> _levels = new();
            private int _direction = 0;
            public Report(int firstLevel)
            {
                _levels.Push(firstLevel);
            }

            public bool TryAdd(int next)
            {
                var last = _levels.Peek();
                if (_levels.Count == 1)
                {
                    _direction = next.CompareTo(last);
                    if (_direction == 0) return false;
                }

                if (next.CompareTo(last) == _direction && Math.Abs(last - next) < 4)
                {
                    _levels.Push(next);
                    return true;
                }

                return false;
            }
            public int Count => _levels.Count;

            public int RemoveLast() => _levels.Pop();

        }
    }
}
