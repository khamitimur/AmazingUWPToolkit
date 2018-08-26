using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Input.Preview.Injection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Gaze.Controls
{
    [TemplatePart(Name = ROOT_PANEL_NAME, Type = typeof(Panel))]
    [TemplateVisualState(GroupName = COMMON_VISUALSTATESGROUP_NAME, Name = POINTEROVER_VISUALSTATE_NAME)]
    [TemplateVisualState(GroupName = COMMON_VISUALSTATESGROUP_NAME, Name = NORMAL_VISUALSTATE_NAME)]
    [TemplateVisualState(GroupName = COMMON_VISUALSTATESGROUP_NAME, Name = PRESSED_VISUALSTATE_NAME)]
    public sealed class GazeButton : Button, IGazeControl
    {
        #region Fields

        private const string ROOT_PANEL_NAME = "RootPanel";

        private const string COMMON_VISUALSTATESGROUP_NAME = "CommonStates";

        private const string NORMAL_VISUALSTATE_NAME = "Normal";
        private const string POINTEROVER_VISUALSTATE_NAME = "PointerOver";
        private const string PRESSED_VISUALSTATE_NAME = "Pressed";

        private const double GAZE_ENTERED_ANIMATION_DURATION = 300;
        private const double GAZE_EXITED_ANIMATION_DURATION = 300;

        private const float GAZE_ENTERED_ANIMATION_SCALE = 1.06f;

        private Panel rootPanel;

        private int originalZIndex;
        private bool isScaled;

        InputInjector inputInjector;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty GazeEnteredAnimationDurationProperty = DependencyProperty.Register(
            nameof(GazeEnteredAnimationDuration),
            typeof(double),
            typeof(GazeButton),
            new PropertyMetadata(GAZE_ENTERED_ANIMATION_DURATION));

        public static readonly DependencyProperty GazeExitedAnimationDurationProperty = DependencyProperty.Register(
            nameof(GazeExitedAnimationDuration),
            typeof(double),
            typeof(GazeButton),
            new PropertyMetadata(GAZE_EXITED_ANIMATION_DURATION));

        public static readonly DependencyProperty GazeEnteredAnimationScaleProperty = DependencyProperty.Register(
            nameof(GazeEnteredAnimationScale),
            typeof(float),
            typeof(GazeButton),
            new PropertyMetadata(GAZE_ENTERED_ANIMATION_SCALE));

        #endregion

        #region Contructor

        public GazeButton()
        {
            DefaultStyleKey = typeof(GazeButton);
        }

        #endregion

        #region Properties

        public double GazeEnteredAnimationDuration
        {
            get => (double)GetValue(GazeEnteredAnimationDurationProperty);
            set => SetValue(GazeEnteredAnimationDurationProperty, value);
        }

        public double GazeExitedAnimationDuration
        {
            get => (double)GetValue(GazeExitedAnimationDurationProperty);
            set => SetValue(GazeExitedAnimationDurationProperty, value);
        }

        public float GazeEnteredAnimationScale
        {
            get => (float)GetValue(GazeEnteredAnimationScaleProperty);
            set => SetValue(GazeEnteredAnimationScaleProperty, value);
        }

        #endregion

        #region Overrides of Control

        protected override void OnApplyTemplate()
        {
            rootPanel = GetTemplateChild(ROOT_PANEL_NAME) as Panel;

            var gazeTracker = Gaze.GetInstance();
            gazeTracker.AddControl(this);
            gazeTracker.Start();

            base.OnApplyTemplate();
        }

        #endregion

        #region Overrides of IGazeControl

        public bool IsGazeEnaled => IsEnabled;

        public void OnGazeEntered()
        {
            VisualStateManager.GoToState(this, POINTEROVER_VISUALSTATE_NAME, true);

            if (rootPanel == null)
                return;

            originalZIndex = Canvas.GetZIndex(this);
            Canvas.SetZIndex(this, short.MaxValue);

            GetScaleAnimation(rootPanel, GazeEnteredAnimationDuration, GazeEnteredAnimationScale).Start();

            isScaled = true;
        }

        public void OnGazeExited()
        {
            inputInjector?.UninitializePenInjection();
            inputInjector = null;

            VisualStateManager.GoToState(this, NORMAL_VISUALSTATE_NAME, true);

            if (rootPanel == null)
                return;

            Canvas.SetZIndex(this, originalZIndex);

            if (isScaled)
            {
                GetScaleAnimation(rootPanel, GazeExitedAnimationDuration, 1).Start();

                isScaled = false;
            }
        }

        public void OnGazeFixationProgressChanged(double progress)
        {
            Debug.WriteLine(progress);
        }

        public void OnGazeDwelled(Point point)
        {
            VisualStateManager.GoToState(this, PRESSED_VISUALSTATE_NAME, true);

            if (rootPanel == null)
                return;

            if (isScaled)
            {
                GetScaleAnimation(rootPanel, GazeExitedAnimationDuration, 1).Start();

                isScaled = false;
            }

            InjectInput(point);
        }

        #endregion

        #region Private Methods

        private AnimationSet GetScaleAnimation(FrameworkElement element, double duration, float scale)
        {
            var centerX = (float)element.ActualWidth / 2;
            var centerY = (float)element.ActualHeight / 2;

            return element.Scale(scale,
                                 scale,
                                 centerX,
                                 centerY,
                                 duration,
                                 easingType: EasingType.Back);
        }

        private void InjectInput(Point point)
        {
            if (inputInjector == null)
            {
                inputInjector = InputInjector.TryCreate();
            }

            if (inputInjector == null)
                return;

            inputInjector.InitializePenInjection(InjectedInputVisualizationMode.None);

            var pointerId = (uint)new Random().Next(0, int.MaxValue);

            var injectedInputPenInfo = new InjectedInputPenInfo
            {
                PointerInfo = new InjectedInputPointerInfo
                {
                    PointerId = pointerId,
                    PixelLocation = new InjectedInputPoint
                    {
                        PositionX = (int)point.X,
                        PositionY = (int)point.Y
                    },
                    PointerOptions = InjectedInputPointerOptions.PointerDown |
                                     InjectedInputPointerOptions.InContact |
                                     InjectedInputPointerOptions.New
                 },
                Pressure = 1.0,
                PenParameters = InjectedInputPenParameters.Pressure
            };

            inputInjector.InjectPenInput(injectedInputPenInfo);

            injectedInputPenInfo = new InjectedInputPenInfo
            {
                PointerInfo = new InjectedInputPointerInfo
                {
                    PointerId = pointerId,
                    PointerOptions = InjectedInputPointerOptions.PointerUp
                }
            };

            inputInjector.InjectPenInput(injectedInputPenInfo);

            //uint pointerId = pointerPoint.PointerId + 1;

            //var appBounds = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;

            //Point appBoundsTopLeft = new Point(appBounds.Left, appBounds.Top);

            //var injectArea = ContainerInject.TransformToVisual(Window.Current.Content);

            //Point injectAreaTopLeft = injectArea.TransformPoint(new Point(0, 0));

            //int pointerPointX = (int)pointerPoint.Position.X;
            //int pointerPointY = (int)pointerPoint.Position.Y;

            //Point injectionPoint =
            //    new Point(
            //        appBoundsTopLeft.X + injectAreaTopLeft.X + pointerPointX,
            //        appBoundsTopLeft.Y + injectAreaTopLeft.Y + pointerPointY);

            //var touchData = new List<InjectedInputTouchInfo>
            //{
            //    new InjectedInputTouchInfo
            //    {
            //        Contact = new InjectedInputRectangle
            //        {
            //            Left = 30, Top = 30, Bottom = 30, Right = 30
            //        },
            //        PointerInfo = new InjectedInputPointerInfo
            //        {
            //            PointerId = pointerId,
            //            PointerOptions =
            //            InjectedInputPointerOptions.PointerDown |
            //            InjectedInputPointerOptions.InContact |
            //            InjectedInputPointerOptions.New,
            //            TimeOffsetInMilliseconds = 0,
            //            PixelLocation = new InjectedInputPoint
            //            {
            //                PositionX = (int)injectionPoint.X ,
            //                PositionY = (int)injectionPoint.Y
            //            }
            //    },
            //    Pressure = 1.0,
            //    TouchParameters =
            //        InjectedInputTouchParameters.Pressure |
            //        InjectedInputTouchParameters.Contact
            //    }
            //};

            //_inputInjector.InjectTouchInput(touchData);

            //touchData = new List<InjectedInputTouchInfo>
            //{
            //    new InjectedInputTouchInfo
            //    {
            //        PointerInfo = new InjectedInputPointerInfo
            //        {
            //            PointerId = pointerId,
            //            PointerOptions = InjectedInputPointerOptions.PointerUp
            //        }
            //    }
            //};

            //_inputInjector.InjectTouchInput(touchData);
        }

        #endregion
    }
}