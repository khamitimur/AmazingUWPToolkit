using JetBrains.Annotations;
using System.ComponentModel;

namespace AmazingUWPToolkit.Controls
{
    internal class TextBoardItem : ITextBoardItem, INotifyPropertyChanged
    {
        #region Contructor

        public TextBoardItem(char @char, bool isRandom)
        {
            Char = @char;
            IsRandom = isRandom;
        }

        #endregion

        #region Overrides of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Overrides of ITextBoardItem

        public char Char { get; private set; }

        public bool IsRandom { get; private set; }

        public void Update([NotNull] ITextBoardItem textBoardItem)
        {
            Char = textBoardItem.Char;
            IsRandom = textBoardItem.IsRandom;
        }

        #endregion
    }
}