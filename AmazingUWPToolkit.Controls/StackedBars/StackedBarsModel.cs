using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Controls
{
    internal class StackedBarsModel
    {
        #region Properties

        public Orientation Orientation { get; set; }

        public double AnimationDuration { get; set; }

        public EasingType AnimationEasingType { get; set; }

        #endregion
    }
}