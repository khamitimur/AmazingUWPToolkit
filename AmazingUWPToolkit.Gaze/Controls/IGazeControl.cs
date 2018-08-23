namespace AmazingUWPToolkit.Gaze.Controls
{
    public interface IGazeControl
    {
        #region Methods

        void OnGazeEntered();

        void OnGazeExited();

        void OnGazeOverProgressChanged(double progress);

        void OnGazeActionRequested();

        #endregion
    }
}