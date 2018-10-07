using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace AmazingUWPToolkit.Gaze
{
    public static class Gaze
    {
        #region Fields

        private static IInputInjectorHelper inputInjectorHelper;
        private static Dictionary<int, GazeTracker> gazeTrackerIntances;

        #endregion

        #region Contructor

        static Gaze()
        {
            inputInjectorHelper = new InputInjectorHelper();
            gazeTrackerIntances = new Dictionary<int, GazeTracker>();
        }

        #endregion

        #region Public Methods

        public static IGazeTracker GetInstance(int? applicationViewId = null)
        {
            if (applicationViewId == null)
            {
                applicationViewId = ApplicationView.GetForCurrentView().Id;
            }

            gazeTrackerIntances.TryGetValue(applicationViewId.Value, out GazeTracker gazeTracker);
            if (gazeTracker == null)
            {
                gazeTracker = new GazeTracker();

                gazeTrackerIntances.Add(applicationViewId.Value, gazeTracker);
            }

            return gazeTracker;
        }

        public static void InjectInput(Point point)
        {
            inputInjectorHelper.Inject(point);
        }

        public static void UninitializeInputInjection()
        {
            inputInjectorHelper.UninitializeInjection();
        }

        #endregion
    }
}