#region Header
//+ <source name="ObservableList.cs" language="C#" begin="21-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2020">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace M64.Engine
{
    /// <summary>
    /// Represents an observable list of itmes that can be accessed by index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class ObservableList<T> : IList<T>
    {
        #region Fields
        private IList<T> list;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the <see cref="ObservableList&lt;T&gt;"/> changes.
        /// </summary>
        public event EventHandler<ListChangedEventArgs<T>> ListChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList&lt;T&gt;"/> class.
        /// </summary>
        public ObservableList()
        {
            list = new System.Collections.Generic.List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList&lt;T&gt;"/> class
        /// with the specified list.
        /// </summary>
        /// <param name="list">The list that is wrapped by this <see cref="ObservableList&lt;T&gt;"/>.</param>
        public ObservableList(IList<T> list)
        {
            this.list = list;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList&lt;T&gt;"/> class
        /// with the specified collection.
        /// </summary>
        /// <param name="collection">The collection that is wrapped by this <see cref="ObservableList&lt;T&gt;"/>.</param>
        public ObservableList(IEnumerable<T> collection)
        {
            list = new System.Collections.Generic.List<T>(collection);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the number of elements in this <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        /// <value>The number of elements in this <see cref="ObservableList&lt;T&gt;"/>.</value>
        public int Count
        {
            get { return list.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ObservableList&lt;T&gt;"/> is read-only.
        /// </summary>
        /// <value><code>true</code> if this <see cref="ObservableList&lt;T&gt;"/> is read-only;
        /// otherwise, <code>false</code>.
        /// </value>
        public bool IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        /// <summary>
        /// Gets or sets the element at the specified index in this <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get { return list[index]; }
            set {
                T oldValue = list[index];
                list[index] = value;
                OnListChanged(new ListChangedEventArgs<T>(ListChangedType.ItemChanged, index, value, oldValue));
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the specified item to this <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The item to add at the end of this <see cref="ObservableList&lt;T&gt;"/>.</param>
        public void Add(T item)
        {
            list.Add(item);
            OnListChanged(new ListChangedEventArgs<T>(ListChangedType.ItemAdded, list.IndexOf(item), item));
        }

        /// <summary>
        /// Removes all the items from this <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        public void Clear()
        {
            list.Clear();
            OnListChanged(new ListChangedEventArgs<T>(ListChangedType.Cleared));
        }

        /// <summary>
        /// Determines whether the specified item is in this <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The item to located in this <see cref="ObservableList&lt;T&gt;"/>.</param>
        /// <returns>
        /// <code>true</code> if <paramref name="item"/> is found in this
        /// <see cref="ObservableList&lt;T&gt;"/>; otherwise, <code>false</code>.
        /// </returns>
        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Copies this <see cref="ObservableList&lt;T&gt;"/> into the specified
        /// array, starting at the specified index.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of the specified
        /// item in this <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The item to located in this <see cref="ObservableList&lt;T&gt;"/>.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of <paramref name="item"/> within this
        /// <see cref="ObservableList&lt;T&gt;"/>, if found; otherwise, -1.
        /// </returns>
        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified item into this <see cref="ObservableList&lt;T&gt;"/>
        /// at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The item to insert.</param>
        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            OnListChanged(new ListChangedEventArgs<T>(ListChangedType.ItemAdded, index, item));
        }

        /// <summary>
        /// Removes the first occurrence of the specified item from this <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>
        /// <code>true</code> if <paramref name="item"/> is successfully found and removed;
        /// otherwise, <code>false</code>. This method also returns <code>false</code>
        /// if <paramref name="item"/> was not found in this <see cref="ObservableList&lt;T&gt;"/>.
        /// </returns>
        public bool Remove(T item)
        {
            int index = list.IndexOf(item);

            if (list.Remove(item)) {
                OnListChanged(new ListChangedEventArgs<T>(ListChangedType.ItemRemoved, index, item));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the item at the specified index of this <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            T item = list[index];
            list.Remove(item);
            OnListChanged(new ListChangedEventArgs<T>(ListChangedType.ItemRemoved, index, item));
        }

        /// <summary>
        /// Returns an enumerator that iterates through this <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        /// <returns>An enumerator for this <see cref="ObservableList&lt;T&gt;"/>.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }

        /// <summary>
        /// Raises the <see cref="ObservableList&lt;T&gt;.ListChanged"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="ObservableList&lt;T&gt;.OnListChanged"/> method also allows
        /// derived classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnListChanged(ListChangedEventArgs<T> args)
        {
            if (ListChanged != null) {
                ListChanged(this, args);
            }
        }
        #endregion
    }
}
