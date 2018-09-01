using Microsoft.Xaml.Interactivity;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Controls.Behaviors
{
    internal class StackedBarItemsGridBehavior : Behavior<Grid>
    {
        #region Dependency Properties

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            nameof(Items),
            typeof(ObservableCollection<StackedBarItem>),
            typeof(StackedBarItemsGridBehavior),
            new PropertyMetadata(null, OnItemsPropertyChanged));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(StackedBarOrientation),
            typeof(StackedBarItemsGridBehavior),
            new PropertyMetadata(default(Orientation)));

        #endregion

        #region Properties

        public ObservableCollection<StackedBarItem> Items
        {
            get => (ObservableCollection<StackedBarItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public StackedBarOrientation Orientation
        {
            get => (StackedBarOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        #endregion

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded += OnLoaded;
            }

            TryToSetGrid();

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= OnLoaded;
            }

            base.OnDetaching();
        }

        #endregion

        #region Private Methods

        private static void OnItemsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is StackedBarItemsGridBehavior stackedBarItemsGridBehavior)
            {
                stackedBarItemsGridBehavior.TryToSetGrid();

                if (e.OldValue is INotifyCollectionChanged oldValue)
                {
                    oldValue.CollectionChanged -= stackedBarItemsGridBehavior.OnItemsCollectionChanged;
                }

                if (e.NewValue is INotifyCollectionChanged newValue)
                {
                    newValue.CollectionChanged += stackedBarItemsGridBehavior.OnItemsCollectionChanged;
                }
            }
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TryToSetGrid();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TryToSetChildren();
        }

        private void TryToSetGrid()
        {
            if (AssociatedObject == null || Items == null)
                return;

            AssociatedObject.ColumnDefinitions.Clear();
            AssociatedObject.RowDefinitions.Clear();

            var itemsCount = Items.Count();
            if (itemsCount == 0)
                return;

            var gridLength = new GridLength(1, GridUnitType.Star);

            if (Orientation == StackedBarOrientation.Horizontal)
            {
                for (int i = 0; i < itemsCount; i++)
                {
                    AssociatedObject.ColumnDefinitions.Add(new ColumnDefinition() { Width = gridLength });
                }
            }
            else
            {
                for (int i = 0; i < itemsCount; i++)
                {
                    AssociatedObject.RowDefinitions.Add(new RowDefinition() { Height = gridLength });
                }
            }

            TryToSetChildren();
        }

        private void TryToSetChildren()
        {
            if (AssociatedObject?.Children?.Count == 0 ||
                Items?.Count == 0)
            {
                return;
            }

            var valueDivider = Items.Sum(item => item.Value) / 100;

            for (int i = 0; i < AssociatedObject.Children.Count; i++)
            {
                if (!(AssociatedObject.Children[i] is ContentPresenter child))
                    continue;

                if (!(child.Content is StackedBarItem stackedBarItem))
                    continue;

                var dividedValue = stackedBarItem.Value / valueDivider;
                var gridLength = new GridLength(dividedValue, GridUnitType.Star);

                if (Orientation == StackedBarOrientation.Horizontal)
                {
                    Grid.SetColumn(child, i);

                    AssociatedObject.ColumnDefinitions[i].Width = gridLength;
                }
                else
                {
                    Grid.SetRow(child, i);

                    AssociatedObject.RowDefinitions[i].Height = gridLength;
                }
            }
        }

        #endregion
    }
}