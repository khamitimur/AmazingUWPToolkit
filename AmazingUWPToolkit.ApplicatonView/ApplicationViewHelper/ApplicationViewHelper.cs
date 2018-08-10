using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using System;

namespace AmazingUWPToolkit.ApplicatonView
{
    public class ApplicationViewHelper : IApplicationViewHelper
    {
        #region Fields

        private readonly IApplicationViewData applicationViewData;
        private readonly UISettings uiSettings;

        private readonly CoreApplicationViewTitleBar coreApplicationViewTitleBar;
        private readonly ApplicationViewTitleBar applicationViewTitleBar;

        private readonly CoreDispatcher coreDispatcher;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationViewHelper"/> using <see cref="IApplicationViewData"/>.
        /// </summary>
        /// <param name="applicationViewData"></param>
        public ApplicationViewHelper(IApplicationViewData applicationViewData)
        {
            this.applicationViewData = applicationViewData;

            uiSettings = new UISettings();
            uiSettings.ColorValuesChanged += OnUiSettingsColorValuesChanged;

            coreApplicationViewTitleBar = CoreApplication.GetCurrentView().TitleBar;
            applicationViewTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            coreDispatcher = CoreApplication.GetCurrentView().CoreWindow.Dispatcher;
        }

        #endregion

        #region Implementation of IApplicationViewHelper

        public async Task SetAsync()
        {
            coreApplicationViewTitleBar.ExtendViewIntoTitleBar = applicationViewData.ExtendIntoTitleBar;

            if (applicationViewData.PreferredMinSize != null)
            {
                var preferredMinSize = applicationViewData.PreferredMinSize;
                var convertPreferredMinSizeUsingRawPixels = applicationViewData.ConvertPreferredMinSizeUsingRawPixels;
                var rawPixelsPerViewPixel = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

                var desiredWidth = convertPreferredMinSizeUsingRawPixels
                    ? rawPixelsPerViewPixel * preferredMinSize.Value.Width
                    : preferredMinSize.Value.Width;
                var desiredHeight = convertPreferredMinSizeUsingRawPixels
                    ? rawPixelsPerViewPixel * preferredMinSize.Value.Height
                    : preferredMinSize.Value.Height;

                ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(desiredWidth, desiredHeight));
            }

            await SetTitleBarAsync();
        }

        #endregion

        #region Private Methods

        private async void OnUiSettingsColorValuesChanged(UISettings sender, object args)
        {
            await SetTitleBarAsync();
        }

        private async Task SetTitleBarAsync()
        {
            if (coreDispatcher.HasThreadAccess)
            {
                SetTitleBarColorResources();
            }
            else
            {
                await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    SetTitleBarColorResources();
                });
            }
        }

        private void SetTitleBarColorResources()
        {
            applicationViewTitleBar.BackgroundColor = GetColorResource(applicationViewData.BackgroundColorResource);
            applicationViewTitleBar.InactiveBackgroundColor = GetColorResource(applicationViewData.InactiveBackgroundColorResource);
            applicationViewTitleBar.ForegroundColor = GetColorResource(applicationViewData.ForegroundColorResource);
            applicationViewTitleBar.InactiveForegroundColor = GetColorResource(applicationViewData.InactiveForegroundColorResource);

            applicationViewTitleBar.ButtonBackgroundColor = GetColorResource(applicationViewData.ButtonBackgroundColorResource);
            applicationViewTitleBar.ButtonHoverBackgroundColor = GetColorResource(applicationViewData.ButtonHoverBackgroundColorResource);
            applicationViewTitleBar.ButtonPressedBackgroundColor = GetColorResource(applicationViewData.ButtonPressedBackgroundColorResource);
            applicationViewTitleBar.ButtonInactiveBackgroundColor = GetColorResource(applicationViewData.ButtonInactiveBackgroundColorResource);

            applicationViewTitleBar.ButtonForegroundColor = GetColorResource(applicationViewData.ButtonForegroundColorResource);
            applicationViewTitleBar.ButtonHoverForegroundColor = GetColorResource(applicationViewData.ButtonHoverForegroundColorResource);
            applicationViewTitleBar.ButtonPressedForegroundColor = GetColorResource(applicationViewData.ButtonPressedForegroundColorResource);
            applicationViewTitleBar.ButtonInactiveForegroundColor = GetColorResource(applicationViewData.ButtonInactiveForegroundColorResource);
        }

        private Color? GetColorResource(string colorResource)
        {
            if (!string.IsNullOrWhiteSpace(colorResource) &&
                Application.Current.Resources.TryGetValue(colorResource, out object colorResourceValue))
            {
                return (Color)colorResourceValue;
            }

            return null;
        }

        #endregion
    }
}