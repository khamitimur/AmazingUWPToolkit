using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AmazingUWPToolkit.Converters
{
    /// <summary>
    /// Converts <see cref="bool"/> to <see cref="Visibility"/>.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        /// Get or sets the value to indicate that result must be inverted.
        /// </summary>
        public bool Invert { get; set; }

        #endregion

        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                if (Invert)
                {
                    boolValue = !boolValue;
                }

                return boolValue
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            return null;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}