using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EQS.Debug
{
    public enum Colors
    {
        Red,
        Green,
        Blue,
        Yellow,
        Cyan,
        Magenta,
        Orange,
        Purple,
        Turquoise,
        Silver,
        Emerald
    }

    public class DebugItem
    {
        private static readonly Dictionary<Colors, Vector4> ColorMap = new Dictionary<Colors, Vector4>()
        {
            // TODO: Write right color vector
            { Colors.Red,       new Vector4(1, 0, 0, 0 ) },
            { Colors.Green,     new Vector4(0, 1, 0, 0 ) },
            { Colors.Blue,      new Vector4(1, 0, 0, 0 ) },
            { Colors.Yellow,    new Vector4(1, 0, 0, 0 ) },
            { Colors.Cyan,      new Vector4(1, 0, 0, 0 ) },
            { Colors.Magenta,   new Vector4(1, 0, 0, 0 ) },
            { Colors.Orange,    new Vector4(1, 0, 0, 0 ) },
            { Colors.Purple,    new Vector4(1, 0, 0, 0 ) },
            { Colors.Turquoise, new Vector4(1, 0, 0, 0 ) },
            { Colors.Silver,    new Vector4(1, 0, 0, 0 ) },
            { Colors.Emerald,   new Vector4(1, 0, 0, 0 ) },
        };

        public Vector3 Point;
        public Single Score;
        public Vector4 Color;

        internal static DebugItem Converter(QueryItem item, Colors color)
        {
            ColorMap.TryGetValue(color, out var value);

            return new DebugItem()
            {
                Point = item.Location,
                Score = item.Score,
                Color = value
            };
        }

        internal static DebugItem Converter(QueryItem item)
        {
            return new DebugItem()
            {
                Point = item.Location,
                Score = item.Score,
                Color = Score2Color(item.Score),
            };

            static Vector4 Score2Color(Single score) => new Vector4(score, 1 - score, 0, 1f);
        }
    }
}
