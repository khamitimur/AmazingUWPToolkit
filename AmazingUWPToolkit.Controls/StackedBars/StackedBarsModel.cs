using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Collections.ObjectModel;

namespace AmazingUWPToolkit.Controls
{
    internal class StackedBarsModel
    {
        #region Properties

        public ObservableCollection<StackedBarItem> Items { get; set; }

        public int ItemsCount => Items?.Count ?? 0;

        public StackedBarOrientation Orientation { get; set; }

        public double AnimationDuration { get; set; }

        public EasingType AnimationEasingType { get; set; }

        #endregion
    }
}