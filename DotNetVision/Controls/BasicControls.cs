using System;
using DotNetVision.Core;

namespace DotNetVision.Controls
{
    /// <summary>
    /// A clickable button control
    /// </summary>
    public class Button : Control, IFocusable
    {
        private bool _isPressed = false;
        
        public event EventHandler? Click;

        public Button() : base()
        {
            Height = 3; // Default button height
            ForeColor = ColorPair.Default;
            BackColor = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray);
        }

        public Button(int x, int y, int width, string text) : base(x, y, width, 3)
        {
            Text = text;
            ForeColor = ColorPair.Default;
            BackColor = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray);
        }

        // IFocusable implementation
        public bool CanFocus { get; set; } = true;
        public bool HasFocus { get; set; } = false;
        public int TabIndex { get; set; } = 0;

        public void OnGotFocus()
        {
            HasFocus = true;
            Invalidate();
        }

        public void OnLostFocus()
        {
            HasFocus = false;
            Invalidate();
        }

        protected override void DoPaint(Rect screenBounds)
        {
            var colors = _isPressed ? 
                new ColorPair(ConsoleColor.White, ConsoleColor.DarkGray) :
                HasFocus ?
                new ColorPair(ConsoleColor.Yellow, ConsoleColor.DarkBlue) :
                BackColor;

            // Fill button background
            Screen.Instance.FillRect(screenBounds, ' ', colors);

            // Draw button border
            Screen.Instance.DrawBox(screenBounds, colors, BoxStyle.Single);

            // Draw button text (centered)
            if (!string.IsNullOrEmpty(Text) && screenBounds.Width > 2)
            {
                var maxTextLength = screenBounds.Width - 2;
                var displayText = Text.Length > maxTextLength ? 
                    Text.Substring(0, maxTextLength) : Text;

                var textX = screenBounds.X + (screenBounds.Width - displayText.Length) / 2;
                var textY = screenBounds.Y + screenBounds.Height / 2;

                Screen.Instance.WriteAt(textX, textY, displayText, colors);
            }
        }

        public override bool OnKeyPressed(ConsoleKeyInfo keyInfo)
        {
            if (!Enabled) return false;

            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    if (HasFocus)
                    {
                        _isPressed = true;
                        Invalidate();
                        
                        // Simulate button press animation (reduced time)
                        System.Threading.Thread.Sleep(50);
                        
                        _isPressed = false;
                        Invalidate();
                        
                        Click?.Invoke(this, EventArgs.Empty);
                        return true;
                    }
                    break;
            }

            return base.OnKeyPressed(keyInfo);
        }

        /// <summary>
        /// Programmatically click the button
        /// </summary>
        public void PerformClick()
        {
            if (Enabled)
            {
                Click?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// A text label control
    /// </summary>
    public class Label : Control
    {
        public TextAlignment Alignment { get; set; } = TextAlignment.Left;

        public Label() : base()
        {
            Height = 1; // Default label height
        }

        public Label(int x, int y, int width, string text) : base(x, y, width, 1)
        {
            Text = text;
        }

        protected override void DoPaint(Rect screenBounds)
        {
            if (string.IsNullOrEmpty(Text)) return;

            var lines = Text.Split('\n');
            var maxLines = Math.Min(lines.Length, screenBounds.Height);

            for (int i = 0; i < maxLines; i++)
            {
                var line = lines[i];
                if (line.Length > screenBounds.Width)
                    line = line.Substring(0, screenBounds.Width);

                int x = screenBounds.X;
                switch (Alignment)
                {
                    case TextAlignment.Center:
                        x = screenBounds.X + (screenBounds.Width - line.Length) / 2;
                        break;
                    case TextAlignment.Right:
                        x = screenBounds.X + screenBounds.Width - line.Length;
                        break;
                }

                Screen.Instance.WriteAt(x, screenBounds.Y + i, line, ForeColor);
            }
        }
    }

    /// <summary>
    /// A text input control
    /// </summary>
    public class TextBox : Control, IFocusable
    {
        private string _text = string.Empty;
        private int _cursorPosition = 0;
        
        public event EventHandler? TextChanged;

        public override string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value ?? string.Empty;
                    _cursorPosition = Math.Min(_cursorPosition, _text.Length);
                    TextChanged?.Invoke(this, EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        // IFocusable implementation
        public bool CanFocus { get; set; } = true;
        public bool HasFocus { get; set; } = false;
        public int TabIndex { get; set; } = 0;

        public void OnGotFocus()
        {
            HasFocus = true;
            Screen.Instance.IsCursorVisible = true;
            Invalidate();
        }

        public void OnLostFocus()
        {
            HasFocus = false;
            Screen.Instance.IsCursorVisible = false;
            Invalidate();
        }

        public int MaxLength { get; set; } = 255;
        public char PasswordChar { get; set; } = '\0'; // Use '\0' for no password masking

        public TextBox() : base()
        {
            Height = 1;
            ForeColor = new ColorPair(ConsoleColor.Black, ConsoleColor.White);
            BackColor = new ColorPair(ConsoleColor.Black, ConsoleColor.White);
        }

        public TextBox(int x, int y, int width) : base(x, y, width, 1)
        {
            ForeColor = new ColorPair(ConsoleColor.Black, ConsoleColor.White);
            BackColor = new ColorPair(ConsoleColor.Black, ConsoleColor.White);
        }

        protected override void DoPaint(Rect screenBounds)
        {
            var colors = HasFocus ? 
                new ColorPair(ConsoleColor.Black, ConsoleColor.White) :
                new ColorPair(ConsoleColor.DarkGray, ConsoleColor.Gray);

            // Fill background
            Screen.Instance.FillRect(screenBounds, ' ', colors);

            // Display text
            if (!string.IsNullOrEmpty(_text))
            {
                var displayText = PasswordChar != '\0' ? 
                    new string(PasswordChar, _text.Length) : _text;

                var maxLength = screenBounds.Width;
                if (displayText.Length > maxLength)
                {
                    // Scroll text if it's too long
                    var start = Math.Max(0, _cursorPosition - maxLength + 1);
                    displayText = displayText.Substring(start, Math.Min(maxLength, displayText.Length - start));
                }

                Screen.Instance.WriteAt(screenBounds.X, screenBounds.Y, displayText, colors);
            }

            // Show cursor if focused
            if (HasFocus && screenBounds.Width > 0)
            {
                var cursorX = screenBounds.X + Math.Min(_cursorPosition, screenBounds.Width - 1);
                Screen.Instance.SetCursor(cursorX, screenBounds.Y);
            }
        }

        public override bool OnKeyPressed(ConsoleKeyInfo keyInfo)
        {
            if (!HasFocus || !Enabled) return false;

            switch (keyInfo.Key)
            {
                case ConsoleKey.Backspace:
                    if (_cursorPosition > 0)
                    {
                        _text = _text.Remove(_cursorPosition - 1, 1);
                        _cursorPosition--;
                        TextChanged?.Invoke(this, EventArgs.Empty);
                        Invalidate();
                        return true;
                    }
                    break;

                case ConsoleKey.Delete:
                    if (_cursorPosition < _text.Length)
                    {
                        _text = _text.Remove(_cursorPosition, 1);
                        TextChanged?.Invoke(this, EventArgs.Empty);
                        Invalidate();
                        return true;
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (_cursorPosition > 0)
                    {
                        _cursorPosition--;
                        Invalidate();
                        return true;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (_cursorPosition < _text.Length)
                    {
                        _cursorPosition++;
                        Invalidate();
                        return true;
                    }
                    break;

                case ConsoleKey.Home:
                    _cursorPosition = 0;
                    Invalidate();
                    return true;

                case ConsoleKey.End:
                    _cursorPosition = _text.Length;
                    Invalidate();
                    return true;

                default:
                    // Handle printable characters
                    if (!char.IsControl(keyInfo.KeyChar) && _text.Length < MaxLength)
                    {
                        _text = _text.Insert(_cursorPosition, keyInfo.KeyChar.ToString());
                        _cursorPosition++;
                        TextChanged?.Invoke(this, EventArgs.Empty);
                        Invalidate();
                        return true;
                    }
                    break;
            }

            return base.OnKeyPressed(keyInfo);
        }
    }

    public enum TextAlignment
    {
        Left,
        Center,
        Right
    }
}
