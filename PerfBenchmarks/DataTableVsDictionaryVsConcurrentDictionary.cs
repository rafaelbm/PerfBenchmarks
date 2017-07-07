using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace PerfBenchmarks
{
    [Config(typeof(BenchConfig))]
    public class DataTableVsDictionaryVsConcurrentDictionaryTests
    {
        const int Items = 10000;
        //const int Lookup = 7777;

        [Params(5, 10, 100)]
        public int ItemsParams;

        //const int Items = 100000;
        //const int Lookup = 95432;

        private DataTable _cacheTable = new DataTable();
        private Dictionary<int, SimpleClass> _cacheDictionary = new Dictionary<int, SimpleClass>();
        private ConcurrentDictionary<int, SimpleClass> _cacheConcurrentDictionary = new ConcurrentDictionary<int, SimpleClass>();

        [GlobalSetup]
        public void Setup()
        {
            _cacheTable = BenchmarkUtils.GetSampleDataTable(Items);
            _cacheDictionary = BenchmarkUtils.GetSampleDictionary(Items);
            _cacheConcurrentDictionary = BenchmarkUtils.GetSampleConcurrentDictionary(Items);
        }

        [Benchmark(Baseline = true)]
        public void DataTableSelect()
        {
            for (int i = 0; i < ItemsParams; i++)
            {
                var item = _cacheTable.Select("ID = " + i);
            }
        }

        [Benchmark()]
        public void DataTableFirstOrDefault()
        {
            for (int i = 0; i < ItemsParams; i++)
            {
                var item = _cacheTable.AsEnumerable().FirstOrDefault(x => Convert.ToInt32(x["ID"]) == i);
            }
        }

        [Benchmark()]
        public void DataTableFirstOrDefaultUsingField()
        {
            for (int i = 0; i < ItemsParams; i++)
            {
                var item = _cacheTable.AsEnumerable().FirstOrDefault(x => x.Field<int>("ID") == i);
            }
        }

        [Benchmark()]
        public void DataTableWhere()
        {
            for (int i = 0; i < ItemsParams; i++)
            {
                var item = _cacheTable.AsEnumerable().Where(x => x.Field<int>("ID") == i).FirstOrDefault();
            }
        }

        [Benchmark()]
        public void DictionaryIndex()
        {
            for (int i = 0; i < ItemsParams; i++)
            {
                var item = _cacheDictionary[i];
            }
        }

        [Benchmark()]
        public void DictionaryTryGetValue()
        {
            for (int i = 0; i < ItemsParams; i++)
            {
                SimpleClass cacheItem;
                var item = _cacheDictionary.TryGetValue(i, out cacheItem);
            }
        }

        [Benchmark()]
        public void ConcurrentDictionaryIndex()
        {
            for (int i = 0; i < ItemsParams; i++)
            {
                var item = _cacheConcurrentDictionary[i];
            }
        }

        [Benchmark()]
        public void ConcurrentDictionaryTryGetValue()
        {
            for (int i = 0; i < ItemsParams; i++)
            {
                SimpleClass cacheItem;
                var item = _cacheConcurrentDictionary.TryGetValue(i, out cacheItem);
            }
        }
    }
}
