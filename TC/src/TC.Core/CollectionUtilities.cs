﻿// TC Core Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;

using SC = System.Collections;

namespace TC
{
    /// <summary>Provides utilities that deal with collections.</summary>
    public static class CollectionUtilities
	{
        /// <summary>Creates an empty read-only collection.</summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <returns>An empty read-only collection.</returns>
        public static IList<T> CreateEmptyCollection<T>() => EmptyCollection<T>.Instance;

        /// <summary>Creates a read-only collection that contains one item.</summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="item">The item to add to the collection.</param>
        /// <returns>A read-only collection that contains the specified item.</returns>
        public static IList<T> CreateOneItemCollection<T>(T item) => new OneItemCollection<T>(item);

        #region ToGeneric

        /// <summary>Converts the specified non-generic collection to a generic collection of the specified type.</summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The non-generic collection to convert.</param>
        /// <returns>The converted collection of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> ToGeneric<T>(this SC.IEnumerable collection)
		{
			if (collection == null) throw new ArgumentNullException("collection");

			return ToGenericCore<T>(collection);
		}

		private static IEnumerable<T> ToGenericCore<T>(this SC.IEnumerable collection)
		{
			foreach (object item in collection)
				yield return (T)item;
		}

		#endregion

		#region GetValue

		/// <summary>Gets the value associated with the specified key.</summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="collection">The collection to get the value from.</param>
		/// <param name="key">The key to get the associated value from.</param>
		/// <returns>The value associated with the specified key, or the default value of type <typeparamref name="TValue"/>.</returns>
		public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			return collection.TryGetValue(key, out TValue value) ? value : default;
		}

