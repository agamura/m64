#region Header
//+ <source name="DictionaryChangedEventArgs.cs" language="C#" begin="25-Nov-2020">
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
    /// Provides data for the <see cref="ObservableDictionary&lt;TKey, TValue&gt;.DictionaryChanged"/> event.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</typeparam>
    /// <typeparam name="TValue">The type of values in <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</typeparam>
    public class DictionaryChangedEventArgs<TKey, TValue> : EventArgs
    {
        #region Fields
        private ListChangedType listChangedType;
        private TKey key;
        private TValue value;
        private TValue oldValue;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>
        /// with the specified <see cref="ListChangedType"/>.
        /// </summary>
        /// <param name="listChangedType">One of the <see cref="ListChangedType"/> values.</param>
        public DictionaryChangedEventArgs(ListChangedType listChangedType)
        {
            this.listChangedType = listChangedType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>
        /// with the specified <see cref="ListChangedType"/>, key, and value.
        /// </summary>
        /// <param name="listChangedType">One of the <see cref="ListChangedType"/> values.</param>
        /// <param name="key">The key of the affected element.</param>
        /// <param name="value">The value of the affected element.</param>
        public DictionaryChangedEventArgs(ListChangedType listChangedType, TKey key, TValue value)
        {
            this.listChangedType = listChangedType;
            this.key = key;
            oldValue = this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>
        /// with the specified <see cref="ListChangedType"/>, key, new value, and old value.
        /// </summary>
        /// <param name="listChangedType">One of the <see cref="ListChangedType"/> values.</param>
        /// <param name="key">The key of the affected element.</param>
        /// <param name="newValue">The new value of the affected element.</param>
        /// <param name="oldValue">The old value of the affected element.</param>
        public DictionaryChangedEventArgs(ListChangedType listChangedType, TKey key, TValue newValue, TValue oldValue)
        {
            this.listChangedType = listChangedType;
            this.key = key;
            this.value = newValue;
            this.oldValue = oldValue;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating how the <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> has changed.
        /// </summary>
        /// <value>One of the <see cref="ListChangedType"/> values.</value>
        public ListChangedType ListChangedType
        {
            get { return listChangedType; }
        }

        /// <summary>
        /// Gets the key of the element affected by the change. 
        /// </summary>
        /// <value>The key of the element affected by the change.</value>
        public TKey Key
        {
            get { return key; }
        }

        /// <summary>
        /// Gets the value of the element affected by the change.
        /// </summary>
        /// <value>The value of the element affected by the change.</value>
        public TValue Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets the new value of the element affected by the change.
        /// </summary>
        /// <value>The new value of the element affected by the change.</value>
        public TValue NewValue
        {
            get { return value; }
        }

        /// <summary>
        /// Gets the old value of the element affected by the change.
        /// </summary>
        /// <value>The old value of the element affected by the change.</value>
        public TValue OldValue
        {
            get { return oldValue; }
        }
        #endregion
    }
}
