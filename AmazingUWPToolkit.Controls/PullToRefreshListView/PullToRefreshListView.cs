using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AmazingUWPToolkit.Controls
{
    /// <summary>
    /// Extension for <c>ListView</c> that adds "Pull To Refresh" for devices with touch input, precision touchpad and even pen (CFU).
    /// </summary>
    [TemplatePart(Name = LAYOUTROOT_NAME, Type = typeof(Grid))]
    [TemplatePart(Name = SCROLLVIEWER_NAME, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = ITEMSPRESENTERCONTAINER_NAME, Type = typeof(Grid))]
    [TemplatePart(Name = ITEMSPRESENTER_NAME, Type = typeof(ItemsPresenter))]
    [TemplatePart(Name = PULLCONTENTPANEL_NAME, Type = typeof(Grid))]
    public class PullToRefreshListView : ListView, INotifyPropertyChanged
    {
        #region Constansts

        private const string LAYOUTROOT_NAME = "LayoutRoot";
        private const string SCROLLVIEWER_NAME = "ScrollViewer";
        private const string ITEMSPRESENTERCONTAINER_NAME = "ItemsPresenterContainer";
        private const string ITEMSPRESENTER_NAME = "ItemsPresenter";
        private const string PULLCONTENTPANEL_NAME = "PullContentPanel";

        #endregion

        #region Fields

        private Grid layoutRoot;
        private ScrollViewer scrollViewer;
        private Grid itemsPresenterContainer;
        private ItemsPresenter itemsPresenter;
        private Grid pullContentPanel;

        private bool inProgress;
        private bool readyToRefresh;
        private DateTime lastReadyToRefreshTime;

        private double pullProgress;
        private PullToRefreshState pullToRefreshState;

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="IsPullToRefreshEnabled"/> property.
        /// </summary>
        public static readonly DependencyProperty IsPullToRefreshEnabledProperty = DependencyProperty.Register(
            nameof(IsPullToRefreshEnabled),
            typeof(bool),
            typeof(PullToRefreshListView),
            new PropertyMetadata(true, OnIsPullToRefreshEnabledPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="PullDistance"/> property.
        /// </summary>
        public static readonly DependencyProperty PullDistanceProperty = DependencyProperty.Register(
            nameof(PullDistance),
            typeof(double),
            typeof(PullToRefreshListView),
            new PropertyMetadata(100d));

        /// <summary>
        /// Identifies the <see cref="PullActivationDistance"/> property.
        /// </summary>
        public static readonly DependencyProperty PullActivationDistanceProperty = DependencyProperty.Register(
            nameof(PullActivationDistance),
            typeof(double),
            typeof(PullToRefreshListView),
            new PropertyMetadata(100d));

        /// <summary>
        /// Identifies the <see cref="PullToRefreshContent"/> property.
        /// </summary>
        public static readonly DependencyProperty PullToRefreshContentProperty = DependencyProperty.Register(
            nameof(PullToRefreshContent),
            typeof(object),
            typeof(PullToRefreshListView),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="PullToRefreshContentVerticalAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty PullToRefreshContentVerticalAlignmentProperty = DependencyProperty.Register(
            nameof(PullToRefreshContentVerticalAlignment),
            typeof(VerticalAlignment),
            typeof(PullToRefreshListView),
            new PropertyMetadata(VerticalAlignment.Stretch));

        /// <summary>
        /// Identifies the <see cref="PullToRefreshContentHorizontalAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty PullToRefreshContentHorizontalAlignmentProperty = DependencyProperty.Register(
            nameof(PullToRefreshContentHorizontalAlignment),
            typeof(HorizontalAlignment),
            typeof(PullToRefreshListView),
            new PropertyMetadata(HorizontalAlignment.Stretch));

        /// <summary>
        /// Identifies the <see cref="RefreshCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty RefreshCommandProperty = DependencyProperty.Register(
            nameof(RefreshCommand),
            typeof(ICommand),
            typeof(PullToRefreshListView),
            new PropertyMetadata(null));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PullToRefreshListView"/>.
        /// </summary>
        public PullToRefreshListView()
        {
            DefaultStyleKey = typeof(PullToRefreshListView);

            SizeChanged += OnSizeChanged;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when pull progress has changed.
        /// </summary>
        public event EventHandler<PullToRefreshProgressEventArgs> PullProgressChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets if pull to refresh is enabled.
        /// <para>If disabled <c>RefreshCommand</c> will not fire.</para>
        /// </summary>
        public bool IsPullToRefreshEnabled
        {
            get { return (bool)GetValue(IsPullToRefreshEnabledProperty); }
            set { SetValue(IsPullToRefreshEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets distance when <c>RefreshCommand</c> will fire.
        /// <para>For the best result must be higher or equal to <c>PullToRefreshContent</c> height.</para>
        /// </summary>
        public double PullDistance
        {
            get => (double)GetValue(PullDistanceProperty);
            set => SetValue(PullDistanceProperty, value);
        }

        /// <summary>
        /// Get or sets distance of <c>ScrollViewer.VerticalOffset</c>
        /// when scrolling to top of it will trigger pull to refresh.
        /// </summary>
        public double PullActivationDistance
        {
            get => (double)GetValue(PullActivationDistanceProperty);
            set => SetValue(PullActivationDistanceProperty, value);
        }

        /// <summary>
        /// Get or sets pull to refresh content.
        /// </summary>
        public object PullToRefreshContent
        {
            get => GetValue(PullToRefreshContentProperty);
            set => SetValue(PullToRefreshContentProperty, value);
        }

        /// <summary>
        /// Get or set <c>PullToRefreshContent</c> vertical alignment.
        /// <para>Default is <c>VerticalAlignment.Stretch</c>.</para>
        /// </summary>
        public VerticalAlignment PullToRefreshContentVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(PullToRefreshContentVerticalAlignmentProperty); }
            set { SetValue(PullToRefreshContentVerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Get or set <c>PullToRefreshContent</c> horizontal alignment.
        /// <para>Default is <c>HorizontalAlignment.Stretch</c>.</para>
        /// </summary>
        public HorizontalAlignment PullToRefreshContentHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(PullToRefreshContentHorizontalAlignmentProperty); }
            set { SetValue(PullToRefreshContentHorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets refresh command.
        /// </summary>
        public ICommand RefreshCommand
        {
            get => (ICommand)GetValue(RefreshCommandProperty);
            set => SetValue(RefreshCommandProperty, value);
        }

        /// <summary>
        /// Gets pull progress.
        /// </summary>
        public double PullProgress
        {
            get { return pullProgress; }
            private set
            {
                if (Equals(value, pullProgress)) return;
                pullProgress = value;
                OnPropertyChanged(nameof(PullProgress));
            }
        }

        /// <summary>
        /// Gets "Pull To Refresh" state.
        /// </summary>
        public PullToRefreshState PullToRefreshState
        {
            get { return pullToRefreshState; }
            private set
            {
                if (Equals(value, pullToRefreshState)) return;
                pullToRefreshState = value;
                OnPropertyChanged(nameof(PullToRefreshState));
            }
        }

        #endregion

        #region Overrides of Control

        protected override void OnApplyTemplate()
        {
            layoutRoot = GetTemplateChild(LAYOUTROOT_NAME) as Grid;
            scrollViewer = GetTemplateChild(SCROLLVIEWER_NAME) as ScrollViewer;
            itemsPresenterContainer = GetTemplateChild(ITEMSPRESENTERCONTAINER_NAME) as Grid;
            itemsPresenter = GetTemplateChild(ITEMSPRESENTER_NAME) as ItemsPresenter;
            pullContentPanel = GetTemplateChild(PULLCONTENTPANEL_NAME) as Grid;

            if (layoutRoot != null &&
                scrollViewer != null &&
                itemsPresenterContainer != null &&
                itemsPresenter != null &&
                pullContentPanel != null)
            {
                pullContentPanel.Offset(offsetY: -(float)pullContentPanel.ActualHeight, duration: 0).Start();
                pullContentPanel.SizeChanged += OnPullContentPanelSizeChanged;

                scrollViewer.DirectManipulationStarted += OnScrollViewerDirectManipulationStarted;
                scrollViewer.DirectManipulationCompleted += OnScrollViewerDirectManipulationCompleted;
            }

            base.OnApplyTemplate();
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private Methods

        private static void OnIsPullToRefreshEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is PullToRefreshListView pullToRefreshListView)
            {
                pullToRefreshListView.Release();
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) => Clip = new RectangleGeometry()
        {
            Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height)
        };

        private void OnPullContentPanelSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!inProgress)
            {
                pullContentPanel.Offset(offsetY: -(float)pullContentPanel.ActualHeight, duration: 0).Start();
            }
        }

        private void OnScrollViewerDirectManipulationStarted(object sender, object e)
        {
            if (scrollViewer.VerticalOffset < PullActivationDistance)
            {
                CompositionTarget.Rendering += OnCompositionTargetRendering;
            }
        }

        private void OnScrollViewerDirectManipulationCompleted(object sender, object e)
        {
            if (readyToRefresh)
            {
                RefreshCommand?.Execute(null);
            }

            Release();
        }

        private void OnCompositionTargetRendering(object sender, object e)
        {
            if (scrollViewer.VerticalOffset > 0)
            {
                Release(false);
            }
            else
            {
                inProgress = true;

                var layoutRootBounds = itemsPresenterContainer.TransformToVisual(layoutRoot).TransformBounds(default(Rect));
                var itemsPresenterContainerOffsetY = layoutRootBounds.Y;
                var totalOffsetY = itemsPresenterContainerOffsetY * 2;

                PullProgress = totalOffsetY / PullDistance;
                if (PullProgress >= 1)
                {
                    itemsPresenter.Offset(offsetY: (float)PullDistance / 2, duration: 0).Start();
                    pullContentPanel.Offset(offsetY: (float)PullDistance - (float)pullContentPanel.ActualHeight, duration: 0).Start();

                    if (IsPullToRefreshEnabled)
                    {
                        lastReadyToRefreshTime = DateTime.Now;

                        PullToRefreshState = PullToRefreshState.ReadyToRefresh;

                        readyToRefresh = true;
                    }
                }
                else
                {
                    itemsPresenter.Offset(offsetY: (float)itemsPresenterContainerOffsetY, duration: 0).Start();
                    pullContentPanel.Offset(offsetY: (float)totalOffsetY - (float)pullContentPanel.ActualHeight, duration: 0).Start();

                    var secondsSinceLastReadyToRefresh = (DateTime.Now - lastReadyToRefreshTime).TotalSeconds;
                    if (secondsSinceLastReadyToRefresh > 1)
                    {
                        if (IsPullToRefreshEnabled)
                        {
                            readyToRefresh = false;

                            PullToRefreshState = PullToRefreshState.Pulling;
                        }
                    }
                    else
                    {
                        PullToRefreshState = PullToRefreshState.ReadyToRefresh;
                    }
                }

                PullProgressChanged?.Invoke(this, new PullToRefreshProgressEventArgs(PullProgress));
            }
        }

        private void Release(bool unsubscribeFromCompositionTargetRendering = true)
        {
            if (itemsPresenter == null || pullContentPanel == null)
                return;

            inProgress = false;
            readyToRefresh = false;

            if (unsubscribeFromCompositionTargetRendering)
            {
                PullToRefreshState = PullToRefreshState.Idle;

                CompositionTarget.Rendering -= OnCompositionTargetRendering;
            }

            itemsPresenter.Offset(offsetY: 0, duration: 0).Start();
            pullContentPanel.Offset(offsetY: -(float)pullContentPanel.ActualHeight, duration: 0).Start();

            PullProgressChanged?.Invoke(this, new PullToRefreshProgressEventArgs(0));
        }

        #endregion
    }
}