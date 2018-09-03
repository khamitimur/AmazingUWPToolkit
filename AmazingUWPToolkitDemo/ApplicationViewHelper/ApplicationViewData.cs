using AmazingUWPToolkit.ApplicatonView;
using Windows.Foundation;

namespace AmazingUWPToolkitDemo
{
    public class ApplicationViewData : IApplicationViewData
    {
        #region Implementation of IApplicationViewData

        public string BackgroundColorResource => "TitleBarBackgroundColor";

        public string InactiveBackgroundColorResource => "TitleBarBackgroundColor";

        public string ForegroundColorResource => "TitleBarForegroundColor";

        public string InactiveForegroundColorResource => "TitleBarDisabledForegroundColor";

        public string ButtonBackgroundColorResource => "TitleBarButtonBackgroundColor";

        public string ButtonHoverBackgroundColorResource => "TitleBarButtonHoverColor";

        public string ButtonPressedBackgroundColorResource => "TitleBarButtonPressedColor";

        public string ButtonInactiveBackgroundColorResource => "TitleBarButtonInactiveBackgroundColor";

        public string ButtonForegroundColorResource => "TitleBarForegroundColor";

        public string ButtonHoverForegroundColorResource => "TitleBarForegroundColor";

        public string ButtonPressedForegroundColorResource => "TitleBarForegroundColor";

        public string ButtonInactiveForegroundColorResource => "TitleBarDisabledForegroundColor";

        public bool ExtendIntoTitleBar => true;

        public Size? PreferredMinSize => new Size(1000, 1000);

        public bool ConvertPreferredMinSizeUsingRawPixels => false;

        #endregion
    }
}
