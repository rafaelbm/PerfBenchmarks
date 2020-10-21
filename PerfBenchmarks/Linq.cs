//using BenchmarkDotNet.Attributes;
//using PerfBenchmarks;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace PerfBenchmarksNetCore
//{
//    [Config(typeof(BenchConfig))]
//    public class LinqTests
//    {
//        private List<Something> _items;

//        [GlobalSetup]
//        public void GlobalSetup()
//        {
//            _items = Enumerable.Range(0, 10_000_000)
//                               .Select(index => new Something { Id = index })
//                               .ToList();
//            _items.Last().InternalId = -1;
//        }

//        [Benchmark]
//        public void Find()
//        {
//            var i = _items.Find(s => s == "Index700");
//        }

//        [Benchmark]
//        public void FirstOrDefault()
//        {
//            var i = _items.FirstOrDefault(s => s == "Index700");
//        }

//        [Benchmark]
//        public void WhereThenFirstOrDefault()
//        {
//            var i = _items.Where(s => s == "Index700").FirstOrDefault();
//        }
//    }

//    public class Something
//    {
//        public int Id { get; set; }
//        public int InternalId { get; set; }
//    }
//}
