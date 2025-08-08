using System;
using DotNetVision.Core;

namespace DotNetVision.Core
{
    /// <summary>
    /// Represents a single character cell in the console buffer
    /// </summary>
    public struct ConsoleCell
    {
        public char Character { get; set; }
        public ColorPair Colors { get; set; }

        public ConsoleCell(char character, ColorPair colors)
        {
            Character = character;
            Colors = colors;
        }

        public static readonly ConsoleCell Empty = new(' ', ColorPair.Default);

        public override bool Equals(object? obj)
        {
            if (obj is ConsoleCell other)
            {
                return Character == other.Character && 
                       Colors.Foreground == other.Colors.Foreground &&
                       Colors.Background == other.Colors.Background;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Character, Colors.Foreground, Colors.Background);
        }

        public static bool operator ==(ConsoleCell left, ConsoleCell right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ConsoleCell left, ConsoleCell right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// Double-buffered console screen management with dirty region tracking
    /// </summary>
    public class BufferedScreen : IScreen
    {
        private ConsoleCell[,]? _frontBuffer;
        private ConsoleCell[,]? _backBuffer;
        private bool _initialized = false;
        private ColorPair _currentColors = ColorPair.Default;
        private int _width;
        private int _height;
        private bool _cursorVisible = false;

        public int Width => _width;
        public int Height => _height;
        public Rect Bounds => new(0, 0, Width, Height);

        public bool IsCursorVisible 
        {
            get => _cursorVisible;
            set 
            {
                if (_cursorVisible != value)
                {
                    _cursorVisible = value;
                    if (_initialized)
                    {
                        Console.CursorVisible = value;
                    }
                }
            }
        }

        /// <summary>
        /// Initialize the buffered screen system
        /// </summary>
        public void Initialize()
        {
            Logger.Instance.LogEntry("BufferedScreen", "Initialize");
            
            if (_initialized) 
            {
                Logger.Instance.LogExit("BufferedScreen", "Initialize", "Already initialized");
                return;
            }

            try
            {
                Logger.Instance.Log("BufferedScreen", "Initialize", "Setting console properties");
                Console.CursorVisible = IsCursorVisible;
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                
                try
                {
                    Logger.Instance.Log("BufferedScreen", "Initialize", $"Current size: {Console.WindowWidth}x{Console.WindowHeight}");
                    if (Console.WindowWidth < 80) Console.WindowWidth = 80;
                    if (Console.WindowHeight < 25) Console.WindowHeight = 25;
                    Logger.Instance.Log("BufferedScreen", "Initialize", $"Final size: {Console.WindowWidth}x{Console.WindowHeight}");
                }
                catch (Exception ex)
                {
                    Logger.Instance.LogError("BufferedScreen", "Initialize", $"Console resize failed: {ex.Message}");
                }

                _width = Console.WindowWidth;
                _height = Console.WindowHeight;
                
                Logger.Instance.Log("BufferedScreen", "Initialize", $"Initializing buffers: {_width}x{_height}");

                _frontBuffer = new ConsoleCell[_height, _width];
                _backBuffer = new ConsoleCell[_height, _width];

                var defaultCell = new ConsoleCell(' ', ColorPair.Default);
                var differentCell = new ConsoleCell('X', new ColorPair(ConsoleColor.Red, ConsoleColor.Blue));
                
                Logger.Instance.Log("BufferedScreen", "Initialize", "Filling buffers");
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        _frontBuffer[y, x] = differentCell;
                        _backBuffer[y, x] = defaultCell;
                    }
                }

                Logger.Instance.Log("BufferedScreen", "Initialize", "Clearing console");
                Console.Clear();
                Console.ResetColor();
                _initialized = true;
                
                Logger.Instance.Log("BufferedScreen", "Initialize", "Forcing initial clear and present");
                Clear();
                Present();
                
                Logger.Instance.LogExit("BufferedScreen", "Initialize", "Success");
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError("BufferedScreen", "Initialize", ex);
                throw;
            }
        }

        /// <summary>
        /// Restore console to normal state
        /// </summary>
        public void Cleanup()
        {
            Logger.Instance.LogEntry("BufferedScreen", "Cleanup");
            
            if (!_initialized) return;

            Console.ResetColor();
            Console.CursorVisible = true;
            Console.Clear();
            _initialized = false;
            
            Logger.Instance.LogExit("BufferedScreen", "Cleanup");
        }

