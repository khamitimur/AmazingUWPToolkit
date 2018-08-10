using System;
using Windows.UI.Xaml;

namespace AmazingUWPToolkit.Extensions
{
    public static class ApplicationExtensions
    {
        #region Public Methods

        public static object GetResource(this Application application, string key)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException($"{nameof(key)} can't be null or empty.", nameof(key));

            return application.Resources[key];
        }

        public static T GetResource<T>(this Application application, string key)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException($"{nameof(key)} can't be null or empty.", nameof(key));

            return (T)application.GetResource(key);
        }

        #endregion
    }
}