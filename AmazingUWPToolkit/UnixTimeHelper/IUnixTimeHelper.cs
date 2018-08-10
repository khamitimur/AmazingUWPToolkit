using System;

namespace AmazingUWPToolkit
{
    public interface IUnixTimeHelper
    {
        #region Methods

        DateTime ToLocalDateTime(long milliseconds);

        #endregion
    }
}