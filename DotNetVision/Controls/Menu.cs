using System;
using System.Collections.Generic;
using System.Linq;
using DotNetVision.Core;

namespace DotNetVision.Controls
{
    /// <summary>
    /// A menu item that can contain text, shortcut, and action
    /// </summary>
    public class MenuItem
    {
        public string Text { get; set; } = string.Empty;
        public string Shortcut { get; set; } = string.Empty;
        public Action? Action { get; set; }
        public List<MenuItem> SubItems { get; set; } = new();
        public bool IsSeparator { get; set; } = false;
        public bool Enabled { get; set; } = true;

        public MenuItem() { }

        public MenuItem(string text, Action? action = null, string shortcut = "")
        {
            Text = text;
            Action = action;
            Shortcut = shortcut;
        }

        public static MenuItem Separator() => new() { IsSeparator = true };

        public bool HasSubItems => SubItems.Count > 0;
    }

    /// <summary>
    /// Top-level menu bar that spans the width of the screen
    /// </summary>
    public class MenuBar : Control
    {
        private readonly List<MenuItem> _menuItems = new();
        private int _selectedIndex = -1;
        private bool _isOpen = false;
        private MenuItem? _openMenu = null;
        private int _selectedSubIndex = -1;

        public event EventHandler<MenuItemEventArgs>? MenuItemSelected;

