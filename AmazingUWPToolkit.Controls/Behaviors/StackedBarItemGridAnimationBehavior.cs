﻿using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Xaml.Interactivity;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AmazingUWPToolkit.Controls.Behaviors
{
    internal class StackedBarItemGridAnimationBehavior : Behavior<Grid>
    {
        #region Fields

        private bool isLoaded;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(StackedBarOrientation),
            typeof(StackedBarItemGridAnimationBehavior),
            new PropertyMetadata(StackedBarOrientation.Horizontal));

        #endregion

        #region Properties

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
                AssociatedObject.SizeChanged += OnSizeChanged;
            }

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= OnLoaded;
                AssociatedObject.SizeChanged -= OnSizeChanged;
            }

            base.OnDetaching();
        }

        #endregion

        #region Private Methods

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;

            TryToAnimate();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            AssociatedObject.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height)
            };

            if ((Orientation == StackedBarOrientation.Horizontal && e.PreviousSize.Width != 0) ||
                (Orientation == StackedBarOrientation.Vertical && e.PreviousSize.Height != 0))
            {
                return;
            }

            TryToAnimate();
        }


        private async void TryToAnimate()
        {
            if (!isLoaded)
                return;

            if (AssociatedObject == null)
                return;

            if (!(AssociatedObject.Children[0] is FrameworkElement child))
                return;

            if (Orientation == StackedBarOrientation.Horizontal)
            {
                await child.Offset(offsetX: -(float)AssociatedObject.ActualWidth, duration: 0).StartAsync();
                await child.Offset(offsetX: 0, duration: 400, easingType: EasingType.Quintic).StartAsync();
            }
            else
            {
                await child.Offset(offsetY: (float)AssociatedObject.ActualHeight, duration: 0).StartAsync();
                await child.Offset(offsetY: 0, duration: 400, easingType: EasingType.Quintic).StartAsync();
            }
        }

        #endregion
    }
}