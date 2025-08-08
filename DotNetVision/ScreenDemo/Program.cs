using System;
using DotNetVision.Core;

namespace ScreenDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check for verbose logging command line argument
            string? logFile = null;
            bool verbose = false;
            
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--verbose" || args[i] == "-v")
                {
                    verbose = true;
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        logFile = args[i + 1];
                        i++; // Skip the log file argument
                    }
                    else
                    {
                        logFile = "screendemo.log";
                    }
                }
                else if (args[i] == "--help" || args[i] == "-h")
                {
                    Console.WriteLine("ScreenDemo - Simple Screen Test");
                    Console.WriteLine("Usage: ScreenDemo.exe [options]");
                    Console.WriteLine("Options:");
                    Console.WriteLine("  -v, --verbose [file]    Enable verbose logging (default: screendemo.log)");
                    Console.WriteLine("  -h, --help             Show this help message");
                    return;
                }
            }

            // Enable logging if requested
            if (verbose && !string.IsNullOrEmpty(logFile))
            {
                Logger.Instance.EnableLogging(logFile);
                Console.WriteLine($"Verbose logging enabled. Log file: {logFile}");
                Console.WriteLine("Starting screen test...");
                System.Threading.Thread.Sleep(1000); // Give user time to see the message
            }

            try
            {
                // Create the screen implementation
                IScreen screen = new BufferedScreen();
                
                Logger.Instance.Log("ScreenDemo", "Main", $"Using {screen.GetType().Name}");
                
                // Test the screen
                TestScreen(screen);
            }
            catch (Exception ex)
            {
                // Ensure logging is disabled before showing error
                if (verbose)
                {
                    Logger.Instance.LogError("ScreenDemo", "Main", ex);
                    Logger.Instance.DisableLogging();
                }
                
                Console.WriteLine($"Fatal error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            finally
            {
                // Always disable logging at the end
                if (verbose)
                {
                    Logger.Instance.DisableLogging();
                }
            }
        }

        static void TestScreen(IScreen screen)
        {
            Logger.Instance.LogEntry("ScreenDemo", "TestScreen");
            
            try
            {
                // Initialize screen
                Logger.Instance.Log("ScreenDemo", "TestScreen", "Initializing screen");
                screen.Initialize();
                
                Logger.Instance.Log("ScreenDemo", "TestScreen", $"Screen size: {screen.Width}x{screen.Height}");
                
                // Clear screen
                Logger.Instance.Log("ScreenDemo", "TestScreen", "Clearing screen");
                screen.Clear();
                
                // Write "Hello World" in the center
                string message = "Hello World!";
                int x = (screen.Width - message.Length) / 2;
                int y = screen.Height / 2;
                
                Logger.Instance.Log("ScreenDemo", "TestScreen", $"Writing '{message}' at ({x},{y})");
                screen.WriteAt(x, y, message, new ColorPair(ConsoleColor.White, ConsoleColor.Blue));
                
                // Draw a box around it
                var boxRect = new Rect(x - 2, y - 1, message.Length + 4, 3);
                Logger.Instance.Log("ScreenDemo", "TestScreen", $"Drawing box at {boxRect}");
                screen.DrawBox(boxRect, new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black));
                
                // Present the frame
                Logger.Instance.Log("ScreenDemo", "TestScreen", "Presenting frame");
                screen.Present();
                
                // Wait for key
                Logger.Instance.Log("ScreenDemo", "TestScreen", "Waiting for key press");
                Console.WriteLine(); // Move to bottom
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                
                // Clear and write "Bye!"
                Logger.Instance.Log("ScreenDemo", "TestScreen", "Clearing for goodbye message");
                screen.Clear();
                
                string byeMessage = "Bye!";
                int byeX = (screen.Width - byeMessage.Length) / 2;
                int byeY = screen.Height / 2;
                
                Logger.Instance.Log("ScreenDemo", "TestScreen", $"Writing '{byeMessage}' at ({byeX},{byeY})");
                screen.WriteAt(byeX, byeY, byeMessage, new ColorPair(ConsoleColor.Red, ConsoleColor.White));
                
                Logger.Instance.Log("ScreenDemo", "TestScreen", "Presenting goodbye frame");
                screen.Present();
                
                // Wait a bit then exit
                System.Threading.Thread.Sleep(1000);
                
                Logger.Instance.Log("ScreenDemo", "TestScreen", "Cleaning up");
                screen.Cleanup();
                
                Logger.Instance.LogExit("ScreenDemo", "TestScreen", "Success");
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError("ScreenDemo", "TestScreen", ex);
                screen.Cleanup();
                throw;
            }
        }
    }
}
