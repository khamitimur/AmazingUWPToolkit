using AmazingUWPToolkit.ApplicatonView;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkitDemo
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        #region Contructor

        public MainPage()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        #endregion

        #region Properties

        public ApplicationViewHelper ApplicationViewHelper { get; private set; }

        #endregion

        #region Overrides of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Methods

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await SetApplicationViewHelperAsync();
        }

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

        #endregion

        private int pressCount = 0;
        private int pressCount1 = 0;

        private void GazeButton_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).Content = $"Pressed! {++pressCount}";
        }

        private void GazeButton_Click_1(object sender, RoutedEventArgs e)
        {
            (sender as Button).Content = $"Pressed! {++pressCount1}";
        }
    }
}