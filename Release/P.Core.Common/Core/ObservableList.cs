using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace P.Core.Common.Core
{
   public class ObservableList<T> : IList<T>, IList, INotifyCollectionChanged
   {
      public event NotifyCollectionChangedEventHandler CollectionChanged;

      #region Properties
      private List<T> _List
      {
         get;
         set;
      }

      public T this[int index]
      {
         get
         {
            return _List[index];
         }
         set
         {
            if (index < 0 || index >= Count)
               throw new IndexOutOfRangeException("The specified index is out of range.");
            var oldItem = _List[index];
            _List[index] = value;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem, index));
         }
      }

      public int Count
      {
         get
         {
            return _List.Count;
         }
      }

      public bool IsReadOnly
      {
         get
         {
            return ((IList<T>)_List).IsReadOnly;
         }
      }

      bool IList.IsFixedSize
      {
         get
         {
            return ((IList)_List).IsFixedSize;
         }
      }

      object IList.this[int index]
      {
         get
         {
            return ((IList)_List)[index];
         }
         set
         {
            if (index < 0 || index >= Count)
               throw new IndexOutOfRangeException("The specified index is out of range.");
            var oldItem = ((IList)_List)[index];
            ((IList)_List)[index] = value;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem, index));
         }
      }

      public bool IsSynchronized
      {
         get
         {
            return ((IList)_List).IsSynchronized;
         }
      }

      public object SyncRoot
      {
         get
         {
            return ((IList)_List).SyncRoot;
         }
      }
      #endregion

      #region Methods
      private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
      {
         var handler = CollectionChanged;
         if (handler != null)
            handler(this, args);
      }

      public void AddRange(IEnumerable<T> collection)
      {
         _List.AddRange(collection);
         var iList = collection as IList;
         OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }

      public int IndexOf(T item)
      {
         return _List.IndexOf(item);
      }

      public void Insert(int index, T item)
      {
         _List.Insert(index, item);
         OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
      }

      public void RemoveAt(int index)
      {
         if (index < 0 || index >= Count)
            throw new IndexOutOfRangeException("The specified index is out of range.");
         var oldItem = _List[index];
         _List.RemoveAt(index);
         OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index));
      }

      public void Add(T item)
      {
         _List.Add(item);
         OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
      }

      public void Clear()
      {
         _List.Clear();
         OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }

      public bool Contains(T item)
      {
         return _List.Contains(item);
      }

      public void CopyTo(T[] array, int arrayIndex)
      {
         _List.CopyTo(array, arrayIndex);
      }

      public bool Remove(T item)
      {
         var result = _List.Remove(item);
         OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
         return result;
      }

      public IEnumerator<T> GetEnumerator()
      {
         return _List.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      int IList.Add(object value)
      {
         var result = ((IList)_List).Add(value);
         ;
         OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
         return result;
      }

      bool IList.Contains(object value)
      {
         return ((IList)_List).Contains(value);
      }

      int IList.IndexOf(object value)
      {
         return ((IList)_List).IndexOf(value);
      }

      void IList.Insert(int index, object value)
      {
         ((IList)_List).Insert(index, value);
         OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, index));
      }

      void IList.Remove(object value)
      {
         ((IList)_List).Remove(value);
         OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value));
      }

      public void CopyTo(Array array, int index)
      {
         ((IList)_List).CopyTo(array, index);
      }
      #endregion

      #region Initialization
      public ObservableList()
      {
         _List = new List<T>();
      }

      public ObservableList(int capacity)
      {
         _List = new List<T>(capacity);
      }

      public ObservableList(IEnumerable<T> collection)
      {
         _List = new List<T>(collection);
      }
      #endregion
   }
}
