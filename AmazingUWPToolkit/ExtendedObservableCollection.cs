using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace AmazingUWPToolkit
{
    public class ExtendedObservableCollection<T> : ObservableCollection<T>
    {
        #region Fields

        private bool supressNotification;

        #endregion

        #region Overrides of ObservableCollection

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!supressNotification)
            {
                base.OnCollectionChanged(e);
            }
        }

        #endregion

        #region Public Methods

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

        public void InsertRange(int index, IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            supressNotification = true;

            foreach (var item in items.Reverse())
            {
                Items.Insert(index, item);
            }

            supressNotification = false;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

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