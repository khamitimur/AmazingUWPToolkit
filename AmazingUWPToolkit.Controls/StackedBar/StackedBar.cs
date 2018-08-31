using JetBrains.Annotations;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Controls
{
    public sealed class StackedBar : Control
    {
        #region Dependency Properties

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            nameof(Items),
            typeof(ObservableCollection<StackedBarItem>),
            typeof(StackedBar),
            new PropertyMetadata(null));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(StackedBarOrientation),
            typeof(StackedBar),
            new PropertyMetadata(StackedBarOrientation.Horizontal));

        public static readonly DependencyProperty CornerRadiusProperty =  DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(StackedBar),
            new PropertyMetadata(default(CornerRadius)));

        #endregion

        #region Contructor

        public StackedBar()
        {
            DefaultStyleKey = typeof(StackedBar);

            DataContext = this;
        }

        #endregion

        #region Properties

        [CanBeNull]
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

        #endregion
    }
}