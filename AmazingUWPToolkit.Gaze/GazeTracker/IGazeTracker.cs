using Windows.UI.Xaml.Controls;

namespace AmazingUWPToolkit.Gaze
{
    public interface IGazeTracker
    {
        #region Methods

        bool AddControl(Control control);

        bool RemoveControl(Control control);

        void Start();

        void Stop();

        #endregion
    }
}