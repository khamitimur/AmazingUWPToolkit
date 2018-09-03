using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AmazingUWPToolkit.Controls.Behaviors
{
    internal class TextBoardItemBehavior : Behavior<TextBoardItem>
    {
        #region Dependency Properties

        public static readonly DependencyProperty TextBoardModelProperty = DependencyProperty.Register(
            nameof(TextBoardModel),
            typeof(ITextBoardModel),
            typeof(TextBoardItemBehavior),
            new PropertyMetadata(null, OnTextBoardModelPropertyChanged));

        #endregion

        #region Properties

        public ITextBoardModel TextBoardModel
        {
            get => (ITextBoardModel)GetValue(TextBoardModelProperty);
            set => SetValue(TextBoardModelProperty, value);
        }

        #endregion

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded += OnLoaded;
            }

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= OnLoaded;
            }

            base.OnDetaching();
        }

        #endregion

        #region Private Methods

        private static void OnTextBoardModelPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            (dependencyObject as TextBoardItemBehavior)?.TryToSetTextProperties();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TryToSetTextProperties();
        }

        private void TryToSetTextProperties()
        {
            if (AssociatedObject == null || TextBoardModel == null)
                return;

            var fontSizeBinding = new Binding
            {
                Source = TextBoardModel,
                Path = new PropertyPath("FontSize"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            var randomTextBoardItemOpacityBinding = new Binding
            {
                Source = TextBoardModel,
                Path = new PropertyPath("RandomTextBoardItemOpacity"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            BindingOperations.SetBinding(AssociatedObject, Control.FontSizeProperty, fontSizeBinding);
            BindingOperations.SetBinding(AssociatedObject, TextBoardItem.RandomTextBoardItemOpacityProperty, randomTextBoardItemOpacityBinding);
        }

        #endregion
    }
}