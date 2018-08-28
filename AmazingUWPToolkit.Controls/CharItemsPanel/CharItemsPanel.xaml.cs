using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Controls
{
    public sealed partial class CharItemsPanel : UserControl, INotifyPropertyChanged
    {
        #region Fields

        private const double MAX_ITEM_WIDTH = 60;
        private const double MAX_ITEM_HEIGHT = 60;

        private const double CHARITEMCONTROL_MAX_FONTSIZE = 40d;
        private readonly double CHARITEMCONTROL_FONTSIZE_RATIO;

        private const int MIN_SPACE_LENGTH_BETWEEN_WORDS = 1;

        private readonly ICharItemsCollectionHelper charItemsCollectionService;

        private double wrapGridMaxWidth;
        private double wrapGridMaxHeight;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(CharItemsPanel),
            new PropertyMetadata(string.Empty, OnTextPropertyChanged));

        #endregion

        #region Constructor

        public CharItemsPanel()
        {
            InitializeComponent();

            CHARITEMCONTROL_FONTSIZE_RATIO = Math.Min(MAX_ITEM_WIDTH, MAX_ITEM_HEIGHT) / CHARITEMCONTROL_MAX_FONTSIZE;

            charItemsCollectionService = new CharItemsCollectionHelper();

            (Content as FrameworkElement).DataContext = this;

            ItemWidth = MAX_ITEM_WIDTH;
            ItemHeight = MAX_ITEM_HEIGHT;
            CharItemControlMaxFontSize = CHARITEMCONTROL_MAX_FONTSIZE;

            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Properties

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        internal ICharItemsCollectionHelper CharItemsCollectionHelper => charItemsCollectionService;

        public double ItemWidth { get; private set; }

        public double ItemHeight { get; private set; }

        public double CharItemControlMaxFontSize { get; private set; }

        public double WrapGridMaxWidth { get; private set; }

        public double WrapGridMaxHeight { get; private set; }

        public double AvailableWidth => ActualWidth - (Padding.Left + Padding.Right);

        public double AvailableHeight => ActualHeight - (Padding.Top + Padding.Bottom);

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Methods

        private static void OnTextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is CharItemsPanel charItemsPanel)
            {
                charItemsPanel.SetText();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CalculateItemSize();
            SetText();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalculateItemSize();
        }

        private void CalculateItemSize()
        {
            if (AvailableWidth <= 0 ||
                AvailableHeight <= 0)
                return;

            if (AvailableWidth / MAX_ITEM_WIDTH >= CharItemsCollectionHelper.ColumnsCount &&
                AvailableHeight / MAX_ITEM_HEIGHT >= CharItemsCollectionHelper.RowsCount)
            {
                ItemWidth = MAX_ITEM_WIDTH;
                ItemHeight = MAX_ITEM_HEIGHT;
            }
            else
            {
                var calculatedItemWidth = AvailableWidth / CharItemsCollectionHelper.ColumnsCount;
                var calculatedItemHeight = AvailableHeight / CharItemsCollectionHelper.RowsCount;

                ItemWidth = Math.Min(calculatedItemWidth, calculatedItemHeight);
                ItemHeight = ItemWidth;
            }

            CalculateCharItemFontSize();
            CalculateWrapGridMaxSize();
        }

        private void CalculateCharItemFontSize()
        {
            CharItemControlMaxFontSize = Math.Min(ItemWidth, ItemHeight) / CHARITEMCONTROL_FONTSIZE_RATIO;
        }

        private void CalculateWrapGridMaxSize()
        {
            WrapGridMaxWidth = ItemWidth * CharItemsCollectionHelper.ColumnsCount;
            WrapGridMaxHeight = ItemHeight * CharItemsCollectionHelper.RowsCount;
        }

        private void SetText()
        {
            if (!CharItemsCollectionHelper.IsInitialized)
                return;

            if (string.IsNullOrWhiteSpace(Text))
            {
                CharItemsCollectionHelper.Reset();
            }
            else
            {
                var charItemsToUpdateDictionary = new Dictionary<int, CharItem>();

                var index = 0;

                var textLines = Text.Split(' ');
                foreach (var textLine in textLines)
                {
                    var isFirstWordInLine = true;

                    var textWords = textLine.Split(' ');
                    foreach (var textWord in textWords)
                    {
                        if (index != 0)
                        {
                            if (isFirstWordInLine)
                            {
                                if (!IsLineEnded(index))
                                {
                                    index = GetNewLineTextStartIndex(index);
                                }
                            }
                            else
                            {
                                var availableSpaceLength = CharItemsCollectionHelper.ColumnsCount - ((index % CharItemsCollectionHelper.ColumnsCount) + MIN_SPACE_LENGTH_BETWEEN_WORDS);
                                if (availableSpaceLength >= textWord.Length)
                                {
                                    if (!IsLineEnded(index))
                                    {
                                        index += MIN_SPACE_LENGTH_BETWEEN_WORDS;
                                    }
                                }
                                else
                                {
                                    index = GetNewLineTextStartIndex(index);
                                }
                            }
                        }

                        for (int i = 0; i < textWord.Length; i++)
                        {
                            charItemsToUpdateDictionary.Add(index + i, new CharItem(textWord[i]));
                        }

                        index += textWord.Length;

                        isFirstWordInLine = false;
                    }
                }

                CharItemsCollectionHelper.Update(new ReadOnlyDictionary<int, CharItem>(charItemsToUpdateDictionary));
            }
        }

        private bool IsLineEnded(int currentIndex)
        {
            return currentIndex % CharItemsCollectionHelper.ColumnsCount == 0;
        }

        private int GetNewLineTextStartIndex(int currentIndex)
        {
            return (currentIndex - (currentIndex % CharItemsCollectionHelper.ColumnsCount)) + CharItemsCollectionHelper.ColumnsCount;
        }

        #endregion
    }
}