		/// <summary>Gets the value associated with the specified key.</summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="collection">The collection to get the value from.</param>
		/// <param name="key">The key to get the associated value from.</param>
		/// <param name="defaultValue">The default value to return if there is no value associated with the specified key.</param>
		/// <returns>The value associated with the specified key, or <paramref name="defaultValue"/>.</returns>
		public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, TValue defaultValue)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			return collection.TryGetValue(key, out TValue value) ? value : defaultValue;
		}

        #endregion

        #region IsEmpty

        /// <summary>Determines whether the specified array is empty.</summary>
        /// <typeparam name="T">The type of the items in the array.</typeparam>
        /// <param name="array">The array to check.</param>
        /// <returns>If the specified array is empty, <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty<T>(this T[] array) => array == null || array.Length == 0;

        /// <summary>Determines whether the specified collection is null or empty.</summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <returns>If the specified collection is null or empty, <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty<T>(this ICollection<T> collection) => collection == null || collection.Count == 0;

        /// <summary>Determines whether the specified collection is null or empty.</summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <returns>If the specified collection is null or empty, <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
		{
			if (collection == null)
				return true;

            if (collection is ICollection<T> countCollection)
                return countCollection.Count == 0;

            using (var enumerator = collection.GetEnumerator())
				return !enumerator.MoveNext();
		}

        /// <summary>Determines whether the specified collection is null or empty.</summary>
        /// <param name="collection">The collection to check.</param>
        /// <returns>If the specified collection is null or empty, <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty(this SC.ICollection collection) => collection == null || collection.Count == 0;

        /// <summary>Determines whether the specified collection is null or empty.</summary>
        /// <param name="collection">The collection to check.</param>
        /// <returns>If the specified collection is null or empty, <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty(this SC.IEnumerable collection)
		{
			if (collection == null)
				return true;

            if (collection is SC.ICollection countCollection)
                return countCollection.Count == 0;

            var enumerator = collection.GetEnumerator();
			try
			{
				return !enumerator.MoveNext();
			}
			finally
			{
                if (enumerator is IDisposable disposable)
                    disposable.Dispose();
            }
		}

        #endregion

        #region HasItems

        /// <summary>Determines whether the specified array has items.</summary>
        /// <typeparam name="T">The type of the items in the array.</typeparam>
        /// <param name="array">The array to check.</param>
        /// <returns>If the specified array has items, <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool HasItems<T>(this T[] array) => array != null && array.Length > 0;

        /// <summary>Determines whether the specified collection has items.</summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <returns>If the specified collection is not null and has items, <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool HasItems<T>(this ICollection<T> collection) => collection != null && collection.Count > 0;

        /// <summary>Determines whether the specified collection has items.</summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <returns>If the specified collection is not null and has items, <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool HasItems<T>(this IEnumerable<T> collection)
		{
			if (collection == null)
				return false;

            if (collection is ICollection<T> countCollection)
                return countCollection.Count > 0;

            using (var enumerator = collection.GetEnumerator())
				return enumerator.MoveNext();
		}

        /// <summary>Determines whether the specified collection has items.</summary>
        /// <param name="collection">The collection to check.</param>
        /// <returns>If the specified collection is not null and has items, <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool HasItems(this SC.ICollection collection) => collection != null && collection.Count > 0;

        /// <summary>Determines whether the specified collection has items.</summary>
        /// <param name="collection">The collection to check.</param>
        /// <returns>If the specified collection is not null and has items, <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool HasItems(this SC.IEnumerable collection)
		{
			if (collection == null)
				return false;

            if (collection is SC.ICollection countCollection)
                return countCollection.Count > 0;

            var enumerator = collection.GetEnumerator();
			try
			{
				return enumerator.MoveNext();
			}
			finally
			{
                if (enumerator is IDisposable disposable)
                    disposable.Dispose();
            }
		}

		#endregion

		#region inner class EmptyCollection

		private sealed class EmptyCollection<T> : IList<T>, IEnumerator<T>
		{
			private EmptyCollection() { }

			public static readonly EmptyCollection<T> Instance = new EmptyCollection<T>();

            #region IList<T> Members

            int IList<T>.IndexOf(T item) => -1;

            void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

            void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

            T IList<T>.this[int index]
            {
                get => throw new ArgumentOutOfRangeException("index");
                set => throw new NotSupportedException();
            }

            #endregion

            #region ICollection<T> Members

            void ICollection<T>.Add(T item) => throw new NotSupportedException();

            void ICollection<T>.Clear() => throw new NotSupportedException();

            bool ICollection<T>.Contains(T item) => false;

            void ICollection<T>.CopyTo(T[] array, int arrayIndex) { }

            int ICollection<T>.Count => 0;

            bool ICollection<T>.IsReadOnly => true;

            bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

            #endregion

            #region IEnumerable<T> Members

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;

            #endregion

            #region IEnumerable Members

            SC.IEnumerator SC.IEnumerable.GetEnumerator() => this;

            #endregion

            #region IEnumerator<T> Members

            T IEnumerator<T>.Current => default;

            #endregion

            #region IDisposable Members

            void IDisposable.Dispose() { }

            #endregion

            #region IEnumerator Members

            object SC.IEnumerator.Current => null;

            bool SC.IEnumerator.MoveNext() => false;

            void SC.IEnumerator.Reset() { }

			#endregion
		}

		#endregion

		#region inner class OneItemCollection

		private sealed class OneItemCollection<T> : IList<T>
		{
			private readonly T _item;

            public OneItemCollection(T item) => _item = item;

            #region IList<T> Members

            int IList<T>.IndexOf(T item) => Equals(_item, item) ? 0 : -1;

            void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

            void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

            T IList<T>.this[int index]
            {
                get => index == 0 ? _item : throw new ArgumentOutOfRangeException("index");
                set => throw new NotSupportedException();
            }

            #endregion

            #region ICollection<T> Members

            void ICollection<T>.Add(T item) => throw new NotSupportedException();

            void ICollection<T>.Clear() => throw new NotSupportedException();

            bool ICollection<T>.Contains(T item) => Equals(_item, item);

            void ICollection<T>.CopyTo(T[] array, int arrayIndex) => array[arrayIndex] = _item;

            int ICollection<T>.Count => 1;

            bool ICollection<T>.IsReadOnly => true;

            bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

            #endregion

            #region IEnumerable<T> Members

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(_item);

            #endregion

            #region IEnumerable Members

            SC.IEnumerator SC.IEnumerable.GetEnumerator() => new Enumerator(_item);

            #endregion

            #region inner class Enumerator

            private sealed class Enumerator : IEnumerator<T>
			{
				private readonly T _item;
				private bool _hasMovedNext;

                public Enumerator(T item) => _item = item;

                #region IEnumerator<T> Members

                T IEnumerator<T>.Current => _item;

                #endregion

                #region IDisposable Members

                void IDisposable.Dispose() { }

				#endregion

				#region IEnumerator Members

				object SC.IEnumerator.Current { get { return _item; } }

				bool SC.IEnumerator.MoveNext()
				{
					try
					{
						return !_hasMovedNext;
					}
					finally
					{
						_hasMovedNext = true;
					}
				}

				void SC.IEnumerator.Reset() { _hasMovedNext = false; }

				#endregion
			}

			#endregion
		}

		#endregion
	}
}
