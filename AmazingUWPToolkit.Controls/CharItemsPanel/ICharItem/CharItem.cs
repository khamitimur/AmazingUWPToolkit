using System;
using System.ComponentModel;
using JetBrains.Annotations;

namespace AmazingUWPToolkit.Controls
{
    internal class CharItem : ICharItem, INotifyPropertyChanged
    {
        #region Constructor

        public CharItem([NotNull] CharItem charItem) : this(charItem.Char, charItem.IsRandom)
        {

        }

        public CharItem(char @char, bool isRandom = false)
        {
            Char = @char;
            IsRandom = isRandom;
        }

        #endregion

        #region Overrides of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Overrides of ICharItem

        public event EventHandler<EventArgs> Updated;

        [CanBeNull]
        public ICharItem PreviousCharItem { get; private set; }

        [NotNull]
        public char Char { get; private set; }

        public bool IsRandom { get; private set; }

        public void Update([NotNull] ICharItem charItem)
        {
            Update(charItem.Char, charItem.IsRandom);
        }

        public void Update(char @char, bool isRandom)
        {
            PreviousCharItem = new CharItem(this);

            Char = @char;
            IsRandom = isRandom;

            Updated?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
