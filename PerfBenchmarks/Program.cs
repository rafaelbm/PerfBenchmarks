using System;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using MemoryDiagnoser = BenchmarkDotNet.Diagnosers.MemoryDiagnoser;

namespace PerfBenchmarks
{
    class BenchConfig : ManualConfig
    {
        public BenchConfig()
        {
            Add(MemoryDiagnoser.Default);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const string benchmarkSuffix = "Tests";

            var benchmarks = Assembly.GetEntryAssembly()
                .DefinedTypes.Where(t => t.Name.EndsWith(benchmarkSuffix))
                .ToDictionary(t => t.Name.Substring(0, t.Name.Length - benchmarkSuffix.Length), t => t, StringComparer.OrdinalIgnoreCase);

            if (args.Length > 0 && args[0].Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Running full benchmarks suite");
                benchmarks.Select(pair => pair.Value).ToList().ForEach(action => BenchmarkRunner.Run(action));
                return;
            }

            if (args.Length == 0 || !benchmarks.ContainsKey(args[0]))
            {
                Console.WriteLine("Please, select benchmark, list of available:");
                benchmarks
                    .Select(pair => pair.Key)
                    .ToList()
                    .ForEach(Console.WriteLine);

                Console.WriteLine("All");
                return;
            }

            BenchmarkRunner.Run(benchmarks[args[0]]);

            Console.Read();
        }
    }
}
