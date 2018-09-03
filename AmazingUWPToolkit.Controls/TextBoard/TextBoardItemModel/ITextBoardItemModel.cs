using System;

namespace AmazingUWPToolkit.Controls
{
    public interface ITextBoardItemModel
    {
        #region Properties

        ITextBoardItemModel PreviousState { get; }

        char Char { get; }

        bool IsRandom { get; }

        #endregion

        #region Events

        event EventHandler<EventArgs> Updated;

        #endregion

        #region Methods

        void Update(ITextBoardItemModel textBoardItemModel);

        #endregion
    }
}