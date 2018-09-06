using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Controls
{
    public class StackedItemsPanel : Grid
    {
        #region Dependency Properties

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(Orientation),
            typeof(StackedItemsPanel),
            new PropertyMetadata(default(Orientation), OnOrientationPropertyChanged));

        #endregion

        #region Properties

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        #endregion

        #region Private Methods

        private static void OnOrientationPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as StackedItemsPanel)?.SetColumnsAndRows();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            SetColumnsAndRows();

            return base.MeasureOverride(availableSize);
        }

        private void SetColumnsAndRows()
        {
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();

            var gridLength = new GridLength(1, GridUnitType.Star);

            if (Children == null || Children.Count == 0)
                return;

            if (Orientation == Orientation.Horizontal)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    ColumnDefinitions.Add(new ColumnDefinition() { Width = gridLength });
                }
            }
            else
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    RowDefinitions.Add(new RowDefinition() { Height = gridLength });
                }
            }

            SetChildren();
        }

        private void SetChildren()
        {
            if (Children == null || Children.Count == 0)
                return;

            for (int i = 0; i < Children.Count; i++)
            {
                if (!(Children[i] is ContentPresenter child))
                    continue;

                if (!(child.Content is IStackedItem stackedItem))
                    continue;

                var gridLength = new GridLength(stackedItem.Value, GridUnitType.Star);
                var spanValue = Children.Count - i;

                if (Orientation == Orientation.Horizontal)
                {
                    SetColumnSpan(child, spanValue);

                    ColumnDefinitions[spanValue - 1].Width = gridLength;
                }
                else
                {
                    SetRow(child, i);
                    SetRowSpan(child, spanValue);

                    RowDefinitions[i].Height = gridLength;
                }
            }
        }

        #endregion
    }
}