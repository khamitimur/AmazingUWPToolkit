using System;

namespace AmazingUWPToolkit.Controls
{
    public class PullToRefreshProgressEventArgs : EventArgs
    {
        #region Constructor

        public PullToRefreshProgressEventArgs(double progress)
        {
            Progress = progress;
        }

        #endregion

        #region Properties

        public double Progress { get; }

        #endregion
    }
}