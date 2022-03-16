using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Media;

namespace Highlighter.Core
{
    public static class Helper
    {
        public static char[] escapes = { ' ', '!', '"', '@', '$', '(', ')', '{','}', '[', ']', '*', '-', '.', '/', '>', '<', '"', ':', ';', ',', '?', '\'', '\n', '\r', '\t', '=' };

        public static List<Color> colors = new();

        public static string LastSolution;

        public static Color Undefined { get; } = Color.FromArgb(0, 0, 0, 0);

        [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
        public static Color ChangeAlpha(this Color c, byte alpha) => Color.FromArgb(alpha, c.R, c.G, c.B);

        public static string ColorToHex(this Color color) => $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";

        public static List<Color> SetupColors(this List<Color> colors)
        {
            colors.Clear();
            colors.Add(HexToColor("#FFB1B9"));
            colors.Add(HexToColor("#EE1E34"));
            colors.Add(HexToColor("#E8142A"));
            colors.Add(HexToColor("#D2041A"));
            colors.Add(HexToColor("#FFCEAE"));
            colors.Add(HexToColor("#FE7A2B"));
            colors.Add(HexToColor("#FA5C00"));
            colors.Add(HexToColor("#EE5700"));
            colors.Add(HexToColor("#A4E8C2"));
            colors.Add(HexToColor("#1F9850"));
            colors.Add(HexToColor("#0C823C"));
            colors.Add(HexToColor("#006929"));
            colors.Add(HexToColor("#D0E2FF"));
            colors.Add(HexToColor("#5d93f8"));
            colors.Add(HexToColor("#1062FF"));
            colors.Add(HexToColor("#0c4dce"));
            colors.Add(HexToColor("#FFDCF9"));
            colors.Add(HexToColor("#DF2F9B"));
            colors.Add(HexToColor("#BD0A78"));
            colors.Add(HexToColor("#8E0157"));
            colors.Add(HexToColor("#EFD1FF"));
            colors.Add(HexToColor("#AF34FE"));
            colors.Add(HexToColor("#9F1FF3"));
            colors.Add(HexToColor("#8911D9"));
            colors.Add(HexToColor("#BEB8E5"));
            colors.Add(HexToColor("#473497"));
            colors.Add(HexToColor("#2A1770"));
            colors.Add(HexToColor("#1C0C56"));
            colors.Add(HexToColor("#BCF2FF"));
            colors.Add(HexToColor("#12B2F3"));
            colors.Add(HexToColor("#019AD8"));
            colors.Add(HexToColor("#01668F"));
            colors.Add(HexToColor("#77F3E6"));
            colors.Add(HexToColor("#15C1B0"));
            colors.Add(HexToColor("#059183"));
            colors.Add(HexToColor("#04685E"));
            colors.Add(HexToColor("#D1F95F"));
            colors.Add(HexToColor("#79A00B"));
            colors.Add(HexToColor("#425900"));
            colors.Add(HexToColor("#334500"));

            return colors;
        }

        public static Color HexToColor(string value)
        {
            value = value.Trim('#');
            switch (value.Length)
            {
                case 0:
                    return Undefined;
                case <= 6:
                    value = "FF" + value.PadLeft(6, '0');
                    break;
            }

            return uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint u)
                ? UIntToColor(u)
                : Undefined;
        }

        public static Color UIntToColor(uint color)
        {
            var a = (byte)(color >> 24);
            var r = (byte)(color >> 16);
            var g = (byte)(color >> 8);
            var b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }
        public static Color GetRandomColor()
        {
            if (colors.Count == 0)
                colors.SetupColors();

            Random r = new(Environment.TickCount);
            return colors[r.Next(0, colors.Count - 1)];
        }

        public static void InitDefaults()
        {
            colors.SetupColors();
        }

        internal static HighlightTag[] GetFillerTags()
        {
            return
                new HighlightTag[]
                {
                    new()
                    {
                        Color = HexToColor("#E8142A"),
                        Criteria = "//#",
                        Shape = TagShape.Line
                    },
                    new()
                    {
                        Color =  HexToColor("#1C6BFF"),
                        Criteria = "//##",
                        Shape = TagShape.Line
                    },
                    new()
                    {
                        Color =  HexToColor("#473497"),
                        Criteria = "//###",
                        Shape = TagShape.Line
                    },
                    new()
                    {
                        Color =  HexToColor("#1F9850"),
                        Criteria = "//####",
                        Shape = TagShape.Line
                    },
                    new()
                    {
                        Color =  HexToColor("#019AD8"),
                        Criteria = "//#####",
                        Shape = TagShape.Line
                    },
                    new()
                    {
                        Color =  HexToColor("#059183"),
                        Criteria = "//######",
                        Shape = TagShape.Line
                    },
                   new()
                    {
                        Color = HexToColor("#E8142A"),
                        Criteria = "//1",
                        Shape = TagShape.LineUnder
                    },
                    new()
                    {
                        Color =  HexToColor("#1C6BFF"),
                        Criteria = "//2",
                        Shape = TagShape.LineUnder
                    },
                    new()
                    {
                        Color =  HexToColor("#473497"),
                        Criteria = "//3",
                        Shape = TagShape.LineUnder
                    },
                    new()
                    {
                        Color =  HexToColor("#1F9850"),
                        Criteria = "//4",
                        Shape = TagShape.LineUnder
                    },
                    new()
                    {
                        Color =  HexToColor("#019AD8"),
                        Criteria = "//5",
                        Shape = TagShape.LineUnder
                    },
                    new()
                    {
                        Color =  HexToColor("#059183"),
                        Criteria = "//6",
                        Shape = TagShape.LineUnder
                    },
                  
                };
        }

    }
}