        public MenuBar() : base(0, 0, 80, 1)
        {
            BackColor = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray);
            ForeColor = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray);
        }

        public void AddMenu(MenuItem menuItem)
        {
            _menuItems.Add(menuItem);
            Invalidate();
        }

        public void AddMenu(string text, params MenuItem[] subItems)
        {
            var menu = new MenuItem(text);
            menu.SubItems.AddRange(subItems);
            AddMenu(menu);
        }

        protected override void DoPaint(Rect screenBounds)
        {
            // Fill menu bar background
            Screen.FillRect(screenBounds, ' ', BackColor);

            // Draw menu items
            int x = 1;
            for (int i = 0; i < _menuItems.Count; i++)
            {
                var item = _menuItems[i];
                var isSelected = i == _selectedIndex;
                var colors = isSelected ? 
                    new ColorPair(ConsoleColor.White, ConsoleColor.Blue) : 
                    BackColor;

                var menuText = $" {item.Text} ";
                Screen.WriteAt(screenBounds.X + x, screenBounds.Y, menuText, colors);
                x += menuText.Length;
            }

            // Draw open submenu if any
            if (_isOpen && _openMenu != null && _selectedIndex >= 0)
            {
                DrawSubMenu(_openMenu, GetMenuPosition(_selectedIndex));
            }
        }

        private void DrawSubMenu(MenuItem menu, Point position)
        {
            if (!menu.HasSubItems) return;

            // Calculate submenu dimensions - need to account for border, padding, and spacing
            var maxTextWidth = menu.SubItems.Where(item => !item.IsSeparator)
                .Max(item => item.Text.Length);
            var maxShortcutWidth = menu.SubItems.Where(item => !item.IsSeparator && !string.IsNullOrEmpty(item.Shortcut))
                .DefaultIfEmpty(new MenuItem())
                .Max(item => item.Shortcut.Length);
            
            // Width = border(2) + left padding(1) + text + spacing(1) + shortcut + right padding(1)
            var maxWidth = Math.Max(12, 2 + 1 + maxTextWidth + (maxShortcutWidth > 0 ? 1 + maxShortcutWidth : 0) + 1);
            var height = menu.SubItems.Count + 2; // +2 for borders

            // Adjust position to stay on screen
            if (position.X + maxWidth >= Screen.Width)
                position.X = Screen.Width - maxWidth - 1;
            if (position.Y + height >= Screen.Height)
                position.Y = Screen.Height - height - 1;

            var menuRect = new Rect(position.X, position.Y, maxWidth, height);

            // Draw submenu background and border
            Screen.FillRect(menuRect, ' ', new ColorPair(ConsoleColor.Black, ConsoleColor.Gray));
            Screen.DrawBox(menuRect, new ColorPair(ConsoleColor.Black, ConsoleColor.Gray), BoxStyle.Single);

            // Draw submenu items
            int y = 1;
            for (int i = 0; i < menu.SubItems.Count; i++)
            {
                var subItem = menu.SubItems[i];
                var itemY = position.Y + y++;

                if (subItem.IsSeparator)
                {
                    // Draw separator line
                    var sepLine = "├" + new string('─', maxWidth - 2) + "┤";
                    Screen.WriteAt(position.X, itemY, sepLine, 
                        new ColorPair(ConsoleColor.Black, ConsoleColor.Gray));
                }
                else
                {
                    var isSelected = i == _selectedSubIndex;
                    var colors = isSelected ?
                        new ColorPair(ConsoleColor.White, ConsoleColor.Blue) :
                        subItem.Enabled ?
                            new ColorPair(ConsoleColor.Black, ConsoleColor.Gray) :
                            new ColorPair(ConsoleColor.DarkGray, ConsoleColor.Gray);

                    var itemText = $" {subItem.Text}";
                    var shortcutText = string.IsNullOrEmpty(subItem.Shortcut) ? "" : subItem.Shortcut;
                    
                    // Calculate exact spacing needed
                    var contentWidth = maxWidth - 2; // Available width inside borders
                    var rightPadding = string.IsNullOrEmpty(shortcutText) ? 1 : shortcutText.Length + 1;
                    var spacesNeeded = contentWidth - itemText.Length - rightPadding;
                    
                    var paddedText = itemText + new string(' ', Math.Max(0, spacesNeeded)) + shortcutText + " ";

                    Screen.WriteAt(position.X + 1, itemY, paddedText, colors);
                }
            }
        }

        private Point GetMenuPosition(int menuIndex)
        {
            int x = 1;
            for (int i = 0; i < menuIndex && i < _menuItems.Count; i++)
            {
                x += $" {_menuItems[i].Text} ".Length;
            }
            return new Point(x, 1); // Below the menu bar
        }

        public override bool OnKeyPressed(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.F10 || 
                (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt) && !_isOpen))
            {
                // Activate menu bar
                _selectedIndex = _selectedIndex < 0 ? 0 : _selectedIndex;
                _isOpen = true;
                Invalidate();
                return true;
            }

            if (!_isOpen) return false;

            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    CloseMenu();
                    return true;

                case ConsoleKey.LeftArrow:
                    if (_selectedIndex > 0)
                    {
                        _selectedIndex--;
                        _selectedSubIndex = -1;
                        _openMenu = _menuItems[_selectedIndex];
                        Invalidate();
                    }
                    return true;

                case ConsoleKey.RightArrow:
                    if (_selectedIndex < _menuItems.Count - 1)
                    {
                        _selectedIndex++;
                        _selectedSubIndex = -1;
                        _openMenu = _menuItems[_selectedIndex];
                        Invalidate();
                    }
                    return true;

                case ConsoleKey.DownArrow:
                    if (_openMenu == null && _selectedIndex >= 0)
                    {
                        _openMenu = _menuItems[_selectedIndex];
                        _selectedSubIndex = 0;
                        Invalidate();
                    }
                    else if (_openMenu != null && _selectedSubIndex < _openMenu.SubItems.Count - 1)
                    {
                        do
                        {
                            _selectedSubIndex++;
                        } while (_selectedSubIndex < _openMenu.SubItems.Count && 
                                _openMenu.SubItems[_selectedSubIndex].IsSeparator);
                        
                        Invalidate();
                    }
                    return true;

                case ConsoleKey.UpArrow:
                    if (_openMenu != null && _selectedSubIndex > 0)
                    {
                        do
                        {
                            _selectedSubIndex--;
                        } while (_selectedSubIndex >= 0 && 
                                _openMenu.SubItems[_selectedSubIndex].IsSeparator);
                        
                        if (_selectedSubIndex < 0) _selectedSubIndex = 0;
                        Invalidate();
                    }
                    return true;

                case ConsoleKey.Enter:
                    if (_openMenu != null && _selectedSubIndex >= 0 && 
                        _selectedSubIndex < _openMenu.SubItems.Count)
                    {
                        var selectedItem = _openMenu.SubItems[_selectedSubIndex];
                        if (selectedItem.Enabled && !selectedItem.IsSeparator)
                        {
                            ExecuteMenuItem(selectedItem);
                            CloseMenu();
                        }
                    }
                    return true;
            }

            // Handle Alt+Letter shortcuts
            if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                var letter = char.ToUpper(keyInfo.KeyChar);
                for (int i = 0; i < _menuItems.Count; i++)
                {
                    var menuText = _menuItems[i].Text.ToUpper();
                    if (menuText.Length > 0 && menuText[0] == letter)
                    {
                        _selectedIndex = i;
                        _openMenu = _menuItems[i];
                        _selectedSubIndex = 0;
                        _isOpen = true;
                        Invalidate();
                        return true;
                    }
                }
            }

            return false;
        }

        private void CloseMenu()
        {
            _isOpen = false;
            _openMenu = null;
            _selectedIndex = -1;
            _selectedSubIndex = -1;
            Invalidate();
        }

        private void ExecuteMenuItem(MenuItem item)
        {
            item.Action?.Invoke();
            MenuItemSelected?.Invoke(this, new MenuItemEventArgs(item));
        }
    }

    /// <summary>
    /// Bottom status bar for displaying application status
    /// </summary>
    public class StatusBar : Control
    {
        private string _leftText = string.Empty;
        private string _centerText = string.Empty;
        private string _rightText = string.Empty;
        private readonly List<StatusPanel> _panels = new();

        public StatusBar() : base(0, 24, 80, 1)
        {
            BackColor = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray);
            ForeColor = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray);
        }

        public string LeftText
        {
            get => _leftText;
            set
            {
                _leftText = value ?? string.Empty;
                Invalidate();
            }
        }

        public string CenterText
        {
            get => _centerText;
            set
            {
                _centerText = value ?? string.Empty;
                Invalidate();
            }
        }

        public string RightText
        {
            get => _rightText;
            set
            {
                _rightText = value ?? string.Empty;
                Invalidate();
            }
        }

        public void AddPanel(string text, int width = -1)
        {
            _panels.Add(new StatusPanel { Text = text, Width = width });
            Invalidate();
        }

        public void SetPanelText(int index, string text)
        {
            if (index >= 0 && index < _panels.Count)
            {
                _panels[index].Text = text;
                Invalidate();
            }
        }

        protected override void DoPaint(Rect screenBounds)
        {
            // Fill status bar background
            Screen.FillRect(screenBounds, ' ', BackColor);

            if (_panels.Count > 0)
            {
                // Draw panels
                int x = 0;
                foreach (var panel in _panels)
                {
                    var width = panel.Width > 0 ? panel.Width : 
                        Math.Min(panel.Text.Length + 2, screenBounds.Width - x);
                    
                    if (x + width > screenBounds.Width) break;

                    var panelText = panel.Text.Length > width - 2 ? 
                        panel.Text.Substring(0, width - 2) : panel.Text;
                    panelText = $" {panelText}".PadRight(width);

                    Screen.WriteAt(screenBounds.X + x, screenBounds.Y, panelText, BackColor);
                    
                    // Draw separator
                    if (x + width < screenBounds.Width)
                    {
                        Screen.WriteAt(screenBounds.X + x + width, screenBounds.Y, "│", BackColor);
                    }
                    
                    x += width + 1;
                }
            }
            else
            {
                // Simple three-part layout
                Screen.WriteAt(screenBounds.X + 1, screenBounds.Y, _leftText, BackColor);

                if (!string.IsNullOrEmpty(_centerText))
                {
                    var centerX = (screenBounds.Width - _centerText.Length) / 2;
                    Screen.WriteAt(screenBounds.X + centerX, screenBounds.Y, _centerText, BackColor);
                }

                if (!string.IsNullOrEmpty(_rightText))
                {
                    var rightX = screenBounds.Width - _rightText.Length - 1;
                    Screen.WriteAt(screenBounds.X + rightX, screenBounds.Y, _rightText, BackColor);
                }
            }
        }
    }

    public class StatusPanel
    {
        public string Text { get; set; } = string.Empty;
        public int Width { get; set; } = -1; // -1 = auto-size
    }

    public class MenuItemEventArgs : EventArgs
    {
        public MenuItem MenuItem { get; }

        public MenuItemEventArgs(MenuItem menuItem)
        {
            MenuItem = menuItem;
        }
    }
}
