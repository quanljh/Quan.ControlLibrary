namespace Quan.ControlLibrary
{
    /// <summary>
    /// Helps boxing <see cref="QuanButton.DisplayType"/> values to prevent boxing and unboxing.
    /// </summary>
    public static class QuanButtonDisplayTypeBoxes

    {
        /// <summary>
        /// Gets a boxed representation for QuanButton DisplayType's "Normal" value.
        /// </summary>
        public static readonly object NormalBox = QuanButton.DisplayType.Normal;

        /// <summary>
        /// Gets a boxed representation for QuanButton DisplayType's "Flat" value.
        /// </summary>
        public static readonly object FlatBox = QuanButton.DisplayType.Flat;

        /// <summary>
        /// Gets a boxed representation for QuanButton DisplayType's "OutLined" value.
        /// </summary>
        public static readonly object OutLinedBox = QuanButton.DisplayType.OutLined;

        /// <summary>
        /// Returns a boxed representation for the specified <see cref="QuanButton.DisplayType"/> value.
        /// </summary>
        /// <param name="value">The value to box.</param>
        /// <returns></returns>
        public static object Box(QuanButton.DisplayType value)
        {
            switch (value)
            {
                case QuanButton.DisplayType.Flat:
                    return FlatBox;
                case QuanButton.DisplayType.OutLined:
                    return OutLinedBox;
                default:
                    return NormalBox;
            }
        }
    }
}
