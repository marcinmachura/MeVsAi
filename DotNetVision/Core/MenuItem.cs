using System;
using System.Collections.Generic;

namespace DotNetVision.Core
{
    /// <summary>
    /// Represents a menu item that can be displayed in a menu bar
    /// </summary>
    public class MenuItem
    {
        public string Text { get; set; } = "";
        public List<MenuItem> SubItems { get; set; } = new();
        public bool IsSeparator { get; set; } = false;
        public bool IsEnabled { get; set; } = true;
        public string? ShortcutKey { get; set; }
        
        /// <summary>
        /// Event fired when this menu item is selected
        /// </summary>
        public event EventHandler<MenuItemEventArgs>? Click;
        
        public MenuItem(string text)
        {
            Text = text;
        }
        
        public MenuItem(string text, params MenuItem[] subItems)
        {
            Text = text;
            SubItems.AddRange(subItems);
        }
        
        /// <summary>
        /// Create a separator menu item
        /// </summary>
        public static MenuItem Separator => new("") { IsSeparator = true };
        
        /// <summary>
        /// Add a sub-item to this menu
        /// </summary>
        public MenuItem AddItem(string text, EventHandler<MenuItemEventArgs>? clickHandler = null)
        {
            var item = new MenuItem(text);
            if (clickHandler != null)
                item.Click += clickHandler;
            SubItems.Add(item);
            return item;
        }
        
        /// <summary>
        /// Trigger the click event
        /// </summary>
        internal void OnClick()
        {
            if (IsEnabled && !IsSeparator)
            {
                Click?.Invoke(this, new MenuItemEventArgs(this));
            }
        }
    }
    
    /// <summary>
    /// Event arguments for menu item events
    /// </summary>
    public class MenuItemEventArgs : EventArgs
    {
        public MenuItem MenuItem { get; }
        
        public MenuItemEventArgs(MenuItem menuItem)
        {
            MenuItem = menuItem;
        }
    }
}
