using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

namespace PerfBenchmarks
{
    class BenchConfig : ManualConfig
    {
        public BenchConfig()
        {
            AddDiagnoser(MemoryDiagnoser.Default);
        }
    }

    class Program
    {
        static void Main(string[] args) =>
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
