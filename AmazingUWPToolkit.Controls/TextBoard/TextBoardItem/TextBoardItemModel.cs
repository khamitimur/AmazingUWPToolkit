using System;
using System.ComponentModel;
using JetBrains.Annotations;

namespace AmazingUWPToolkit.Controls
{
    public class TextBoardItemModel : ITextBoardItemModel, INotifyPropertyChanged
    {
        #region Constructor

        public TextBoardItemModel(char @char, bool isRandom = false)
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

        public ITextBoardItemModel PreviousState { get; private set; }

        public char Char { get; private set; }

        public bool IsRandom { get; private set; }

        public void Update([NotNull] ITextBoardItemModel textBoardItemModel)
        {
            if (Char == textBoardItemModel.Char &&
                IsRandom == textBoardItemModel.IsRandom)
            {
                return;
            }

            PreviousState = new TextBoardItemModel(Char, IsRandom);

            Char = textBoardItemModel.Char;
            IsRandom = textBoardItemModel.IsRandom;

            Updated?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}