using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AmazingUWPToolkit.ApplicatonView
{
    public interface IApplicationViewHelper
    {
        #region Properties

        /// <summary>
        /// Returns height of <see cref="Windows.ApplicationModel.Core.CoreApplicationViewTitleBar"/>.
        /// <remarks><para>
        /// Will return 0 when <see cref="Windows.ApplicationModel.Core.CoreApplicationViewTitleBar.IsVisible"/> is false.
        /// </para></remarks>
        /// </summary>
        double TitleBarHeight { get; }

        /// <summary>
        /// Returns margin where top will have value of a height of <see cref="Windows.ApplicationModel.Core.CoreApplicationViewTitleBar"/> and other sides will be 0.
        /// </summary>
        Thickness TitleBarMargin { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets title bar properties.
        /// </summary>
        /// <returns></returns>
        Task SetAsync();

        #endregion
    }
}