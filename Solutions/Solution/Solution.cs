namespace Solutions
{
    public interface ISolution
    {
        string SolveFirst(IEnumerable<string> lines);
        string SolveSecond(IEnumerable<string> lines);
    }
}