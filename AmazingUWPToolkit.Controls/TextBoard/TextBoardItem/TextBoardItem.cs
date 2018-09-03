using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AmazingUWPToolkit.Controls
{
    [TemplatePart(Name = ROOTPANEL_NAME, Type = typeof(Panel))]
    internal sealed class TextBoardItem : Control, ITextBoardItem
    {
        #region Fields

        private const string ROOTPANEL_NAME = "RootPanel";

        private const double ANIMATION_DURATION = 600;
        private readonly int[] ANIMATION_DELAYS_ARRAY = { 100, 200, 300, 400 };

        private const double DEFAULT_RANDOM_TEXTBOARDITEM_OPACITY = 0.05;

        private Panel rootPanel;

        AnimationSet currentTextBlockAnimation;
        AnimationSet nextTextBlockAnimation;

        private Random animationDelayRandom;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            nameof(Model),
            typeof(ITextBoardItemModel),
            typeof(TextBoardItem),
            new PropertyMetadata(null, OnTextBoardItemModelPropertyChanged));

        public static readonly DependencyProperty RandomTextBoardItemOpacityProperty = DependencyProperty.Register(
            nameof(RandomTextBoardItemOpacity),
            typeof(double),
            typeof(TextBoardItem),
            new PropertyMetadata(DEFAULT_RANDOM_TEXTBOARDITEM_OPACITY, OnRandomTextBoardItemOpacityPropertyChanged));

        #endregion

        #region Contructor

        public TextBoardItem()
        {
            DefaultStyleKey = typeof(TextBoardItem);

            animationDelayRandom = new Random();

            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Properties

        private bool AreAnimationsCompleted =>
            (currentTextBlockAnimation == null || currentTextBlockAnimation.State == AnimationSetState.Completed) &&
            (nextTextBlockAnimation == null || nextTextBlockAnimation.State != AnimationSetState.Completed);

        #endregion

        #region Implementation of ITextBoardItem

        public ITextBoardItemModel Model
        {
            get => (ITextBoardItemModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
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

        private static void OnTextBoardItemModelPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is TextBoardItem textBoardItem)
            {
                if (e.OldValue is ITextBoardItemModel oldTextBoardItemModel)
                {
                    oldTextBoardItemModel.Updated -= textBoardItem.OnTextBoardItemModelUpdated;
                }

                if (e.NewValue is ITextBoardItemModel newTextBoardItemModel)
                {
                    newTextBoardItemModel.Updated += textBoardItem.OnTextBoardItemModelUpdated;
                }

                textBoardItem.Update();
            }
        }

        private static void OnRandomTextBoardItemOpacityPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoardItem)?.SetRandomTextBoardItemOpacity();
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

        private void OnTextBoardItemModelUpdated(object sender, EventArgs e)
        {
            Update();
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

        private void Update()
        {
            if (rootPanel?.Children == null ||
                rootPanel.Children.Count < 2 ||
                Model == null)
            {
                return;
            }

            if (Model.PreviousState == null)
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

            SetTextBlockProperties(currentTextBlock);
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

            SetTextBlockProperties(nextTextBlock);

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

        private void SetTextBlockProperties(TextBlock textBlock)
        {
            textBlock.Text = Model.ToString();
            textBlock.Tag = Model.IsRandom;
            textBlock.Opacity = Model.IsRandom
                ? RandomTextBoardItemOpacity
                : 1;
        }

        private void SetRandomTextBoardItemOpacity()
        {
            if (rootPanel?.Children == null)
                return;

            foreach (var textBlock in rootPanel.Children.Cast<TextBlock>())
            {
                if (textBlock.Tag is bool isRandom && isRandom)
                {
                    textBlock.Opacity = RandomTextBoardItemOpacity;
                }
            }
        }

        #endregion
    }
}