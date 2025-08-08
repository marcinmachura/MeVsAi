using System;
using System.Collections.Generic;
using System.Linq;
using DotNetVision.Core;
using DotNetVision.Controls;

namespace DotNetVision.Forms
{
    /// <summary>
    /// Main application class that manages the UI event loop
    /// </summary>
    public class Application
    {
        private static Application? _instance;
        private readonly IScreen _screen;
        private readonly List<Window> _windows = new();
        private Window? _activeWindow;
        private bool _running = false;
        private bool _needsRepaint = false;
        private MenuBar? _menuBar = null;
        private StatusBar? _statusBar = null;
        private AppConfiguration _configuration = new();

        public static Application Instance 
        {
            get
            {
                if (_instance == null)
                {
                    // This provides a default for convenience but allows injection for tests
                    _instance = new Application(new BufferedScreen());
                }
                return _instance;
            }
        }

        /// <summary>
        /// Get or set the application configuration
        /// </summary>
        public AppConfiguration Configuration
        {
            get => _configuration;
            set
            {
                _configuration = value ?? new AppConfiguration();
                RequestRepaint(); // Repaint with new configuration
            }
        }

        // Internal constructor for dependency injection
        internal Application(IScreen screen)
        {
            _screen = screen;
            Screen.Instance = _screen; // Set the static instance for global access
            _instance = this;
        }

