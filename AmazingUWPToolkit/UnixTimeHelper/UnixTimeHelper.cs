using System;

namespace AmazingUWPToolkit
{
    public class UnixTimeHelper : IUnixTimeHelper
    {
        #region Fields

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        #region Overrides of IUnixTimeHelper

        public DateTime ToLocalDateTime(long milliseconds)
        {
            return Epoch.AddSeconds(milliseconds).ToLocalTime();
        }

        #endregion
    }
}