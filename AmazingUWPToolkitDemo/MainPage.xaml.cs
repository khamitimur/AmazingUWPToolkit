using AmazingUWPToolkit.ApplicatonView;
using AmazingUWPToolkit.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AmazingUWPToolkitDemo
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        #region Contructor

        public MainPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public ApplicationViewHelper ApplicationViewHelper { get; private set; }

        #endregion

        #region Overrides of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Overrides of Page

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await SetApplicationViewHelperAsync();

            base.OnNavigatedTo(e);
        }

        #endregion

        #region ApplicationViewHelper

        private async Task SetApplicationViewHelperAsync()
        {
            // Initializes a new instance of a ApplicationViewHelper with internal implementation of IApplicationViewData.
            // Also you can use default implementation of IApplicationViewData.
            ApplicationViewHelper = new ApplicationViewHelper(new ApplicationViewData());
            await ApplicationViewHelper.SetAsync();
        }

        private async void OnColorSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:personalization-colors"));

        }

        #endregion

        #region Gaze

        private int pressCount = 0;
        private int pressCount1 = 0;

        private void OnFirstGazeButtonClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).Content = $"Pressed! {++pressCount}";
        }

        private void OnSecondGazeButtonClick(object sender, RoutedEventArgs e)
        {
            (sender as Button).Content = $"Pressed! {++pressCount1}";
        }

        #endregion
    }
}