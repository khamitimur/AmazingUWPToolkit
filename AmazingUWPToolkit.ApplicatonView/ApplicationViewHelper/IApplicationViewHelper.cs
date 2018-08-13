using System.Threading.Tasks;

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