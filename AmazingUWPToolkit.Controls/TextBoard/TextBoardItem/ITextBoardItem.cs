using JetBrains.Annotations;
using System;

namespace AmazingUWPToolkit.Controls
{
    public interface ITextBoardItem
    {
        #region Properties

        [CanBeNull]
        ITextBoardItem PreviousState { get; }

        char Char { get; }

        bool IsRandom { get; }

        #endregion

        #region Events

        event EventHandler<EventArgs> Updated;

        #endregion

        #region Methods

        void Update([NotNull] ITextBoardItem textBoardItem);

        #endregion
    }
}