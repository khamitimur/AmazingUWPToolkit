using System.Collections.ObjectModel;

namespace AmazingUWPToolkit.Controls
{
    internal interface ITextBoard
    {
        #region Properties

        ObservableCollection<ITextBoardItemModel> Items { get; }

        string Text { get; set; }

        string RandomCharsSet { get; set; }

        int ColumnsCount { get; set; }

        int RowsCount { get; set; }

        double FontSizeToItemSizeRatio { get; set; }

        double MaxItemWidth { get; set; }

        double MaxItemHeight { get; set; }

        double RandomTextBoardItemOpacity { get; set; }

        #endregion
    }
}