        /// <summary>
        /// Clear the back buffer (preparing for new frame)
        /// </summary>
        public void Clear()
        {
            if (!_initialized) return;

            var clearCell = new ConsoleCell(' ', ColorPair.Default);

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _backBuffer![y, x] = clearCell;
                }
            }
        }

        private void SetCell(int x, int y, char character, ColorPair? colors = null)
        {
            if (!_initialized || x < 0 || x >= _width || y < 0 || y >= _height) return;

            var cellColors = colors ?? _currentColors;
            _backBuffer![y, x] = new ConsoleCell(character, cellColors);
        }

        public void SetCursor(int x, int y)
        {
            if (_initialized && IsCursorVisible)
            {
                Console.SetCursorPosition(x, y);
            }
        }

        public void SetCursor(Point position) => SetCursor(position.X, position.Y);

        public void SetColors(ColorPair colors)
        {
            _currentColors = colors;
        }

        public void WriteAt(int x, int y, string text, ColorPair? colors = null)
        {
            if (!_initialized || string.IsNullOrEmpty(text)) return;

            var writeColors = colors ?? _currentColors;
            
            for (int i = 0; i < text.Length && x + i < _width; i++)
            {
                if (y >= 0 && y < _height && x + i >= 0)
                {
                    _backBuffer![y, x + i] = new ConsoleCell(text[i], writeColors);
                }
            }
        }

        public void WriteAt(Point position, string text, ColorPair? colors = null) =>
            WriteAt(position.X, position.Y, text, colors);

        public void FillRect(Rect rect, char character = ' ', ColorPair? colors = null)
        {
            if (!_initialized) return;

            var fillColors = colors ?? _currentColors;
            var fillCell = new ConsoleCell(character, fillColors);
            
            for (int y = rect.Y; y < rect.Y + rect.Height && y < _height; y++)
            {
                for (int x = rect.X; x < rect.X + rect.Width && x < _width; x++)
                {
                    if (x >= 0 && y >= 0)
                    {
                        _backBuffer![y, x] = fillCell;
                    }
                }
            }
        }

        public void DrawBox(Rect rect, ColorPair? colors = null, BoxStyle style = BoxStyle.Single)
        {
            if (!_initialized) return;

            var boxColors = colors ?? _currentColors;
            var chars = GetBoxChars(style);

            // Top border
            if (rect.Y >= 0 && rect.Y < _height)
            {
                if (rect.X >= 0 && rect.X < _width)
                    SetCell(rect.X, rect.Y, chars.TopLeft, boxColors);
                
                for (int x = 1; x < rect.Width - 1; x++)
                {
                    if (rect.X + x >= 0 && rect.X + x < _width)
                        SetCell(rect.X + x, rect.Y, chars.Horizontal, boxColors);
                }
                
                if (rect.X + rect.Width - 1 >= 0 && rect.X + rect.Width - 1 < _width)
                    SetCell(rect.X + rect.Width - 1, rect.Y, chars.TopRight, boxColors);
            }

            // Sides
            for (int y = 1; y < rect.Height - 1; y++)
            {
                if (rect.Y + y >= 0 && rect.Y + y < _height)
                {
                    if (rect.X >= 0 && rect.X < _width)
                        SetCell(rect.X, rect.Y + y, chars.Vertical, boxColors);
                    
                    if (rect.X + rect.Width - 1 >= 0 && rect.X + rect.Width - 1 < _width)
                        SetCell(rect.X + rect.Width - 1, rect.Y + y, chars.Vertical, boxColors);
                }
            }

            // Bottom border
            if (rect.Y + rect.Height - 1 >= 0 && rect.Y + rect.Height - 1 < _height)
            {
                if (rect.X >= 0 && rect.X < _width)
                    SetCell(rect.X, rect.Y + rect.Height - 1, chars.BottomLeft, boxColors);
                
                for (int x = 1; x < rect.Width - 1; x++)
                {
                    if (rect.X + x >= 0 && rect.X + x < _width)
                        SetCell(rect.X + x, rect.Y + rect.Height - 1, chars.Horizontal, boxColors);
                }
                
                if (rect.X + rect.Width - 1 >= 0 && rect.X + rect.Width - 1 < _width)
                    SetCell(rect.X + rect.Width - 1, rect.Y + rect.Height - 1, chars.BottomRight, boxColors);
            }
        }

        public void Present()
        {
            if (!_initialized) return;

            ConsoleColor currentForeground = Console.ForegroundColor;
            ConsoleColor currentBackground = Console.BackgroundColor;

            try
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        var backCell = _backBuffer![y, x];
                        var frontCell = _frontBuffer![y, x];

                        if (backCell != frontCell)
                        {
                            if (currentForeground != backCell.Colors.Foreground)
                            {
                                Console.ForegroundColor = backCell.Colors.Foreground;
                                currentForeground = backCell.Colors.Foreground;
                            }
                            
                            if (currentBackground != backCell.Colors.Background)
                            {
                                Console.BackgroundColor = backCell.Colors.Background;
                                currentBackground = backCell.Colors.Background;
                            }

                            if (x >= 0 && x < Console.WindowWidth && y >= 0 && y < Console.WindowHeight)
                            {
                                Console.SetCursorPosition(x, y);
                                Console.Write(backCell.Character);
                            }

                            _frontBuffer![y, x] = backCell;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError("BufferedScreen", "Present", ex);
            }
        }

        private static BoxChars GetBoxChars(BoxStyle style) => style switch
        {
            BoxStyle.Single => new BoxChars('┌', '┐', '└', '┘', '─', '│'),
            BoxStyle.Double => new BoxChars('╔', '╗', '╚', '╝', '═', '║'),
            BoxStyle.Ascii => new BoxChars('+', '+', '+', '+', '-', '|'),
            _ => new BoxChars('┌', '┐', '└', '┘', '─', '│')
        };

        private record BoxChars(char TopLeft, char TopRight, char BottomLeft, char BottomRight, char Horizontal, char Vertical);
    }
}
