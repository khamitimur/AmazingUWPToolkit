using System;
using System.ComponentModel;
using JetBrains.Annotations;

namespace AmazingUWPToolkit.Controls
{
    public class TextBoardItem : ITextBoardItem, INotifyPropertyChanged
    {
        #region Constructor

        public TextBoardItem(char @char, bool isRandom = false)
        {
            Char = @char;
            IsRandom = isRandom;
        }

        #endregion

        #region Overrides of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Overrides of object

        public override string ToString()
        {
            return Char.ToString();
        }

        #endregion

        #region Overrides of ICharItem

        public event EventHandler<EventArgs> Updated;

        [CanBeNull]
        public ITextBoardItem PreviousState { get; private set; }

        public char Char { get; private set; }

        public bool IsRandom { get; private set; }

        public void Update([NotNull] ITextBoardItem textBoardItem)
        {
            if (Char == textBoardItem.Char &&
                IsRandom == textBoardItem.IsRandom)
            {
                return;
            }

            PreviousState = new TextBoardItem(Char, IsRandom);

            Char = textBoardItem.Char;
            IsRandom = textBoardItem.IsRandom;

            Updated?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}