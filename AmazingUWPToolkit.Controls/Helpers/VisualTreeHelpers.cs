using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace AmazingUWPToolkit.Controls.Helpers
{
    public class VisualTreeHelpers : IVisualTreeHelpers
    {
        #region Implementation of IVisualTreeHelpers

        public T FindChildByName<T>(FrameworkElement element, string name) where T : FrameworkElement
        {
            var childElement = default(T);

            var childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < childrenCount; i++)
            {
                if (!(VisualTreeHelper.GetChild(element, i) is FrameworkElement child))
                    continue;

                if (child is T && child.Name.Equals(name))
                {
                    childElement = (T)child;

                    break;
                }

                childElement = FindChildByName<T>(child, name);

                if (childElement != null)
                    break;
            }

            return childElement;
        }

        #endregion
    }
}