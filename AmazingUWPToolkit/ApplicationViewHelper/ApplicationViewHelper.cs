using AmazingUWPToolkit.Extensions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using System;

namespace AmazingUWPToolkit
{
    public class ApplicationViewHelper : IApplicationViewHelper
    {
        #region Fields

        private readonly IApplicationViewData applicationViewData;

        private UISettings uiSettings;

        private CoreApplicationViewTitleBar coreApplicationViewTitleBar;
        private ApplicationViewTitleBar applicationViewTitleBar;

        private CoreDispatcher coreDispatcher;

        #endregion

        #region Constructor

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

        public async Task InitializeAsync()
        {
            coreApplicationViewTitleBar.ExtendViewIntoTitleBar = applicationViewData.ExtendIntoTitleBar;

            if (applicationViewData.PreferredMinSize != default)
            {
                var preferredMinSize = applicationViewData.PreferredMinSize;
                var convertPreferredMinSizeUsingRawPixels = applicationViewData.ConvertPreferredMinSizeUsingRawPixels;
                var rawPixelsPerViewPixel = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

                var desiredWidth = convertPreferredMinSizeUsingRawPixels
                    ? rawPixelsPerViewPixel * preferredMinSize.Width
                    : preferredMinSize.Width;
                var desiredHeight = convertPreferredMinSizeUsingRawPixels
                    ? rawPixelsPerViewPixel * preferredMinSize.Height
                    : preferredMinSize.Height;

                ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(desiredWidth, desiredHeight));
            }

            await SetTitleBar();
        }

        #endregion

        #region Private Methods

        private async void OnUiSettingsColorValuesChanged(UISettings sender, object args)
        {
            await SetTitleBar();
        }

        private async Task SetTitleBar()
        {
            if (coreDispatcher.HasThreadAccess)
            {
                applicationViewTitleBar.ButtonBackgroundColor = Application.Current.GetResource<Color>(applicationViewData.ButtonBackgroundColorResource);
                applicationViewTitleBar.ButtonHoverBackgroundColor = Application.Current.GetResource<Color>(applicationViewData.ButtonHoverBackgroundColorResource);
                applicationViewTitleBar.ButtonPressedBackgroundColor = Application.Current.GetResource<Color>(applicationViewData.ButtonPressedBackgroundColorResource);
                applicationViewTitleBar.ButtonInactiveBackgroundColor = Application.Current.GetResource<Color>(applicationViewData.ButtonInactiveBackgroundColorResource);
            }
            else
            {
                await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    applicationViewTitleBar.ButtonBackgroundColor = Application.Current.GetResource<Color>(applicationViewData.ButtonBackgroundColorResource);
                    applicationViewTitleBar.ButtonHoverBackgroundColor = Application.Current.GetResource<Color>(applicationViewData.ButtonHoverBackgroundColorResource);
                    applicationViewTitleBar.ButtonPressedBackgroundColor = Application.Current.GetResource<Color>(applicationViewData.ButtonPressedBackgroundColorResource);
                    applicationViewTitleBar.ButtonInactiveBackgroundColor = Application.Current.GetResource<Color>(applicationViewData.ButtonInactiveBackgroundColorResource);
                });
            }
        }

        #endregion
    }
}