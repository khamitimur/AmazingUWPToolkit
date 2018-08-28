using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using JetBrains.Annotations;

namespace AmazingUWPToolkit.Controls
{
    [TemplatePart(Name = LAYOUTROOT_GRID_NAME, Type = typeof(Grid))]
    internal class CharItemControl : Control
    {
        #region Fields

        private const string LAYOUTROOT_GRID_NAME = "LayoutRoot";

        private const double ANIMATION_DURATION = 600;
        private readonly int[] ANIMATION_DELAYS_ARRAY = { 100, 200, 300, 400 };

        private Grid layoutRootGrid;

        private bool isAnimationInProgress;
        private Random animationDelayRandom;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty CharItemProperty = DependencyProperty.Register(
            nameof(CharItem),
            typeof(ICharItem),
            typeof(CharItemControl),
            new PropertyMetadata(null, OnCharItemPropertyChanged));

        #endregion

        #region Constructor

        public CharItemControl()
        {
            DefaultStyleKey = typeof(CharItemControl);

            animationDelayRandom = new Random();

            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Properties

        [CanBeNull]
        public ICharItem CharItem
        {
            get => (ICharItem)GetValue(CharItemProperty);
            set => SetValue(CharItemProperty, value);
        }

        #endregion

        #region Overrides of Control

        protected override void OnApplyTemplate()
        {
            layoutRootGrid = GetTemplateChild(LAYOUTROOT_GRID_NAME) as Grid;
            if (layoutRootGrid != null)
            {
                layoutRootGrid.Loaded += OnLayoutRootGridLoaded;
                layoutRootGrid.SizeChanged += OnLayoutRootGridSizeChanged;
            }

            base.OnApplyTemplate();
        }

        #endregion

        #region Private Methods

        private static void OnCharItemPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is CharItemControl charItemControl)
            {
                if (e.OldValue is CharItem oldCharItem)
                {
                    oldCharItem.Updated -= charItemControl.OnCharItemUpdated;
                }

                if (e.NewValue is CharItem newCharItem)
                {
                    newCharItem.Updated += charItemControl.OnCharItemUpdated;
                }

                charItemControl.Update();
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) => Clip = new RectangleGeometry()
        {
            Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height)
        };

        private async void OnLayoutRootGridLoaded(object sender, RoutedEventArgs e)
        {
            await SetOffset();

            Update();
        }

        private async void OnLayoutRootGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            await SetOffset();
        }

        private void OnCharItemUpdated(object sender, EventArgs e)
        {
            Update();
        }

        private async Task SetOffset()
        {
            if (isAnimationInProgress)
                return;

            if (layoutRootGrid?.Children?.Count > 1 &&
                layoutRootGrid.Children[1] is TextBlock nextTextBlock)
            {
                await nextTextBlock.Offset(offsetY: (float)ActualHeight, duration: 0).StartAsync();
            }
        }

        private void Update()
        {
            if (layoutRootGrid == null ||
                layoutRootGrid.Children?.Count < 2 ||
                CharItem == null)
            {
                return;
            }

            if (CharItem.PreviousCharItem == null)
            {
                UpdateWithoutAnimation();
            }
            else
            {
                UpdateWithAnimation();
            }
        }

        private void UpdateWithoutAnimation()
        {
            var currentTextBlock = layoutRootGrid.Children[0] as TextBlock;
            if (currentTextBlock == null)
                return;

            currentTextBlock.Text = CharItem.Char.ToString();
            currentTextBlock.Opacity = CharItem.IsRandom
                ? 0.05
                : 1;
        }

        private void UpdateWithAnimation()
        {
            var currentTextBlock = layoutRootGrid.Children[0] as TextBlock;
            var nextTextBlock = layoutRootGrid.Children[1] as TextBlock;

            if (nextTextBlock == null)
            {
                UpdateWithoutAnimation();

                return;
            }

            isAnimationInProgress = true;

            nextTextBlock.Text = CharItem.Char.ToString();
            nextTextBlock.Opacity = CharItem.IsRandom
                ? 0.05
                : 1;

            var animationDelay = ANIMATION_DELAYS_ARRAY[animationDelayRandom.Next(0, ANIMATION_DELAYS_ARRAY.Length)];

            var currentTextBlockAnimation = currentTextBlock.Offset(offsetY: -(float)ActualHeight, duration: ANIMATION_DURATION, delay: animationDelay);
            var nextTextBlockAnimation = nextTextBlock.Offset(offsetY: 0, duration: ANIMATION_DURATION, delay: animationDelay);
            nextTextBlockAnimation.Completed += (sender, e) =>
            {
                layoutRootGrid.Children.Remove(currentTextBlock);
                layoutRootGrid.Children.Insert(1, currentTextBlock);

                currentTextBlock.Offset(offsetY: (float)ActualHeight, duration: 0).Start();

                isAnimationInProgress = false;
            };

            currentTextBlockAnimation.Start();
            nextTextBlockAnimation.Start();
        }

        #endregion
    }
}