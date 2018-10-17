using AmazingUWPToolkit.Controls.Helpers;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Controls
{
    [TemplatePart(Name = GRIDVIEW_NAME, Type = typeof(GridView))]
    [TemplatePart(Name = ITEMS_PANEL_NAME, Type = typeof(VariableSizedWrapGrid))]
    public class TextBoard : Control
    {
        #region Fields

        private const string GRIDVIEW_NAME = "GridView";
        private const string ITEMS_PANEL_NAME = "ItemsPanel";

        private const int MIN_SPACE_LENGTH_BETWEEN_WORDS = 1;

        private const string DEFAULT_RANDOM_CHARS = "abcdefghijklmnopqrstuvwxyz1234567890";

        private readonly IVisualTreeHelpers visualTreeHelpers;

        private GridView gridView;
        private VariableSizedWrapGrid itemsPanel;

        private Random charItemsRandom;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(TextBoard),
            new PropertyMetadata(null, OnTextPropertyChanged));

        public static readonly DependencyProperty RandomCharsProperty = DependencyProperty.Register(
            nameof(RandomChars),
            typeof(string),
            typeof(TextBoard),
            new PropertyMetadata(null, OnRandomCharsPropertyChanged));

        public static readonly DependencyProperty ColumnsCountProperty = DependencyProperty.Register(
            nameof(ColumnsCount),
            typeof(int),
            typeof(TextBoard),
            new PropertyMetadata(10, OnColumnsCountPropertyChanged));

        public static readonly DependencyProperty RowsCountProperty = DependencyProperty.Register(
            nameof(RowsCount),
            typeof(int),
            typeof(TextBoard),
            new PropertyMetadata(10, OnRowsCountPropertyChanged));

        public static readonly DependencyProperty MinItemWidthProperty = DependencyProperty.Register(
            nameof(MinItemWidth),
            typeof(double),
            typeof(TextBoard),
            new PropertyMetadata(40d));

        public static readonly DependencyProperty MinItemHeightProperty = DependencyProperty.Register(
            nameof(MinItemHeight),
            typeof(double),
            typeof(TextBoard),
            new PropertyMetadata(40d));

        public static readonly DependencyProperty MaxItemWidthProperty = DependencyProperty.Register(
            nameof(MaxItemWidth),
            typeof(double),
            typeof(TextBoard),
            new PropertyMetadata(60d, OnMaxItemWidthPropertyChanged));

        public static readonly DependencyProperty MaxItemHeightProperty = DependencyProperty.Register(
            nameof(MaxItemHeight),
            typeof(double),
            typeof(TextBoard),
            new PropertyMetadata(60d, OnMaxItemHeightPropertyChanged));

        #endregion

        #region Contructor

        public TextBoard()
        {
            DefaultStyleKey = typeof(TextBoard);

            visualTreeHelpers = new VisualTreeHelpers();

            charItemsRandom = new Random();

            Items = new ObservableCollection<ITextBoardItem>();

            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Properties

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string RandomChars
        {
            get => (string)GetValue(RandomCharsProperty);
            set => SetValue(RandomCharsProperty, value);
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

        public double MinItemWidth
        {
            get => (double)GetValue(MinItemWidthProperty);
            set => SetValue(MinItemWidthProperty, value);
        }

        public double MinItemHeight
        {
            get => (double)GetValue(MinItemHeightProperty);
            set => SetValue(MinItemHeightProperty, value);
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

        private ObservableCollection<ITextBoardItem> Items { get; }

        private double AvailableWidth => ActualWidth - (Padding.Left + Padding.Right);

        private double AvailableHeight => ActualHeight - (Padding.Top + Padding.Bottom);

        #endregion

        #region Overrides of Control

        protected override void OnApplyTemplate()
        {
            gridView = GetTemplateChild(GRIDVIEW_NAME) as GridView;
            if (gridView != null)
            {
                gridView.Loaded += OnGridViewLoaded;

                gridView.ItemsSource = Items;
            }

            SetItems();
            SetText();

            base.OnApplyTemplate();
        }

        #endregion

        #region Private Methods

        private static void OnTextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoard)?.SetText();
        }

        private static void OnRandomCharsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string stringValue && string.IsNullOrEmpty(stringValue)) throw new ArgumentOutOfRangeException($"{nameof(RandomChars)} can't be null or empty.");

            (dependencyObject as TextBoard)?.SetItems();
        }

        private static void OnColumnsCountPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double doubleValue && doubleValue < 0) throw new ArgumentOutOfRangeException($"{nameof(ColumnsCount)} can't be lower than zero.");

            if (dependencyObject is TextBoard textBoard)
            {
                textBoard.SetItemsPanel();
                textBoard.SetItems();
            }
        }

        private static void OnRowsCountPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double doubleValue && doubleValue < 0) throw new ArgumentOutOfRangeException($"{nameof(RowsCount)} can't be lower than zero.");

            if (dependencyObject is TextBoard textBoard)
            {
                textBoard.SetItemsPanel();
                textBoard.SetItems();
            }
        }

        private static void OnMaxItemWidthPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double doubleValue && doubleValue < 0) throw new ArgumentOutOfRangeException($"{nameof(MaxItemWidth)} can't be lower than zero.");

            (dependencyObject as TextBoard)?.SetItemsPanel();
        }

        private static void OnMaxItemHeightPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double doubleValue && doubleValue < 0) throw new ArgumentOutOfRangeException($"{nameof(MaxItemHeight)} can't be lower than zero.");

            (dependencyObject as TextBoard)?.SetItemsPanel();
        }

        private void OnGridViewLoaded(object sender, RoutedEventArgs e)
        {
            itemsPanel = visualTreeHelpers.FindChildByName<VariableSizedWrapGrid>(gridView, ITEMS_PANEL_NAME);
            if (itemsPanel != null)
            {
                SetItemsPanel();
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetItemsPanel();
        }

        private void SetItemsPanel()
        {
            if (itemsPanel == null)
                return;

            if (AvailableWidth <= 0 ||
                AvailableHeight <= 0)
                return;

            if (AvailableWidth / MaxItemWidth >= ColumnsCount &&
                AvailableHeight / MaxItemHeight >= RowsCount)
            {
                itemsPanel.ItemWidth = MaxItemWidth;
                itemsPanel.ItemHeight = MaxItemHeight;
            }
            else
            {
                var calculatedItemWidth = AvailableWidth / ColumnsCount;
                var calculatedItemHeight = AvailableHeight / RowsCount;

                var minValue = Math.Min(calculatedItemWidth, calculatedItemHeight);

                itemsPanel.ItemWidth = minValue > MinItemWidth ? minValue : MinItemWidth;
                itemsPanel.ItemHeight = minValue > MinItemHeight ? minValue : MinItemHeight;
            }

            //itemsPanel.MinWidth = MinItemWidth * ColumnsCount;
            //itemsPanel.MinHeight = MinItemHeight * RowsCount;
            itemsPanel.MaxWidth = itemsPanel.ItemWidth * ColumnsCount;
            itemsPanel.MaxHeight = itemsPanel.ItemHeight * RowsCount;
        }

        private void SetItems()
        {
            if (string.IsNullOrWhiteSpace(RandomChars) || ColumnsCount == 0 || RowsCount == 0)
                return;

            var desiredItemsCount = ColumnsCount * RowsCount;
            if (desiredItemsCount == 0)
                return;

            if (desiredItemsCount > Items.Count)
            {
                for (int i = Items.Count; i < desiredItemsCount; i++)
                {
                    Items.Add(GetRandomTextBoardItem());
                }
            }
            else if (desiredItemsCount < Items.Count)
            {
                for (int i = Items.Count - 1; i >= desiredItemsCount; i--)
                {
                    Items.RemoveAt(i);
                }
            }

            if (!string.IsNullOrWhiteSpace(Text))
            {
                SetText();
            }
        }

        private void SetText()
        {
            if (Items.Count == 0)
                return;

            if (string.IsNullOrWhiteSpace(Text))
            {
                ResetText();
            }
            else
            {
                var textBoardItemsDictionary = new Dictionary<int, ITextBoardItem>();

                var currentIndex = 0;

                var textLines = Text.Split(new string[] { "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var textLine in textLines)
                {
                    var isFirstWordInLine = true;

                    var textWords = textLine.Split(' ');
                    foreach (var textWord in textWords)
                    {
                        if (currentIndex != 0)
                        {
                            if (isFirstWordInLine)
                            {
                                if (!IsLineEnded(currentIndex))
                                {
                                    currentIndex = GetNewLineTextStartIndex(currentIndex);
                                }
                            }
                            else
                            {
                                var availableSpaceLength = ColumnsCount - ((currentIndex % ColumnsCount) + MIN_SPACE_LENGTH_BETWEEN_WORDS);
                                if (availableSpaceLength >= textWord.Length)
                                {
                                    if (!IsLineEnded(currentIndex))
                                    {
                                        currentIndex += MIN_SPACE_LENGTH_BETWEEN_WORDS;
                                    }
                                }
                                else
                                {
                                    currentIndex = GetNewLineTextStartIndex(currentIndex);
                                }
                            }
                        }

                        for (int i = 0; i < textWord.Length; i++)
                        {
                            textBoardItemsDictionary.Add(currentIndex + i, new TextBoardItem(textWord[i], false));
                        }

                        currentIndex += textWord.Length;

                        isFirstWordInLine = false;
                    }
                }

                Update(new ReadOnlyDictionary<int, ITextBoardItem>(textBoardItemsDictionary));
            }
        }

        private void ResetText()
        {
            foreach (var notRandomTextBoardItem in Items.Where(textBoardItem => !textBoardItem.IsRandom))
            {
                notRandomTextBoardItem.Update(GetRandomTextBoardItem());
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

        private void Update([NotNull, ItemNotNull] ReadOnlyDictionary<int, ITextBoardItem> textBoardItemsDictionary)
        {
            var notRandomTextBoardItemModelsIndexes = GetNotRandomTextBoardItemIndexes();

            foreach (var textBoardItemModel in textBoardItemsDictionary)
            {
                var index = textBoardItemModel.Key;
                if (index > Items.Count - 1)
                    return;

                var textBoardItem = textBoardItemModel.Value;

                if (notRandomTextBoardItemModelsIndexes.Contains(index))
                {
                    notRandomTextBoardItemModelsIndexes.Remove(index);
                }

                Items[index].Update(textBoardItem);
            }

            foreach (var notRandomTextBoardItemModelIndex in notRandomTextBoardItemModelsIndexes)
            {
                Items[notRandomTextBoardItemModelIndex].Update(GetRandomTextBoardItem());
            }
        }

        private List<int> GetNotRandomTextBoardItemIndexes()
        {
            var notRandomTextBoardItemIndexes = new List<int>();

            foreach (var notRandomTextBoardItem in Items.Where(textBoardItem => !textBoardItem.IsRandom))
            {
                notRandomTextBoardItemIndexes.Add(Items.IndexOf(notRandomTextBoardItem));
            }

            return notRandomTextBoardItemIndexes;
        }

        private ITextBoardItem GetRandomTextBoardItem()
        {
            var @char = RandomChars[charItemsRandom.Next(0, RandomChars.Length - 1)];

            return new TextBoardItem(@char, true);
        }

        #endregion
    }
}