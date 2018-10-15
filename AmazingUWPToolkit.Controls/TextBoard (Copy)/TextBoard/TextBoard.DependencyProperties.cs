using JetBrains.Annotations;
using Windows.UI.Xaml;

namespace AmazingUWPToolkit.Controls
{
    public partial class TextBoard : ITextBoard
    {
        #region Fields

        private const int MIN_SPACE_LENGTH_BETWEEN_WORDS = 1;

        private const double DEFAULT_FONTSIZE_TO_ITEMSIZE_RATIO = 2d;
        private const double DEFAULT_MAX_ITEM_WIDTH = 60;
        private const double DEFAULT_MAX_ITEM_HEIGHT = 60;
        private const double DEFAULT_RANDOM_TEXTBOARDITEM_OPACITY = 0.05;

        #endregion

        #region Dependency Properties

        internal static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            nameof(Model),
            typeof(ITextBoardModel),
            typeof(TextBoard),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(TextBoard),
            new PropertyMetadata(null, OnTextPropertyChanged));

        public static readonly DependencyProperty RandomCharsSetProperty = DependencyProperty.Register(
            nameof(RandomCharsSet),
            typeof(string),
            typeof(TextBoard),
            new PropertyMetadata(null, OnRandomCharsSetPropertyChanged));

        public static readonly DependencyProperty ColumnsCountProperty = DependencyProperty.Register(
            nameof(ColumnsCount),
            typeof(int),
            typeof(TextBoard),
            new PropertyMetadata(0, OnColumnsCountPropertyChanged));

        public static readonly DependencyProperty RowsCountProperty = DependencyProperty.Register(
            nameof(RowsCount),
            typeof(int),
            typeof(TextBoard),
            new PropertyMetadata(0, OnRowsCountPropertyChanged));

        public static readonly DependencyProperty FontSizeToItemSizeRatioProperty = DependencyProperty.Register(
            nameof(FontSizeToItemSizeRatio),
            typeof(double),
            typeof(TextBoard),
            new PropertyMetadata(DEFAULT_FONTSIZE_TO_ITEMSIZE_RATIO, OnFontSizeToItemSizeRatioPropertyChanged));

        public static readonly DependencyProperty MaxItemWidthProperty = DependencyProperty.Register(
            nameof(MaxItemWidth),
            typeof(double),
            typeof(TextBoard),
            new PropertyMetadata(DEFAULT_MAX_ITEM_WIDTH, OnMaxItemSizeChanged));

        public static readonly DependencyProperty MaxItemHeightProperty = DependencyProperty.Register(
            nameof(MaxItemHeight),
            typeof(double),
            typeof(TextBoard),
            new PropertyMetadata(DEFAULT_MAX_ITEM_HEIGHT, OnMaxItemSizeChanged));

        public static readonly DependencyProperty RandomTextBoardItemOpacityProperty = DependencyProperty.Register(
            nameof(RandomTextBoardItemOpacity),
            typeof(double),
            typeof(TextBoard),
            new PropertyMetadata(DEFAULT_RANDOM_TEXTBOARDITEM_OPACITY, OnRandomTextBoardItemOpacityPropertyChanged));

        #endregion

        #region Properties

        [NotNull]
        internal ITextBoardModel Model
        {
            get => (ITextBoardModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        #endregion

        #region Implementation of ITextBoard

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string RandomCharsSet
        {
            get => (string)GetValue(RandomCharsSetProperty);
            set => SetValue(RandomCharsSetProperty, value);
        }

        public int ColumnsCount
        {
            get => (int)GetValue(ColumnsCountProperty);
            set => SetValue(ColumnsCountProperty, value);
        }

        public int RowsCount
        {
            get => (int)GetValue(RowsCountProperty);
            set => SetValue(RowsCountProperty, value);
        }

        public double FontSizeToItemSizeRatio
        {
            get => (double)GetValue(FontSizeToItemSizeRatioProperty);
            set => SetValue(FontSizeToItemSizeRatioProperty, value);
        }

        public double MaxItemWidth
        {
            get => (double)GetValue(MaxItemWidthProperty);
            set => SetValue(MaxItemWidthProperty, value);
        }

        public double MaxItemHeight
        {
            get => (double)GetValue(MaxItemHeightProperty);
            set => SetValue(MaxItemHeightProperty, value);
        }

        public double RandomTextBoardItemOpacity
        {
            get => (double)GetValue(RandomTextBoardItemOpacityProperty);
            set => SetValue(RandomTextBoardItemOpacityProperty, value);
        }

        #endregion

        #region Private Methods

        private static void OnTextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoard)?.SetText();
        }

        private static void OnRandomCharsSetPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoard).Initialize();
        }

        private static void OnColumnsCountPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoard).Initialize();
        }

        private static void OnRowsCountPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoard).Initialize();
        }

        private static void OnFontSizeToItemSizeRatioPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoard).SetItemFontSize();
        }

        private static void OnMaxItemSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoard).SetModel();
        }

        private static void OnRandomTextBoardItemOpacityPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoard).SetRandomTextBoardItemOpacity();
        }

        #endregion
    }
}