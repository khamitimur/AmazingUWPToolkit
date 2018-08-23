using Windows.Foundation;

namespace AmazingUWPToolkit.Gaze.Controls
{
    public interface IGazeControl
    {
        #region Methods

        bool IsGazeEnaled { get; }

        #endregion

        #region Methods

        void OnGazeEntered();

        void OnGazeExited();

        void OnGazeFixationProgressChanged(double progress);

        void OnGazeDwelled(Point point);

        #endregion
    }
}