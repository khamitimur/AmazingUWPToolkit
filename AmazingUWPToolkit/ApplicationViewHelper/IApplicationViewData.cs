using Windows.Foundation;

namespace AmazingUWPToolkit
{
    public interface IApplicationViewData
    {
        #region Properties

        string ButtonBackgroundColorResource { get; }

        string ButtonHoverBackgroundColorResource { get; }

        string ButtonPressedBackgroundColorResource { get; }

        string ButtonInactiveBackgroundColorResource { get; }

        bool ExtendIntoTitleBar { get; }

        Size PreferredMinSize { get; }

        bool ConvertPreferredMinSizeUsingRawPixels { get; }

        #endregion
    }
}