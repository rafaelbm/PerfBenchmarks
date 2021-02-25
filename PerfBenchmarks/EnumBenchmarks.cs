using System;
using BenchmarkDotNet.Attributes;

namespace PerfBenchmarks
{
    [Config(typeof(BenchConfig))]
    public class EnumBenchmarks
    {
        private string _myEnumValueAsString;

        [GlobalSetup]
        public void Setup()
        {
            _myEnumValueAsString = "Value1";
        }


        [Benchmark(Baseline = true)]
        public bool EqualityCheckCastingEnumToString()
        {
            return _myEnumValueAsString == MyEnum.Value1.ToString();
        }

        [Benchmark]
        public bool EqualityCheckIsDefined()
        {
            return Enum.IsDefined(typeof(MyEnum), _myEnumValueAsString);
        }

        [Benchmark]
        public bool EqualityCheckReturningTrueTryParse()
        {
            return Enum.TryParse(_myEnumValueAsString, out MyEnum _);
        }
    }

    public enum MyEnum
    {
        Value0,
        Value1,
    }
}