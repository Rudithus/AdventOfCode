using System.Reflection;

namespace Solutions
{
    public interface ISolution
    {
        string SolveFirst(IEnumerable<string> lines);
        string SolveSecond(IEnumerable<string> lines);
        string GetExampleFilePath();
        string GetInputFilePath();
    }

    public abstract class BaseSolution : ISolution
    {
        protected abstract string ExampleFileName { get; }
        protected abstract string InputFileName { get; }

        public string GetExampleFilePath()
        {
            var directory = GetSolutionDirectory();
            return Path.Combine(directory, ExampleFileName);
        }

        public string GetInputFilePath()
        {
            string directory = GetSolutionDirectory();
            return Path.Combine(directory, InputFileName);
        }

        private string GetSolutionDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetAssembly(GetType())?.Location) ?? throw new Exception();
        }

        public abstract string SolveFirst(IEnumerable<string> lines);
        public abstract string SolveSecond(IEnumerable<string> lines);
    }
}