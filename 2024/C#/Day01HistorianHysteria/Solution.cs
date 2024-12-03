using Solutions;

namespace Day01HistorianHysteria
{
    public class Solution : BaseSolution
    {
        protected override string ExampleFileName => "Day01_Example.txt";

        protected override string InputFileName => "Day01_Input.txt";

        public override int SolveFirst(IEnumerable<string> lines)
        {
            var leftNumbers = new List<int>();
            var rightNumbers = new List<int>();

            foreach (var item in lines)
            {
                var numbers = item.Split("   ").Select(s => Convert.ToInt32(s)).ToArray();
                var left = numbers.First();
                var right = numbers.Last();

                leftNumbers.Add(left);
                rightNumbers.Add(right);
            }
            leftNumbers.Sort();
            rightNumbers.Sort();

            var totalDistance = leftNumbers.Zip(rightNumbers).Select(pair => Math.Abs(pair.First - pair.Second)).Sum();

            return totalDistance;
        }


        public override int SolveSecond(IEnumerable<string> lines)
        {
            var leftNumbers = new List<int>();
            var rightNumbers = new Dictionary<int, int>();

            foreach (var item in lines)
            {
                var numbers = item.Split("   ").Select(s => Convert.ToInt32(s)).ToArray();
                var left = numbers.First();
                var right = numbers.Last();

                leftNumbers.Add(left);

                if (!rightNumbers.TryAdd(right, 1))
                {
                    rightNumbers[right]++;
                }
            }

            var accu = 0;
            foreach (var number in leftNumbers)
            {
                rightNumbers.TryGetValue(number, out var multiplier);
                accu += multiplier * number;
            }

            return accu;
        }
    }
}
