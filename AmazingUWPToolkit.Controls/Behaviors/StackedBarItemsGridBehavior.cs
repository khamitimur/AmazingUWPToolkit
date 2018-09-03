using Microsoft.Xaml.Interactivity;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Controls.Behaviors
{
    internal class StackedBarItemsGridBehavior : Behavior<Grid>
    {
        #region Dependency Properties

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            nameof(Model),
            typeof(StackedBarsItemsModel),
            typeof(StackedBarItemsGridBehavior),
            new PropertyMetadata(null, OnModelPropertyChanged));

        #endregion

        #region Properties

        public StackedBarsItemsModel Model
        {
            get => (StackedBarsItemsModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        private bool CanSetGrid => AssociatedObject != null && Model?.Items != null;

        private bool CanSetChildren =>
            AssociatedObject?.Children != null &&
            AssociatedObject.Children.Count != 0 &&
            Model != null &&
            Model.ItemsCount != 0 &&
            Model.ItemsCount == AssociatedObject.Children.Count;


        #endregion

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.LayoutUpdated += OnLayoutUpdated;
            }

            TryToSetGrid();

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.LayoutUpdated -= OnLayoutUpdated;
            }

            base.OnDetaching();
        }

        #endregion

        #region Private Methods

        private static void OnModelPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is StackedBarItemsGridBehavior stackedBarItemsGridBehavior)
            {
                stackedBarItemsGridBehavior.TryToSetGrid();

                if (e.OldValue is StackedBarsItemsModel oldStackedBarsItemsPanelModel &&
                    oldStackedBarsItemsPanelModel.Items is INotifyCollectionChanged oldItems)
                {
                    oldItems.CollectionChanged -= stackedBarItemsGridBehavior.OnItemsCollectionChanged;
                }

                if (e.NewValue is StackedBarsItemsModel newStackedBarsItemsPanelModel &&
                    newStackedBarsItemsPanelModel.Items is INotifyCollectionChanged newItems)
                {
                    newItems.CollectionChanged += stackedBarItemsGridBehavior.OnItemsCollectionChanged;
                }
            }
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TryToSetGrid();
        }

        private void OnLayoutUpdated(object sender, object e)
        {
            TryToSetChildren();
        }

        private void TryToSetGrid()
        {
            if (!CanSetGrid)
                return;

            AssociatedObject.ColumnDefinitions.Clear();
            AssociatedObject.RowDefinitions.Clear();

            if (Model.ItemsCount == 0)
                return;

            var gridLength = new GridLength(1, GridUnitType.Star);

            if (Model.Orientation == StackedBarOrientation.Horizontal)
            {
                for (int i = 0; i < Model.ItemsCount; i++)
                {
                    AssociatedObject.ColumnDefinitions.Add(new ColumnDefinition() { Width = gridLength });
                }
            }
            else
            {
                for (int i = 0; i < Model.ItemsCount; i++)
                {
                    AssociatedObject.RowDefinitions.Add(new RowDefinition() { Height = gridLength });
                }
            }

            TryToSetChildren();
        }

        private void TryToSetChildren()
        {
            if (!CanSetChildren)
                return;

            var valueDivider = Model.Items.Sum(item => item.Value) / 100;

            for (int i = 0; i < AssociatedObject.Children.Count; i++)
            {
                if (!(AssociatedObject.Children[i] is ContentPresenter child))
                    continue;

                if (!(child.Content is StackedBarItem stackedBarItem))
                    continue;

                var dividedValue = stackedBarItem.Value / valueDivider;
                var gridLength = new GridLength(dividedValue, GridUnitType.Star);

                if (Model.Orientation == StackedBarOrientation.Horizontal)
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