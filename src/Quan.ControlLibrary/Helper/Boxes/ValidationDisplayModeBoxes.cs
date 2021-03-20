namespace Quan.ControlLibrary
{
    /// <summary>
    /// Helps boxing <see cref="ValidationHelper.DisplayMode"/> values to prevent boxing and unboxing.
    /// </summary>
    public static class ValidationDisplayModeBoxes
    {

        /// <summary>
        /// Gets a boxed representation for ValidationHelper DisplayMode's "Text" value.
        /// </summary>
        public static readonly object TextBox = ValidationHelper.DisplayMode.Text;

        /// <summary>
        /// Gets a boxed representation for ValidationHelper DisplayMode's "Popup" value.
        /// </summary>
        public static readonly object PopupBox = ValidationHelper.DisplayMode.Popup;

        /// <summary>
        /// Returns a boxed representation for the specified <see cref="ValidationHelper.DisplayMode"/> value.
        /// </summary>
        /// <param name="value">The value to box.</param>
        /// <returns></returns>
        public static object Box(ValidationHelper.DisplayMode value)
        {
            if (value == ValidationHelper.DisplayMode.Text)
                return TextBox;
            else
                return PopupBox;
        }
    }
}
