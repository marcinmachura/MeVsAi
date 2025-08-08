using System;

namespace DotNetVision.Core
{
    /// <summary>
    /// Represents a point in the console coordinate system
    /// </summary>
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);

        public override string ToString() => $"({X}, {Y})";
    }

    /// <summary>
    /// Represents a rectangular area in the console
    /// </summary>
    public struct Rect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Rect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Point TopLeft => new(X, Y);
        public Point TopRight => new(X + Width - 1, Y);
        public Point BottomLeft => new(X, Y + Height - 1);
        public Point BottomRight => new(X + Width - 1, Y + Height - 1);

        public bool Contains(Point point) =>
            point.X >= X && point.X < X + Width &&
            point.Y >= Y && point.Y < Y + Height;

        public bool Contains(int x, int y) => Contains(new Point(x, y));

        public bool IsEmpty => Width <= 0 || Height <= 0;

        public static Rect Intersect(Rect a, Rect b)
        {
            int x1 = Math.Max(a.X, b.X);
            int y1 = Math.Max(a.Y, b.Y);
            int x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            int y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
                return new Rect(x1, y1, x2 - x1, y2 - y1);
            else
                return new Rect(0, 0, 0, 0); // Empty rectangle
        }

        public override string ToString() => $"Rect({X}, {Y}, {Width}, {Height})";
    }

    /// <summary>
    /// Color attributes for console text
    /// </summary>
    public struct ColorPair
    {
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

        public ColorPair(ConsoleColor foreground, ConsoleColor background = ConsoleColor.Black)
        {
            Foreground = foreground;
            Background = background;
        }

        public static readonly ColorPair Default = new(ConsoleColor.Gray, ConsoleColor.Black);
        public static readonly ColorPair Highlighted = new(ConsoleColor.Black, ConsoleColor.Gray);
        public static readonly ColorPair Error = new(ConsoleColor.White, ConsoleColor.Red);
        public static readonly ColorPair Success = new(ConsoleColor.Black, ConsoleColor.Green);
        public static readonly ColorPair Warning = new(ConsoleColor.Black, ConsoleColor.Yellow);
    }

    /// <summary>
    /// Box drawing styles for borders
    /// </summary>
    public enum BoxStyle
    {
        Single,
        Double,
        Ascii
    }
}
