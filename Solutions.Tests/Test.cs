using Xunit.Abstractions;
using FluentAssertions;

namespace Solutions.Tests
{
    public class Test(ITestOutputHelper testOutputHelper)
    {
        private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

        private (string example, string input) _getFilePath(string fullNamespace)
        {
            var basePath = fullNamespace.Split('.') switch
            {
            [_, var year, _, var day, _] => $"Inputs/{year.Substring(1)}/{day}/",
                _ => throw new NotImplementedException()
            };
            return (Path.Combine(basePath, "Example.txt"), Path.Combine(basePath, "Input.txt"));

        }

        [Theory]
        [InlineData(typeof(Y2024.CSharp.Day10HoofIt.Solution), 36)]
        [InlineData(typeof(Y2024.CSharp.Day09DiskFragmenter.Solution), 1928)]
        [InlineData(typeof(Y2024.CSharp.Day08ResonantCollinearity.Solution), 14)]
        [InlineData(typeof(Y2024.CSharp.Day07BridgeRepair.Solution), 3749)]
        [InlineData(typeof(Y2024.CSharp.Day06GuardGallivant.Solution), 41)]
        [InlineData(typeof(Y2024.CSharp.Day05PrintQueue.Solution), 143)]
        [InlineData(typeof(Y2024.CSharp.Day04CeresSearch.Solution), 18)]
        [InlineData(typeof(Y2024.CSharp.Day03MullItOver.Solution), 161)]
        [InlineData(typeof(Y2024.CSharp.Day02RedNosedReports.Solution), 2)]
        [InlineData(typeof(Y2024.CSharp.Day01HistorianHysteria.Solution), 11)]
        public void SolveFirst(Type sol, int firstExampleAnswer)
        {
            var solution = Activator.CreateInstance(sol) as ISolution
                ?? throw new ArgumentNullException(nameof(sol));
            var filePaths = _getFilePath(sol.FullName);

            var exampleFile = File.ReadLines(filePaths.example);
            var inputFile = File.ReadLines(filePaths.input);

            var firstAnswer = solution.SolveFirst(exampleFile);
            firstAnswer.Should().Be(firstExampleAnswer.ToString());
            firstAnswer = solution.SolveFirst(inputFile);

            _testOutputHelper.WriteLine(firstAnswer.ToString());
        }

        [Theory]
        [InlineData(typeof(Y2024.CSharp.Day10HoofIt.Solution), 81)]
        [InlineData(typeof(Y2024.CSharp.Day09DiskFragmenter.Solution), 2858)]
        [InlineData(typeof(Y2024.CSharp.Day08ResonantCollinearity.Solution), 34)]
        [InlineData(typeof(Y2024.CSharp.Day07BridgeRepair.Solution), 11387)]
        [InlineData(typeof(Y2024.CSharp.Day06GuardGallivant.Solution), 6)]
        [InlineData(typeof(Y2024.CSharp.Day05PrintQueue.Solution), 123)]
        [InlineData(typeof(Y2024.CSharp.Day04CeresSearch.Solution), 9)]
        [InlineData(typeof(Y2024.CSharp.Day03MullItOver.Solution), 48)]
        [InlineData(typeof(Y2024.CSharp.Day02RedNosedReports.Solution), 8)]
        [InlineData(typeof(Y2024.CSharp.Day01HistorianHysteria.Solution), 31)]
        public void SolveSecond(Type sol, int secondExampleAnswer)
        {
            var solution = Activator.CreateInstance(sol) as ISolution
                ?? throw new ArgumentNullException(nameof(sol));
            var filePaths = _getFilePath(sol.FullName);

            var exampleFile = File.ReadLines(filePaths.example);
            var inputFile = File.ReadLines(filePaths.input);

            var secondAnswer = solution.SolveSecond(exampleFile);
            secondAnswer.Should().Be(secondExampleAnswer.ToString());
            secondAnswer = solution.SolveSecond(inputFile);

            _testOutputHelper.WriteLine(secondAnswer.ToString());
        }
    }
}
