#region Header
//+ <source name="ObservableDictionary.cs" language="C#" begin="21-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3e</author>
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
    /// Represents an observable collection of key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Fields
        private IDictionary<TKey, TValue> dictionary;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> changes.
        /// </summary>
        public event EventHandler<DictionaryChangedEventArgs<TKey, TValue>> DictionaryChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public ObservableDictionary()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> class
        /// with the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary that is wrapped by this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</param>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the number of elements in this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        /// <value>The number of elements in this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</value>
        public int Count
        {
            get { return dictionary.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> is read-only.
        /// </summary>
        /// <value><see langword="true"/> if this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> is read-only;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public bool IsReadOnly
        {
            get { return dictionary.IsReadOnly; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public TValue this[TKey key]
        {
            get { return dictionary[key]; }
            set {
                TValue oldValue = dictionary[key];
                dictionary[key] = value;
                OnDictionaryChanged(new DictionaryChangedEventArgs<TKey, TValue>(ListChangedType.ItemChanged, key, value, oldValue));
            }
        }

        /// <summary>
        /// Gets a collection containing the keys in this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        /// <value>The keys in this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</value>
        public ICollection<TKey> Keys
        {
            get { return dictionary.Keys; }
        }

        /// <summary>
        /// Gets a collection containing the values in this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        /// <value>The values in this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</value>
        public ICollection<TValue> Values
        {
            get { return dictionary.Values; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the specified key and value to this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        /// <param name="key">The key of the element to add to this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</param>
        /// <param name="value">The value of the element to add to this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</param>
        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
            OnDictionaryChanged(new DictionaryChangedEventArgs<TKey, TValue>(ListChangedType.ItemAdded, key, value));
        }

        /// <summary>
        /// Adds the specified item to this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        /// <param name="item">The item to add to this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dictionary.Add(item);
            OnDictionaryChanged(new DictionaryChangedEventArgs<TKey, TValue>(ListChangedType.ItemAdded, item.Key, item.Value));
        }

        /// <summary>
        /// Removes all keys and values from this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
            OnDictionaryChanged(new DictionaryChangedEventArgs<TKey, TValue>(ListChangedType.Cleared));
        }

        /// <summary>
        /// Determines whether this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> contains the
        /// specified item.
        /// </summary>
        /// <param name="item">The item to locate in this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</param>
        /// <returns>
        /// <see langword="true"/> if this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>
        /// contains the specified item; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.Contains(item);
        }

        /// <summary>
        /// Determines whether this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> contains the
        /// specified key.
        /// </summary>
        /// <param name="key">The key to locate in this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</param>
        /// <returns>
        /// <see langword="true"/> if this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>
        /// contains the specified key; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Copies this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> into the specified
        /// array, starting at the specified index.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the value with the specified key from this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// <see langword="true"/> if the element with the specified key is successfully found and removed;
        /// otherwise, <see langword="false"/>. This method returns <see langword="false"/>
        /// if <paramref name="key"/> is not found in this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.
        /// </returns>
        public bool Remove(TKey key)
        {
            TValue value = dictionary[key];

            if (dictionary.Remove(key)) {
                OnDictionaryChanged(new DictionaryChangedEventArgs<TKey, TValue>(ListChangedType.ItemRemoved, key, value));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the first occurrence of the specified item from this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="item"/> is successfully found and removed;
        /// otherwise, <see langword="false"/>. This method also returns <see langword="false"/>
        /// if <paramref name="item"/> was not found in this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.
        /// </returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (dictionary.Remove(item)) {
                OnDictionaryChanged(new DictionaryChangedEventArgs<TKey, TValue>(ListChangedType.ItemRemoved, item.Key, item.Value));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to get the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">The value to get.</param>
        /// <returns><see langword="true"/> if this  <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>
        /// contains an element with the specified key; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        /// <returns>An enumerator for this <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/>.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dictionary).GetEnumerator();
        }

        /// <summary>
        /// Raises the <see cref="ObservableDictionary&lt;TKey, TValue&gt;.DictionaryChanged"/> event.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <remarks>
        /// The <see cref="ObservableDictionary&lt;TKey, TValue&gt;.OnDictionaryChanged"/>
        /// method also allows derived classes to handle the event without attaching a delegate.
        /// </remarks>
        protected virtual void OnDictionaryChanged(DictionaryChangedEventArgs<TKey, TValue> args)
        {
            if (DictionaryChanged != null) {
                DictionaryChanged(this, args);
            }
        }
        #endregion
    }
}
