namespace AmazingUWPToolkit.Controls
{
    internal interface ITextBoardItem
    {
        #region Properties

        ITextBoardItemModel TextBoardItemModel { get; set; }

        double RandomTextBoardItemOpacity { get; set; }

        #endregion
    }
}