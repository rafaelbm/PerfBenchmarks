using System.Data;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace PerfBenchmarks
{
    [Config(typeof(BenchConfig))]
    public class LinqJoinVsNestedForTests
    {
        const int Items = 1000000;

        private DataTable _pseudoCountersTable;
        private DataTable _pseudoFieldsTable;

        [GlobalSetup]
        public void Setup()
        {
            _pseudoCountersTable = BenchmarkUtils.GetSampleDataTable(Items);
            _pseudoFieldsTable = BenchmarkUtils.GetSampleDataTable(Items);
        }

        [Benchmark(Baseline = true)]
        public void NestedLoop()
        {
            var temp = _pseudoCountersTable.Clone();

            temp.Columns.Add("AnotherValue_Fields", typeof(int));
            temp.Columns.Add("StringValue_Fields", typeof(int));

            foreach (DataRow fieldsRow in _pseudoFieldsTable.AsEnumerable())
            {
                foreach (DataRow counters in temp.AsEnumerable().Where(x => x.Field<int>("ID") == fieldsRow.Field<int>("ID")))
                {
                    counters["AnotherValue_Fields"] = counters["AnotherValue"];
                    counters["StringValue_Fields"] = counters["StringValue"];
                }
            }
        }

        [Benchmark]
        public void LinqJoin()
        {
            var temp = _pseudoCountersTable.Clone();

            temp.Columns.Add("AnotherValue_Fields", typeof(int));
            temp.Columns.Add("StringValue_Fields", typeof(int));

            var joinedResults = _pseudoFieldsTable.AsEnumerable()
                .Join(temp.AsEnumerable(), outer => outer.Field<int>("ID"), inner => inner.Field<int>("ID"), (resultTemp, resultOuter) => new { resultTemp, resultOuter });

            foreach (var joinedResult in joinedResults)
            {
                joinedResult.resultTemp["AnotherValue_Fields"] = joinedResult.resultOuter["AnotherValue"];
                joinedResult.resultTemp["StringValue_Fields"] = joinedResult.resultOuter["StringValue"];
            }
        }
    }
}
