using DotNetVision.Core;

namespace DemoEditor;

class Program
{
    private static IScreen _screen = null!;
    private static string _currentFile = "";
    private static bool _modified = false;
    private static AppConfiguration _config = new();
    private static MenuManager _menuManager = null!;
    private static MultiLineTextBox _textBox = null!;

    static void Main(string[] args)
    {
        Logger.Instance.EnableLogging("DemoEditor.log");
        Logger.Instance.Log("DemoEditor", "Main", "Starting");

        try
        {
            // Initialize screen (use BufferedScreen for better performance)
            _screen = new BufferedScreen();
            _screen.Initialize();

            // Initialize menu system
            _menuManager = new MenuManager(_screen, _config);
            SetupMenus();

            // Initialize text box - starts right after menu bar (line 1), leave space for status bar
            var textBoxBounds = new Rect(1, 2, Console.WindowWidth - 2, Console.WindowHeight - 4);
            _textBox = new MultiLineTextBox(_screen, _config, textBoxBounds, _menuManager);
            _textBox.TextChanged += (s, e) => _modified = _textBox.IsModified;

            bool running = true;
            while (running)
            {
                DrawScreen();
                var key = Console.ReadKey(true);
                
                // Let menu manager handle input first
                if (_menuManager.HandleInput(key))
                {
                    continue; // Menu handled the input
                }
                
                // Let text box handle input
                if (_textBox.HandleInput(key))
                {
                    continue; // Text box handled the input
                }
                
                // Handle other input (like Escape to exit)
                if (key.Key == ConsoleKey.Escape)
                {
                    running = false;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Instance.LogError("DemoEditor", "Main", ex);
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            _screen?.Cleanup();
            Logger.Instance.Log("DemoEditor", "Main", "Ended");
        }
    }

    private static void SetupMenus()
    {
        // File menu
        var fileMenu = new MenuItem("File");
        fileMenu.AddItem("New", (s, e) => NewFile());
        fileMenu.AddItem("Open", (s, e) => OpenFile());
        fileMenu.AddItem("Save", (s, e) => SaveFile());
        fileMenu.AddItem("Save As", (s, e) => SaveAsFile());
        fileMenu.SubItems.Add(MenuItem.Separator);
        fileMenu.AddItem("Exit", (s, e) => Environment.Exit(0));

        // Edit menu
        var editMenu = new MenuItem("Edit");
        editMenu.AddItem("Undo", (s, e) => { /* TODO: Implement undo */ });
        editMenu.AddItem("Redo", (s, e) => { /* TODO: Implement redo */ });
        editMenu.SubItems.Add(MenuItem.Separator);
        editMenu.AddItem("Cut", (s, e) => { /* TODO: Implement cut */ });
        editMenu.AddItem("Copy", (s, e) => { /* TODO: Implement copy */ });
        editMenu.AddItem("Paste", (s, e) => { /* TODO: Implement paste */ });

        // View menu  
        var viewMenu = new MenuItem("View");
        viewMenu.AddItem("Zoom In", (s, e) => { /* TODO: Implement zoom */ });
        viewMenu.AddItem("Zoom Out", (s, e) => { /* TODO: Implement zoom */ });

        // Options menu
        var optionsMenu = new MenuItem("Options");
        optionsMenu.AddItem("QuickBasic Colors", (s, e) => { _config.ColorScheme = ColorScheme.QuickBasic; });
        optionsMenu.AddItem("Black & White Colors", (s, e) => { _config.ColorScheme = ColorScheme.BlackAndWhite; });
        optionsMenu.AddItem("TurboVision Colors", (s, e) => { _config.ColorScheme = ColorScheme.TurboVision; });
        optionsMenu.SubItems.Add(MenuItem.Separator);
        optionsMenu.AddItem("Single Frame", (s, e) => { _config.DefaultFrameStyle = FrameStyle.Single; });
        optionsMenu.AddItem("Double Frame", (s, e) => { _config.DefaultFrameStyle = FrameStyle.Double; });
        optionsMenu.AddItem("ASCII Frame", (s, e) => { _config.DefaultFrameStyle = FrameStyle.Ascii; });

        // Help menu
        var helpMenu = new MenuItem("Help");
        helpMenu.AddItem("About", (s, e) => ShowAbout());
        helpMenu.AddItem("Help", (s, e) => ShowHelp());

        // Set up the menu bar
        _menuManager.SetMenuItems(fileMenu, editMenu, viewMenu, optionsMenu, helpMenu);
    }

    private static void DrawScreen()
    {
        _screen.Clear();
        
        var colors = _config.GetColorSet();
        
        // Draw menu using MenuManager (starts at line 0 now since no title bar)
        _menuManager.Render();
        
        // Draw framed text box with file title in the frame
        string frameTitle = string.IsNullOrEmpty(_currentFile) ? "Untitled" : Path.GetFileName(_currentFile);
        if (_modified) frameTitle += "*";
        
        var frameBounds = new Rect(0, 1, Console.WindowWidth, Console.WindowHeight - 2);
        _screen.DrawBox(frameBounds, colors.Editor, _config.GetBoxStyle());
        
        // Draw title in the top frame
        if (!string.IsNullOrEmpty(frameTitle))
        {
            string title = $" {frameTitle} ";
            int titleX = 2; // Small offset from left edge
            _screen.WriteAt(titleX, 1, title, colors.Editor);
        }
        
        // Render the text box content
        _textBox.Render();
        
        // Draw status bar (bottom line)
        int statusY = Console.WindowHeight - 1;
        _screen.FillRect(new Rect(0, statusY, Console.WindowWidth, 1), ' ', colors.StatusBar);
        
        string status = $" Line {_textBox.CursorLine + 1}, Col {_textBox.CursorColumn + 1}";
        if (_modified) status += " [Modified]";
        status += $" | Lines: {_textBox.LineCount}";
        if (!string.IsNullOrEmpty(_currentFile))
            status += $" | {_currentFile}";
        status += $" | {_config.ColorScheme} | {_config.DefaultFrameStyle}";
        
        _screen.WriteAt(0, statusY, status, colors.StatusBar);

        // Update cursor position
        if (!_menuManager.IsMenuActive)
        {
            _textBox.UpdateCursor();
        }
        else
        {
            Console.CursorVisible = false;
        }

        _screen.Present();
    }

    private static void ShowAbout()
    {
        // Simple about dialog - could be enhanced with a proper dialog system
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write("DotNetVision Editor v1.0 - Press any key...");
        Console.ReadKey();
    }

    private static void ShowHelp()
    {
        // Simple help dialog - could be enhanced with a proper dialog system
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write("F10=Menu, Arrows=Navigate, Enter=Select, Esc=Cancel - Press any key...");
        Console.ReadKey();
    }

    private static void NewFile()
    {
        _textBox.Clear();
        _currentFile = "";
        _modified = false;
    }

    private static void OpenFile()
    {
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write("Open file: ");
        string filename = Console.ReadLine() ?? "";
        
        if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
        {
            try
            {
                var lines = File.ReadAllLines(filename);
                _textBox.SetText(lines);
                _currentFile = filename;
                _modified = false;
                Logger.Instance.Log("DemoEditor", "OpenFile", $"Opened file: {filename}");
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError("DemoEditor", "OpenFile", ex);
            }
        }
    }

    private static void SaveFile()
    {
        if (string.IsNullOrEmpty(_currentFile))
        {
            SaveAsFile();
            return;
        }

        try
        {
            File.WriteAllLines(_currentFile, _textBox.GetLines());
            _modified = false;
            Logger.Instance.Log("DemoEditor", "SaveFile", $"Saved file: {_currentFile}");
        }
        catch (Exception ex)
        {
            Logger.Instance.LogError("DemoEditor", "SaveFile", ex);
        }
    }

    private static void SaveAsFile()
    {
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write("Save as: ");
        string filename = Console.ReadLine() ?? "";
        
        if (!string.IsNullOrEmpty(filename))
        {
            try
            {
                File.WriteAllLines(filename, _textBox.GetLines());
                _currentFile = filename;
                _modified = false;
                Logger.Instance.Log("DemoEditor", "SaveAsFile", $"Saved file as: {filename}");
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError("DemoEditor", "SaveAsFile", ex);
            }
        }
    }
}
