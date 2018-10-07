using Windows.Foundation;

namespace AmazingUWPToolkit.Gaze
{
    internal interface IInputInjectorHelper
    {
        #region Methods

        void Inject(Point point);

        void UninitializeInjection();

        #endregion
    }
}