using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace PerfBenchmarks
{
    [MemoryDiagnoser]
    public class ArrayPoolBenchmarks
    {
        private Processor _processor;

        [Params(20, 100, 1_000, 10_000, 100_000)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _processor = new Processor();
        }

        [Benchmark(Baseline = true)]
        public void Original()
        {
            _processor.DoSomeWorkVeryOften(Size);
        }

        [Benchmark]
        public void UsingArrayPool()
        {
            _processor.DoSomeWorkVeryOftenUsingArrayPool(Size);
        }
    }

    public class Processor
    {
        public void DoSomeWorkVeryOften(int size)
        {
            var buffer = new byte[size];

            DoSomethingWithBuffer(buffer);
        }

        public void DoSomeWorkVeryOftenUsingArrayPool(int size)
        {
            var arrayPool = ArrayPool<byte>.Shared;
            var buffer = arrayPool.Rent(size);

            try
            {
                DoSomethingWithBuffer(buffer);

            }
            finally
            {
                arrayPool.Return(buffer);
            }
        }

        private void DoSomethingWithBuffer(byte[] buffer)
        {
            //  use the array
        }
    }
}