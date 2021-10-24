using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerfBenchmarks
{
    [Config(typeof(BenchConfig))]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(RuntimeMoniker.Net50)]
    [SimpleJob(RuntimeMoniker.Net60)]
    public class ParallelVsSequencial
    {
        private DataTable _foos;
        private List<FieldMapping> _mappings1;
        private List<FieldMapping> _mappings2;

        [Params(100, 1000)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _foos = BenchmarkUtils.GetSampleDataTable(Size);

            _mappings1 = new List<FieldMapping>
            {
                new FieldMapping() { From = "From1", To = "To1" },
                new FieldMapping() { From = "From2", To = "To2" },
                new FieldMapping() { From = "From3", To = "To3" },
                new FieldMapping() { From = "From4", To = "To4" },
                new FieldMapping() { From = "From5", To = "To5" },
                new FieldMapping() { From = "From6", To = "To6" },
                new FieldMapping() { From = "From7", To = "To7" },
                new FieldMapping() { From = "AnotherValue", To = "ToAnotherValue" },
                new FieldMapping() { From = "From8", To = "To8" },
                new FieldMapping() { From = "From9", To = "To9" },
                new FieldMapping() { From = "From10", To = "To10" },
            };

            _mappings2 = new List<FieldMapping>
            {
                new FieldMapping() { From = "From1", To = "To1" },
                new FieldMapping() { From = "From2", To = "To2" },
                new FieldMapping() { From = "From3", To = "To3" },
                new FieldMapping() { From = "From4", To = "To4" },
                new FieldMapping() { From = "From5", To = "To5" },
                new FieldMapping() { From = "From6", To = "To6" },
                new FieldMapping() { From = "StringValue", To = "ToStringValue" },
                new FieldMapping() { From = "From7", To = "To7" },
                new FieldMapping() { From = "From8", To = "To8" },
                new FieldMapping() { From = "From9", To = "To9" },
            };
        }

        [Benchmark(Baseline = true)]
        public ConcurrentBag<object> ParallelUsingConcurrentBag()
        {
            ConcurrentBag<object> result = new ConcurrentBag<object>();

            _foos
                .AsEnumerable()
                .AsParallel()
                .ForAll(row =>
                {
                    dynamic mappings = new ExpandoObject();
                    dynamic mappings1 = new ExpandoObject();
                    dynamic mappings2 = new ExpandoObject();

                    foreach (var mapping1 in _mappings1)
                    {
                        if (_foos.Columns.Contains(mapping1.From))
                        {
                            ((IDictionary<string, object>)mapping1).Add(mapping1.To, row[mapping1.From]);
                        }
                    }

                    foreach (var mapping2 in _mappings2)
                    {
                        if (_foos.Columns.Contains(mapping2.From))
                        {
                            ((IDictionary<string, Object>)mappings2).Add(mapping2.To, row[mapping2.From]);
                        }
                    }

                    mappings.Mappings1 = mappings1;
                    mappings.Mappings2 = mappings2;

                    result.Add(mappings);
                });

            return result;
        }

        [Benchmark]
        public List<dynamic> ParallelUsingSelectAndToList()
        {
            return _foos
                .AsEnumerable()
                .AsParallel()
                .Select(row =>
                {
                    dynamic mappings = new ExpandoObject();
                    dynamic mappings1 = new ExpandoObject();
                    dynamic mappings2 = new ExpandoObject();

                    foreach (var mapping1 in _mappings1)
                    {
                        if (_foos.Columns.Contains(mapping1.From))
                        {
                            ((IDictionary<string, object>)mapping1).Add(mapping1.To, row[mapping1.From]);
                        }
                    }

                    foreach (var mapping2 in _mappings2)
                    {
                        if (_foos.Columns.Contains(mapping2.From))
                        {
                            ((IDictionary<string, Object>)mappings2).Add(mapping2.To, row[mapping2.From]);
                        }
                    }

                    mappings.Mappings1 = mappings1;
                    mappings.Mappings2 = mappings2;

                    return mappings;
                }).ToList();
        }

        [Benchmark]
        public ConcurrentBag<object> SequencialUsingConcurrentBag()
        {
            ConcurrentBag<object> result = new ConcurrentBag<object>();

            foreach (var row in _foos.AsEnumerable())
            {
                dynamic mappings = new ExpandoObject();
                dynamic mappings1 = new ExpandoObject();
                dynamic mappings2 = new ExpandoObject();

                foreach (var mapping1 in _mappings1)
                {
                    if (_foos.Columns.Contains(mapping1.From))
                    {
                        ((IDictionary<string, object>)mapping1).Add(mapping1.To, row[mapping1.From]);
                    }
                }

                foreach (var mapping2 in _mappings2)
                {
                    if (_foos.Columns.Contains(mapping2.From))
                    {
                        ((IDictionary<string, Object>)mappings2).Add(mapping2.To, row[mapping2.From]);
                    }
                }

                mappings.Mappings1 = mappings1;
                mappings.Mappings2 = mappings2;

                result.Add(mappings);
            }

            return result;
        }

        [Benchmark]
        public List<object> SequencialUsingList()
        {
            List<object> result = new List<object>();

            foreach (var row in _foos.AsEnumerable())
            {
                dynamic mappings = new ExpandoObject();
                dynamic mappings1 = new ExpandoObject();
                dynamic mappings2 = new ExpandoObject();

                foreach (var mapping1 in _mappings1)
                {
                    if (_foos.Columns.Contains(mapping1.From))
                    {
                        ((IDictionary<string, object>)mapping1).Add(mapping1.To, row[mapping1.From]);
                    }
                }

                foreach (var mapping2 in _mappings2)
                {
                    if (_foos.Columns.Contains(mapping2.From))
                    {
                        ((IDictionary<string, Object>)mappings2).Add(mapping2.To, row[mapping2.From]);
                    }
                }

                mappings.Mappings1 = mappings1;
                mappings.Mappings2 = mappings2;

                result.Add(mappings);
            }

            return result;
        }

        [Benchmark]
        public List<dynamic> SequencialUsingSelectAndToList()
        {

            return _foos.AsEnumerable()
                 .Select(row =>
                 {
                     dynamic mappings = new ExpandoObject();
                     dynamic mappings1 = new ExpandoObject();
                     dynamic mappings2 = new ExpandoObject();

                     foreach (var mapping1 in _mappings1)
                     {
                         if (_foos.Columns.Contains(mapping1.From))
                         {
                             ((IDictionary<string, object>)mapping1).Add(mapping1.To, row[mapping1.From]);
                         }
                     }

                     foreach (var mapping2 in _mappings2)
                     {
                         if (_foos.Columns.Contains(mapping2.From))
                         {
                             ((IDictionary<string, Object>)mappings2).Add(mapping2.To, row[mapping2.From]);
                         }
                     }

                     mappings.Mappings1 = mappings1;
                     mappings.Mappings2 = mappings2;

                     return mappings;
                 }).ToList();
        }
    }

    public class FieldMapping
    {
        public string From { get; set; }
        public string To { get; set; }
    }
}