using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Controls
{
    /// <summary>
    /// Extension for <see cref="FontIcon"/> that fixes issues with applying style.
    /// <remarks><para>
    /// For some reason <see cref="FontIcon"/> prefer to use it's local values over values set in style.
    /// </para></remarks>
    /// </summary>
    public class FontIconFxd : FontIcon
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FontIconFxd"/>.
        /// </summary>
        public FontIconFxd()
        {
            ClearValue(FontFamilyProperty);
            ClearValue(FontSizeProperty);
            ClearValue(FontStyleProperty);
            ClearValue(FontWeightProperty);
        }

        #endregion
    }
}