﻿using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

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

        private const double GAZE_ENTERED_ANIMATION_DURATION = 200;
        private const double GAZE_EXITED_ANIMATION_DURATION = 200;

        private const float GAZE_ENTERED_ANIMATION_SCALE = 1.04f;

        private Panel rootPanel;

        private int originalZIndex;
        private bool isScaled;

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

        #region Events

        public event EventHandler GazeActionRequested;

        #endregion

        #region Overrides of Control

        protected override void OnApplyTemplate()
        {
            rootPanel = GetTemplateChild(ROOT_PANEL_NAME) as Panel;

            base.OnApplyTemplate();
        }

        #endregion

        #region Overrides of IGazeControl

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

        public void OnGazeOverProgressChanged(double progress)
        {

        }

        public void OnGazeActionRequested()
        {
            VisualStateManager.GoToState(this, PRESSED_VISUALSTATE_NAME, true);

            if (rootPanel == null)
                return;

            if (isScaled)
            {
                GetScaleAnimation(rootPanel, GazeExitedAnimationDuration, 1).Start();

                isScaled = false;
            }

            GazeActionRequested?.Invoke(this, EventArgs.Empty);

            var eventInfo = typeof(Button).GetEvent("Click");
            var eventRaiseMethod = eventInfo.GetRaiseMethod(true);
            eventRaiseMethod.Invoke(this, new object[] { EventArgs.Empty });
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

        #endregion
    }
}