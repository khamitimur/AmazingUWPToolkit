using Windows.UI.Xaml;

namespace AmazingUWPToolkit.Controls.Helpers
{
    internal interface IVisualTreeHelpers
    {
        #region Methods

        T FindChildByName<T>(FrameworkElement element, string name) where T : FrameworkElement;

        #endregion
    }
}