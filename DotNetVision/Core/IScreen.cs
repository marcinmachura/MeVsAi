using System;
using DotNetVision.Core;

namespace DotNetVision.Core
{
    /// <summary>
    /// Interface for screen management implementations
    /// </summary>
    public interface IScreen
    {
        /// <summary>
        /// Screen width in characters
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Screen height in characters
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Screen bounds rectangle
        /// </summary>
        Rect Bounds { get; }

        /// <summary>
        /// Gets or sets whether the cursor is visible
        /// </summary>
        bool IsCursorVisible { get; set; }

        /// <summary>
        /// Initialize the screen system
        /// </summary>
        void Initialize();

        /// <summary>
        /// Cleanup and restore console
        /// </summary>
        void Cleanup();

        /// <summary>
        /// Clear the screen
        /// </summary>
        void Clear();

        /// <summary>
        /// Set cursor position
        /// </summary>
        void SetCursor(int x, int y);

        /// <summary>
        /// Set cursor position using Point
        /// </summary>
        void SetCursor(Point position);

        /// <summary>
        /// Set drawing colors
        /// </summary>
        void SetColors(ColorPair colors);

        /// <summary>
        /// Write text at specific position
        /// </summary>
        void WriteAt(int x, int y, string text, ColorPair? colors = null);

        /// <summary>
        /// Write text at specific position using Point
        /// </summary>
        void WriteAt(Point position, string text, ColorPair? colors = null);

        /// <summary>
        /// Fill a rectangular area
        /// </summary>
        void FillRect(Rect rect, char character = ' ', ColorPair? colors = null);

        /// <summary>
        /// Draw a box border
        /// </summary>
        void DrawBox(Rect rect, ColorPair? colors = null, BoxStyle style = BoxStyle.Single);

        /// <summary>
        /// Present/flush the current frame to screen
        /// </summary>
        void Present();
    }
}
