using System;
using JetBrains.Annotations;

namespace AmazingUWPToolkit.Controls
{
    internal interface ICharItem
    {
        #region Properties

        [CanBeNull]
        ICharItem PreviousCharItem { get; }

        [NotNull]
        char Char { get; }

        bool IsRandom { get; }

        #endregion

        #region Events

        event EventHandler<EventArgs> Updated;

        #endregion

        #region Methods

        void Update([NotNull] ICharItem charItem);

        void Update(char @char, bool isRandom);

        #endregion
    }
}