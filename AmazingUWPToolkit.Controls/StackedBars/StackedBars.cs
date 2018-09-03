using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Controls
{
    public sealed class StackedBars : Control
    {
        #region Fields

        private const double DEFAULT_ANIMATION_DURATION = 400;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            nameof(Items),
            typeof(ICollection<StackedBarItem>),
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

        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register(
            nameof(AnimationDuration),
            typeof(double),
            typeof(StackedBars),
            new PropertyMetadata(DEFAULT_ANIMATION_DURATION, OnAnimationDurationPropertyChanged));

        public static readonly DependencyProperty AnimationEasingTypeProperty = DependencyProperty.Register(
            nameof(AnimationEasingType),
            typeof(EasingType),
            typeof(StackedBars),
            new PropertyMetadata(EasingType.Quartic, OnAnimationEasingTypePropertyChanged));

        internal static readonly DependencyProperty StackedBarsModelProperty = DependencyProperty.Register(
            nameof(StackedBarsModel),
            typeof(StackedBarsModel),
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

        public ICollection<StackedBarItem> Items
        {
            get => (ICollection<StackedBarItem>)GetValue(ItemsProperty);
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

        public double AnimationDuration
        {
            get => (double)GetValue(AnimationDurationProperty);
            set => SetValue(AnimationDurationProperty, value);
        }

        public EasingType AnimationEasingType
        {
            get => (EasingType)GetValue(AnimationEasingTypeProperty);
            set => SetValue(AnimationEasingTypeProperty, value);
        }

        internal StackedBarsModel StackedBarsModel
        {
            get => (StackedBarsModel)GetValue(StackedBarsModelProperty);
            set => SetValue(StackedBarsModelProperty, value);
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

        private static void OnAnimationDurationPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as StackedBars)?.SetStackBarsItemsPanelModel();
        }

        private static void OnAnimationEasingTypePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as StackedBars)?.SetStackBarsItemsPanelModel();
        }

        private void SetStackBarsItemsPanelModel()
        {
            StackedBarsModel = new StackedBarsModel
            {
                Items = Items,
                Orientation = Orientation,
                AnimationDuration = AnimationDuration,
                AnimationEasingType = AnimationEasingType
            };
        }

        #endregion
    }
}