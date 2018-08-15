using Windows.Foundation;

namespace AmazingUWPToolkit.ApplicatonView
{
    /// <summary>
    /// Holds application view data.
    /// </summary>
    public interface IApplicationViewData
    {
        /// <summary>
        /// Gets the name of a color resource for the background color of a title bar.
        /// </summary>
        string BackgroundColorResource { get; }

        /// <summary>
        /// Gets the name of a color resource for the background color of a title bar when it's inactive.
        /// </summary>
        string InactiveBackgroundColorResource { get; }

        /// <summary>
        /// Gets the name of a color resource for the foreground color of a title bar.
        /// </summary>
        string ForegroundColorResource { get; }

        /// <summary>
        /// Gets the name of a color resource for the foreground color of a title bar when it's inactive.
        /// </summary>
        string InactiveForegroundColorResource { get; }

        /// <summary>
        /// Gets the name of a color resource for the background color of a title bar button.
        /// </summary>
        string ButtonBackgroundColorResource { get; }

        /// <summary>
        /// Gets the name of a color resource for the background color of a title bar button when the pointer is over.
        /// </summary>
        string ButtonHoverBackgroundColorResource { get; }

        /// <summary>
        /// Gets the name of a color resource for the background color of a title bar button when it's pressed.
        /// </summary>
        string ButtonPressedBackgroundColorResource { get; }

        /// <summary>
        /// Gets the name of a color resource for the background color of a title bar button when it's inactive.
        /// </summary>
        string ButtonInactiveBackgroundColorResource { get; }

        /// <summary>
        /// Gets the name of a color resource for the foreground color of a title bar button.
        /// </summary>
        string ButtonForegroundColorResource { get; }

        /// <summary>
        /// Gets the name of a color resource for the foreground color of a title bar button when the pointer is over.
        /// </summary>
        string ButtonHoverForegroundColorResource { get; }

        /// <summary>
        /// Gets the name of a color resource for the foreground color of a title bar button when it's pressed.
        /// </summary>
        string ButtonPressedForegroundColorResource { get; }

        /// <summary>
        /// Gets the name of a color resource for the foreground color of a title bar button when it's inactive.
        /// </summary>
        string ButtonInactiveForegroundColorResource { get; }

        /// <summary>
        /// Gets a value that specifies whether a title bar should replace the default window title bar.
        /// </summary>
        bool ExtendIntoTitleBar { get; }

        /// <summary>
        /// Gets a value that specifies the smallest size allowed for the application window.
        /// </summary>
        Size? PreferredMinSize { get; }

        /// <summary>
        /// Gets a value that specifies whether a <see cref="PreferredMinSize"/> will be calculated using raw (physical) pixels.
        /// </summary>
        bool ConvertPreferredMinSizeUsingRawPixels { get; }
    }
}