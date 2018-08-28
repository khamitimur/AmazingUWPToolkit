using JetBrains.Annotations;
using System.Collections.ObjectModel;

namespace AmazingUWPToolkit.Controls
{
    internal interface ICharItemsCollectionHelper
    {
        #region Properties

        int ColumnsCount { get; }

        int RowsCount { get; }

        [NotNull]
        ExtendedObservableCollection<CharItem> Collection { get; }

        bool IsInitialized { get; }

        #endregion

        #region Methods

        void Reset();

        void Update([NotNull, ItemNotNull] ReadOnlyDictionary<int, CharItem> charItemsToUpdateDictionary);

        #endregion
    }
}