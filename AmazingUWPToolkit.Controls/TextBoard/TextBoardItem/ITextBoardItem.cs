using JetBrains.Annotations;

namespace AmazingUWPToolkit.Controls
{
    internal interface ITextBoardItem
    {
        #region Properties

        char Char { get; }

        bool IsRandom { get; }

        #endregion

        #region Methods

        void Update([NotNull] ITextBoardItem textBoardItem);

        #endregion
    }
}