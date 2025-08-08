using System;
using System.Collections.Generic;
using DotNetVision.Core;

namespace DotNetVision.Controls
{
    /// <summary>
    /// Base class for all UI controls
    /// </summary>
    public abstract class Control
    {
        private Rect _bounds;
        private bool _visible = true;
        private bool _enabled = true;
        private Control? _parent;
        private readonly List<Control> _children = new();

        public Rect Bounds
        {
            get => _bounds;
            set
            {
                _bounds = value;
                Invalidate();
            }
        }

        public Point Position
        {
            get => new(_bounds.X, _bounds.Y);
            set => Bounds = new Rect(value.X, value.Y, _bounds.Width, _bounds.Height);
        }

        public Point Size
        {
            get => new(_bounds.Width, _bounds.Height);
            set => Bounds = new Rect(_bounds.X, _bounds.Y, value.X, value.Y);
        }

        public int X
        {
            get => _bounds.X;
            set => Bounds = new Rect(value, _bounds.Y, _bounds.Width, _bounds.Height);
        }

        public int Y
        {
            get => _bounds.Y;
            set => Bounds = new Rect(_bounds.X, value, _bounds.Width, _bounds.Height);
        }

        public int Width
        {
            get => _bounds.Width;
            set => Bounds = new Rect(_bounds.X, _bounds.Y, value, _bounds.Height);
        }

        public int Height
        {
            get => _bounds.Height;
            set => Bounds = new Rect(_bounds.X, _bounds.Y, _bounds.Width, value);
        }

        public bool Visible
        {
            get => _visible;
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    Invalidate();
                }
            }
        }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    Invalidate();
                }
            }
        }

        public Control? Parent
        {
            get => _parent;
            private set => _parent = value;
        }

        public IReadOnlyList<Control> Children => _children.AsReadOnly();

        public ColorPair ForeColor { get; set; } = ColorPair.Default;
        public ColorPair BackColor { get; set; } = ColorPair.Default;
        public virtual string Text { get; set; } = string.Empty;

        // Events
        public event EventHandler<KeyEventArgs>? KeyPressed;
        public event EventHandler<PaintEventArgs>? Paint;
        public event Action<Control>? Invalidated;

        protected Control()
        {
        }

        protected Control(int x, int y, int width, int height)
        {
            _bounds = new Rect(x, y, width, height);
        }

        /// <summary>
        /// Add a child control
        /// </summary>
        public virtual void AddChild(Control child)
        {
            if (child.Parent != null)
                child.Parent.RemoveChild(child);

            child.Parent = this;
            _children.Add(child);
            child.Invalidated += OnChildInvalidated;
            Invalidate();
        }

        /// <summary>
        /// Remove a child control
        /// </summary>
        public virtual bool RemoveChild(Control child)
        {
            if (_children.Remove(child))
            {
                child.Parent = null;
                child.Invalidated -= OnChildInvalidated;
                Invalidate();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get absolute screen coordinates for this control
        /// </summary>
        public Rect GetScreenBounds()
        {
            var bounds = Bounds;
            var parent = Parent;

            while (parent != null)
            {
                bounds.X += parent.Bounds.X;
                bounds.Y += parent.Bounds.Y;
                parent = parent.Parent;
            }

            return bounds;
        }

        /// <summary>
        /// Convert screen coordinates to control-relative coordinates
        /// </summary>
        public Point ScreenToClient(Point screenPoint)
        {
            var screenBounds = GetScreenBounds();
            return new Point(screenPoint.X - screenBounds.X, screenPoint.Y - screenBounds.Y);
        }

        /// <summary>
        /// Mark the control as needing to be redrawn
        /// </summary>
        public virtual void Invalidate()
        {
            Invalidated?.Invoke(this);
        }

        /// <summary>
        /// Handle key input
        /// </summary>
        public virtual bool OnKeyPressed(ConsoleKeyInfo keyInfo)
        {
            var args = new KeyEventArgs(keyInfo);
            KeyPressed?.Invoke(this, args);
            return args.Handled;
        }

        /// <summary>
        /// Paint the control
        /// </summary>
        public virtual void OnPaint(Rect clipRect)
        {
            Logger.Instance.Log("Control", "OnPaint", $"Type={GetType().Name} Visible={Visible} ClipRect={clipRect}");
            
            if (!Visible) return;

            var screenBounds = GetScreenBounds();
            Logger.Instance.Log("Control", "OnPaint", $"Type={GetType().Name} ScreenBounds={screenBounds}");
            
            // Only paint if we intersect with the clip rectangle
            bool intersects = clipRect.Contains(screenBounds.TopLeft) || 
                            clipRect.Contains(screenBounds.BottomRight) ||
                            screenBounds.Contains(clipRect.TopLeft) ||
                            screenBounds.Contains(clipRect.BottomRight);
                            
            Logger.Instance.Log("Control", "OnPaint", $"Type={GetType().Name} Intersects={intersects}");
            
            if (intersects)
            {
                Logger.Instance.Log("Control", "OnPaint", $"Type={GetType().Name} Calling DoPaint");
                DoPaint(screenBounds);
                
                // Paint children
                foreach (var child in _children)
                {
                    child.OnPaint(clipRect);
                }
            }

            Paint?.Invoke(this, new PaintEventArgs(clipRect));
        }

        /// <summary>
        /// Override this to implement control-specific painting
        /// </summary>
        protected abstract void DoPaint(Rect screenBounds);

        private void OnChildInvalidated(Control source)
        {
            // Bubble the invalidation event up the hierarchy
            Invalidate();
        }

        /// <summary>
        /// Find the topmost child at the given point
        /// </summary>
        public virtual Control? GetChildAt(Point point)
        {
            // Check children in reverse order (top to bottom)
            for (int i = _children.Count - 1; i >= 0; i--)
            {
                var child = _children[i];
                if (child.Visible && child.Bounds.Contains(point))
                {
                    var childPoint = new Point(point.X - child.Bounds.X, point.Y - child.Bounds.Y);
                    return child.GetChildAt(childPoint) ?? child;
                }
            }
            return null;
        }
    }
}
