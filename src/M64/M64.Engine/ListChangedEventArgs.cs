#region Header
//+ <source name="ListChangedEventArgs.cs" language="C#" begin="21-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2020">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
#endregion

namespace M64.Engine
{
    /// <summary>
    /// Provides data for the <see cref="ObservableList&lt;T&gt;.ListChanged"/> event.
    /// </summary>
    /// <typeparam name="T">The type of elements in <see cref="ObservableList&lt;T&gt;"/>.</typeparam>
    public class ListChangedEventArgs<T> : EventArgs
    {
        #region Fields
        private ListChangedType listChangedType;
        private int index;
        private T item;
        private T oldItem;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ListChangedEventArgs&lt;T&gt;"/> class
        /// with the specified <see cref="ListChangedType"/>.
        /// </summary>
        /// <param name="listChangedType">One of the <see cref="ListChangedType"/> values.</param>
        public ListChangedEventArgs(ListChangedType listChangedType)
        {
            this.listChangedType = listChangedType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListChangedEventArgs&lt;T&gt;"/> class
        /// with the specified <see cref="ListChangedType"/>, index, and value.
        /// </summary>
        /// <param name="listChangedType">One of the <see cref="ListChangedType"/> values.</param>
        /// <param name="index">The index of the affected item.</param>
        /// <param name="item">The affected item.</param>
        public ListChangedEventArgs(ListChangedType listChangedType, int index, T item)
        {
            this.listChangedType = listChangedType;
            this.index = index;
            oldItem = this.item = item;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListChangedEventArgs&lt;T&gt;"/> class
        /// with the specified <see cref="ListChangedType"/>, index, new value, and old value.
        /// </summary>
        /// <param name="listChangedType">One of the <see cref="ListChangedType"/> values.</param>
        /// <param name="index">The index of the affected item.</param>
        /// <param name="newItem">The new item.</param>
        /// <param name="oldItem">The old item.</param>
        public ListChangedEventArgs(ListChangedType listChangedType, int index, T newItem, T oldItem)
        {
            this.listChangedType = listChangedType;
            this.index = index;
            item = newItem;
            this.oldItem = oldItem;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating how the <see cref="ObservableList&lt;T&gt;"/> has changed.
        /// </summary>
        /// <value>One of the <see cref="ListChangedType"/> values.</value>
        public ListChangedType ListChangedType
        {
            get { return listChangedType; }
        }

        /// <summary>
        /// Gets the index of the item affected by the change.
        /// </summary>
        /// <value>The index of the item affected by the change.</value>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        /// Gets the item affected by the change.
        /// </summary>
        /// <value>The item affected by the change.</value>
        public T Item
        {
            get { return item; }
        }

        /// <summary>
        /// Gets the new item that replaces <see cref="ListChangedEventArgs&lt;T&gt;.OldItem"/>.
        /// </summary>
        /// <value>The new item.</value>
        public T NewItem
        {
            get { return item; }
        }

        /// <summary>
        /// Gets the old item replaced by <see cref="ListChangedEventArgs&lt;T&gt;.NewItem"/>.
        /// </summary>
        /// <value>The old item.</value>
        public T OldItem
        {
            get { return oldItem; }
        }
        #endregion
    }
}
