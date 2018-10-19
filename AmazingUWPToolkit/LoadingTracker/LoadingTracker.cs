namespace AmazingUWPToolkit
{
    public class LoadingTracker : ILoadingTracker
    {
        #region Fields

        private readonly ILoadingTrackable loadingTrackable;

        #endregion

        #region Constructor

        public LoadingTracker(ILoadingTrackable loadingTrackable)
        {
            this.loadingTrackable = loadingTrackable;

            this.loadingTrackable.IsLoading = true;
            this.loadingTrackable.OnBeginLoading();
        }

        #endregion

        #region Implementation of ILoadingTracker

        public void Dispose()
        {
            loadingTrackable.IsLoading = false;
            loadingTrackable.OnEndLoading();
        }

        #endregion
    }
}