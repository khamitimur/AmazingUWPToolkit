namespace AmazingUWPToolkit
{
    public interface ILoadingTrackable
    {
        #region Properties

        bool IsLoading { get; set; }

        #endregion

        #region Methods

        void OnBeginLoading();

        void OnEndLoading();

        #endregion
    }
}