        /// <summary>
        /// Run the application with the specified main window
        /// </summary>
        public void Run(Window mainWindow)
        {
            Logger.Instance.LogEntry("Application", "Run", $"MainWindow: {mainWindow.Title}");
            
            if (_running)
            {
                var ex = new InvalidOperationException("Application is already running");
                Logger.Instance.LogError("Application", "Run", ex);
                throw ex;
            }

            Logger.Instance.Log("Application", "Run", "Initializing screen");
            _screen.Initialize();
            _running = true;

            try
            {
                Logger.Instance.Log("Application", "Run", "Adding main window");
                AddWindow(mainWindow);
                SetActiveWindow(mainWindow);

                Logger.Instance.Log("Application", "Run", "Starting main loop");
                MainLoop();
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError("Application", "Run", ex);
                // Make sure console is restored before showing error
                _screen.Cleanup();
                Console.WriteLine($"Application error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                throw; // Re-throw for caller
            }
            finally
            {
                Logger.Instance.Log("Application", "Run", "Cleaning up");
                _screen.Cleanup();
                _running = false;
                Logger.Instance.LogExit("Application", "Run");
            }
        }

        /// <summary>
        /// Exit the application
        /// </summary>
        public void Exit()
        {
            _running = false;
        }

        /// <summary>
        /// Add a window to the application
        /// </summary>
        public void AddWindow(Window window)
        {
            if (!_windows.Contains(window))
            {
                _windows.Add(window);
                
                if (_activeWindow == null)
                    SetActiveWindow(window);
                
                RequestRepaint();
            }
        }

        /// <summary>
        /// Remove a window from the application
        /// </summary>
        public bool RemoveWindow(Window window)
        {
            if (_windows.Remove(window))
            {
                if (_activeWindow == window)
                {
                    _activeWindow = _windows.Count > 0 ? _windows[^1] : null;
                }
                
                RequestRepaint();
                
                // Exit if no windows remain
                if (_windows.Count == 0)
                    Exit();
                
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set the active window
        /// </summary>
        public void SetActiveWindow(Window? window)
        {
            if (_activeWindow != window)
            {
                _activeWindow = window;
                
                // Move active window to top
                if (window != null && _windows.Contains(window))
                {
                    _windows.Remove(window);
                    _windows.Add(window);
                }
                
                RequestRepaint();
            }
        }

        public Window? ActiveWindow => _activeWindow;
        public IReadOnlyList<Window> Windows => _windows.AsReadOnly();

        /// <summary>
        /// Get or set the application menu bar
        /// </summary>
        public MenuBar? MenuBar
        {
            get => _menuBar;
            set
            {
                _menuBar = value;
                if (_menuBar != null)
                {
                    _menuBar.Width = _screen.Width;
                    _menuBar.X = 0;
                    _menuBar.Y = 0;
                }
                AdjustWindowPositions();
                RequestRepaint();
            }
        }

        /// <summary>
        /// Get or set the application status bar
        /// </summary>
        public StatusBar? StatusBar
        {
            get => _statusBar;
            set
            {
                _statusBar = value;
                if (_statusBar != null)
                {
                    _statusBar.Width = _screen.Width;
                    _statusBar.X = 0;
                    _statusBar.Y = _screen.Height - 1;
                }
                AdjustWindowPositions();
                RequestRepaint();
            }
        }

        /// <summary>
        /// Get the available client area (excluding menu bar and status bar)
        /// </summary>
        public Rect ClientArea
        {
            get
            {
                var top = _menuBar != null ? 1 : 0;
                var bottom = _statusBar != null ? 1 : 0;
                return new Rect(0, top, _screen.Width, _screen.Height - top - bottom);
            }
        }

        private void AdjustWindowPositions()
        {
            // Update menu bar and status bar positions when screen size changes
            if (_menuBar != null)
            {
                _menuBar.Width = _screen.Width;
                _menuBar.Y = 0;
            }
            
            if (_statusBar != null)
            {
                _statusBar.Width = _screen.Width;
                _statusBar.Y = _screen.Height - 1;
            }
            
            // Ensure windows stay within client area
            var clientArea = ClientArea;
            foreach (var window in _windows)
            {
                if (window.Y < clientArea.Y)
                    window.Y = clientArea.Y;
                if (window.Y + window.Height > clientArea.Y + clientArea.Height)
                    window.Y = Math.Max(clientArea.Y, clientArea.Y + clientArea.Height - window.Height);
            }
        }

        private void MainLoop()
        {
            Logger.Instance.LogEntry("Application", "MainLoop");
            _needsRepaint = true; // Initial paint

            int frameCount = 0;
            while (_running)
            {
                frameCount++;
                if (frameCount % 100 == 0) // Log every 100 frames to avoid spam
                {
                    Logger.Instance.Log("Application", "MainLoop", $"Frame {frameCount}");
                }

                // Handle input
                var keyInfo = Input.ReadKey();
                if (keyInfo.HasValue)
                {
                    Logger.Instance.Log("Application", "MainLoop", $"Key pressed: {keyInfo.Value.Key}");
                    HandleInput(keyInfo.Value);
                }

                // Paint only if needed
                if (_needsRepaint)
                {
                    Logger.Instance.Log("Application", "MainLoop", "Repainting screen");
                    Paint();
                    _needsRepaint = false;
                }

                // Small delay to prevent busy waiting
                System.Threading.Thread.Sleep(16); // ~60 FPS max, but only when needed
            }
            
            Logger.Instance.LogExit("Application", "MainLoop", $"Total frames: {frameCount}");
        }

        private void HandleInput(ConsoleKeyInfo keyInfo)
        {
            // Let menu bar handle input first (for F10, Alt+keys, etc.)
            if (_menuBar != null && _menuBar.OnKeyPressed(keyInfo))
            {
                RequestRepaint();
                return;
            }

            // Global shortcuts
            if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.F4: // Alt+F4 to close active window
                        if (_activeWindow != null)
                        {
                            RemoveWindow(_activeWindow);
                            return;
                        }
                        break;
                    
                    case ConsoleKey.Tab: // Alt+Tab to switch windows
                        if (_windows.Count > 1)
                        {
                            var currentIndex = _activeWindow != null ? _windows.IndexOf(_activeWindow) : -1;
                            var nextIndex = (currentIndex + 1) % _windows.Count;
                            SetActiveWindow(_windows[nextIndex]);
                            return;
                        }
                        break;
                }
            }

            // Send input to active window
            if (_activeWindow != null)
            {
                var handled = ProcessInput(_activeWindow, keyInfo);
                // Always repaint after input processing to update visual states
                RequestRepaint();
            }
        }

        private bool ProcessInput(Control control, ConsoleKeyInfo keyInfo)
        {
            // First, try to find a focused control
            var focusedControl = FindFocusedControl(control);
            if (focusedControl != null && focusedControl.OnKeyPressed(keyInfo))
                return true;

            // If no focused control handled it, send to the control itself
            return control.OnKeyPressed(keyInfo);
        }

        private Control? FindFocusedControl(Control control)
        {
            // Look for controls with focus (TextBox, Button, etc.)
            if (control is TextBox textBox && textBox.HasFocus)
                return textBox;
            
            if (control is Button button && button.HasFocus)
                return button;

            // Check children
            foreach (var child in control.Children)
            {
                var focused = FindFocusedControl(child);
                if (focused != null)
                    return focused;
            }

            return null;
        }

        private void Paint()
        {
            using var timer = new LogTimer("Application", "Paint");
            
            // Clear the back buffer
            _screen.Clear();

            // Paint status bar first (bottom layer - no submenus to worry about)
            if (_statusBar != null)
            {
                Logger.Instance.Log("Application", "Paint", "Painting status bar");
                _statusBar.OnPaint(new Rect(_statusBar.X, _statusBar.Y, _statusBar.Width, _statusBar.Height));
            }

            // Paint all windows in the client area (middle layer)
            var clientArea = ClientArea;
            Logger.Instance.Log("Application", "Paint", $"Painting {_windows.Count} windows in client area {clientArea}");
            foreach (var window in _windows)
            {
                // Clip window to client area
                var windowBounds = new Rect(window.X, window.Y, window.Width, window.Height);
                var clippedBounds = Rect.Intersect(windowBounds, clientArea);
                
                Logger.Instance.Log("Application", "Paint", $"Window '{window.Title}' bounds={windowBounds} clipped={clippedBounds} isEmpty={clippedBounds.IsEmpty}");
                
                if (!clippedBounds.IsEmpty)
                {
                    Logger.Instance.Log("Application", "Paint", $"Calling OnPaint for window '{window.Title}' at {clippedBounds}");
                    window.OnPaint(clippedBounds);
                }
                else
                {
                    Logger.Instance.Log("Application", "Paint", $"Skipping window '{window.Title}' - clipped bounds are empty");
                }
            }

            // Paint menu bar LAST (top layer) so submenus appear above everything
            if (_menuBar != null)
            {
                Logger.Instance.Log("Application", "Paint", "Painting menu bar");
                _menuBar.OnPaint(new Rect(_menuBar.X, _menuBar.Y, _menuBar.Width, _menuBar.Height));
            }

            // Present the back buffer to screen (only updates changed cells)
            Logger.Instance.Log("Application", "Paint", "Presenting frame");
            _screen.Present();
        }

        /// <summary>
        /// Public method for controls to request a repaint
        /// </summary>
        public void RequestRepaint()
        {
            _needsRepaint = true;
        }
    }

