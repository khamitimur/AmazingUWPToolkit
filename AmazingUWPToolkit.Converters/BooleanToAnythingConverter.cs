using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AmazingUWPToolkit.Converters
{
    public class BooleanToAnythingConverter : DependencyObject,
                                              IValueConverter
    {
        #region Dependency Properties

        public static readonly DependencyProperty TrueValueProperty = DependencyProperty.Register(
            nameof(TrueValue),
            typeof(object),
            typeof(BooleanToAnythingConverter),
            null);

        public static readonly DependencyProperty FalseValueProperty = DependencyProperty.Register(
            nameof(FalseValue),
            typeof(object),
            typeof(BooleanToAnythingConverter),
            null);

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(
            nameof(DefaultValue),
            typeof(object),
            typeof(BooleanToAnythingConverter),
            null);

        #endregion

        #region Properties

        public object TrueValue
        {
            get => GetValue(TrueValueProperty);
            set => SetValue(TrueValueProperty, value);
        }

        public object FalseValue
        {
            get => GetValue(FalseValueProperty);
            set => SetValue(FalseValueProperty, value);
        }

        public object DefaultValue
        {
            get => GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        public bool Invert { get; set; }

        #endregion

        #region Overrides of ConverterBase

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value is bool boolValue)
            {
                if (Invert)
                {
                    boolValue = !boolValue;
                }

                return boolValue
                    ? TrueValue
                    : FalseValue;
            }

            return DefaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}