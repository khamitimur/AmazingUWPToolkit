using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Numerics;
using Windows.Devices.Sensors;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using EF = Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ExpressionFunctions;

namespace AmazingUWPToolkit.Controls
{
    [ContentProperty(Name = "Content")]
    public sealed class FluidRotatePanel : Control
    {
        #region Fields

        private Accelerometer _accelerometer;

        private CompositionPropertySet _reading;
        private double _progress;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content),
            typeof(FrameworkElement),
            typeof(FluidRotatePanel),
            new PropertyMetadata(null, OnContentPropertyChanged));

        #endregion

        #region Contructor

        public FluidRotatePanel()
        {
            DefaultStyleKey = typeof(FluidRotatePanel);
        }

        #endregion

        #region Properties

        public FrameworkElement Content
        {
            get => (FrameworkElement)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        #endregion

        #region Overrides of Control

        protected override void OnApplyTemplate()
        {
            Initialize();

            base.OnApplyTemplate();
        }

        #endregion

        #region Private Methods

        private static void OnContentPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as FluidRotatePanel)?.Initialize();
        }

        private void Initialize()
        {
            if (Content == null)
                return;

            var compositor = Window.Current.Compositor;

            InitializeAccelerometer();

            _reading = compositor.CreatePropertySet();
            _reading.InsertScalar("Progress", 0);

            if (_accelerometer != null)
            {
                RunRotationExpressionAnimation();
            }

            void RunRotationExpressionAnimation()
            {
                var progressExpressionNode = _reading.GetReference().GetScalarProperty("Progress");

                var contentVisual = VisualExtensions.GetVisual(Content);
                contentVisual.CenterPoint = new Vector3((float)660 / 2, (float)660 / 2, 0);

                contentVisual.StartAnimation(nameof(Visual.RotationAngleInDegrees), progressExpressionNode * 90);
            }
        }

        private void InitializeAccelerometer()
        {
            _accelerometer = Accelerometer.GetDefault();
            if (_accelerometer != null)
            {
                _accelerometer.ReportInterval = _accelerometer.MinimumReportInterval;

                Window.Current.VisibilityChanged += (s, e) =>
                {
                    if (e.Visible)
                    {
                        RunAccelerometer();
                    }
                    else
                    {
                        StopAccelerometer();
                    }
                };
            }
        }

        private void RunAccelerometer()
        {
            _accelerometer.ReportInterval = _accelerometer.MinimumReportInterval;
            _accelerometer.ReadingChanged += OnAccelerometerReadingChanged;
        }

        private void StopAccelerometer()
        {
            _accelerometer.ReportInterval = 0;
            _accelerometer.ReadingChanged -= OnAccelerometerReadingChanged;
        }

        private async void OnAccelerometerReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs e)
        {
            var progress = 1 - Math.Round(e.Reading.AccelerationX, 2);

            if (Math.Abs(_progress - progress) > 1 / 180.0)
            {
                _progress = _progress.Lerp(progress, 0.1);
            }

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => _reading.InsertScalar("Progress", (float)_progress));
        }

        #endregion
    }

    public static class Extensions
    {
        public static double Lerp(this double value1, double value2, double by)
        {
            return value1 * (1 - by) + value2 * by;
        }
    }
}