using System;

namespace DotNetVision.Core
{
    /// <summary>
    /// Configuration object for DotNetVision application appearance and behavior
    /// </summary>
    public class AppConfiguration
    {
        public ColorScheme ColorScheme { get; set; } = ColorScheme.QuickBasic;
        public FrameStyle DefaultFrameStyle { get; set; } = FrameStyle.Single;
        
        /// <summary>
        /// Get the color set for the current color scheme
        /// </summary>
        public ColorSet GetColorSet()
        {
            return ColorScheme switch
            {
                ColorScheme.QuickBasic => ColorSet.QuickBasic,
                ColorScheme.BlackAndWhite => ColorSet.BlackAndWhite,
                ColorScheme.TurboVision => ColorSet.TurboVision,
                _ => ColorSet.QuickBasic
            };
        }
        
        /// <summary>
        /// Get the box style for the current frame style
        /// </summary>
        public BoxStyle GetBoxStyle()
        {
            return DefaultFrameStyle switch
            {
                FrameStyle.Single => BoxStyle.Single,
                FrameStyle.Double => BoxStyle.Double,
                FrameStyle.Ascii => BoxStyle.Ascii,
                _ => BoxStyle.Single
            };
        }
    }

    /// <summary>
    /// Available color schemes
    /// </summary>
    public enum ColorScheme
    {
        QuickBasic,
        BlackAndWhite,
        TurboVision
    }

    /// <summary>
    /// Available frame styles
    /// </summary>
    public enum FrameStyle
    {
        Single,
        Double,
        Ascii
    }

    /// <summary>
    /// Color set defining the colors for different UI elements
    /// </summary>
    public class ColorSet
    {
        public ColorPair TitleBar { get; set; }
        public ColorPair MenuBar { get; set; }
        public ColorPair MenuBarSelected { get; set; }
        public ColorPair Editor { get; set; }
        public ColorPair EditorCurrentLine { get; set; }
        public ColorPair StatusBar { get; set; }
        public ColorPair WindowBackground { get; set; }
        public ColorPair WindowBorder { get; set; }
        public ColorPair Dialog { get; set; }
        public ColorPair DialogSelected { get; set; }
        public ColorPair Button { get; set; }
        public ColorPair ButtonSelected { get; set; }

        /// <summary>
        /// QuickBasic-style color scheme (blue backgrounds, cyan menus)
        /// </summary>
        public static ColorSet QuickBasic => new()
        {
            TitleBar = new ColorPair(ConsoleColor.White, ConsoleColor.Blue),
            MenuBar = new ColorPair(ConsoleColor.Black, ConsoleColor.Cyan),
            MenuBarSelected = new ColorPair(ConsoleColor.Yellow, ConsoleColor.Black),
            Editor = new ColorPair(ConsoleColor.White, ConsoleColor.Blue),
            EditorCurrentLine = new ColorPair(ConsoleColor.Yellow, ConsoleColor.Blue),
            StatusBar = new ColorPair(ConsoleColor.Black, ConsoleColor.Cyan),
            WindowBackground = new ColorPair(ConsoleColor.White, ConsoleColor.Blue),
            WindowBorder = new ColorPair(ConsoleColor.White, ConsoleColor.Blue),
            Dialog = new ColorPair(ConsoleColor.Black, ConsoleColor.White),
            DialogSelected = new ColorPair(ConsoleColor.White, ConsoleColor.Black),
            Button = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray),
            ButtonSelected = new ColorPair(ConsoleColor.White, ConsoleColor.Black)
        };

        /// <summary>
        /// Black and white color scheme (monochrome)
        /// </summary>
        public static ColorSet BlackAndWhite => new()
        {
            TitleBar = new ColorPair(ConsoleColor.Black, ConsoleColor.White),
            MenuBar = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray),
            MenuBarSelected = new ColorPair(ConsoleColor.White, ConsoleColor.Black),
            Editor = new ColorPair(ConsoleColor.Black, ConsoleColor.White),
            EditorCurrentLine = new ColorPair(ConsoleColor.White, ConsoleColor.Black),
            StatusBar = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray),
            WindowBackground = new ColorPair(ConsoleColor.Black, ConsoleColor.White),
            WindowBorder = new ColorPair(ConsoleColor.Black, ConsoleColor.White),
            Dialog = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray),
            DialogSelected = new ColorPair(ConsoleColor.White, ConsoleColor.Black),
            Button = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray),
            ButtonSelected = new ColorPair(ConsoleColor.White, ConsoleColor.Black)
        };

        /// <summary>
        /// TurboVision-style color scheme (gray backgrounds, blue accents)
        /// </summary>
        public static ColorSet TurboVision => new()
        {
            TitleBar = new ColorPair(ConsoleColor.White, ConsoleColor.Blue),
            MenuBar = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray),
            MenuBarSelected = new ColorPair(ConsoleColor.White, ConsoleColor.Blue),
            Editor = new ColorPair(ConsoleColor.Black, ConsoleColor.White),
            EditorCurrentLine = new ColorPair(ConsoleColor.White, ConsoleColor.Blue),
            StatusBar = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray),
            WindowBackground = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray),
            WindowBorder = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray),
            Dialog = new ColorPair(ConsoleColor.Black, ConsoleColor.White),
            DialogSelected = new ColorPair(ConsoleColor.White, ConsoleColor.Blue),
            Button = new ColorPair(ConsoleColor.Black, ConsoleColor.Gray),
            ButtonSelected = new ColorPair(ConsoleColor.White, ConsoleColor.Blue)
        };
    }
}
