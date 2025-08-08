using System;
using DotNetVision.Core;

namespace DotNetVision.Controls
{
    /// <summary>
    /// A container control that can hold other controls
    /// </summary>
    public class Panel : Control
    {
        public BoxStyle BorderStyle { get; set; } = BoxStyle.Single;
        public bool ShowBorder { get; set; } = true;

        public Panel() : base()
        {
            BackColor = ColorPair.Default;
        }

        public Panel(int x, int y, int width, int height) : base(x, y, width, height)
        {
            BackColor = ColorPair.Default;
        }

        protected override void DoPaint(Rect screenBounds)
        {
            // Fill background
            Screen.Instance.FillRect(screenBounds, ' ', BackColor);

            // Draw border if enabled
            if (ShowBorder && screenBounds.Width > 2 && screenBounds.Height > 2)
            {
                Screen.Instance.DrawBox(screenBounds, ForeColor, BorderStyle);
            }
        }

        public override void AddChild(Control child)
        {
            base.AddChild(child);
            
            // Adjust child position if we have a border
            if (ShowBorder && child.Parent == this)
            {
                // Ensure child is within the panel's client area
                var clientRect = GetClientRect();
                if (child.X < 0) child.X = 0;
                if (child.Y < 0) child.Y = 0;
                if (child.X + child.Width > clientRect.Width)
                    child.Width = Math.Max(1, clientRect.Width - child.X);
                if (child.Y + child.Height > clientRect.Height)
                    child.Height = Math.Max(1, clientRect.Height - child.Y);
            }
        }

        /// <summary>
        /// Get the client rectangle (interior area excluding border)
        /// </summary>
        public Rect GetClientRect()
        {
            if (ShowBorder && Width > 2 && Height > 2)
            {
                return new Rect(1, 1, Width - 2, Height - 2);
            }
            return new Rect(0, 0, Width, Height);
        }
    }

    /// <summary>
    /// A window is a special panel with title bar and window management
    /// </summary>
    public class Window : Panel
    {
        private readonly FocusManager _focusManager;
        public string Title { get; set; } = string.Empty;
        public bool CanMove { get; set; } = true;
        public bool CanResize { get; set; } = false;

        public Window() : base()
        {
            ShowBorder = true;
            BorderStyle = BoxStyle.Double;
            _focusManager = new FocusManager(this);
        }

        public Window(int x, int y, int width, int height, string title = "") : base(x, y, width, height)
        {
            Title = title;
            ShowBorder = true;
            BorderStyle = BoxStyle.Double;
            _focusManager = new FocusManager(this);
        }

        protected override void DoPaint(Rect screenBounds)
        {
            // Draw the panel first
            base.DoPaint(screenBounds);

            // Draw title bar if we have a title
            if (!string.IsNullOrEmpty(Title) && screenBounds.Width > 4)
            {
                var titleText = Title;
                var maxTitleLength = screenBounds.Width - 4; // Leave space for borders

                if (titleText.Length > maxTitleLength)
                    titleText = titleText.Substring(0, maxTitleLength - 3) + "...";

                // Draw title centered in title bar
                var titleWithSpaces = $" {titleText} ";
                Screen.Instance.WriteAt(screenBounds.X + 2, screenBounds.Y, titleWithSpaces, ForeColor);
            }
        }

        public override bool OnKeyPressed(ConsoleKeyInfo keyInfo)
        {
            // Handle focus navigation first
            if (keyInfo.Key == ConsoleKey.Tab)
            {
                if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    _focusManager.FocusPrevious();
                else
                    _focusManager.FocusNext();
                
                return true;
            }

            // Send input to focused control
            var focusedControl = _focusManager.FocusedControl as Control;
            if (focusedControl != null && focusedControl.OnKeyPressed(keyInfo))
            {
                return true;
            }

            // Handle window-specific keys
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                // Close window with Escape key
                Parent?.RemoveChild(this);
                return true;
            }

            return base.OnKeyPressed(keyInfo);
        }

        /// <summary>
        /// Get the client rectangle (interior area excluding title bar and border)
        /// </summary>
        public new Rect GetClientRect()
        {
            var baseClient = base.GetClientRect();
            
            // If we have a title, reduce height by 1 for title bar
            if (!string.IsNullOrEmpty(Title) && baseClient.Height > 1)
            {
                return new Rect(baseClient.X, baseClient.Y + 1, baseClient.Width, baseClient.Height - 1);
            }
            
            return baseClient;
        }
    }
}
