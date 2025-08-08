using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetVision.Core
{
    /// <summary>
    /// A multi-line text editor control
    /// </summary>
    public class MultiLineTextBox
    {
        private readonly IScreen _screen;
        private readonly AppConfiguration _config;
        private List<string> _lines = new();
        private int _cursorLine = 0;
        private int _cursorCol = 0;
        private int _scrollTop = 0;
        private bool _modified = false;
        private Rect _bounds;
        private bool _focused = true;
        private MenuManager? _menuManager;
        
        public event EventHandler<EventArgs>? TextChanged;
        public event EventHandler<EventArgs>? CursorPositionChanged;
        
        /// <summary>
        /// Gets or sets the bounds of the text box
        /// </summary>
        public Rect Bounds
        {
            get => _bounds;
            set => _bounds = value;
        }
        
        /// <summary>
        /// Gets whether the text has been modified
        /// </summary>
        public bool IsModified => _modified;
        
        /// <summary>
        /// Gets the current cursor line (0-based)
        /// </summary>
        public int CursorLine => _cursorLine;
        
        /// <summary>
        /// Gets the current cursor column (0-based)
        /// </summary>
        public int CursorColumn => _cursorCol;
        
        /// <summary>
        /// Gets the total number of lines
        /// </summary>
        public int LineCount => _lines.Count;
        
        /// <summary>
        /// Gets or sets whether the control is focused
        /// </summary>
        public bool Focused
        {
            get => _focused;
            set => _focused = value;
        }
        
        public MultiLineTextBox(IScreen screen, AppConfiguration config, Rect bounds, MenuManager? menuManager = null)
        {
            _screen = screen;
            _config = config;
            _bounds = bounds;
            _menuManager = menuManager;
            _lines.Add(""); // Start with one empty line
        }
        
        /// <summary>
        /// Set the text content of the editor
        /// </summary>
        public void SetText(string[] lines)
        {
            _lines = lines?.ToList() ?? new List<string>();
            if (_lines.Count == 0) _lines.Add("");
            _cursorLine = 0;
            _cursorCol = 0;
            _scrollTop = 0;
            _modified = false;
            OnTextChanged();
        }
        
        /// <summary>
        /// Get all lines of text
        /// </summary>
        public string[] GetLines()
        {
            return _lines.ToArray();
        }
        
        /// <summary>
        /// Clear all text
        /// </summary>
        public void Clear()
        {
            _lines.Clear();
            _lines.Add("");
            _cursorLine = 0;
            _cursorCol = 0;
            _scrollTop = 0;
            _modified = false;
            OnTextChanged();
        }
        
        /// <summary>
        /// Handle keyboard input
        /// </summary>
        public bool HandleInput(ConsoleKeyInfo key)
        {
            if (!_focused) return false;
            
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    MoveCursor(0, -1);
                    return true;

                case ConsoleKey.DownArrow:
                    MoveCursor(0, 1);
                    return true;

                case ConsoleKey.LeftArrow:
                    MoveCursor(-1, 0);
                    return true;

                case ConsoleKey.RightArrow:
                    MoveCursor(1, 0);
                    return true;

                case ConsoleKey.Home:
                    _cursorCol = 0;
                    OnCursorPositionChanged();
                    return true;

                case ConsoleKey.End:
                    _cursorCol = _lines[_cursorLine].Length;
                    OnCursorPositionChanged();
                    return true;

                case ConsoleKey.PageUp:
                    MoveCursor(0, -(_bounds.Height - 1));
                    return true;

                case ConsoleKey.PageDown:
                    MoveCursor(0, _bounds.Height - 1);
                    return true;

                case ConsoleKey.Backspace:
                    HandleBackspace();
                    return true;

                case ConsoleKey.Delete:
                    HandleDelete();
                    return true;

                case ConsoleKey.Enter:
                    HandleEnter();
                    return true;

                case ConsoleKey.Tab:
                    InsertText("    "); // 4 spaces for tab
                    return true;

                default:
                    if (key.KeyChar >= 32 && key.KeyChar <= 126) // Printable characters
                    {
                        InsertText(key.KeyChar.ToString());
                        return true;
                    }
                    break;
            }
            
            return false;
        }
        
        /// <summary>
        /// Render the text box
        /// </summary>
        public void Render()
        {
            var colors = _config.GetColorSet();
            var menuArea = _menuManager?.GetMenuArea();
            
            // Draw text content
            for (int i = 0; i < _bounds.Height; i++)
            {
                int lineIndex = _scrollTop + i;
                int screenY = _bounds.Y + i;
                
                if (lineIndex < _lines.Count)
                {
                    string line = _lines[lineIndex];
                    if (line.Length > _bounds.Width)
                        line = line.Substring(0, _bounds.Width);
                    
                    var lineColor = (lineIndex == _cursorLine && _focused) ? colors.EditorCurrentLine : colors.Editor;
                    
                    // Check if this line overlaps with menu area
                    if (menuArea.HasValue && screenY >= menuArea.Value.Y && screenY < menuArea.Value.Y + menuArea.Value.Height)
                    {
                        // Draw line in segments, skipping the menu area
                        DrawLineAvoidingMenu(screenY, line, lineColor, menuArea.Value);
                    }
                    else
                    {
                        // Normal line drawing
                        _screen.FillRect(new Rect(_bounds.X, screenY, _bounds.Width, 1), ' ', lineColor);
                        _screen.WriteAt(_bounds.X, screenY, line, lineColor);
                    }
                }
                else
                {
                    // Empty line
                    if (menuArea.HasValue && screenY >= menuArea.Value.Y && screenY < menuArea.Value.Y + menuArea.Value.Height)
                    {
                        // Fill empty line in segments, skipping the menu area
                        DrawLineAvoidingMenu(screenY, "", colors.Editor, menuArea.Value);
                    }
                    else
                    {
                        // Fill empty lines with editor background
                        _screen.FillRect(new Rect(_bounds.X, screenY, _bounds.Width, 1), ' ', colors.Editor);
                    }
                }
            }
        }
        
        private void DrawLineAvoidingMenu(int screenY, string text, ColorPair color, Rect menuArea)
        {
            // Draw the part before the menu
            if (_bounds.X < menuArea.X)
            {
                int leftWidth = Math.Min(menuArea.X - _bounds.X, _bounds.Width);
                _screen.FillRect(new Rect(_bounds.X, screenY, leftWidth, 1), ' ', color);
                
                if (text.Length > 0)
                {
                    string leftText = text.Length > leftWidth ? text.Substring(0, leftWidth) : text;
                    _screen.WriteAt(_bounds.X, screenY, leftText, color);
                }
            }
            
            // Draw the part after the menu
            int menuRightEdge = menuArea.X + menuArea.Width;
            int textBoxRightEdge = _bounds.X + _bounds.Width;
            if (menuRightEdge < textBoxRightEdge)
            {
                int rightX = Math.Max(menuRightEdge, _bounds.X);
                int rightWidth = textBoxRightEdge - rightX;
                
                if (rightWidth > 0)
                {
                    _screen.FillRect(new Rect(rightX, screenY, rightWidth, 1), ' ', color);
                    
                    if (text.Length > 0)
                    {
                        int textOffset = rightX - _bounds.X;
                        if (textOffset < text.Length)
                        {
                            string rightText = text.Substring(textOffset);
                            if (rightText.Length > rightWidth)
                                rightText = rightText.Substring(0, rightWidth);
                            _screen.WriteAt(rightX, screenY, rightText, color);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Update cursor visibility and position
        /// </summary>
        public void UpdateCursor()
        {
            if (!_focused) return;
            
            if (_cursorLine >= _scrollTop && _cursorLine < _scrollTop + _bounds.Height)
            {
                int screenY = _cursorLine - _scrollTop + _bounds.Y;
                int screenX = Math.Min(_cursorCol + _bounds.X, _bounds.X + _bounds.Width - 1);
                Console.SetCursorPosition(screenX, screenY);
                Console.CursorVisible = true;
            }
            else
            {
                Console.CursorVisible = false;
            }
        }
        
        private void MoveCursor(int deltaX, int deltaY)
        {
            _cursorLine = Math.Max(0, Math.Min(_lines.Count - 1, _cursorLine + deltaY));
            _cursorCol = Math.Max(0, Math.Min(_lines[_cursorLine].Length, _cursorCol + deltaX));
            
            // Adjust scroll position
            if (_cursorLine < _scrollTop)
                _scrollTop = _cursorLine;
            else if (_cursorLine >= _scrollTop + _bounds.Height)
                _scrollTop = _cursorLine - _bounds.Height + 1;
                
            OnCursorPositionChanged();
        }
        
        private void InsertText(string text)
        {
            string line = _lines[_cursorLine];
            _lines[_cursorLine] = line.Insert(_cursorCol, text);
            _cursorCol += text.Length;
            _modified = true;
            OnTextChanged();
            OnCursorPositionChanged();
        }
        
        private void HandleBackspace()
        {
            if (_cursorCol > 0)
            {
                string line = _lines[_cursorLine];
                _lines[_cursorLine] = line.Remove(_cursorCol - 1, 1);
                _cursorCol--;
                _modified = true;
                OnTextChanged();
                OnCursorPositionChanged();
            }
            else if (_cursorLine > 0)
            {
                // Join with previous line
                _cursorCol = _lines[_cursorLine - 1].Length;
                _lines[_cursorLine - 1] += _lines[_cursorLine];
                _lines.RemoveAt(_cursorLine);
                _cursorLine--;
                _modified = true;
                OnTextChanged();
                OnCursorPositionChanged();
            }
        }
        
        private void HandleDelete()
        {
            string line = _lines[_cursorLine];
            if (_cursorCol < line.Length)
            {
                _lines[_cursorLine] = line.Remove(_cursorCol, 1);
                _modified = true;
                OnTextChanged();
            }
            else if (_cursorLine < _lines.Count - 1)
            {
                // Join with next line
                _lines[_cursorLine] += _lines[_cursorLine + 1];
                _lines.RemoveAt(_cursorLine + 1);
                _modified = true;
                OnTextChanged();
            }
        }
        
        private void HandleEnter()
        {
            string line = _lines[_cursorLine];
            string leftPart = line.Substring(0, _cursorCol);
            string rightPart = line.Substring(_cursorCol);
            
            _lines[_cursorLine] = leftPart;
            _lines.Insert(_cursorLine + 1, rightPart);
            _cursorLine++;
            _cursorCol = 0;
            _modified = true;
            OnTextChanged();
            OnCursorPositionChanged();
        }
        
        private void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }
        
        private void OnCursorPositionChanged()
        {
            CursorPositionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
