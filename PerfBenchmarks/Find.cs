using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace PerfBenchmarks
{
    [Config(typeof(BenchConfig))]
    public class FindTests
    {
        private readonly List<string> _items = Enumerable.Range(0, 50000).Select(i => "Index" + i).ToList();

        [Benchmark]
        public void Find()
        {
            var i = _items.Find(s => s == "Index700");
        }

        [Benchmark]
        public void FirstOrDefault()
        {
            var i = _items.FirstOrDefault(s => s == "Index700");
        }

        [Benchmark]
        public void WhereThenFirstOrDefault()
        {
            var i = _items.Where(s => s == "Index700").FirstOrDefault();
        }
    }
}
