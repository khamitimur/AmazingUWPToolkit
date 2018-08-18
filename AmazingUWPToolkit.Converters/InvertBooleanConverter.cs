using System;
using Windows.UI.Xaml.Data;

namespace AmazingUWPToolkit.Converters
{
    /// <summary>
    /// Converts <see cref="bool"/> value to it's inverted value.
    /// </summary>
    public class InvertBooleanConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
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