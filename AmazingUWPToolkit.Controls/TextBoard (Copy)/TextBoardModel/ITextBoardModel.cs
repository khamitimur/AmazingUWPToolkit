namespace AmazingUWPToolkit.Controls
{
    internal interface ITextBoardModel
    {
        #region Properties

        double ItemWidth { get; set; }

        double ItemHeight { get; set; }

        double WrapPanelMaxWidth { get; set; }

        double WrapPanelMaxHeight { get; set; }

        double FontSize { get; set; }

        double RandomTextBoardItemOpacity { get; set; }

        #endregion
    }
}