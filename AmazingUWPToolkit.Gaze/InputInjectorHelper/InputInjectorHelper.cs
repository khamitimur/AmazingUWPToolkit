using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Input.Preview.Injection;
using Windows.UI.ViewManagement;

namespace AmazingUWPToolkit.Gaze
{
    internal class InputInjectorHelper : IInputInjectorHelper
    {
        #region Fields

        private readonly InputInjectorPointerDeviceType deviceType;

        private InputInjector inputInjector;

        #endregion

        #region Constructor

        public InputInjectorHelper(InputInjectorPointerDeviceType deviceType = InputInjectorPointerDeviceType.Touch)
        {
            this.deviceType = deviceType;
        }

        #endregion

        #region Implementation of IInputInjectorHelper

        public void Inject(Point point)
        {
            if (inputInjector == null)
            {
                inputInjector = InputInjector.TryCreate();
            }

            if (inputInjector == null)
                return;

            var pointerId = GetPointerId();
            var relativePoint = GetRelativePoint(point);

            switch (deviceType)
            {
                case InputInjectorPointerDeviceType.Touch:
                    InjectTouchInput(relativePoint, pointerId);
                    break;

                case InputInjectorPointerDeviceType.Pen:
                    InjectPenInput(relativePoint, pointerId);
                    break;
            }
        }

        public void UninitializeInjection()
        {
            switch (deviceType)
            {
                case InputInjectorPointerDeviceType.Touch:
                    inputInjector?.UninitializeTouchInjection();
                    break;

                case InputInjectorPointerDeviceType.Pen:
                    inputInjector?.UninitializePenInjection();
                    break;
            }

            inputInjector = null;
        }

        #endregion

        #region Private Methods

        private void InjectPenInput(Point point, uint pointerId)
        {
            inputInjector.InitializePenInjection(InjectedInputVisualizationMode.None);

            var injectedInputPenInfo = new InjectedInputPenInfo
            {
                PointerInfo = new InjectedInputPointerInfo
                {
                    PointerId = pointerId,
                    PixelLocation = new InjectedInputPoint
                    {
                        PositionX = (int)point.X,
                        PositionY = (int)point.Y
                    },
                    PointerOptions = InjectedInputPointerOptions.PointerDown |
                                     InjectedInputPointerOptions.InContact |
                                     InjectedInputPointerOptions.New
                },
                Pressure = 1.0,
                PenParameters = InjectedInputPenParameters.Pressure
            };

            inputInjector.InjectPenInput(injectedInputPenInfo);

            injectedInputPenInfo = new InjectedInputPenInfo
            {
                PointerInfo = new InjectedInputPointerInfo
                {
                    PointerId = pointerId,
                    PointerOptions = InjectedInputPointerOptions.PointerUp
                }
            };

            inputInjector.InjectPenInput(injectedInputPenInfo);
        }

        private void InjectTouchInput(Point point, uint pointerId)
        {
            inputInjector.InitializeTouchInjection(InjectedInputVisualizationMode.None);

            var injectedInputTouchInfoList = new List<InjectedInputTouchInfo>
            {
                new InjectedInputTouchInfo
                {
                    Contact = new InjectedInputRectangle
                    {
                        Left = 30, Top = 30, Bottom = 30, Right = 30
                    },
                    PointerInfo = new InjectedInputPointerInfo
                    {
                        PointerId = pointerId,
                        PixelLocation = new InjectedInputPoint
                        {
                            PositionX = (int)point.X,
                            PositionY = (int)point.Y
                        },
                        PointerOptions = InjectedInputPointerOptions.PointerDown |
                                         InjectedInputPointerOptions.InContact |
                                         InjectedInputPointerOptions.New
                },
                Pressure = 1.0,
                TouchParameters = InjectedInputTouchParameters.Pressure |
                                  InjectedInputTouchParameters.Contact
                }
            };

            inputInjector.InjectTouchInput(injectedInputTouchInfoList);

            injectedInputTouchInfoList = new List<InjectedInputTouchInfo>
            {
                new InjectedInputTouchInfo
                {
                    PointerInfo = new InjectedInputPointerInfo
                    {
                        PointerId = pointerId,
                        PointerOptions = InjectedInputPointerOptions.PointerUp
                    }
                }
            };

            inputInjector.InjectTouchInput(injectedInputTouchInfoList);
        }

        private static uint GetPointerId() => (uint)new Random().Next(0, int.MaxValue);

        // TODO: Добавить поиск AplicationView по id.
        private static Point GetRelativePoint(Point point)
        {
            var applicationVisibleBounds = ApplicationView.GetForCurrentView().VisibleBounds;

            return new Point(point.X + applicationVisibleBounds.Left, point.Y + applicationVisibleBounds.Top);
        }

        #endregion
    }
}