    /// <summary>
    /// Simple message box implementation
    /// </summary>
    public static class MessageBox
    {
        public static void Show(string message, string title = "Message", 
            MessageBoxButtons buttons = MessageBoxButtons.OK, Action<MessageBoxResult>? callback = null)
        {
            var lines = message.Split('\n');
            var maxLineLength = Math.Max(title.Length, lines.Max(l => l.Length));
            var width = Math.Min(Math.Max(maxLineLength + 6, 20), Screen.Instance.Width - 4);
            var height = lines.Length + 6; // Message + title + buttons + borders

            var x = (Screen.Instance.Width - width) / 2;
            var y = (Screen.Instance.Height - height) / 2;

            var dialog = new Window(x, y, width, height, title);
            dialog.BackColor = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray);

            // Add message text
            var messageY = 1;
            foreach (var line in lines)
            {
                var label = new Label(2, messageY++, width - 4, line);
                label.ForeColor = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray);
                dialog.AddChild(label);
            }

            // Add buttons
            var buttonY = height - 4;

            void CloseDialog(MessageBoxResult result)
            {
                Application.Instance.RemoveWindow(dialog);
                callback?.Invoke(result);
            }

            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    var okButton = new Button((width - 8) / 2, buttonY, 8, "OK");
                    okButton.HasFocus = true;
                    okButton.Click += (s, e) => CloseDialog(MessageBoxResult.OK);
                    dialog.AddChild(okButton);
                    break;

                case MessageBoxButtons.YesNo:
                    var yesButton = new Button(width / 2 - 10, buttonY, 8, "Yes");
                    var noButton = new Button(width / 2 + 2, buttonY, 8, "No");
                    
                    yesButton.HasFocus = true;
                    yesButton.Click += (s, e) => CloseDialog(MessageBoxResult.Yes);
                    noButton.Click += (s, e) => CloseDialog(MessageBoxResult.No);
                    
                    // Handle Tab to switch focus between buttons
                    dialog.KeyPressed += (s, e) => {
                        if (e.KeyInfo.Key == ConsoleKey.Tab)
                        {
                            yesButton.HasFocus = !yesButton.HasFocus;
                            noButton.HasFocus = !noButton.HasFocus;
                            e.Handled = true;
                        }
                    };
                    
                    dialog.AddChild(yesButton);
                    dialog.AddChild(noButton);
                    break;
            }

            Application.Instance.AddWindow(dialog);
            Application.Instance.SetActiveWindow(dialog);
        }

        // Convenience method for simple messages
        public static void Show(string message, string title = "Message")
        {
            Show(message, title, MessageBoxButtons.OK);
        }
    }

    public enum MessageBoxButtons
    {
        OK,
        YesNo
    }

    public enum MessageBoxResult
    {
        None,
        OK,
        Cancel,
        Yes,
        No
    }
}
