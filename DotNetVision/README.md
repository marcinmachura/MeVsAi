# DotNetVision

A TurboVision-inspired console UI library for .NET, bringing retro text-based user interface components to modern C# applications.

## Overview

DotNetVision is a modern interpretation of the classic TurboVision library that was popular in the Turbo Pascal era. It provides a rich set of console-based UI components for creating sophisticated text-mode applications in C#.

## Features

### Core Components

- **Screen Management**: Low-level console manipulation with proper color handling and cursor positioning
- **Event System**: Keyboard input handling with event-driven architecture
- **Layout Management**: Flexible positioning and sizing of UI elements

### Controls

- **Window**: Top-level containers with title bars, borders, and window management
- **Panel**: Container controls for grouping other components
- **Button**: Clickable buttons with focus management and visual feedback
- **Label**: Text display with alignment options
- **TextBox**: Text input with cursor navigation and password masking
- **MessageBox**: Modal dialogs for user interaction

### Visual Features

- **Multiple Border Styles**: Single line, double line, and ASCII borders
- **Color Support**: Full console color palette with customizable foreground/background
- **Unicode Characters**: Beautiful box-drawing characters for modern terminals
- **Focus Management**: Tab navigation between controls

## Getting Started

### Building the Library

```bash
cd DotNetVision
dotnet build
```

### Running the Demo

```bash
cd Demo
dotnet run
```

### Basic Usage

```csharp
using DotNetVision.Core;
using DotNetVision.Controls;
using DotNetVision.Forms;

// Create the main window
var mainWindow = new Window(10, 5, 60, 15, "My Application");

// Add controls
var label = new Label(2, 2, 40, "Hello, DotNetVision!");
var button = new Button(2, 4, 15, "Click Me!");
button.Click += (s, e) => MessageBox.Show("Button clicked!");

mainWindow.AddChild(label);
mainWindow.AddChild(button);

// Run the application
Application.Instance.Run(mainWindow);
```

## Architecture

### Core Namespace (`DotNetVision.Core`)

- `Screen`: Static class for low-level console operations
- `Input`: Keyboard input handling
- `Types`: Basic types like `Point`, `Rect`, `ColorPair`

### Controls Namespace (`DotNetVision.Controls`)

- `Control`: Base class for all UI elements
- `Panel` & `Window`: Container controls
- `Button`, `Label`, `TextBox`: Basic input/output controls

### Forms Namespace (`DotNetVision.Forms`)

- `Application`: Main application loop and window management
- `MessageBox`: Modal dialog utilities

## Key Features Comparison with TurboVision

| Feature | TurboVision | DotNetVision |
|---------|-------------|--------------|
| Language | Pascal/C++ | C# |
| Platform | DOS/Windows | .NET (Cross-platform) |
| Memory Management | Manual | Garbage Collected |
| Event Model | Procedural | Event-driven |
| Unicode Support | No | Yes |
| Modern Terminals | Limited | Full Support |

## Controls Reference

### Window
- **Purpose**: Top-level container with title bar
- **Features**: Moveable, closeable, focus management
- **Keys**: Esc (close), Alt+F4 (close), Alt+Tab (switch)

### Button
- **Purpose**: Clickable action trigger
- **Features**: Focus highlight, press animation
- **Keys**: Enter/Space (click), Tab (focus navigation)

### TextBox
- **Purpose**: Text input field
- **Features**: Cursor navigation, password masking, text selection
- **Keys**: Arrow keys (navigation), Home/End, Backspace/Delete

### Panel
- **Purpose**: Container for grouping controls
- **Features**: Optional borders, client area management

### Label
- **Purpose**: Static text display
- **Features**: Multi-line text, text alignment (left/center/right)

## Advanced Features

### Color Schemes
```csharp
// Predefined color pairs
ColorPair.Default    // Gray on Black
ColorPair.Highlighted // Black on Gray  
ColorPair.Error      // White on Red
ColorPair.Success    // Black on Green
ColorPair.Warning    // Black on Yellow

// Custom colors
var customColors = new ColorPair(ConsoleColor.Yellow, ConsoleColor.Blue);
```

### Border Styles
```csharp
// Available border styles
BoxStyle.Single   // ┌─┐│└─┘ (default)
BoxStyle.Double   // ╔═╗║╚═╝
BoxStyle.Ascii    // +-+|+-+
```

### Event Handling
```csharp
// Keyboard events
control.KeyPressed += (sender, args) => {
    if (args.KeyInfo.Key == ConsoleKey.F1) {
        ShowHelp();
        args.Handled = true;
    }
};

// Button clicks
button.Click += (sender, args) => {
    // Handle button click
};
```

## Demo Application

The included demo showcases:

- Multiple windows with different styles
- Text input with regular and password fields
- Button interactions and message boxes
- Focus management and keyboard navigation
- Window switching and management

### Demo Controls

- **Tab**: Cycle through focusable controls
- **Enter/Space**: Activate buttons
- **Esc**: Close windows
- **Alt+F4**: Close active window
- **Alt+Tab**: Switch between windows
- **Arrow Keys**: Navigate in text boxes

## Requirements

- .NET 8.0 or later
- Windows/Linux/macOS console
- Terminal with Unicode support (recommended)

## Compatibility

DotNetVision works best with:
- Windows Terminal
- VS Code integrated terminal
- Modern Linux terminals (gnome-terminal, konsole, etc.)
- macOS Terminal.app

## Future Enhancements

Planned features for future versions:

- **Menus**: Menu bars and context menus
- **List Controls**: ListBox and ComboBox
- **Dialogs**: File dialogs, input dialogs
- **Themes**: Customizable color schemes
- **Mouse Support**: Click and drag operations
- **Layouts**: Automatic layout managers
- **Grid Control**: Tabular data display
- **Status Bar**: Application status display

## Contributing

This is a demonstration project showcasing modern C# techniques for creating TurboVision-style applications. The code is designed to be educational and extensible.

## License

This project is provided as-is for educational purposes. Feel free to use and modify the code for your own projects.

---

*DotNetVision - Bringing retro console UI to modern .NET applications*
