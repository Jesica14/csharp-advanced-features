using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<StringBenchmark>();
    }
}

[MemoryDiagnoser]
public class StringBenchmark
{
    private const int Iterations = 10_000;

    [Benchmark(Baseline = true)]
    public string StringConcatenation()
    {
        string result = "";
        for (int i = 0; i < Iterations; i++)
            result += $"Order #{i} | Customer_{i} | {i * 10m:C}\n";
        return result;
    }

    [Benchmark]
    public string StringBuilderBasic()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Iterations; i++)
            sb.AppendLine($"Order #{i} | Customer_{i} | {i * 10m:C}");
        return sb.ToString();
    }

    [Benchmark]
    public string StringBuilderWithCapacity()
    {
        var sb = new StringBuilder(capacity: Iterations * 60);
        for (int i = 0; i < Iterations; i++)
            sb.AppendLine($"Order #{i} | Customer_{i} | {i * 10m:C}");
        return sb.ToString();
    }
}