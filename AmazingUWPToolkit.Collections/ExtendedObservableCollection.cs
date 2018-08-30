using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace AmazingUWPToolkit.Collections
{
    /// <summary>
    /// A variation of a <see cref="ObservableCollection{T}"/>
    /// that adds ability to manipulate items in a range.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExtendedObservableCollection<T> : ObservableCollection<T>
    {
        #region Fields

        private bool supressNotification;

        #endregion

        #region Overrides of ObservableCollection

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (supressNotification)
                return;

            base.OnCollectionChanged(e);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a range of items.
        /// </summary>
        /// <param name="items">The range of items to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when collection of items is null.</exception>
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            supressNotification = true;

            foreach (var item in items)
            {
                Items.Add(item);
            }

            supressNotification = false;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Inserts a range of items at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which range of items should be inserted.</param>
        /// <param name="items">The range of items to insert.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is less than zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown when collection of items is null.</exception>
        public void InsertRange(int index, IEnumerable<T> items)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (items == null) throw new ArgumentNullException(nameof(items));

            supressNotification = true;

            foreach (var item in items.Reverse())
            {
                Items.Insert(index, item);
            }

            supressNotification = false;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Removes a range of items.
        /// </summary>
        /// <param name="items">The range of items to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when collection of items is null.</exception>
        public void RemoveRange(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            supressNotification = true;

            foreach (var item in items)
            {
                Items.Remove(item);
            }

            supressNotification = false;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion
    }
}