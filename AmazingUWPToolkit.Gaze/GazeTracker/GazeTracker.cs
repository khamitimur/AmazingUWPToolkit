using AmazingUWPToolkit.Gaze.Controls;
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
        private const double ACTION_INTERVAL = 400;

        private static GazeDeviceWatcherPreview gazeDeviceWatcherPreview;
        private static GazeInputSourcePreview gazeInputSourcePreview;

        private readonly IGazeControl gazeControl;

        private static DispatcherTimer timer;
        private double currentTimerValue;
        private bool isFocused;
        private bool isGazeDwelledCalled;

        #endregion

        #region Contructor

        public GazeTracker(IGazeControl gazeControl)
        {
            this.gazeControl = gazeControl;

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

        Point lastHitPoint;

        private void OnGazeInputSourcePreviewGazeMoved(GazeInputSourcePreview sender, GazeMovedPreviewEventArgs args)
        {
            if (gazeControl == null)
                return;

            if (args.CurrentPoint.EyeGazePosition != null)
            {
                double gazePointX = args.CurrentPoint.EyeGazePosition.Value.X;
                double gazePointY = args.CurrentPoint.EyeGazePosition.Value.Y;

                var gazePoint = new Point(gazePointX, gazePointY);

                var doesHitControl = DoesElementContainPoint(gazePoint);

                if (doesHitControl)
                {
                    lastHitPoint = gazePoint;

                    if (isFocused)
                        return;

                    isFocused = true;

                    gazeControl.OnGazeEntered();

                    StartTimer();
                }
                else
                {
                    if (!isFocused)
                        return;

                    isFocused = false;

                    StopTimer();

                    gazeControl.OnGazeFixationProgressChanged(0);
                    gazeControl.OnGazeExited();
                }

                args.Handled = true;
            }
        }

        private bool DoesElementContainPoint(Point gazePoint)
        {
            var uielements = VisualTreeHelper.FindElementsInHostCoordinates(gazePoint, gazeControl as Control, true);
            foreach (var uielement in uielements)
            {
                if (uielement is FrameworkElement frameworkElement)
                {
                    if (frameworkElement == gazeControl)
                        return true;
                }
            }

            return false;
        }

        private void StartTimer()
        {
            currentTimerValue = 0;
            isGazeDwelledCalled = false;

            timer.Start();
        }

        private void StopTimer()
        {
            timer.Stop();
        }

        private void OnTimerTick(object sender, object e)
        {
            if (currentTimerValue >= ACTION_INTERVAL)
            {
                if (!isGazeDwelledCalled)
                {
                    isGazeDwelledCalled = true;

                    gazeControl?.OnGazeDwelled(lastHitPoint);
                }
            }
            else
            {
                currentTimerValue += TIMER_INTERVAL;

                gazeControl?.OnGazeFixationProgressChanged(currentTimerValue);
            }
        }

        #endregion
    }
}