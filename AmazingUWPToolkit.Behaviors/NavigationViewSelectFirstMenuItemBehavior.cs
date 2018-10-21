using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Behaviors
{
    public class NavigationViewSelectFirstMenuItemBehavior : Behavior<NavigationView>
    {
        #region Overrides of Behavior

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += OnAssociatedObjectLoaded;

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnAssociatedObjectLoaded;

            base.OnDetaching();
        }

        #endregion

        #region Private Methods

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.MenuItems == null || AssociatedObject.MenuItems.Count == 0)
                return;

            AssociatedObject.SelectedItem = AssociatedObject.MenuItems[0];
        }

        #endregion
    }
}