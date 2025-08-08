using System;
using DotNetVision.Core;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse command line arguments
            string? logFile = null;
            bool verbose = false;
            bool help = false;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--verbose" or "-v":
                        verbose = true;
                        if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                        {
                            logFile = args[i + 1];
                            i++; // Skip the log file argument
                        }
                        else
                        {
                            logFile = "demo.log";
                        }
                        break;
                    case "--help" or "-h":
                        help = true;
                        break;
                }
            }

            if (help)
            {
                Console.WriteLine("DotNetVision Demo - Interface-Based Version");
                Console.WriteLine("Usage: Demo.exe [options]");
                Console.WriteLine("Options:");
                Console.WriteLine("  -v, --verbose [file]    Enable verbose logging (default: demo.log)");
                Console.WriteLine("  -h, --help             Show this help message");
                return;
            }

            // Enable logging if requested
            if (verbose && !string.IsNullOrEmpty(logFile))
            {
                Logger.Instance.EnableLogging(logFile);
                Console.WriteLine($"Verbose logging enabled. Log file: {logFile}");
                Console.WriteLine("Starting demo...");
                System.Threading.Thread.Sleep(1000);
            }

            try
            {
                // Create the screen implementation
                IScreen screen = new BufferedScreen();
                
                Logger.Instance.Log("Demo", "Main", $"Using {screen.GetType().Name}");
                
                // Run the demo
                RunDemo(screen);
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError("Demo", "Main", ex);
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Logger.Instance.Log("Demo", "Main", "Demo ended");
            }
        }

        static void RunDemo(IScreen screen)
        {
            Logger.Instance.LogEntry("Demo", "RunDemo");
            
            try
            {
                // Initialize screen
                screen.Initialize();
                Logger.Instance.Log("Demo", "RunDemo", $"Screen initialized: {screen.Width}x{screen.Height}");

                // Demo 1: Welcome screen
                ShowWelcomeScreen(screen);
                
                // Demo 2: Color palette demo
                ShowColorDemo(screen);
                
                // Demo 3: Box drawing demo
                ShowBoxDemo(screen);
                
                // Demo 4: Text and layout demo
                ShowLayoutDemo(screen);
                
                // Demo 5: Performance test
                ShowPerformanceDemo(screen);
                
                // Final goodbye
                ShowGoodbyeScreen(screen);
            }
            finally
            {
                screen.Cleanup();
                Logger.Instance.LogExit("Demo", "RunDemo");
            }
        }

        static void ShowWelcomeScreen(IScreen screen)
        {
            screen.Clear();
            
            // Title
            string title = "DotNetVision Demo - Interface-Based Architecture";
            int titleX = (screen.Width - title.Length) / 2;
            screen.WriteAt(titleX, 2, title, new ColorPair(ConsoleColor.Yellow, ConsoleColor.Blue));
            
            // Subtitle
            string subtitle = $"Running with {screen.GetType().Name}";
            int subtitleX = (screen.Width - subtitle.Length) / 2;
            screen.WriteAt(subtitleX, 4, subtitle, new ColorPair(ConsoleColor.White, ConsoleColor.Blue));
            
            // Main border
            var mainRect = new Rect(5, 6, screen.Width - 10, screen.Height - 12);
            screen.DrawBox(mainRect, new ColorPair(ConsoleColor.Cyan, ConsoleColor.Blue), BoxStyle.Double);
            
            // Welcome message
            string[] messages = {
                "Welcome to the DotNetVision Demo!",
                "",
                "This demo showcases the interface-based architecture",
                "with both DirectScreen and BufferedScreen implementations.",
                "",
                "Features demonstrated:",
                "• Text rendering with colors",
                "• Box drawing with different styles",
                "• Layout and positioning",
                "• Performance comparison",
                "",
                "Press any key to continue..."
            };
            
            int startY = mainRect.Y + 2;
            for (int i = 0; i < messages.Length; i++)
            {
                if (i == 0) // Title
                {
                    int msgX = (screen.Width - messages[i].Length) / 2;
                    screen.WriteAt(msgX, startY + i, messages[i], new ColorPair(ConsoleColor.White, ConsoleColor.Blue));
                }
                else if (messages[i].StartsWith("•")) // Bullet points
                {
                    screen.WriteAt(mainRect.X + 4, startY + i, messages[i], new ColorPair(ConsoleColor.Green, ConsoleColor.Blue));
                }
                else if (messages[i] == "Press any key to continue...")
                {
                    int msgX = (screen.Width - messages[i].Length) / 2;
                    screen.WriteAt(msgX, startY + i, messages[i], new ColorPair(ConsoleColor.Yellow, ConsoleColor.Blue));
                }
                else
                {
                    int msgX = (screen.Width - messages[i].Length) / 2;
                    screen.WriteAt(msgX, startY + i, messages[i], new ColorPair(ConsoleColor.Gray, ConsoleColor.Blue));
                }
            }
            
            screen.Present();
            Console.ReadKey(true);
        }

        static void ShowColorDemo(IScreen screen)
        {
            screen.Clear();
            
            // Title
            string title = "Color Palette Demo";
            int titleX = (screen.Width - title.Length) / 2;
            screen.WriteAt(titleX, 1, title, new ColorPair(ConsoleColor.White, ConsoleColor.Black));
            
            // Color grid
            var colors = new[] {
                ConsoleColor.Black, ConsoleColor.DarkBlue, ConsoleColor.DarkGreen, ConsoleColor.DarkCyan,
                ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.Gray,
                ConsoleColor.DarkGray, ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.Cyan,
                ConsoleColor.Red, ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.White
            };
            
            int startX = (screen.Width - 48) / 2; // 16 colors * 3 chars each
            int startY = 4;
            
            for (int i = 0; i < colors.Length; i++)
            {
                int x = startX + (i % 8) * 6;
                int y = startY + (i / 8) * 4;
                
                // Color block
                var rect = new Rect(x, y, 4, 2);
                screen.FillRect(rect, '█', new ColorPair(colors[i], colors[i]));
                
                // Color name
                string colorName = colors[i].ToString();
                screen.WriteAt(x, y + 2, colorName.Substring(0, Math.Min(4, colorName.Length)), 
                    new ColorPair(ConsoleColor.White, ConsoleColor.Black));
            }
            
            // Instructions
            string instruction = "Press any key to continue...";
            int instrX = (screen.Width - instruction.Length) / 2;
            screen.WriteAt(instrX, screen.Height - 3, instruction, new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black));
            
            screen.Present();
            Console.ReadKey(true);
        }

        static void ShowBoxDemo(IScreen screen)
        {
            screen.Clear();
            
            // Title
            string title = "Box Drawing Styles Demo";
            int titleX = (screen.Width - title.Length) / 2;
            screen.WriteAt(titleX, 1, title, new ColorPair(ConsoleColor.White, ConsoleColor.Black));
            
            int centerX = screen.Width / 2;
            int centerY = screen.Height / 2;
            
            // Single line box
            var singleRect = new Rect(centerX - 30, centerY - 6, 20, 8);
            screen.DrawBox(singleRect, new ColorPair(ConsoleColor.Green, ConsoleColor.Black), BoxStyle.Single);
            screen.WriteAt(singleRect.X + 2, singleRect.Y + 1, "Single Line", new ColorPair(ConsoleColor.Green, ConsoleColor.Black));
            screen.WriteAt(singleRect.X + 2, singleRect.Y + 3, "┌─┐", new ColorPair(ConsoleColor.Green, ConsoleColor.Black));
            screen.WriteAt(singleRect.X + 2, singleRect.Y + 4, "│ │", new ColorPair(ConsoleColor.Green, ConsoleColor.Black));
            screen.WriteAt(singleRect.X + 2, singleRect.Y + 5, "└─┘", new ColorPair(ConsoleColor.Green, ConsoleColor.Black));
            
            // Double line box
            var doubleRect = new Rect(centerX - 5, centerY - 6, 20, 8);
            screen.DrawBox(doubleRect, new ColorPair(ConsoleColor.Cyan, ConsoleColor.Black), BoxStyle.Double);
            screen.WriteAt(doubleRect.X + 2, doubleRect.Y + 1, "Double Line", new ColorPair(ConsoleColor.Cyan, ConsoleColor.Black));
            screen.WriteAt(doubleRect.X + 2, doubleRect.Y + 3, "╔═╗", new ColorPair(ConsoleColor.Cyan, ConsoleColor.Black));
            screen.WriteAt(doubleRect.X + 2, doubleRect.Y + 4, "║ ║", new ColorPair(ConsoleColor.Cyan, ConsoleColor.Black));
            screen.WriteAt(doubleRect.X + 2, doubleRect.Y + 5, "╚═╝", new ColorPair(ConsoleColor.Cyan, ConsoleColor.Black));
            
            // ASCII box
            var asciiRect = new Rect(centerX + 20, centerY - 6, 20, 8);
            screen.DrawBox(asciiRect, new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black), BoxStyle.Ascii);
            screen.WriteAt(asciiRect.X + 2, asciiRect.Y + 1, "ASCII Style", new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black));
            screen.WriteAt(asciiRect.X + 2, asciiRect.Y + 3, "+-+", new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black));
            screen.WriteAt(asciiRect.X + 2, asciiRect.Y + 4, "| |", new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black));
            screen.WriteAt(asciiRect.X + 2, asciiRect.Y + 5, "+-+", new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black));
            
            // Instructions
            string instruction = "Press any key to continue...";
            int instrX = (screen.Width - instruction.Length) / 2;
            screen.WriteAt(instrX, screen.Height - 3, instruction, new ColorPair(ConsoleColor.White, ConsoleColor.Black));
            
            screen.Present();
            Console.ReadKey(true);
        }

        static void ShowLayoutDemo(IScreen screen)
        {
            screen.Clear();
            
            // Title
            string title = "Layout and Text Demo";
            int titleX = (screen.Width - title.Length) / 2;
            screen.WriteAt(titleX, 1, title, new ColorPair(ConsoleColor.White, ConsoleColor.Black));
            
            // Create a simulated window layout
            var mainWindow = new Rect(2, 3, screen.Width - 4, screen.Height - 6);
            screen.DrawBox(mainWindow, new ColorPair(ConsoleColor.Gray, ConsoleColor.Black), BoxStyle.Double);
            
            // Title bar
            var titleBar = new Rect(mainWindow.X + 1, mainWindow.Y, mainWindow.Width - 2, 1);
            screen.FillRect(titleBar, ' ', new ColorPair(ConsoleColor.White, ConsoleColor.Blue));
            screen.WriteAt(titleBar.X + 1, titleBar.Y, "Sample Window", new ColorPair(ConsoleColor.White, ConsoleColor.Blue));
            screen.WriteAt(titleBar.X + titleBar.Width - 4, titleBar.Y, "[X]", new ColorPair(ConsoleColor.Yellow, ConsoleColor.Blue));
            
            // Content area
            var content = new Rect(mainWindow.X + 2, mainWindow.Y + 2, mainWindow.Width - 4, mainWindow.Height - 4);
            
            // Some sample content
            string[] contentLines = {
                "This demonstrates text layout within windows.",
                "",
                "Features shown:",
                "• Window with title bar",
                "• Multi-line text content",
                "• Proper clipping and positioning",
                "• Color coordination",
                "",
                "The interface-based design allows easy switching",
                "between DirectScreen and BufferedScreen implementations",
                "without changing the application logic.",
                "",
                "Both implementations provide the same functionality",
                "but with different performance characteristics."
            };
            
            for (int i = 0; i < contentLines.Length && i < content.Height; i++)
            {
                ConsoleColor color = contentLines[i].StartsWith("•") ? ConsoleColor.Green : ConsoleColor.Gray;
                screen.WriteAt(content.X, content.Y + i, contentLines[i], new ColorPair(color, ConsoleColor.Black));
            }
            
            // Status bar
            var statusBar = new Rect(mainWindow.X + 1, mainWindow.Y + mainWindow.Height - 1, mainWindow.Width - 2, 1);
            screen.FillRect(statusBar, ' ', new ColorPair(ConsoleColor.Black, ConsoleColor.Gray));
            screen.WriteAt(statusBar.X + 1, statusBar.Y, "Ready", new ColorPair(ConsoleColor.Black, ConsoleColor.Gray));
            string implementation = $"Implementation: {screen.GetType().Name}";
            screen.WriteAt(statusBar.X + statusBar.Width - implementation.Length - 1, statusBar.Y, 
                implementation, new ColorPair(ConsoleColor.Black, ConsoleColor.Gray));
            
            // Instructions
            string instruction = "Press any key to continue...";
            int instrX = (screen.Width - instruction.Length) / 2;
            screen.WriteAt(instrX, screen.Height - 2, instruction, new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black));
            
            screen.Present();
            Console.ReadKey(true);
        }

        static void ShowPerformanceDemo(IScreen screen)
        {
            screen.Clear();
            
            // Title
            string title = "Performance Demo";
            int titleX = (screen.Width - title.Length) / 2;
            screen.WriteAt(titleX, 1, title, new ColorPair(ConsoleColor.White, ConsoleColor.Black));
            
            string implementation = $"Testing {screen.GetType().Name} performance...";
            int implX = (screen.Width - implementation.Length) / 2;
            screen.WriteAt(implX, 3, implementation, new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black));
            
            screen.Present();
            
            // Performance test - rapid updates
            var testRect = new Rect(10, 6, screen.Width - 20, screen.Height - 12);
            screen.DrawBox(testRect, new ColorPair(ConsoleColor.Cyan, ConsoleColor.Black), BoxStyle.Single);
            
            var startTime = DateTime.Now;
            int updates = 0;
            var colors = new[] { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Magenta };
            
            // Run for 2 seconds
            while ((DateTime.Now - startTime).TotalSeconds < 2)
            {
                // Fill with random colored characters
                var color = colors[updates % colors.Length];
                char ch = (char)('A' + (updates % 26));
                
                for (int y = testRect.Y + 1; y < testRect.Y + testRect.Height - 1; y++)
                {
                    for (int x = testRect.X + 1; x < testRect.X + testRect.Width - 1; x++)
                    {
                        if ((x + y + updates) % 3 == 0)
                        {
                            screen.WriteAt(x, y, ch.ToString(), new ColorPair(color, ConsoleColor.Black));
                        }
                    }
                }
                
                // Update counter
                string counter = $"Updates: {updates}";
                screen.WriteAt(testRect.X + 2, testRect.Y + testRect.Height - 2, counter, 
                    new ColorPair(ConsoleColor.White, ConsoleColor.Black));
                
                screen.Present();
                updates++;
            }
            
            var endTime = DateTime.Now;
            var elapsed = endTime - startTime;
            
            // Show results
            screen.FillRect(testRect, ' ', new ColorPair(ConsoleColor.Gray, ConsoleColor.Black));
            
            string[] results = {
                "Performance Test Results:",
                "",
                $"Implementation: {screen.GetType().Name}",
                $"Test Duration: {elapsed.TotalSeconds:F2} seconds",
                $"Total Updates: {updates}",
                $"Updates/Second: {updates / elapsed.TotalSeconds:F1}",
                "",
                "Note: BufferedScreen should show better performance",
                "for complex updates due to dirty region tracking."
            };
            
            for (int i = 0; i < results.Length; i++)
            {
                ConsoleColor color = i == 0 ? ConsoleColor.Yellow : ConsoleColor.Gray;
                if (results[i].Contains("Updates/Second"))
                    color = ConsoleColor.Green;
                    
                int x = testRect.X + 2;
                int y = testRect.Y + 2 + i;
                screen.WriteAt(x, y, results[i], new ColorPair(color, ConsoleColor.Black));
            }
            
            // Instructions
            string instruction = "Press any key to continue...";
            int instrX = (screen.Width - instruction.Length) / 2;
            screen.WriteAt(instrX, screen.Height - 2, instruction, new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black));
            
            screen.Present();
            Console.ReadKey(true);
        }

        static void ShowGoodbyeScreen(IScreen screen)
        {
            screen.Clear();
            
            // Title
            string title = "Demo Complete!";
            int titleX = (screen.Width - title.Length) / 2;
            screen.WriteAt(titleX, screen.Height / 2 - 3, title, new ColorPair(ConsoleColor.Green, ConsoleColor.Black));
            
            // Implementation info
            string impl = $"Demonstrated with {screen.GetType().Name}";
            int implX = (screen.Width - impl.Length) / 2;
            screen.WriteAt(implX, screen.Height / 2 - 1, impl, new ColorPair(ConsoleColor.Cyan, ConsoleColor.Black));
            
            // Thank you
            string thanks = "Thank you for trying DotNetVision!";
            int thanksX = (screen.Width - thanks.Length) / 2;
            screen.WriteAt(thanksX, screen.Height / 2 + 1, thanks, new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black));
            
            // Exit instruction
            string exit = "Press any key to exit...";
            int exitX = (screen.Width - exit.Length) / 2;
            screen.WriteAt(exitX, screen.Height / 2 + 3, exit, new ColorPair(ConsoleColor.White, ConsoleColor.Black));
            
            screen.Present();
            Console.ReadKey(true);
        }
    }
}
