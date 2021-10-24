using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace PerfBenchmarks
{
    [Config(typeof(BenchConfig))]
    public class LinqSelectManyCountVsLinqSelectManyAny
    {
        private List<Foo> _foos;

        [Params(100_000)]
        public int OuterSize { get; set; }
        [Params(0)]
        public int InnerSize { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _foos = new List<Foo>();

            for (int i = 0; i < OuterSize; i++)
            {
                var foo = new Foo
                {
                    Id = i,
                    Name = $"Name_{i}"
                };

                for (int j = 0; j < InnerSize; j++)
                {
                    foo.Bars.Add(new Bar { Id = j, Something = $"Something_{j}" });
                }

                _foos.Add(foo);
            }
        }

        [Benchmark(Baseline = true)]
        public bool UsingAny()
        {
            return !_foos.SelectMany(s => s.Bars).Any();
        }

        [Benchmark]
        public bool UsingCount()
        {
            return _foos.SelectMany(s => s.Bars).Count() == 0;
        }

        [Benchmark]
        public bool UsingCountAndAny()
        {
            return _foos.Count(p => !p.Bars.Any()) == 0;
        }

        [Benchmark]
        public bool DoubleAny()
        {
            return !_foos.Any(s => s.Bars.Any());
        }
    }

    public class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Bar> Bars { get; set; } = new List<Bar>();
    }

    public class Bar
    {
        public int Id { get; set; }
        public string Something { get; set; }
    }
}