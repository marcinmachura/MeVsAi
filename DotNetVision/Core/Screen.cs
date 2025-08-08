namespace DotNetVision.Core
{
    public static class Screen
    {
        private static IScreen? _instance;

        public static IScreen Instance 
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("Screen has not been initialized. Call Application.Run() first.");
                }
                return _instance;
            }
            internal set => _instance = value;
        }

        public static int Width => Instance.Width;
        public static int Height => Instance.Height;
        public static Rect Bounds => Instance.Bounds;
        public static bool IsCursorVisible 
        {
            get => Instance.IsCursorVisible;
            set => Instance.IsCursorVisible = value;
        }

        public static void Clear() => Instance.Clear();
        public static void SetCursor(int x, int y) => Instance.SetCursor(x, y);
        public static void SetCursor(Point position) => Instance.SetCursor(position);
        public static void SetColors(ColorPair colors) => Instance.SetColors(colors);
        public static void WriteAt(int x, int y, string text, ColorPair? colors = null) => Instance.WriteAt(x, y, text, colors);
        public static void WriteAt(Point position, string text, ColorPair? colors = null) => Instance.WriteAt(position, text, colors);
        public static void FillRect(Rect rect, char character = ' ', ColorPair? colors = null) => Instance.FillRect(rect, character, colors);
        public static void DrawBox(Rect rect, ColorPair? colors = null, BoxStyle style = BoxStyle.Single) => Instance.DrawBox(rect, colors, style);
        public static void Present() => Instance.Present();
    }
}
