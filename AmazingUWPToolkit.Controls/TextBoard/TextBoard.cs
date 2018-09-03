using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Controls
{
    public sealed class TextBoard : Control, ITextBoard, INotifyPropertyChanged
    {
        #region Fields

        private readonly double CHARITEMCONTROL_FONTSIZE_RATIO;

        private const double MAX_ITEM_WIDTH = 60;
        private const double MAX_ITEM_HEIGHT = 60;

        private const double CHARITEMCONTROL_MAX_FONTSIZE = 40d;

        private const int MIN_SPACE_LENGTH_BETWEEN_WORDS = 1;

        private Random charItemsRandom;
        private bool isInitialTextSet;

        #endregion

        #region Dependency Properties

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

        #endregion

        #region Contructor

        public TextBoard()
        {
            DefaultStyleKey = typeof(TextBoard);

            charItemsRandom = new Random();

            Items = new ObservableCollection<ITextBoardItem>();

            DataContext = this;

            CHARITEMCONTROL_FONTSIZE_RATIO = Math.Min(MAX_ITEM_WIDTH, MAX_ITEM_HEIGHT) / CHARITEMCONTROL_MAX_FONTSIZE;

            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Properties

        internal bool IsInitialized { get; set; }

        internal double ItemWidth { get; set; }

        internal double ItemHeight { get; set; }

        internal double TextBoardItemControlMaxFontSize { get; set; }

        internal double WrapPanelMaxWidth { get; set; }

        internal double WrapPanelMaxHeight { get; set; }

        internal double AvailableWidth => ActualWidth - (Padding.Left + Padding.Right);

        internal double AvailableHeight => ActualHeight - (Padding.Top + Padding.Bottom);

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Implementation of ITextBoard

        [NotNull]
        public ObservableCollection<ITextBoardItem> Items { get; }

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

        #endregion

        #region Overrides of Control

        protected override void OnApplyTemplate()
        {
            SetText();

            base.OnApplyTemplate();
        }

        #endregion

        #region Private Methods

        private static void OnTextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoard)?.SetText();
        }

        private static void OnRandomCharsSetPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is TextBoard textBoard)
            {
                textBoard.TryInitialize();
            }
        }

        private static void OnColumnsCountPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is TextBoard textBoard)
            {
                textBoard.TryInitialize();
            }
        }

        private static void OnRowsCountPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is TextBoard textBoard)
            {
                textBoard.TryInitialize();
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetSizes();
        }

        private void TryInitialize()
        {
            if (string.IsNullOrWhiteSpace(RandomCharsSet) ||
                ColumnsCount == 0 ||
                RowsCount == 0)
            {
                return;
            }

            var textBoardItemsToAdd = new List<ITextBoardItem>();

            for (int i = 0; i < ColumnsCount * RowsCount; i++)
            {
                textBoardItemsToAdd.Add(GetRandomTextBoardItem());
            }

            foreach (var textBoardItemToAdd in textBoardItemsToAdd)
            {
                Items.Add(textBoardItemToAdd);
            }

            IsInitialized = true;

            SetSizes();

            if (!isInitialTextSet &&
                !string.IsNullOrWhiteSpace(Text))
            {
                SetText();
            }
        }

        private ITextBoardItem GetRandomTextBoardItem()
        {
            var @char = RandomCharsSet[charItemsRandom.Next(0, RandomCharsSet.Length - 1)];

            return new TextBoardItem(@char, true);
        }

        private List<int> GetNotRandomTextBoardItemsIndexes()
        {
            var notRandomCTextBoardItemsIndexes = new List<int>();

            foreach (var notRandomTextBoardItem in Items.Where(textBoardItem => !textBoardItem.IsRandom))
            {
                notRandomCTextBoardItemsIndexes.Add(Items.IndexOf(notRandomTextBoardItem));
            }

            return notRandomCTextBoardItemsIndexes;
        }

        private void SetSizes()
        {
            if (!IsInitialized)
                return;

            if (AvailableWidth <= 0 ||
                AvailableHeight <= 0)
                return;

            if (AvailableWidth / MAX_ITEM_WIDTH >= ColumnsCount &&
                AvailableHeight / MAX_ITEM_HEIGHT >= RowsCount)
            {
                ItemWidth = MAX_ITEM_WIDTH;
                ItemHeight = MAX_ITEM_HEIGHT;
            }
            else
            {
                var calculatedItemWidth = AvailableWidth / ColumnsCount;
                var calculatedItemHeight = AvailableHeight / RowsCount;

                ItemWidth = Math.Min(calculatedItemWidth, calculatedItemHeight);
                ItemHeight = ItemWidth;
            }

            TextBoardItemControlMaxFontSize = Math.Min(ItemWidth, ItemHeight) / CHARITEMCONTROL_FONTSIZE_RATIO;

            WrapPanelMaxWidth = ItemWidth * ColumnsCount;
            WrapPanelMaxHeight = ItemHeight * RowsCount;
        }

        private void SetText()
        {
            if (!IsInitialized)
                return;

            isInitialTextSet = true;

            if (string.IsNullOrWhiteSpace(Text))
            {
                Reset();
            }
            else
            {
                var textBoardItemsDictionary = new Dictionary<int, ITextBoardItem>();

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
                                var availableSpaceLength = ColumnsCount - ((index % ColumnsCount) + MIN_SPACE_LENGTH_BETWEEN_WORDS);
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
                            textBoardItemsDictionary.Add(index + i, new TextBoardItem(textWord[i]));
                        }

                        index += textWord.Length;

                        isFirstWordInLine = false;
                    }
                }

                Update(new ReadOnlyDictionary<int, ITextBoardItem>(textBoardItemsDictionary));
            }
        }

        private bool IsLineEnded(int currentIndex)
        {
            return currentIndex % ColumnsCount == 0;
        }

        private int GetNewLineTextStartIndex(int currentIndex)
        {
            return currentIndex - (currentIndex % ColumnsCount) + ColumnsCount;
        }

        private void Update([NotNull, ItemNotNull] ReadOnlyDictionary<int, ITextBoardItem> newTextBoardItemsDictionary)
        {
            var notRandomTextBoardItemsIndexes = GetNotRandomTextBoardItemsIndexes();

            foreach (var newTextBoardItem in newTextBoardItemsDictionary)
            {
                var index = newTextBoardItem.Key;
                var textBoardItem = newTextBoardItem.Value;

                if (notRandomTextBoardItemsIndexes.Contains(index))
                {
                    notRandomTextBoardItemsIndexes.Remove(index);
                }

                Items[index].Update(textBoardItem);
            }

            foreach (var notRandomCharsToResetIndex in notRandomTextBoardItemsIndexes)
            {
                Items[notRandomCharsToResetIndex].Update(GetRandomTextBoardItem());
            }
        }

        private void Reset()
        {
            foreach (var notTextBoardItem in Items.Where(textBoardItem => !textBoardItem.IsRandom))
            {
                notTextBoardItem.Update(GetRandomTextBoardItem());
            }
        }

        #endregion
    }
}