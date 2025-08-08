using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetVision.Core
{
    /// <summary>
    /// Manages menu bar rendering and input handling
    /// </summary>
    public class MenuManager
    {
        private readonly IScreen _screen;
        private readonly AppConfiguration _config;
        private List<MenuItem> _menuItems = new();
        private bool _showingMenu = false;
        private bool _showingSubmenu = false;
        private int _selectedMenu = 0;
        private int _selectedSubmenu = 0;
        
        public MenuManager(IScreen screen, AppConfiguration config)
        {
            _screen = screen;
            _config = config;
        }
        
        /// <summary>
        /// Set the menu items for the menu bar
        /// </summary>
        public void SetMenuItems(params MenuItem[] menuItems)
        {
            _menuItems = menuItems.ToList();
        }
        
        /// <summary>
        /// Check if the menu system is currently active
        /// </summary>
        public bool IsMenuActive => _showingMenu;
        
        /// <summary>
        /// Get the area that should be avoided when drawing other controls
        /// </summary>
        public Rect? GetMenuArea()
        {
            if (!_showingMenu || !_showingSubmenu || _selectedMenu >= _menuItems.Count)
                return null;
                
            var currentMenu = _menuItems[_selectedMenu];
            if (currentMenu.SubItems.Count == 0)
                return null;
                
            // Calculate menu position (same logic as DrawSubmenu)
            int menuX = 1;
            for (int i = 0; i < _selectedMenu; i++)
            {
                menuX += _menuItems[i].Text.Length + 2;
            }
            int menuY = 1; // Menu drops down from line 0
            
            // Calculate menu dimensions
            int maxItemLength = currentMenu.SubItems.Max(item => item.Text.Length);
            int menuWidth = maxItemLength + 4;
            int menuHeight = currentMenu.SubItems.Count + 2;
            
            return new Rect(menuX, menuY, menuWidth, menuHeight);
        }
        
        /// <summary>
        /// Handle keyboard input for the menu system
        /// </summary>
        public bool HandleInput(ConsoleKeyInfo key)
        {
            if (!_showingMenu && key.Key == ConsoleKey.F10)
            {
                _showingMenu = true;
                _showingSubmenu = false;
                _selectedMenu = 0;
                return true;
            }
            
            if (!_showingMenu) return false;
            
            switch (key.Key)
            {
                case ConsoleKey.Escape:
                    if (_showingSubmenu)
                    {
                        _showingSubmenu = false;
                    }
                    else
                    {
                        _showingMenu = false;
                    }
                    return true;

                case ConsoleKey.LeftArrow:
                    if (_showingSubmenu)
                    {
                        _showingSubmenu = false;
                    }
                    if (_selectedMenu > 0) _selectedMenu--;
                    return true;

                case ConsoleKey.RightArrow:
                    if (_showingSubmenu)
                    {
                        _showingSubmenu = false;
                    }
                    if (_selectedMenu < _menuItems.Count - 1) _selectedMenu++;
                    return true;

                case ConsoleKey.DownArrow:
                    if (!_showingSubmenu)
                    {
                        if (_selectedMenu < _menuItems.Count && _menuItems[_selectedMenu].SubItems.Count > 0)
                        {
                            _showingSubmenu = true;
                            _selectedSubmenu = 0;
                        }
                    }
                    else
                    {
                        var currentMenu = _menuItems[_selectedMenu];
                        if (_selectedSubmenu < currentMenu.SubItems.Count - 1)
                            _selectedSubmenu++;
                    }
                    return true;

                case ConsoleKey.UpArrow:
                    if (_showingSubmenu)
                    {
                        if (_selectedSubmenu > 0)
                            _selectedSubmenu--;
                    }
                    return true;

                case ConsoleKey.Enter:
                    if (!_showingSubmenu)
                    {
                        if (_selectedMenu < _menuItems.Count && _menuItems[_selectedMenu].SubItems.Count > 0)
                        {
                            _showingSubmenu = true;
                            _selectedSubmenu = 0;
                        }
                        else
                        {
                            // Execute top-level menu item
                            if (_selectedMenu < _menuItems.Count)
                            {
                                _menuItems[_selectedMenu].OnClick();
                                _showingMenu = false;
                            }
                        }
                    }
                    else
                    {
                        // Execute submenu item
                        var currentMenu = _menuItems[_selectedMenu];
                        if (_selectedSubmenu < currentMenu.SubItems.Count)
                        {
                            currentMenu.SubItems[_selectedSubmenu].OnClick();
                            _showingMenu = false;
                            _showingSubmenu = false;
                        }
                    }
                    return true;

                default:
                    return true;
            }
        }
        
        /// <summary>
        /// Render the menu bar and any open submenus
        /// </summary>
        public void Render()
        {
            var colors = _config.GetColorSet();
            
            // Draw menu bar (line 0 - no title bar anymore)
            _screen.FillRect(new Rect(0, 0, Console.WindowWidth, 1), ' ', colors.MenuBar);
            
            int x = 1;
            for (int i = 0; i < _menuItems.Count; i++)
            {
                var color = (_showingMenu && i == _selectedMenu) ? colors.MenuBarSelected : colors.MenuBar;
                _screen.WriteAt(x, 0, $" {_menuItems[i].Text} ", color);
                x += _menuItems[i].Text.Length + 2;
            }
            
            // Draw submenu if showing
            if (_showingMenu && _showingSubmenu && _selectedMenu < _menuItems.Count)
            {
                var currentMenu = _menuItems[_selectedMenu];
                if (currentMenu.SubItems.Count > 0)
                {
                    DrawSubmenu(currentMenu, _selectedMenu);
                }
            }
        }
        
        private void DrawSubmenu(MenuItem parentMenu, int parentIndex)
        {
            var colors = _config.GetColorSet();
            
            // Calculate menu position
            int menuX = 1;
            for (int i = 0; i < parentIndex; i++)
            {
                menuX += _menuItems[i].Text.Length + 2;
            }
            int menuY = 1; // Menu drops down from line 0
            
            // Calculate menu dimensions
            int maxItemLength = parentMenu.SubItems.Max(item => item.Text.Length);
            int menuWidth = maxItemLength + 4; // 2 spaces padding on each side
            int menuHeight = parentMenu.SubItems.Count + 2; // border top and bottom
            
            // Draw menu box
            _screen.DrawBox(new Rect(menuX, menuY, menuWidth, menuHeight), colors.Dialog, _config.GetBoxStyle());
            _screen.FillRect(new Rect(menuX + 1, menuY + 1, menuWidth - 2, menuHeight - 2), ' ', colors.Dialog);
            
            // Draw menu items
            for (int i = 0; i < parentMenu.SubItems.Count; i++)
            {
                var item = parentMenu.SubItems[i];
                var color = (i == _selectedSubmenu) ? colors.DialogSelected : colors.Dialog;
                
                if (item.IsSeparator)
                {
                    // Draw separator line
                    _screen.WriteAt(menuX + 1, menuY + 1 + i, new string('─', menuWidth - 2), colors.Dialog);
                }
                else
                {
                    string menuItem = $" {item.Text.PadRight(maxItemLength)} ";
                    if (!item.IsEnabled)
                    {
                        // Draw disabled items in dimmed colors
                        color = new ColorPair(ConsoleColor.DarkGray, color.Background);
                    }
                    _screen.WriteAt(menuX + 1, menuY + 1 + i, menuItem, color);
                }
            }
        }
        
        /// <summary>
        /// Force close the menu system
        /// </summary>
        public void CloseMenu()
        {
            _showingMenu = false;
            _showingSubmenu = false;
        }
    }
}
