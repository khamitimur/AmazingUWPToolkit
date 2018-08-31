using JetBrains.Annotations;
using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace AmazingUWPToolkit.Controls
{
    public class StackedBarItem
    {
        #region Contructor

        /// <summary>
        /// Initializes new instance of <see cref="StackedBarItem"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="background">Background brush.</param>
        /// <param name="label">Label.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="background"/> is null.</exception>
        public StackedBarItem(double value, [NotNull] Brush background, [CanBeNull] string label = null)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} can't be less than zero.");

            Value = value;
            Background = background;
            Label = label;
        }

        /// <summary>
        /// Initializes new instance of <see cref="StackedBarItem"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="background">Background color.</param>
        /// <param name="label">Label.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than zero.</exception>
        public StackedBarItem(double value, Color background, [CanBeNull] string label = null)
            : this(value, new SolidColorBrush(background), label)
        {

        }

        #endregion

        #region Properties

        public double Value { get; }

        [NotNull]
        public Brush Background { get; }

        [CanBeNull]
        public string Label { get; }

        #endregion
    }
}