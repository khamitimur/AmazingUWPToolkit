using System;
using Windows.Devices.Input.Preview;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AmazingUWPToolkit.Gaze
{
    public class GazeTracker : IGazeTracker
    {
        #region Fields

        private const double TIMER_INTERVAL = 20;
        private const double ACTION_INTERVAL = 200;

        private static GazeDeviceWatcherPreview gazeDeviceWatcherPreview;
        private static GazeInputSourcePreview gazeInputSourcePreview;

        private static DispatcherTimer timer;
        private static bool timerStarted;

        private readonly Control control;

        #endregion

        #region Contructor

        public GazeTracker(Control control)
        {
            this.control = control;

            gazeDeviceWatcherPreview = GazeInputSourcePreview.CreateWatcher();

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(TIMER_INTERVAL)
            };
            timer.Tick += OnTimerTick;
        }

        #endregion

        #region Overrides of IGazeTracker

        public void Start()
        {
            gazeDeviceWatcherPreview.Added += OnGazeDeviceWatcherAdded;
            gazeDeviceWatcherPreview.Updated += OnGazeDeviceWatcherUpdated;
            gazeDeviceWatcherPreview.Removed += OnGazeDeviceWatcherRemoved;

            gazeDeviceWatcherPreview.Start();
        }

        public void Stop()
        {
            if (gazeDeviceWatcherPreview != null)
            {
                gazeDeviceWatcherPreview.Stop();

                gazeDeviceWatcherPreview.Added -= OnGazeDeviceWatcherAdded;
                gazeDeviceWatcherPreview.Updated -= OnGazeDeviceWatcherUpdated;
                gazeDeviceWatcherPreview.Removed -= OnGazeDeviceWatcherRemoved;

                gazeDeviceWatcherPreview = null;
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region Static Methods

        private static bool IsSupportedDevice(GazeDevicePreview gazeDevice)
        {
            return gazeDevice.CanTrackEyes &&
                gazeDevice.ConfigurationState == GazeDeviceConfigurationStatePreview.Ready;
        }

        #endregion

        #region Private Methods

        private void OnGazeDeviceWatcherAdded(GazeDeviceWatcherPreview sender, GazeDeviceWatcherAddedPreviewEventArgs args)
        {
            StartGazeTrackingAsync(args.Device);
        }

        private void OnGazeDeviceWatcherUpdated(GazeDeviceWatcherPreview sender, GazeDeviceWatcherUpdatedPreviewEventArgs args)
        {
            StartGazeTrackingAsync(args.Device);
        }

        private void OnGazeDeviceWatcherRemoved(GazeDeviceWatcherPreview sender, GazeDeviceWatcherRemovedPreviewEventArgs args)
        {
            StopGazeTracking();
        }

        private async void StartGazeTrackingAsync(GazeDevicePreview gazeDevice)
        {
            if (IsSupportedDevice(gazeDevice))
            {
                gazeInputSourcePreview = GazeInputSourcePreview.GetForCurrentView();

                gazeInputSourcePreview.GazeMoved += OnGazeInputSourcePreviewGazeMoved;
            }
            else if (gazeDevice.ConfigurationState == GazeDeviceConfigurationStatePreview.UserCalibrationNeeded ||
                     gazeDevice.ConfigurationState == GazeDeviceConfigurationStatePreview.ScreenSetupNeeded)
            {
                System.Diagnostics.Debug.WriteLine("Your device needs to calibrate. Please wait for it to finish.");

                await gazeDevice.RequestCalibrationAsync();
            }
            else if (gazeDevice.ConfigurationState == GazeDeviceConfigurationStatePreview.Configuring)
            {
               System.Diagnostics.Debug.WriteLine("Your device is being configured. Please wait for it to finish");
            }
            else if (gazeDevice.ConfigurationState == GazeDeviceConfigurationStatePreview.Unknown)
            {
                System.Diagnostics.Debug.WriteLine("Your device is not ready. Please set up your device or reconfigure it.");
            }
        }

        private void StopGazeTracking()
        {
            gazeInputSourcePreview.GazeMoved -= OnGazeInputSourcePreviewGazeMoved;
        }

        private void OnGazeInputSourcePreviewGazeMoved(GazeInputSourcePreview sender, GazeMovedPreviewEventArgs args)
        {
            if (control == null)
                return;

            if (args.CurrentPoint.EyeGazePosition != null)
            {
                double gazePointX = args.CurrentPoint.EyeGazePosition.Value.X;
                double gazePointY = args.CurrentPoint.EyeGazePosition.Value.Y;

                //double ellipseLeft = gazePointX - (eyeGazePositionEllipse.Width / 2.0f);
                //double ellipseTop =  gazePointY - (eyeGazePositionEllipse.Height / 2.0f) - (int)Header.ActualHeight;

                //var translateEllipse = new TranslateTransform
                //{
                //    X = ellipseLeft,
                //    Y = ellipseTop
                //};

                //eyeGazePositionEllipse.RenderTransform = translateEllipse;

                var gazePoint = new Point(gazePointX, gazePointY);

                var doesHitControl = DoesElementContainPoint(gazePoint);

                if (doesHitControl)
                {
                    VisualStateManager.GoToState(control, "GazeOver", true);
                }
                else
                {
                    VisualStateManager.GoToState(control, "GazeNormal", true);
                }

                args.Handled = true;
            }
        }

        private bool DoesElementContainPoint(Point gazePoint)
        {
            var uielements = VisualTreeHelper.FindElementsInHostCoordinates(gazePoint, control, true);
            foreach (var uielement in uielements)
            {
                if (uielement is FrameworkElement frameworkElement)
                {
                    if (frameworkElement == control)
                    {
                        //if (!timerStarted)
                        //{
                        //    timer.Start();
                        //    timerStarted = true;
                        //}

                        return true;
                    }
                }
            }

            timer.Stop();

            timerStarted = false;

            return false;
        }

        private static void OnTimerTick(object sender, object e)
        {

        }

        #endregion
    }
}