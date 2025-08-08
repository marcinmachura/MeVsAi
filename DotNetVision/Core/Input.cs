using System;

namespace DotNetVision.Core
{
    /// <summary>
    /// Keyboard and mouse input handling
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// Read a key press without blocking
        /// </summary>
        public static ConsoleKeyInfo? ReadKey()
        {
            try
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    Logger.Instance.Log("Input", "ReadKey", $"Key: {key.Key}, Char: '{key.KeyChar}', Modifiers: {key.Modifiers}");
                    return key;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError("Input", "ReadKey", ex);
                // If keyboard input fails, just return null
                return null;
            }
        }

        /// <summary>
        /// Wait for and read a key press
        /// </summary>
        public static ConsoleKeyInfo WaitForKey()
        {
            return Console.ReadKey(true);
        }

        /// <summary>
        /// Check if a specific key is pressed
        /// </summary>
        public static bool IsKeyPressed(ConsoleKey key)
        {
            var keyInfo = ReadKey();
            return keyInfo?.Key == key;
        }

        /// <summary>
        /// Wait for Enter key
        /// </summary>
        public static void WaitForEnter()
        {
            ConsoleKeyInfo key;
            do
            {
                key = WaitForKey();
            } while (key.Key != ConsoleKey.Enter);
        }

        /// <summary>
        /// Wait for Escape key
        /// </summary>
        public static void WaitForEscape()
        {
            ConsoleKeyInfo key;
            do
            {
                key = WaitForKey();
            } while (key.Key != ConsoleKey.Escape);
        }
    }

    /// <summary>
    /// Event arguments for UI events
    /// </summary>
    public class KeyEventArgs : EventArgs
    {
        public ConsoleKeyInfo KeyInfo { get; }
        public bool Handled { get; set; }

        public KeyEventArgs(ConsoleKeyInfo keyInfo)
        {
            KeyInfo = keyInfo;
        }
    }

    /// <summary>
    /// Event arguments for paint events
    /// </summary>
    public class PaintEventArgs : EventArgs
    {
        public Rect ClipRect { get; }

        public PaintEventArgs(Rect clipRect)
        {
            ClipRect = clipRect;
        }
    }
}
