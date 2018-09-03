using System;

namespace AmazingUWPToolkit.Controls
{
    public interface ITextBoardItem
    {
        #region Properties

        ITextBoardItem PreviousState { get; }

        char Char { get; }

        bool IsRandom { get; }

        #endregion

        #region Events

        event EventHandler<EventArgs> Updated;

        #endregion

        #region Methods

        void Update(ITextBoardItem textBoardItem);

        #endregion
    }
}