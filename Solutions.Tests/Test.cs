﻿using Xunit.Abstractions;
using FluentAssertions;

namespace Solutions.Tests
{
    public class Test(ITestOutputHelper testOutputHelper)
    {
        private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

        [Theory]
        [InlineData(typeof(Day08ResonantCollinearity.Solution), 14)]
        [InlineData(typeof(Day07BridgeRepair.Solution), 3749)]
        [InlineData(typeof(Day06GuardGallivant.Solution), 41)]
        [InlineData(typeof(Day05PrintQueue.Solution), 143)]
        [InlineData(typeof(Day04CeresSearch.Solution), 18)]
        [InlineData(typeof(Day03MullItOver.Solution), 161)]
        [InlineData(typeof(Day02RedNosedReports.Solution), 2)]
        [InlineData(typeof(Day01HistorianHysteria.Solution), 11)]
        public void SolveFirst(Type sol, int firstExampleAnswer)
        {
            var solution = Activator.CreateInstance(sol) as ISolution
                ?? throw new ArgumentNullException(nameof(sol));

            var exampleFile = File.ReadLines(solution.GetExampleFilePath());
            var inputFile = File.ReadLines(solution.GetInputFilePath());

            var firstAnswer = solution.SolveFirst(exampleFile);
            firstAnswer.Should().Be(firstExampleAnswer.ToString());
            firstAnswer = solution.SolveFirst(inputFile);

            _testOutputHelper.WriteLine(firstAnswer.ToString());
        }

        [Theory]
        [InlineData(typeof(Day08ResonantCollinearity.Solution), 34)]
        [InlineData(typeof(Day07BridgeRepair.Solution), 11387)]
        [InlineData(typeof(Day06GuardGallivant.Solution), 6)]
        [InlineData(typeof(Day05PrintQueue.Solution), 123)]
        [InlineData(typeof(Day04CeresSearch.Solution), 9)]
        [InlineData(typeof(Day03MullItOver.Solution), 48)]
        [InlineData(typeof(Day02RedNosedReports.Solution), 8)]
        [InlineData(typeof(Day01HistorianHysteria.Solution), 31)]
        public void SolveSecond(Type sol, int secondExampleAnswer)
        {
            var solution = Activator.CreateInstance(sol) as ISolution
                ?? throw new ArgumentNullException(nameof(sol));

            var exampleFile = File.ReadLines(solution.GetExampleFilePath());
            var inputFile = File.ReadLines(solution.GetInputFilePath());

            var secondAnswer = solution.SolveSecond(exampleFile);
            secondAnswer.Should().Be(secondExampleAnswer.ToString());
            secondAnswer = solution.SolveSecond(inputFile);

            _testOutputHelper.WriteLine(secondAnswer.ToString());
        }
    }
}
