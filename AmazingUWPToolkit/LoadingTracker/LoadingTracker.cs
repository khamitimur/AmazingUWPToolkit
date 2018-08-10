using System;

namespace AmazingUWPToolkit
{
    public class LoadingTracker : ILoadingTracker
    {
        #region Fields

        private readonly ILoadSupportedViewModel viewModel;

        #endregion

        #region Constructor

        public LoadingTracker(ILoadSupportedViewModel viewModel)
        {
            this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            this.viewModel.IsLoading = true;
            this.viewModel.OnBeginLoading();
        }

        #endregion

        #region Implementation of ILoadingTracker

        public void Dispose()
        {
            viewModel.IsLoading = false;
            viewModel.OnEndLoading();
        }

        #endregion
    }
}