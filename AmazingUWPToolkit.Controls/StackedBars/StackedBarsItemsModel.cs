using System.Collections.ObjectModel;

namespace AmazingUWPToolkit.Controls
{
    internal class StackedBarsItemsModel
    {
        #region Properties

        public ObservableCollection<StackedBarItem> Items { get; set; }

        public StackedBarOrientation Orientation { get; set; }

        public int ItemsCount => Items?.Count ?? 0;

        #endregion
    }
}