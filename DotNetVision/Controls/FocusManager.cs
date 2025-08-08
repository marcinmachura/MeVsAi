using System.Collections.Generic;
using System.Linq;
using DotNetVision.Controls;

namespace DotNetVision.Controls
{
    /// <summary>
    /// Manages input focus for controls within a container
    /// </summary>
    public class FocusManager
    {
        private readonly Control _container;
        private readonly List<IFocusable> _focusableControls = new();
        private int _currentFocusIndex = -1;

        public FocusManager(Control container)
        {
            _container = container;
            BuildFocusList(_container);
        }

        /// <summary>
        /// The currently focused control
        /// </summary>
        public IFocusable? FocusedControl => 
            _currentFocusIndex >= 0 && _currentFocusIndex < _focusableControls.Count 
                ? _focusableControls[_currentFocusIndex] 
                : null;

        /// <summary>
        /// Recursively find all focusable controls and sort them by TabIndex
        /// </summary>
        private void BuildFocusList(Control parent)
        {
            foreach (var child in parent.Children)
            {
                if (child is IFocusable focusable && focusable.CanFocus)
                {
                    _focusableControls.Add(focusable);
                }
                // Recurse into child containers
                if (child.Children.Any())
                {
                    BuildFocusList(child);
                }
            }

            // Sort controls by their tab index
            _focusableControls.Sort((a, b) => a.TabIndex.CompareTo(b.TabIndex));
        }

        /// <summary>
        /// Set focus to the first available control
        /// </summary>
        public void FocusFirst()
        {
            SetFocus(0);
        }

        /// <summary>
        /// Move focus to the next control in the tab order
        /// </summary>
        public void FocusNext()
        {
            if (_focusableControls.Count == 0) return;
            
            var nextIndex = (_currentFocusIndex + 1) % _focusableControls.Count;
            SetFocus(nextIndex);
        }

        /// <summary>
        /// Move focus to the previous control in the tab order
        /// </summary>
        public void FocusPrevious()
        {
            if (_focusableControls.Count == 0) return;

            var prevIndex = (_currentFocusIndex - 1 + _focusableControls.Count) % _focusableControls.Count;
            SetFocus(prevIndex);
        }

        /// <summary>
        /// Set focus to a specific control by its index in the focus list
        /// </summary>
        private void SetFocus(int index)
        {
            if (index < 0 || index >= _focusableControls.Count) return;

            // Unfocus the current control
            FocusedControl?.OnLostFocus();

            _currentFocusIndex = index;

            // Focus the new control
            FocusedControl?.OnGotFocus();
        }

        /// <summary>
        /// Set focus to a specific control instance
        /// </summary>
        public void SetFocus(IFocusable control)
        {
            var index = _focusableControls.IndexOf(control);
            if (index != -1)
            {
                SetFocus(index);
            }
        }
    }
}
