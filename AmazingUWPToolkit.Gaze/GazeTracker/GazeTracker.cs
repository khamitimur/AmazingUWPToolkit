﻿using AmazingUWPToolkit.Gaze.Controls;
using System;
using System.Collections.Generic;
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

        private readonly List<Control> controls;
        private Control currentControlUnderGaze;

        private static DispatcherTimer timer;
        private double currentTimerValue;
        private bool isGazeDwelledCalled;
        private Point currentGazePoint;

        #endregion

        #region Contructor

        public GazeTracker()
        {
            controls = new List<Control>();

            gazeDeviceWatcherPreview = GazeInputSourcePreview.CreateWatcher();

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(TIMER_INTERVAL)
            };
            timer.Tick += OnTimerTick;
        }

        #endregion

        #region Overrides of IGazeTracker

        public bool AddControl(Control control)
        {
            if (controls.Contains(control))
                return false;

            controls.Add(control);

            return true;
        }

        public bool RemoveControl(Control control)
        {
            if (!controls.Contains(control))
                return false;

            controls.Remove(control);

            return true;
        }

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

        private static bool IsSupportedDevice(GazeDevicePreview gazeDevicePreview)
        {
            return gazeDevicePreview.CanTrackEyes &&
                gazeDevicePreview.ConfigurationState == GazeDeviceConfigurationStatePreview.Ready;
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
            if (controls?.Count == 0)
                return;

            if (args.CurrentPoint.EyeGazePosition == null)
            {
                DiscardCurrentControlUnderGaze();
            }

            var gazePointX = args.CurrentPoint.EyeGazePosition?.X;
            var gazePointY = args.CurrentPoint.EyeGazePosition?.Y;

            if (gazePointX == null || gazePointY == null)
            {
                DiscardCurrentControlUnderGaze();
            }

            var gazePoint = new Point(gazePointX.Value, gazePointY.Value);

            var controlUnderGaze = FindControlUnderGaze(gazePoint);
            if (controlUnderGaze == null)
            {
                DiscardCurrentControlUnderGaze();
            }
            else if (currentControlUnderGaze != null &&
                     currentControlUnderGaze != controlUnderGaze)
            {
                DiscardCurrentControlUnderGaze();
                SetControlUnderGaze(controlUnderGaze, gazePoint);
            }
            else if (currentControlUnderGaze == null)
            {
                SetControlUnderGaze(controlUnderGaze, gazePoint);
            }

            currentGazePoint = gazePoint;

            args.Handled = true;
        }

        private void SetControlUnderGaze(Control control, Point gazePoint)
        {
            currentControlUnderGaze = control;

            (currentControlUnderGaze as IGazeControl).OnGazeEntered();

            StartTimer();
        }

        private void DiscardCurrentControlUnderGaze()
        {
            StopTimer();

            (currentControlUnderGaze as IGazeControl)?.OnGazeFixationProgressChanged(0);
            (currentControlUnderGaze as IGazeControl)?.OnGazeExited();

            currentControlUnderGaze = null;
        }

        private Control FindControlUnderGaze(Point gazePoint)
        {
            for (var i = 0; i < controls.Count; i++)
            {
                var control = controls[i];
                if (IsControlUnderGaze(control, gazePoint))
                {
                    return control;
                }
            }

            return null;
        }

        private bool IsControlUnderGaze(Control control, Point gazePoint)
        {
            var uiElements = VisualTreeHelper.FindElementsInHostCoordinates(gazePoint, control, true);
            foreach (var uiElement in uiElements)
            {
                if (uiElement is FrameworkElement frameworkElement)
                {
                    if (frameworkElement == control)
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

                    (currentControlUnderGaze as IGazeControl)?.OnGazeDwelled(currentGazePoint);
                }
            }
            else
            {
                currentTimerValue += TIMER_INTERVAL;

                (currentControlUnderGaze as IGazeControl)?.OnGazeFixationProgressChanged(currentTimerValue);
            }
        }

        #endregion
    }
}