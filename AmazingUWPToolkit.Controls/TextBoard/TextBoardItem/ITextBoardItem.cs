namespace AmazingUWPToolkit.Controls
{
    internal interface ITextBoardItem
    {
        #region Properties

        ITextBoardItemModel Model { get; set; }

        double RandomTextBoardItemOpacity { get; set; }

        #endregion
    }
}