using Solutions;
using System.Globalization;
using System.Linq;

namespace Day07BridgeRepair
{
    public partial class Solution : BaseSolution
    {
        protected override string ExampleFileName => "Day07_Example.txt";

        protected override string InputFileName => "Day07_Input.txt";

        private readonly Func<long, long, long> Add = (a, b) => a + b;
        private readonly Func<long, long, long> Multiply = (a, b) => a * b;
        private readonly Func<long, long, long> Concat = (a, b) => long.Parse(a.ToString(CultureInfo.InvariantCulture) + b.ToString(CultureInfo.InvariantCulture));

        public override string SolveFirst(IEnumerable<string> lines)
        {
            return lines.Select(ParseLine).Where(IsCalibrated).Sum(calibration => calibration.result).ToString();

            (long result, int[] numbers) ParseLine(string line)
            {
                return line.Split(':') switch
                {
                [var v, var i] => (long.Parse(v), i.Trim().Split(' ').Select(int.Parse).ToArray()),
                    _ => throw new NotImplementedException()
                };
            }

            bool IsCalibrated((long result, int[] numbers) equation)
            {
                var (result, numbers) = equation;
                var results = new HashSet<long>() { numbers.First() };

                foreach (var number in numbers.Skip(1))
                {
                    results = results.Select(result => new long[] {
                        Add(result, number),
                        Multiply(result, number)
                    }).SelectMany(a => a).ToHashSet();
                }

                return results.Contains(result);
            }
        }

        public override string SolveSecond(IEnumerable<string> lines)
        {
            return lines.Select(ParseLine).Where(IsCalibrated).Sum(calibration => calibration.result).ToString();

            (long result, int[] numbers) ParseLine(string line)
            {
                return line.Split(':') switch
                {
                [var v, var i] => (long.Parse(v), i.Trim().Split(' ').Select(int.Parse).ToArray()),
                    _ => throw new NotImplementedException()
                };
            }

            bool IsCalibrated((long result, int[] numbers) equation)
            {
                var (result, numbers) = equation;
                var results = new HashSet<long>() { numbers.First() };

                foreach (var number in numbers.Skip(1))
                {
                    results = results.Select(result => new long[] {
                        Add(result, number),
                        Multiply(result, number),
                        Concat(result, number)
                    }).SelectMany(a => a).ToHashSet();
                }

                return results.Contains(result);
            }
        }
    }
}
