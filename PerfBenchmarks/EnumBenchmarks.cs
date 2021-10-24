using System;
using BenchmarkDotNet.Attributes;

namespace PerfBenchmarks
{
    [Config(typeof(BenchConfig))]
    public class EnumBenchmarks
    {
        private string _myEnumValueAsString;
        private int _myEnumValueAsInt;

        [GlobalSetup]
        public void Setup()
        {
            _myEnumValueAsString = "Value1";
            _myEnumValueAsInt = 1;
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

        [Benchmark]
        public bool EnumValueAsIntEqualityCheckReturningTrueToObject()
        {
            return (MyEnum)Enum.ToObject(typeof(MyEnum), _myEnumValueAsInt) == MyEnum.Value1;
        }

        [Benchmark]
        public bool EnumValueAsIntEqualityCheckCastingEnumToString()
        {
            return _myEnumValueAsInt == MyEnum.Value1.GetHashCode();
        }

        [Benchmark]
        public bool EnumValueAsIntEqualityCheckIsDefined()
        {
            return Enum.IsDefined(typeof(MyEnum), _myEnumValueAsInt);
        }
    }

    public enum MyEnum
    {
        Value0,
        Value1,
    }
}