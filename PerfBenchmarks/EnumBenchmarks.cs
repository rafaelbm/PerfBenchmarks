using System;
using BenchmarkDotNet.Attributes;
using NetEscapades.EnumGenerators;

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

        [Benchmark]
        public string EnumToString()
        {
            return Color.LightGreen.ToString();
        }

        [Benchmark]
        public string EnumToStringFast()
        {
            return Color.LightGreen.ToStringFast();
        }

        [Benchmark]
        public bool EnumIsDefined()
        {
            return Enum.IsDefined(typeof(Color), (Color)60);
        }

        [Benchmark]
        public bool EnumIsDefinedFast()
        {
            return ColorExtensions.IsDefined((Color)60);
        }

        [Benchmark]
        public (bool, Color) EnumTryParse()
        {
            var couldParse = Enum.TryParse("LightGreen", false, out Color value);
            return (couldParse, value);
        }

        [Benchmark]
        public (bool, Color) EnumTryParseFast()
        {
            var couldParse = ColorExtensions.TryParse("LightGreen", false, out Color value);
            return (couldParse, value);
        }
    }

    public enum MyEnum
    {
        Value0,
        Value1,
    }

    [EnumExtensions]
    public enum Color
    {
        ActiveBorder = 1,
        ActiveCaption = 2,
        ActiveCaptionText = 3,
        AppWorkspace = 4,
        Control = 5,
        ControlDark = 6,
        ControlDarkDark = 7,
        ControlLight = 8,
        ControlLightLight = 9,
        ControlText = 10,
        Desktop = 11,
        GrayText = 12,
        Highlight = 13,
        HighlightText = 14,
        HotTrack = 15,
        InactiveBorder = 16,
        InactiveCaption = 17,
        InactiveCaptionText = 18,
        Info = 19,
        InfoText = 20,
        Menu = 21,
        MenuText = 22,
        ScrollBar = 23,
        Window = 24,
        WindowFrame = 25,
        WindowText = 26,
        Transparent = 27,
        AliceBlue = 28,
        AntiqueWhite = 29,
        Aqua = 30,
        Aquamarine = 31,
        Azure = 32,
        Beige = 33,
        Bisque = 34,
        Black = 35,
        BlanchedAlmond = 36,
        Blue = 37,
        BlueViolet = 38,
        Brown = 39,
        BurlyWood = 40,
        CadetBlue = 41,
        Chartreuse = 42,
        Chocolate = 43,
        Coral = 44,
        CornflowerBlue = 45,
        Cornsilk = 46,
        Crimson = 47,
        Cyan = 48,
        DarkBlue = 49,
        DarkCyan = 50,
        DarkGoldenrod = 51,
        DarkGray = 52,
        DarkGreen = 53,
        DarkKhaki = 54,
        DarkMagenta = 55,
        DarkOliveGreen = 56,
        DarkOrange = 57,
        DarkOrchid = 58,
        DarkRed = 59,
        DarkSalmon = 60,
        DarkSeaGreen = 61,
        DarkSlateBlue = 62,
        DarkSlateGray = 63,
        DarkTurquoise = 64,
        DarkViolet = 65,
        DeepPink = 66,
        DeepSkyBlue = 67,
        DimGray = 68,
        DodgerBlue = 69,
        Firebrick = 70,
        FloralWhite = 71,
        ForestGreen = 72,
        Fuchsia = 73,
        Gainsboro = 74,
        GhostWhite = 75,
        Gold = 76,
        Goldenrod = 77,
        Gray = 78,
        Green = 79,
        GreenYellow = 80,
        Honeydew = 81,
        HotPink = 82,
        IndianRed = 83,
        Indigo = 84,
        Ivory = 85,
        Khaki = 86,
        Lavender = 87,
        LavenderBlush = 88,
        LawnGreen = 89,
        LemonChiffon = 90,
        LightBlue = 91,
        LightCoral = 92,
        LightCyan = 93,
        LightGoldenrodYellow = 94,
        LightGray = 95,
        LightGreen = 96,
        LightPink = 97,
        LightSalmon = 98,
        LightSeaGreen = 99,
        LightSkyBlue = 100,
        LightSlateGray = 101,
        LightSteelBlue = 102,
        LightYellow = 103,
        Lime = 104,
        LimeGreen = 105,
        Linen = 106,
        Magenta = 107,
        Maroon = 108,
        MediumAquamarine = 109,
        MediumBlue = 110,
        MediumOrchid = 111,
        MediumPurple = 112,
        MediumSeaGreen = 113,
        MediumSlateBlue = 114,
        MediumSpringGreen = 115,
        MediumTurquoise = 116,
        MediumVioletRed = 117,
        MidnightBlue = 118,
        MintCream = 119,
        MistyRose = 120,
        Moccasin = 121,
        NavajoWhite = 122,
        Navy = 123,
        OldLace = 124,
        Olive = 125,
        OliveDrab = 126,
        Orange = 127,
        OrangeRed = 128,
        Orchid = 129,
        PaleGoldenrod = 130,
        PaleGreen = 131,
        PaleTurquoise = 132,
        PaleVioletRed = 133,
        PapayaWhip = 134,
        PeachPuff = 135,
        Peru = 136,
        Pink = 137,
        Plum = 138,
        PowderBlue = 139,
        Purple = 140,
        Red = 141,
        RosyBrown = 142,
        RoyalBlue = 143,
        SaddleBrown = 144,
        Salmon = 145,
        SandyBrown = 146,
        SeaGreen = 147,
        SeaShell = 148,
        Sienna = 149,
        Silver = 150,
        SkyBlue = 151,
        SlateBlue = 152,
        SlateGray = 153,
        Snow = 154,
        SpringGreen = 155,
        SteelBlue = 156,
        Tan = 157,
        Teal = 158,
        Thistle = 159,
        Tomato = 160,
        Turquoise = 161,
        Violet = 162,
        Wheat = 163,
        White = 164,
        WhiteSmoke = 165,
        Yellow = 166,
        YellowGreen = 167,
        ButtonFace = 168,
        ButtonHighlight = 169,
        ButtonShadow = 170,
        GradientActiveCaption = 171,
        GradientInactiveCaption = 172,
        MenuBar = 173,
        MenuHighlight = 174,
        RebeccaPurple = 175
    }
}