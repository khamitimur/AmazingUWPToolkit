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
    public sealed partial class TextBoard : Control, ITextBoard, INotifyPropertyChanged
    {
        #region Fields

        private Random charItemsRandom;
        private bool isInitialTextSet;

        #endregion

        #region Contructor

        public TextBoard()
        {
            DefaultStyleKey = typeof(TextBoard);

            charItemsRandom = new Random();

            Model = new TextBoardModel();
            Items = new ObservableCollection<ITextBoardItemModel>();

            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Properties

        internal bool IsInitialized { get; set; }

        internal double TextBoardItemControlMaxFontSize { get; set; }

        internal double AvailableWidth => ActualWidth - (Padding.Left + Padding.Right);

        internal double AvailableHeight => ActualHeight - (Padding.Top + Padding.Bottom);

        #endregion

        #region Implementation of ITextBoard

        [NotNull]
        public ObservableCollection<ITextBoardItemModel> Items { get; }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Overrides of Control

        protected override void OnApplyTemplate()
        {
            TryToSetText();

            base.OnApplyTemplate();
        }

        #endregion

        #region Private Methods

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TryToSetModel();
        }

        private void TryToInitialize()
        {
            if (string.IsNullOrWhiteSpace(RandomCharsSet) ||
                ColumnsCount == 0 ||
                RowsCount == 0)
            {
                return;
            }

            var textBoardItemModelsToAdd = new List<ITextBoardItemModel>();

            for (int i = 0; i < ColumnsCount * RowsCount; i++)
            {
                textBoardItemModelsToAdd.Add(GetRandomTextBoardItemModel());
            }

            foreach (var textBoardItemModelToAdd in textBoardItemModelsToAdd)
            {
                Items.Add(textBoardItemModelToAdd);
            }

            IsInitialized = true;

            TryToSetModel();

            if (!isInitialTextSet &&
                !string.IsNullOrWhiteSpace(Text))
            {
                TryToSetText();
            }
        }

        private ITextBoardItemModel GetRandomTextBoardItemModel()
        {
            var @char = RandomCharsSet[charItemsRandom.Next(0, RandomCharsSet.Length - 1)];

            return new TextBoardItemModel(@char, true);
        }

        private List<int> GetNotRandomTextBoardItemModelsIndexes()
        {
            var notRandomCTextBoardItemsIndexes = new List<int>();

            foreach (var notRandomTextBoardItem in Items.Where(textBoardItemModel => !textBoardItemModel.IsRandom))
            {
                notRandomCTextBoardItemsIndexes.Add(Items.IndexOf(notRandomTextBoardItem));
            }

            return notRandomCTextBoardItemsIndexes;
        }

        private void TryToSetModel()
        {
            if (!IsInitialized)
                return;

            if (AvailableWidth <= 0 ||
                AvailableHeight <= 0)
                return;

            if (AvailableWidth / MaxItemWidth >= ColumnsCount &&
                AvailableHeight / MaxItemHeight >= RowsCount)
            {
                Model.ItemWidth = MaxItemWidth;
                Model.ItemHeight = MaxItemHeight;
            }
            else
            {
                var calculatedItemWidth = AvailableWidth / ColumnsCount;
                var calculatedItemHeight = AvailableHeight / RowsCount;

                Model.ItemWidth = Math.Min(calculatedItemWidth, calculatedItemHeight);
                Model.ItemHeight = Model.ItemWidth;
            }

            Model.WrapPanelMaxWidth = Model.ItemWidth * ColumnsCount;
            Model.WrapPanelMaxHeight = Model.ItemHeight * RowsCount;

            SetItemFontSize();
        }

        private void SetItemFontSize()
        {
            Model.FontSize = Math.Min(Model.ItemWidth, Model.ItemHeight) / FontSizeToItemSizeRatio;
        }

        private void SetRandomTextBoardItemOpacity()
        {
            Model.RandomTextBoardItemOpacity = RandomTextBoardItemOpacity;
        }

        private void TryToSetText()
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
                var textBoardItemModelsDictionary = new Dictionary<int, ITextBoardItemModel>();

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
                            textBoardItemModelsDictionary.Add(index + i, new TextBoardItemModel(textWord[i]));
                        }

                        index += textWord.Length;

                        isFirstWordInLine = false;
                    }
                }

                Update(new ReadOnlyDictionary<int, ITextBoardItemModel>(textBoardItemModelsDictionary));
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

        private void Update([NotNull, ItemNotNull] ReadOnlyDictionary<int, ITextBoardItemModel> textBoardItemModelsDictionary)
        {
            var notRandomTextBoardItemModelsIndexes = GetNotRandomTextBoardItemModelsIndexes();

            foreach (var textBoardItemModel in textBoardItemModelsDictionary)
            {
                var index = textBoardItemModel.Key;
                var textBoardItem = textBoardItemModel.Value;

                if (notRandomTextBoardItemModelsIndexes.Contains(index))
                {
                    notRandomTextBoardItemModelsIndexes.Remove(index);
                }

                Items[index].Update(textBoardItem);
            }

            foreach (var notRandomTextBoardItemModelIndex in notRandomTextBoardItemModelsIndexes)
            {
                Items[notRandomTextBoardItemModelIndex].Update(GetRandomTextBoardItemModel());
            }
        }

        private void Reset()
        {
            foreach (var notRandomTextBoardItemModel in Items.Where(textBoardItemModel => !textBoardItemModel.IsRandom))
            {
                notRandomTextBoardItemModel.Update(GetRandomTextBoardItemModel());
            }
        }

        #endregion
    }
}