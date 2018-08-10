namespace AmazingUWPToolkit
{
    public interface ILoadSupportedViewModel
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