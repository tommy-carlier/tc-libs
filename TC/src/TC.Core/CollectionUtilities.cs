// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using SC = System.Collections;

namespace TC
{
	/// <summary>Provides utilities that deal with collections.</summary>
	public static class CollectionUtilities
	{
		/// <summary>Creates an empty read-only collection.</summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <returns>An empty read-only collection.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Because there are no arguments, I cannot see a different possible design.")]
		public static IList<T> CreateEmptyCollection<T>()
		{
			return EmptyCollection<T>.Instance;
		}

		/// <summary>Creates a read-only collection that contains one item.</summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <param name="item">The item to add to the collection.</param>
		/// <returns>A read-only collection that contains the specified item.</returns>
		public static IList<T> CreateOneItemCollection<T>(T item)
		{
			return new OneItemCollection<T>(item);
		}

		/// <summary>Converts the items of the specified collection.</summary>
		/// <typeparam name="TInput">The type of the items in the original collection.</typeparam>
		/// <typeparam name="TOutput">The type to convert the items to.</typeparam>
		/// <param name="collection">The collection to convert the items of.</param>
		/// <param name="converter">The function that converts the items.</param>
		/// <returns>A collection of the converted items.</returns>
		public static IEnumerable<TOutput> Convert<TInput, TOutput>(
			this IEnumerable<TInput> collection, 
			Converter<TInput, TOutput> converter)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			if (converter == null) throw new ArgumentNullException("converter");

			return ConvertCore(collection, converter);
		}

		private static IEnumerable<TOutput> ConvertCore<TInput, TOutput>(
			IEnumerable<TInput> collection, 
			Converter<TInput, TOutput> converter)
		{
			foreach (TInput item in collection)
				yield return converter(item);
		}

		/// <summary>Converts the specified non-generic collection to a generic collection of the specified type.</summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <param name="collection">The non-generic collection to convert.</param>
		/// <returns>The converted collection of type <typeparamref name="T"/>.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type T is an important parameter and knowledge of generics is essential for using this function.")]
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

		#region inner class EmptyCollection

		private sealed class EmptyCollection<T> : IList<T>, IEnumerator<T>
		{
			private EmptyCollection() { }
			public static readonly EmptyCollection<T> Instance = new EmptyCollection<T>();

			#region IList<T> Members

			int IList<T>.IndexOf(T item) { return -1; }
			void IList<T>.Insert(int index, T item) { throw new NotSupportedException(); }
			void IList<T>.RemoveAt(int index) { throw new NotSupportedException(); }
			T IList<T>.this[int index]
			{
				get { throw new ArgumentOutOfRangeException("index"); }
				set { throw new NotSupportedException(); }
			}

			#endregion

			#region ICollection<T> Members

			void ICollection<T>.Add(T item) { throw new NotSupportedException(); }
			void ICollection<T>.Clear() { throw new NotSupportedException(); }
			bool ICollection<T>.Contains(T item) { return false; }
			void ICollection<T>.CopyTo(T[] array, int arrayIndex) { }
			int ICollection<T>.Count { get { return 0; } }
			bool ICollection<T>.IsReadOnly { get { return true; } }
			bool ICollection<T>.Remove(T item) { throw new NotSupportedException(); }

			#endregion

			#region IEnumerable<T> Members

			IEnumerator<T> IEnumerable<T>.GetEnumerator() { return this; }

			#endregion

			#region IEnumerable Members

			SC.IEnumerator SC.IEnumerable.GetEnumerator() { return this; }

			#endregion

			#region IEnumerator<T> Members

			T IEnumerator<T>.Current { get { return default(T); } }

			#endregion

			#region IDisposable Members

			void IDisposable.Dispose() { }

			#endregion

			#region IEnumerator Members

			object SC.IEnumerator.Current { get { return null; } }

			bool SC.IEnumerator.MoveNext() { return false; }

			void SC.IEnumerator.Reset() { }

			#endregion
		}

		#endregion

		#region inner class OneItemCollection

		private sealed class OneItemCollection<T> : IList<T>
		{
			private readonly T _item;
			public OneItemCollection(T item) { _item = item; }

			#region IList<T> Members

			int IList<T>.IndexOf(T item) { return object.Equals(_item, item) ? 0 : -1; }
			void IList<T>.Insert(int index, T item) { throw new NotSupportedException(); }
			void IList<T>.RemoveAt(int index) { throw new NotSupportedException(); }
			T IList<T>.this[int index]
			{
				get
				{
					if (index == 0) return _item;
					else throw new ArgumentOutOfRangeException("index");
				}

				set { throw new NotSupportedException(); }
			}

			#endregion

			#region ICollection<T> Members

			void ICollection<T>.Add(T item) { throw new NotSupportedException(); }
			void ICollection<T>.Clear() { throw new NotSupportedException(); }
			bool ICollection<T>.Contains(T item) { return object.Equals(_item, item); }
			void ICollection<T>.CopyTo(T[] array, int arrayIndex) { array[arrayIndex] = _item; }
			int ICollection<T>.Count { get { return 1; } }
			bool ICollection<T>.IsReadOnly { get { return true; } }
			bool ICollection<T>.Remove(T item) { throw new NotSupportedException(); }

			#endregion

			#region IEnumerable<T> Members

			IEnumerator<T> IEnumerable<T>.GetEnumerator() { return new Enumerator(_item); }

			#endregion

			#region IEnumerable Members

			SC.IEnumerator SC.IEnumerable.GetEnumerator() { return new Enumerator(_item); }

			#endregion

			#region inner class Enumerator

			private sealed class Enumerator : IEnumerator<T>
			{
				private readonly T _item;
				private bool _hasMovedNext;
				public Enumerator(T item) { _item = item; }

				#region IEnumerator<T> Members

				T IEnumerator<T>.Current { get { return _item; } }

				#endregion

				#region IDisposable Members

				void IDisposable.Dispose() { }

				#endregion

				#region IEnumerator Members

				object SC.IEnumerator.Current { get { return _item; } }
				bool SC.IEnumerator.MoveNext()
				{
					try { return !_hasMovedNext; }
					finally { _hasMovedNext = true; }
				}

				void SC.IEnumerator.Reset() { _hasMovedNext = false; }

				#endregion
			}

			#endregion
		}

		#endregion
	}
}
