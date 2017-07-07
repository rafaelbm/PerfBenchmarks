using System;
using BenchmarkDotNet.Attributes;

namespace PerfBenchmarks
{
    [Config(typeof(BenchConfig))]
    public class StringTests
    {
        private const string UperCasedString = "ASDIIOOIJDS)(214KÇLKDLÇKDSOPKSPD()*()8192I4IOPKFKLÇKPOKPF(*9031841JLFJFOKFPOKFPOKFOIMSMADFOIJOFIASJFIO";
        private const string LowerCasedString = "asdiiooijds)(214kçlkdlçkdsopkspd()*()8192i4iopkfklçkpokpf(*9031841jlfjfokfpokfpokfoimsmadfoijofiasjfio";
        private const string MixedCasedString = "AsdiIOoijDS)(214kÇLkdlçkdsopkspd()*()8192i4iopKFklçkpokPF(*9031841jlFJFOKFPOKFPOKFOImSMADFoijofiASJFIO";

        [Benchmark]
        public bool EqualsUsingToLowerThanEqual()
        {
            return MixedCasedString.ToLower() == LowerCasedString;
        }

        [Benchmark]
        public bool EqualsUsingToUpperThanEqual()
        {
            return MixedCasedString.ToUpper() == UperCasedString;
        }

        [Benchmark]
        public bool EqualsUsingStringOrdinalIgnoreCase()
        {
            return MixedCasedString.Equals(LowerCasedString, StringComparison.OrdinalIgnoreCase);
        }
    }
}
