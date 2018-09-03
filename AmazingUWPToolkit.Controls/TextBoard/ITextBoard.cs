using System.Collections.ObjectModel;

namespace AmazingUWPToolkit.Controls
{
    internal interface ITextBoard
    {
        #region Properties

        ObservableCollection<ITextBoardItem> Items { get; }

        string Text { get; set; }

        string RandomCharsSet { get; set; }

        int ColumnsCount { get; set; }

        int RowsCount { get; set; }

        #endregion
    }
}