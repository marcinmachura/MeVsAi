namespace DotNetVision.Controls
{
    /// <summary>
    /// Defines the contract for a control that can receive input focus
    /// </summary>
    public interface IFocusable
    {
        /// <summary>
        /// Gets or sets whether the control can receive focus
        /// </summary>
        bool CanFocus { get; set; }

        /// <summary>
        /// Gets or sets whether the control currently has focus
        /// </summary>
        bool HasFocus { get; set; }

        /// <summary>
        /// Gets or sets the tab order of the control within its container
        /// </summary>
        int TabIndex { get; set; }

        /// <summary>
        /// Called when the control receives focus
        /// </summary>
        void OnGotFocus();

        /// <summary>
        /// Called when the control loses focus
        /// </summary>
        void OnLostFocus();
    }
}
