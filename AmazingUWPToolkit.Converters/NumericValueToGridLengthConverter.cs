using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AmazingUWPToolkit.Converters
{
    /// <summary>
    /// Converts numeric value to <see cref="GridLength"/>.
    /// </summary>
    /// <remaks><para>
    /// Currently supports <see cref="int"/> and <see cref="double"/> values.
    /// </para></remaks>
    public class NumericValueToGridLengthConverter : DependencyObject,
                                                     IValueConverter
    {
        #region Dependency Properties

        /// <summary>
        /// Default <see cref="GridLength"/> to be used when convert is not possible.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Default value is <c>Auto</c>.
        /// <para>
        /// This dependency property can be accessed via the <see cref="DefaultValue"/> property.
        /// </para>
        /// </para>
        /// </remarks>
        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(
            nameof(DefaultValue),
            typeof(GridLength),
            typeof(NumericValueToGridLengthConverter),
            new PropertyMetadata(GridLength.Auto));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default <see cref="GridLength"/> to be used when convert is not possible.
        /// </summary>
        /// <remaks><para>
        /// Default value is <c>Auto</c>.
        /// </para></remaks>
        public GridLength DefaultValue
        {
            get => (GridLength)GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        #endregion

        #region Implementation of IValueConverter

        /// <inheritdoc />
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

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}