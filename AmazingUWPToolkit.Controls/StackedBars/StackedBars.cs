using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Controls
{
    public sealed class StackedBars : Control
    {
        #region Dependency Properties

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            nameof(Items),
            typeof(ObservableCollection<StackedBarItem>),
            typeof(StackedBars),
            new PropertyMetadata(null, OnItemsPropertyChanged));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(StackedBarOrientation),
            typeof(StackedBars),
            new PropertyMetadata(StackedBarOrientation.Horizontal, OnOrientationPropertyChanged));

        public static readonly DependencyProperty CornerRadiusProperty =  DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(StackedBars),
            new PropertyMetadata(default(CornerRadius)));

        internal static readonly DependencyProperty StackedBarsItemsModelProperty = DependencyProperty.Register(
            nameof(StackedBarsItemsModel),
            typeof(StackedBarsItemsModel),
            typeof(StackedBars),
            new PropertyMetadata(null));

        #endregion

        #region Contructor

        public StackedBars()
        {
            DefaultStyleKey = typeof(StackedBars);
        }

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

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        internal StackedBarsItemsModel StackedBarsItemsModel
        {
            get => (StackedBarsItemsModel)GetValue(StackedBarsItemsModelProperty);
            set => SetValue(StackedBarsItemsModelProperty, value);
        }

        #endregion

        #region Private Methods

        private static void OnItemsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as StackedBars)?.SetStackBarsItemsPanelModel();
        }

        private static void OnOrientationPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as StackedBars)?.SetStackBarsItemsPanelModel();
        }

        private void SetStackBarsItemsPanelModel()
        {
            StackedBarsItemsModel = new StackedBarsItemsModel
            {
                Items = Items,
                Orientation = Orientation
            };
        }

        #endregion
    }
}