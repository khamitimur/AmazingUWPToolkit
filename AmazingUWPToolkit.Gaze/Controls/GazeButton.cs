using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Gaze.Controls
{
    public sealed class GazeButton : Button
    {
        #region Fields

        private GazeTracker gazeTracker;

        #endregion

        #region Contructor

        public GazeButton()
        {
            DefaultStyleKey = typeof(GazeButton);
        }

        #endregion

        #region Overrides of Control

        protected override void OnApplyTemplate()
        {
            gazeTracker = new GazeTracker(this);
            gazeTracker.Start();

            base.OnApplyTemplate();
        }

        #endregion
    }
}