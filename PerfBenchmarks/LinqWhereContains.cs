using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace PerfBenchmarks
{
    [Config(typeof(BenchConfig))]
    public class LinqWhereContainsTests
    {
        private List<string> _items;

        [Params(1, 10, 100)]
        public int NumberOfTimesToExecute;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _items = Enumerable.Range(0, 50000)
                .Select(i => "Index" + i)
                .ToList();
        }


        [Benchmark(Baseline = true)]
        public void WhereContainsCreatingObjectBeforeLoop()
        {
            var arrayBeforeLoop = new[] { "aff", "Index700" };
            for (int j = 0; j < NumberOfTimesToExecute; j++)
            {
                var _ = _items.Where(x => arrayBeforeLoop.Contains(x)).ToList();
            }
        }

        public static readonly string[] ConstantArrayToUseContains = new[]
        {
            "aff",
            "Index700"
        };

        [Benchmark]
        public void WhereContainsCreatingObjectOnClass()
        {
            for (int i = 0; i < NumberOfTimesToExecute; i++)
            {
                var _ = _items.Where(x => ConstantArrayToUseContains.Contains(x)).ToList();
            }
        }

        [Benchmark]
        public void WhereContainsCreatingObjectInsideLoop()
        {

            for (int i = 0; i < NumberOfTimesToExecute; i++)
            {
                var arrayInsideLoop = new[] { "aff", "Index700" };
                var _ = _items.Where(x => arrayInsideLoop.Contains(x)).ToList();
            }
        }

        [Benchmark]
        public void WhereContainsCreateObjectInsideWhere()
        {
            for (int i = 0; i < NumberOfTimesToExecute; i++)
            {
                var _ = _items.Where(x => new[] { "aff", "Index700" }.Contains(x)).ToList();
            }
        }
    }
}
