using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AmazingUWPToolkit.Controls
{
    internal class CharItemsCollectionHelper : ICharItemsCollectionHelper, INotifyPropertyChanged
    {
        #region Fields

        private const string ALLOWED_RANDOM_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        private Random charItemsRandom;

        #endregion

        #region Constructor

        public CharItemsCollectionHelper()
        {
            Collection = new ExtendedObservableCollection<CharItem>();

            charItemsRandom = new Random();

            Initialize();
        }

        #endregion

        #region Properties

        private int Size => ColumnsCount * RowsCount;

        #endregion

        #region Overrides of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Implementation of ICharItemsCollectionHelper

        public int ColumnsCount => 11;

        public int RowsCount => 11;

        [NotNull]
        public ExtendedObservableCollection<CharItem> Collection { get; }

        public bool IsInitialized => Collection?.Count != 0;

        public void Reset()
        {
            foreach (var notRandomCharItem in Collection.Where(charItem => !charItem.IsRandom))
            {
                notRandomCharItem.Update(GetRandomCharItem());
            }
        }

        public void Update([NotNull, ItemNotNull] ReadOnlyDictionary<int, CharItem> charItemsToUpdateDictionary)
        {
            if (!IsInitialized) throw new InvalidOperationException("Collection wasn't initialized.");

            var notRandomCharsToResetIndexesList = GetNotRandomCharsIndexes();

            foreach (var charItemToUpdate in charItemsToUpdateDictionary)
            {
                var index = charItemToUpdate.Key;
                var charItem = charItemToUpdate.Value;

                if (notRandomCharsToResetIndexesList.Contains(index))
                {
                    notRandomCharsToResetIndexesList.Remove(index);
                }

                Collection[index].Update(charItem);
            }

            foreach (var notRandomCharsToResetIndex in notRandomCharsToResetIndexesList)
            {
                Collection[notRandomCharsToResetIndex].Update(GetRandomCharItem());
            }
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            var charItemsToAdd = new List<CharItem>();

            for (int i = 0; i < Size; i++)
            {
                charItemsToAdd.Add(GetRandomCharItem());
            }

            Collection.AddRange(charItemsToAdd);
        }

        private CharItem GetRandomCharItem()
        {
            var @char = ALLOWED_RANDOM_CHARS[charItemsRandom.Next(0, ALLOWED_RANDOM_CHARS.Length - 1)];

            return new CharItem(@char, true);
        }

        private List<int> GetNotRandomCharsIndexes()
        {
            var notRandomCharsIndexesList = new List<int>();

            foreach (var notRandomCharItem in Collection.Where(charItem => !charItem.IsRandom))
            {
                notRandomCharsIndexesList.Add(Collection.IndexOf(notRandomCharItem));
            }

            return notRandomCharsIndexesList;
        }

        #endregion
    }
}