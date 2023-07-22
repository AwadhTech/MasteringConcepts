using System.Text.RegularExpressions;

namespace DataProcessing.Benchmarks;

public class CompiledRegexBenchmarks
{
    private const string Row = "[AA] [54a96b1b-77e3-43d4-8a9b-4ad969690217] [Italien] [Some Company J]";

    private static readonly Regex? CompiledRegex = new (@"\[(?<data>.*?)\]",
        RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    [Benchmark]
    public void NonCompiled() => Regex.Matches(Row, @"\[(?<data>.*?)\]", 
        RegexOptions.None, TimeSpan.FromSeconds(1));

    [Benchmark]
    public void Compiled() => CompiledRegex!.Matches(Row);
}
