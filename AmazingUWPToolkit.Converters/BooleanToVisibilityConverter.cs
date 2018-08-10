using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AmazingUWPToolkit.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        #region Properties

        public bool Invert { get; set; }

        #endregion

        #region Implementation of IValueConverter

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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}