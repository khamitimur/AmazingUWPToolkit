using AmazingUWPToolkit.ApplicatonView;
using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkitDemo
{
    public sealed partial class MainPage : Page
    {
        #region Contructor

        public MainPage()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        #endregion

        #region Private Methods

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await SetApplicationViewHelperAsync();
        }

        #endregion

        #region ApplicationViewHelper

        private async Task SetApplicationViewHelperAsync()
        {
            // Initializes a new instance of a ApplicationViewHelper with internal implementation of IApplicationViewData.
            // Also you can use default implementation of IApplicationViewData.
            var applicationViewHelper = new ApplicationViewHelper(new ApplicationViewData());
            await applicationViewHelper.SetAsync();
        }

        #endregion

        private async void OnColorSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:personalization-colors"));

        }
    }
}