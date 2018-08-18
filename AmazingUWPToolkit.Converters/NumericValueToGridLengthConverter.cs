using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AmazingUWPToolkit.Converters
{
    public class NumericValueToGridLengthConverter : DependencyObject,
                                                     IValueConverter
    {
        #region Dependency Properties

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(
            nameof(DefaultValue),
            typeof(GridLength),
            typeof(NumericValueToGridLengthConverter),
            new PropertyMetadata(GridLength.Auto));

        #endregion

        #region Properties

        public GridLength DefaultValue
        {
            get => (GridLength)GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        #endregion

        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intValue)
            {
                return new GridLength(intValue);
            }

            if (value is double doubleValue)
            {
                return new GridLength(doubleValue);
            }

            return DefaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}