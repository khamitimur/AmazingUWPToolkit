using System.ComponentModel;

namespace AmazingUWPToolkit.Controls
{
    internal class TextBoardModel : ITextBoardModel, INotifyPropertyChanged
    {
        #region Implementation of ITextBoardModel

        public double ItemWidth { get; set; }

        public double ItemHeight { get; set; }

        public double WrapPanelMaxWidth { get; set; }

        public double WrapPanelMaxHeight { get; set; }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}