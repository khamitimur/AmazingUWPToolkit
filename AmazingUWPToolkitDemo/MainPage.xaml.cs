using AmazingUWPToolkit.ApplicatonView;
using System.Threading.Tasks;
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
            // Initializes a new instance of ApplicationViewHelper with internal implementation of IApplicationViewData.
            var applicationViewHelper = new ApplicationViewHelper(new ApplicationViewData());
            await applicationViewHelper.SetAsync();
        }

        private void OnLightThemeButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.RequestedTheme = ApplicationTheme.Light;
        }

        private void OnDarkThemeButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.RequestedTheme = ApplicationTheme.Dark;
        }

        #endregion
    }
}