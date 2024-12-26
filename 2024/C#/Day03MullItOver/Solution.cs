using Solutions;
using System.Text.RegularExpressions;

namespace Day03MullItOver
{
    public partial class Solution : BaseSolution
    {
        protected override string ExampleFileName => "Day03_Example.txt";

        protected override string InputFileName => "Day03_Input.txt";

        public override string SolveFirst(IEnumerable<string> lines)
        {
            return lines.SelectMany(line => Regex
                        .Matches(line, "mul\\(?(\\d{0,3},\\d{0,3})\\)"))
                .Select(match => match.Groups[1].Value.Split(',')
                .Select(int.Parse)
                .Aggregate(Multiply))
                .Sum().ToString();
        }


        public override string SolveSecond(IEnumerable<string> lines)
        {
            var matches = lines.SelectMany(line => Regex
                        .Matches(line, "(?'do'do\\(\\))|mul\\(?(?'mul'\\d{0,3},\\d{0,3})\\)|(?'skip'don't\\(\\))"));
            var sum = 0;
            var take = true;
            foreach (var match in matches)
            {
                if (match.Groups["skip"].Success) take = false;

                if (take && match.Groups["mul"].Success)
                {
                    sum += match.Groups["mul"].Value.Split(',').Select(int.Parse).Aggregate(Multiply);
                }

                if (match.Groups["do"].Success) take = true;
            }

            return sum.ToString();
        }

        private static int Multiply(int a, int b) => a * b;
    }
}
