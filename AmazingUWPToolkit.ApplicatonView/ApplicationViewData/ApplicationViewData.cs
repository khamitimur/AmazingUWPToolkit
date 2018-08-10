using Windows.Foundation;

namespace AmazingUWPToolkit.ApplicatonView
{
    /// <summary>
    /// Default implementation of <see cref="IApplicationViewData"/>.
    /// </summary>
    public class ApplicationViewData : IApplicationViewData
    {
        #region Fields

        private const string EMPTY_COLOR_RESOURCE = "";

        #endregion

        #region Contructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationViewData"/>.
        /// </summary>
        /// <param name="backgroundColorResource"></param>
        /// <param name="inactiveBackgroundColorResource"></param>
        /// <param name="foregroundColorResource"></param>
        /// <param name="inactiveForegroundColorResource"></param>
        /// <param name="buttonBackgroundColorResource"></param>
        /// <param name="buttonHoverBackgroundColorResource"></param>
        /// <param name="buttonPressedBackgroundColorResource"></param>
        /// <param name="buttonInactiveBackgroundColorResource"></param>
        /// <param name="buttonForegroundColorResource"></param>
        /// <param name="buttonHoverForegroundColorResource"></param>
        /// <param name="buttonPressedForegroundColorResource"></param>
        /// <param name="buttonInactiveForegroundColorResource"></param>
        /// <param name="extendIntoTitleBar"></param>
        /// <param name="preferredMinSize"></param>
        /// <param name="convertPreferredMinSizeUsingRawPixels"></param>
        public ApplicationViewData(string backgroundColorResource = EMPTY_COLOR_RESOURCE,
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