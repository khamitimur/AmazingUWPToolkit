using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AmazingUWPToolkit.Controls
{
    [TemplatePart(Name = ROOT_PANEL_NAME, Type = typeof(Panel))]
    internal class FlipCharItem : Control
    {
        #region Fields

        private const string ROOT_PANEL_NAME = "RootPanel";

        private const string CURRENT_TEXTBLOCK_TAG = "CurrentTextBlock";

        private const double ANIMATION_DURATION = 600;
        private readonly int[] ANIMATION_DELAYS_ARRAY = { 100, 200, 300, 400 };

        private Panel rootPanel;

        AnimationSet currentTextBlockAnimation;
        AnimationSet nextTextBlockAnimation;

        private Random animationDelayRandom;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty CharProperty = DependencyProperty.Register(
            nameof(Char),
            typeof(char),
            typeof(FlipCharItem),
            new PropertyMetadata(null, OnCharPropertyChanged));

        public static readonly DependencyProperty CharOpacityProperty = DependencyProperty.Register(
            nameof(CharOpacity),
            typeof(double),
            typeof(FlipCharItem),
            new PropertyMetadata(1d, OnCharOpacityPropertyChanged));

        public static readonly DependencyProperty SizeToCharRatioProperty = DependencyProperty.Register(
            nameof(SizeToCharRatio),
            typeof(double),
            typeof(FlipCharItem),
            new PropertyMetadata(2d, OnSizeToCharRatioPropertyChanged));

        #endregion

        #region Contructor

        public FlipCharItem()
        {
            DefaultStyleKey = typeof(FlipCharItem);

            animationDelayRandom = new Random();

            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Properties

        public char Char
        {
            get => (char)GetValue(CharProperty);
            set => SetValue(CharProperty, value);
        }

        public double CharOpacity
        {
            get => (double)GetValue(CharOpacityProperty);
            set => SetValue(CharOpacityProperty, value);
        }

        public double SizeToCharRatio
        {
            get => (double)GetValue(SizeToCharRatioProperty);
            set => SetValue(SizeToCharRatioProperty, value);
        }

        private bool AreAnimationsCompleted =>
            (currentTextBlockAnimation == null || currentTextBlockAnimation.State == AnimationSetState.Completed) &&
            (nextTextBlockAnimation == null || nextTextBlockAnimation.State != AnimationSetState.Completed);

        #endregion

        #region Overrides of Control

        protected override void OnApplyTemplate()
        {
            rootPanel = GetTemplateChild(ROOT_PANEL_NAME) as Panel;
            if (rootPanel != null)
            {
                rootPanel.Loaded += OnRootPanelLoaded;
            }

            base.OnApplyTemplate();
        }

        #endregion

        #region Private Methods

        private static void OnCharPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as FlipCharItem)?.SetChar(e.NewValue as char?);
        }

        private static void OnCharOpacityPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as FlipCharItem)?.SetCharOpacity();
        }

        private static void OnSizeToCharRatioPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as FlipCharItem)?.SetCharFontSize();
        }

        private async void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height)
            };

            await SetOffset();

            SetCharFontSize();
        }

        private async void OnRootPanelLoaded(object sender, RoutedEventArgs e)
        {
            await SetOffset();

            SetChar(null);
            SetCharOpacity();
            SetCharFontSize();
        }

        private async Task SetOffset()
        {
            if (!AreAnimationsCompleted)
                return;

            if (rootPanel?.Children?.Count > 1 &&
                rootPanel.Children[1] is TextBlock nextTextBlock)
            {
                await nextTextBlock.Offset(offsetY: (float)ActualHeight, duration: 0).StartAsync();
            }
        }

        private void SetChar(char? newChar)
        {
            if (rootPanel?.Children == null || rootPanel.Children.Count < 2)
                return;

            if (newChar == null)
            {
                SetCharWithoutAnimation();
            }
            else
            {
                SetCharWithAnimation();
            }
        }

        private void SetCharWithoutAnimation()
        {
            var currentTextBlock = rootPanel.Children[0] as TextBlock;
            if (currentTextBlock == null)
                return;

            currentTextBlock.Text = Char.ToString().ToUpper();
            currentTextBlock.Opacity = Opacity;
            currentTextBlock.Tag = CURRENT_TEXTBLOCK_TAG;
        }

        private void SetCharWithAnimation()
        {
            var currentTextBlock = rootPanel.Children[0] as TextBlock;
            if (currentTextBlock == null)
                return;

            var nextTextBlock = rootPanel.Children[1] as TextBlock;
            if (nextTextBlock == null)
                return;

            currentTextBlock.Tag = null;

            nextTextBlock.Text = Char.ToString().ToUpper();
            nextTextBlock.Opacity = Opacity;
            nextTextBlock.Tag = CURRENT_TEXTBLOCK_TAG;

            var animationDelay = ANIMATION_DELAYS_ARRAY[animationDelayRandom.Next(0, ANIMATION_DELAYS_ARRAY.Length)];

            currentTextBlockAnimation = currentTextBlock.Offset(offsetY: -(float)ActualHeight, duration: ANIMATION_DURATION, delay: animationDelay, easingType: EasingType.Quintic);
            nextTextBlockAnimation = nextTextBlock.Offset(offsetY: 0, duration: ANIMATION_DURATION, delay: animationDelay, easingType: EasingType.Quintic);
            nextTextBlockAnimation.Completed += (sender, e) =>
            {
                rootPanel.Children.Remove(currentTextBlock);
                rootPanel.Children.Insert(1, currentTextBlock);

                currentTextBlock.Offset(offsetY: (float)ActualHeight, duration: 0).Start();
            };

            currentTextBlockAnimation.Start();
            nextTextBlockAnimation.Start();
        }

        private void SetCharOpacity()
        {
            var currentTextBlock = rootPanel?.Children?.FirstOrDefault(child => ((child as TextBlock)?.Tag as string) == CURRENT_TEXTBLOCK_TAG);
            if (currentTextBlock == null)
                return;

            currentTextBlock.Opacity = CharOpacity;
        }

        private void SetCharFontSize()
        {
            if (rootPanel == null || rootPanel.Children.Count < 2)
                return;

            var charFontSize = Math.Min(ActualWidth, ActualHeight) / SizeToCharRatio;
            if (charFontSize == 0)
                return;

            if (rootPanel.Children[0] is TextBlock currentTextBlock)
            {
                currentTextBlock.FontSize = charFontSize;
            }

            if (rootPanel.Children[1] is TextBlock nextTextBlock)
            {
                nextTextBlock.FontSize = charFontSize;
            }
        }

        #endregion
    }
}