using Windows.Foundation;

namespace AmazingUWPToolkit.ApplicatonView
{
    /// <inheritdoc/>
    public class DefaultApplicationViewData : IApplicationViewData
    {
        #region Fields

        private const string EMPTY_COLOR_RESOURCE = "";

        #endregion

        #region Contructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultApplicationViewData"/>.
        /// </summary>
        /// <param name="backgroundColorResource">Name of a color resource for the background color of a title bar.</param>
        /// <param name="inactiveBackgroundColorResource">Name of a color resource for the background color of a title bar when it's inactive.</param>
        /// <param name="foregroundColorResource">Name of a color resource for the foreground color of a title bar.</param>
        /// <param name="inactiveForegroundColorResource">Name of a color resource for the foreground color of a title bar when it's inactive.</param>
        /// <param name="buttonBackgroundColorResource">Name of a color resource for the background color of a title bar button.</param>
        /// <param name="buttonHoverBackgroundColorResource">Name of a color resource for the background color of a title bar button when the pointer is over.</param>
        /// <param name="buttonPressedBackgroundColorResource">Name of a color resource for the background color of a title bar button when it's pressed.</param>
        /// <param name="buttonInactiveBackgroundColorResource">Name of a color resource for the background color of a title bar button when it's inactive.</param>
        /// <param name="buttonForegroundColorResource">Name of a color resource for the foreground color of a title bar button.</param>
        /// <param name="buttonHoverForegroundColorResource">Name of a color resource for the foreground color of a title bar button when the pointer is over.</param>
        /// <param name="buttonPressedForegroundColorResource">Name of a color resource for the foreground color of a title bar button when it's pressed.</param>
        /// <param name="buttonInactiveForegroundColorResource">Name of a color resource for the foreground color of a title bar button when it's inactive.</param>
        /// <param name="extendIntoTitleBar">Specifies whether a title bar should replace the default window title bar.</param>
        /// <param name="preferredMinSize">Specifies the smallest size allowed for the application window.</param>
        /// <param name="convertPreferredMinSizeUsingRawPixels">Specifies whether a <paramref name="preferredMinSize"/> will be calculated using raw (physical) pixels.</param>
        public DefaultApplicationViewData(string backgroundColorResource = EMPTY_COLOR_RESOURCE,
                                          string inactiveBackgroundColorResource = EMPTY_COLOR_RESOURCE,
                                          string foregroundColorResource = EMPTY_COLOR_RESOURCE,
                                          string inactiveForegroundColorResource = EMPTY_COLOR_RESOURCE,
                                          string buttonBackgroundColorResource = EMPTY_COLOR_RESOURCE,
                                          string buttonHoverBackgroundColorResource = EMPTY_COLOR_RESOURCE,
                                          string buttonPressedBackgroundColorResource = EMPTY_COLOR_RESOURCE,
                                          string buttonInactiveBackgroundColorResource = EMPTY_COLOR_RESOURCE,
                                          string buttonForegroundColorResource = EMPTY_COLOR_RESOURCE,
                                          string buttonHoverForegroundColorResource = EMPTY_COLOR_RESOURCE,
                                          string buttonPressedForegroundColorResource = EMPTY_COLOR_RESOURCE,
                                          string buttonInactiveForegroundColorResource = EMPTY_COLOR_RESOURCE,
                                          bool extendIntoTitleBar = false,
                                          Size? preferredMinSize = null,
                                          bool convertPreferredMinSizeUsingRawPixels = false)
        {
            BackgroundColorResource = backgroundColorResource;
            InactiveBackgroundColorResource = inactiveBackgroundColorResource;
            ForegroundColorResource = foregroundColorResource;
            InactiveForegroundColorResource = InactiveForegroundColorResource;
            ButtonBackgroundColorResource = buttonBackgroundColorResource;
            ButtonHoverBackgroundColorResource = buttonHoverBackgroundColorResource;
            ButtonPressedBackgroundColorResource = buttonPressedBackgroundColorResource;
            ButtonInactiveBackgroundColorResource = buttonInactiveBackgroundColorResource;
            ButtonForegroundColorResource = buttonForegroundColorResource;
            ButtonHoverForegroundColorResource = buttonHoverForegroundColorResource;
            ButtonPressedForegroundColorResource = buttonPressedForegroundColorResource;
            ButtonInactiveForegroundColorResource = buttonInactiveForegroundColorResource;
            ExtendIntoTitleBar = extendIntoTitleBar;
            PreferredMinSize = preferredMinSize;
            ConvertPreferredMinSizeUsingRawPixels = convertPreferredMinSizeUsingRawPixels;
        }

        #endregion

        #region Overrides of IApplicationViewData

        /// <inheritdoc/>
        public string BackgroundColorResource { get; }

        /// <inheritdoc/>
        public string InactiveBackgroundColorResource { get; }

        /// <inheritdoc/>
        public string ForegroundColorResource { get; }

        /// <inheritdoc/>
        public string InactiveForegroundColorResource { get; }

        /// <inheritdoc/>
        public string ButtonBackgroundColorResource { get; }

        /// <inheritdoc/>
        public string ButtonHoverBackgroundColorResource { get; }

        /// <inheritdoc/>
        public string ButtonPressedBackgroundColorResource { get; }

        /// <inheritdoc/>
        public string ButtonInactiveBackgroundColorResource { get; }

        /// <inheritdoc/>
        public string ButtonForegroundColorResource { get; }

        /// <inheritdoc/>
        public string ButtonHoverForegroundColorResource { get; }

        /// <inheritdoc/>
        public string ButtonPressedForegroundColorResource { get; }

        /// <inheritdoc/>
        public string ButtonInactiveForegroundColorResource { get; }

        /// <inheritdoc/>
        public bool ExtendIntoTitleBar { get; }

        /// <inheritdoc/>
        public Size? PreferredMinSize { get; }

        /// <inheritdoc/>
        public bool ConvertPreferredMinSizeUsingRawPixels { get; }

        #endregion
    }
}