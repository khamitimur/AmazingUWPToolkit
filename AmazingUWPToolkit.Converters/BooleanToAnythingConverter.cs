using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AmazingUWPToolkit.Converters
{
    /// <summary>
    /// Converts <see cref="bool"/> to basically anything.
    /// </summary>
    public class BooleanToAnythingConverter : DependencyObject,
                                              IValueConverter
    {
        #region Dependency Properties

        /// <summary>
        /// Value for true statement.
        /// </summary>
        /// <remarks><para>
        /// This dependency property can be accessed via the <see cref="TrueValue"/> property.
        /// </para></remarks>
        public static readonly DependencyProperty TrueValueProperty = DependencyProperty.Register(
            nameof(TrueValue),
            typeof(object),
            typeof(BooleanToAnythingConverter),
            null);

        /// <summary>
        /// Value for false statement.
        /// </summary>
        /// <remarks><para>
        /// This dependency property can be accessed via the <see cref="FalseValue"/> property.
        /// </para></remarks>
        public static readonly DependencyProperty FalseValueProperty = DependencyProperty.Register(
            nameof(FalseValue),
            typeof(object),
            typeof(BooleanToAnythingConverter),
            null);

        /// <summary>
        /// Default value to be used when convert is not possible.
        /// </summary>
        /// <remarks><para>
        /// This dependency property can be accessed via the <see cref="DefaultValue"/> property.
        /// </para></remarks>
        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(
            nameof(DefaultValue),
            typeof(object),
            typeof(BooleanToAnythingConverter),
            null);

        #endregion

        #region Properties

        /// <summary>
        /// Get or sets the value for true statement.
        /// </summary>
        public object TrueValue
        {
            get => GetValue(TrueValueProperty);
            set => SetValue(TrueValueProperty, value);
        }

        /// <summary>
        /// Get or sets the value for false statement.
        /// </summary>
        public object FalseValue
        {
            get => GetValue(FalseValueProperty);
            set => SetValue(FalseValueProperty, value);
        }

        /// <summary>
        /// Get or sets the default value to be used when convert is not possible.
        /// </summary>
        public object DefaultValue
        {
            get => GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        #endregion

        #region Overrides of ConverterBase

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value is bool boolValue)
            {
                return boolValue
                    ? TrueValue
                    : FalseValue;
            }

            return DefaultValue;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}