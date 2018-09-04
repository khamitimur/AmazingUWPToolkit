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
        #region Fields

        private Random random;

        #endregion

        #region Contructor

        public MainPage()
        {
            InitializeComponent();

            random = new Random();

            SetStackedBars();
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

        #region StackedBar

        public ObservableCollection<StackedBarItem> StackedBars { get; private set; }

        private void SetStackedBars()
        {
            StackedBars = new ObservableCollection<StackedBarItem>()
            {
                new StackedBarItem(20, Colors.Blue, "Flowers"),
                new StackedBarItem(30, Colors.Red, "Food"),
                new StackedBarItem(10, Colors.Gold, "Tea"),
                new StackedBarItem(20, Colors.LightBlue, "Travel"),
                new StackedBarItem(10, Colors.Magenta, "Water"),
                new StackedBarItem(10, Colors.Yellow, "Gift")
            };
        }

        private void OnAddStakedBarItemButtonClick(object sender, RoutedEventArgs e)
        {
            var randomStackedBarItemValue = random.Next(10, 50);
            var randomStackedBarItemColor = Color.FromArgb(255, (byte)random.Next(50, 255), (byte)random.Next(50, 255), (byte)random.Next(50, 255));
            var randomStackedBarItem = new StackedBarItem(randomStackedBarItemValue, randomStackedBarItemColor);

            StackedBars.Add(randomStackedBarItem);
        }

        private void OnRemoveStakedBarItemButtonClick(object sender, RoutedEventArgs e)
        {
            if (StackedBars.Count == 0)
                return;

            var randomStackedBarItemIndex = random.Next(0, StackedBars.Count);

            StackedBars.RemoveAt(randomStackedBarItemIndex);
        }

        private void OnResetStakedBarItemButtonClick(object sender, RoutedEventArgs e)
        {
            SetStackedBars();
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