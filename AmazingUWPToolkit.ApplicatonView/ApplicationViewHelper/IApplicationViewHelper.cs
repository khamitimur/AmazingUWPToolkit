using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace AmazingUWPToolkit.ApplicatonView
{
    /// <summary>
    /// Controls <see cref="CoreApplicationViewTitleBar"/> and <see cref="ApplicationViewTitleBar"/>.
    /// </summary>
    public interface IApplicationViewHelper
    {
        #region Properties

        /// <summary>
        /// Returns height of <see cref="CoreApplicationViewTitleBar"/>.
        /// <remarks><para>
        /// Will return 0 when <see cref="CoreApplicationViewTitleBar.IsVisible"/> is false.
        /// </para></remarks>
        /// </summary>
        double TitleBarHeight { get; }

        /// <summary>
        /// Returns margin where top will have value of a height of <see cref="CoreApplicationViewTitleBar"/> and other sides will be 0.
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