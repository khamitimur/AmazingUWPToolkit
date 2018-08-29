using JetBrains.Annotations;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AmazingUWPToolkit.Controls
{
    [TemplatePart(Name = ROOTPANEL_NAME, Type = typeof(Panel))]
    public sealed class TextBoardItemControl : Control
    {
        #region Fields

        private const string ROOTPANEL_NAME = "RootPanel";

        private const double ANIMATION_DURATION = 600;
        private readonly int[] ANIMATION_DELAYS_ARRAY = { 100, 200, 300, 400 };

        private const double DEFAULT_RANDOM_TEXTBOARDITEM_OPACITY = 0.05;

        private Panel rootPanel;

        private bool isAnimationInProgress;
        private Random animationDelayRandom;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty TextBoardItemProperty = DependencyProperty.Register(
            nameof(TextBoardItem),
            typeof(ITextBoardItem),
            typeof(TextBoardItemControl),
            new PropertyMetadata(null, OnTextBoardItemPropertyChanged));

        public static readonly DependencyProperty RandomTextBoardItemOpacityProperty = DependencyProperty.Register(
            nameof(RandomTextBoardItemOpacity),
            typeof(double),
            typeof(TextBoardItemControl),
            new PropertyMetadata(DEFAULT_RANDOM_TEXTBOARDITEM_OPACITY, OnRandomTextBoardItemOpacityPropertyChanged));

        #endregion

        #region Contructor

        public TextBoardItemControl()
        {
            DefaultStyleKey = typeof(TextBoardItemControl);

            animationDelayRandom = new Random();

            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Properties

        [CanBeNull]
        public ITextBoardItem TextBoardItem
        {
            get => (ITextBoardItem)GetValue(TextBoardItemProperty);
            set => SetValue(TextBoardItemProperty, value);
        }

        public double RandomTextBoardItemOpacity
        {
            get => (double)GetValue(RandomTextBoardItemOpacityProperty);
            set => SetValue(RandomTextBoardItemOpacityProperty, value);
        }

        #endregion

        #region Overrides of Control

        protected override void OnApplyTemplate()
        {
            rootPanel = GetTemplateChild(ROOTPANEL_NAME) as Panel;
            if (rootPanel != null)
            {
                rootPanel.Loaded += OnRootPanelLoaded;
                rootPanel.SizeChanged += OnRootPanelSizeChanged;
            }

            base.OnApplyTemplate();
        }

        #endregion

        #region Private Methods

        private static void OnTextBoardItemPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is TextBoardItemControl textBoardItemControl)
            {
                if (e.OldValue is TextBoardItem oldTextBoardItem)
                {
                    oldTextBoardItem.Updated -= textBoardItemControl.OnTextBoardItemUpdated;
                }

                if (e.NewValue is TextBoardItem newTextBoardItem)
                {
                    newTextBoardItem.Updated += textBoardItemControl.OnTextBoardItemUpdated;
                }

                textBoardItemControl.Update();
            }
        }

        private static void OnRandomTextBoardItemOpacityPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoardItemControl)?.SetRandomTextBoardItemOpacity();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height)
            };
        }

        private async void OnRootPanelLoaded(object sender, RoutedEventArgs e)
        {
            await SetOffset();

            Update();
        }

        private async void OnRootPanelSizeChanged(object sender, SizeChangedEventArgs e)
        {
            await SetOffset();
        }

        private void OnTextBoardItemUpdated(object sender, EventArgs e)
        {
            Update();
        }

        private async Task SetOffset()
        {
            if (isAnimationInProgress)
                return;

            if (rootPanel?.Children?.Count > 1 &&
                rootPanel.Children[1] is TextBlock nextTextBlock)
            {
                await nextTextBlock.Offset(offsetY: (float)ActualHeight, duration: 0).StartAsync();
            }
        }

        private void Update()
        {
            if (rootPanel == null ||
                rootPanel.Children?.Count < 2 ||
                TextBoardItem == null)
            {
                return;
            }

            if (TextBoardItem.PreviousState == null)
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
            var currentTextBlock = rootPanel.Children[0] as TextBlock;
            if (currentTextBlock == null)
                return;

            currentTextBlock.Text = TextBoardItem.ToString();
            currentTextBlock.Opacity = TextBoardItem.IsRandom
                ? 0.05
                : 1;
        }

        private void UpdateWithAnimation()
        {
            var currentTextBlock = rootPanel.Children[0] as TextBlock;
            var nextTextBlock = rootPanel.Children[1] as TextBlock;

            if (nextTextBlock == null)
            {
                UpdateWithoutAnimation();

                return;
            }

            isAnimationInProgress = true;

            nextTextBlock.Text = TextBoardItem.ToString();
            nextTextBlock.Opacity = TextBoardItem.IsRandom
                ? 0.05
                : 1;

            var animationDelay = ANIMATION_DELAYS_ARRAY[animationDelayRandom.Next(0, ANIMATION_DELAYS_ARRAY.Length)];

            var currentTextBlockAnimation = currentTextBlock.Offset(offsetY: -(float)ActualHeight, duration: ANIMATION_DURATION, delay: animationDelay);
            var nextTextBlockAnimation = nextTextBlock.Offset(offsetY: 0, duration: ANIMATION_DURATION, delay: animationDelay);
            nextTextBlockAnimation.Completed += (sender, e) =>
            {
                rootPanel.Children.Remove(currentTextBlock);
                rootPanel.Children.Insert(1, currentTextBlock);

                currentTextBlock.Offset(offsetY: (float)ActualHeight, duration: 0).Start();

                isAnimationInProgress = false;
            };

            currentTextBlockAnimation.Start();
            nextTextBlockAnimation.Start();
        }

        private void SetRandomTextBoardItemOpacity()
        {
            // TODO
        }

        #endregion
    }
}