﻿// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TC
{
	/// <summary>Provides utilities that deal with strings.</summary>
	public static class StringUtilities
	{
		#region Join-methods

		/// <summary>Concatenates the elements of the specified collection, yielding a single concatenated string.</summary>
		/// <param name="collection">The collection of elements to join.</param>
		/// <returns>The single concatenated string.</returns>
		public static string Join(this IEnumerable<string> collection)
		{
			if (collection == null) throw new ArgumentNullException("collection");

			StringBuilder builder = new StringBuilder();
			JoinInternal(builder, collection);
			return builder.ToString();
		}

		/// <summary>Concatenates the specified separator between each element of the specified collection of strings, yielding a single concatenated string.</summary>
		/// <param name="collection">The collection of elements to join.</param>
		/// <param name="separator">The separator to concatenate between each element.</param>
		/// <returns>The single concatenated string.</returns>
		public static string Join(this IEnumerable<string> collection, string separator)
		{
			if (collection == null) throw new ArgumentNullException("collection");

			StringBuilder builder = new StringBuilder();

			if (string.IsNullOrEmpty(separator))
				JoinInternal(builder, collection);
			else JoinInternal(builder, collection, separator);

			return builder.ToString();
		}

		/// <summary>Concatenates the specified separator between each element of the specified collection of strings, with an optional prefix and suffix, yielding a single concatenated string.</summary>
		/// <param name="collection">The collection of elements to join.</param>
		/// <param name="prefix">The optional prefix to concatenate before the first element.</param>
		/// <param name="separator">The separator to concatenate between each element.</param>
		/// <param name="suffix">The optional suffix to concatenate after the last element.</param>
		/// <returns>The single concatenated string.</returns>
		public static string Join(this IEnumerable<string> collection, string prefix, string separator, string suffix)
		{
			if (collection == null) throw new ArgumentNullException("collection");

			StringBuilder builder = new StringBuilder();

			AppendIfNotEmpty(builder, prefix);
			if (string.IsNullOrEmpty(separator))
				JoinInternal(builder, collection);
			else JoinInternal(builder, collection, separator);
			AppendIfNotEmpty(builder, suffix);

			return builder.ToString();
		}

		private static void JoinInternal(StringBuilder builder, IEnumerable<string> collection)
		{
			foreach (string item in collection)
				AppendIfNotEmpty(builder, item);
		}

		private static void JoinInternal(StringBuilder builder, IEnumerable<string> collection, string separator)
		{
			using (var enumerator = collection.GetEnumerator())
				if (enumerator.MoveNext())
				{
					AppendIfNotEmpty(builder, enumerator.Current);
					while (enumerator.MoveNext())
					{
						builder.Append(separator);
						AppendIfNotEmpty(builder, enumerator.Current);
					}
				}
		}

		private static void AppendIfNotEmpty(StringBuilder builder, string value)
		{
			if (!string.IsNullOrEmpty(value))
				builder.Append(value);
		}

		#endregion

		#region LazySplit-methods

		/// <summary>Splits the specified value into substrings that are delimited by the specified separator.</summary>
		/// <param name="value">The value to split.</param>
		/// <param name="separator">The separator that delimits the substrings.</param>
		/// <returns>The collection of substrings.</returns>
		public static IEnumerable<string> LazySplit(this string value, string separator)
		{
			if (value == null) throw new ArgumentNullException("value");
			if (separator == null) throw new ArgumentNullException("separator");

			return value.Length > 0
				? separator.Length > 0
					? LazySplitInternal(value, separator)
					: CollectionUtilities.CreateOneItemCollection(value)
				: CollectionUtilities.CreateEmptyCollection<string>();
		}

		/// <summary>Splits the specified value into substrings that are delimited by the specified separator.</summary>
		/// <param name="value">The value to split.</param>
		/// <param name="separator">The separator that delimits the substrings.</param>
		/// <returns>The collection of substrings.</returns>
		public static IEnumerable<string> LazySplit(this string value, char separator)
		{
			if (value == null) throw new ArgumentNullException("value");

			return value.Length > 0
				? LazySplitInternal(value, separator)
				: CollectionUtilities.CreateEmptyCollection<string>();
		}

		/// <summary>Splits the specified value into substrings that are delimited by the specified separators.</summary>
		/// <param name="value">The value to split.</param>
		/// <param name="separators">The separators that delimits the substrings.</param>
		/// <returns>The collection of substrings.</returns>
		public static IEnumerable<string> LazySplit(this string value, params char[] separators)
		{
			if (value == null) throw new ArgumentNullException("value");
			if (separators == null) throw new ArgumentNullException("separators");

			return value.Length > 0
				? separators.Length > 0
					? LazySplitInternal(value, separators)
					: CollectionUtilities.CreateOneItemCollection(value)
				: CollectionUtilities.CreateEmptyCollection<string>();
		}

		private static IEnumerable<string> LazySplitInternal(string value, string separator)
		{
			// offset = the start index of each found substring
			int offset = 0;

			// nextIndex = the index of the next separator
			int nextIndex = value.IndexOf(separator);
			while (nextIndex >= 0)
			{
				// length = the length of each found substring
				int length = nextIndex - offset;

				// if length is positive, yield the found substring, else yield an empty string
				yield return length > 0 ? value.Substring(offset, length) : String.Empty;

				// check if there is still text after the separator
				offset = nextIndex + 1;
				if (offset >= value.Length)
				{
					yield return String.Empty;
					break;
				}

				// find the next separator (start searching just after the previous one)
				nextIndex = value.IndexOf(separator, offset);
			}

			// check if there is still text after the last separator
			if (offset < value.Length) yield return value.Substring(offset);
		}

		private static IEnumerable<string> LazySplitInternal(string value, char separator)
		{
			// offset = the start index of each found substring
			int offset = 0;

			// nextIndex = the index of the next separator
			int nextIndex = value.IndexOf(separator);
			while (nextIndex >= 0)
			{
				// length = the length of each found substring
				int length = nextIndex - offset;

				// if length is positive, yield the found substring, else yield an empty string
				yield return length > 0 ? value.Substring(offset, length) : String.Empty;

				// check if there is still text after the separator
				offset = nextIndex + 1;
				if (offset >= value.Length)
				{
					yield return String.Empty;
					break;
				}

				// find the next separator (start searching just after the previous one)
				nextIndex = value.IndexOf(separator, offset);
			}

			// check if there is still text after the last separator
			if (offset < value.Length) yield return value.Substring(offset);
		}

		private static IEnumerable<string> LazySplitInternal(string value, char[] separators)
		{
			// offset = the start index of each found substring
			int offset = 0;

			// nextIndex = the index of the next separator
			int nextIndex = value.IndexOfAny(separators);
			while (nextIndex >= 0)
			{
				// length = the length of each found substring
				int length = nextIndex - offset;

				// if length is positive, yield the found substring, else yield an empty string
				yield return length > 0 ? value.Substring(offset, length) : String.Empty;

				// check if there is still text after the separator
				offset = nextIndex + 1;
				if (offset >= value.Length)
				{
					yield return String.Empty;
					break;
				}

				// find the next separator (start searching just after the previous one)
				nextIndex = value.IndexOfAny(separators, offset);
			}

			// check if there is still text after the last separator
			if (offset < value.Length) yield return value.Substring(offset);
		}

		#endregion

		/// <summary>Replaces the format item in <paramref name="format"/> with the text equivalent of the value of a corresponding object in <paramref name="args"/>.</summary>
		/// <param name="format">The composite format string.</param>
		/// <param name="args">The objects to format.</param>
		/// <returns>A copy of <paramref name="format"/> with the format item replaced by the string equivalent of a corresponding object in <paramref name="args"/>.</returns>
		public static string FormatInvariant(this string format, params object[] args)
		{
			if (format == null) throw new ArgumentNullException("format");
			return string.Format(CultureInfo.InvariantCulture, format, args);
		}

		/// <summary>Returns all the items of the specified collection, skipping nulls and empty strings.</summary>
		/// <param name="collection">The collection of strings to enumerate.</param>
		/// <returns>The collection of all the non-empty elements of the specified collection.</returns>
		public static IEnumerable<string> SkipEmpty(this IEnumerable<string> collection)
		{
			if (collection == null) throw new ArgumentNullException("collection");

			foreach (string item in collection)
				if (!string.IsNullOrEmpty(item))
					yield return item;
		}

		/// <summary>Returns all the items of the specified collection, trimming the leading and trailing white-space from each element.</summary>
		/// <param name="collection">The collection of strings to enumerate.</param>
		/// <returns>The collection of all the trimmed elements.</returns>
		public static IEnumerable<string> Trim(this IEnumerable<string> collection)
		{
			if (collection == null) throw new ArgumentNullException("collection");

			foreach (string item in collection)
				yield return string.IsNullOrEmpty(item) ? String.Empty : item.Trim();
		}

		/// <summary>Determines whether the first character of the specified string is the specified character.</summary>
		/// <param name="str">The string to check.</param>
		/// <param name="firstChar">The character to check with.</param>
		/// <returns>If the specified string starts with the specified character, true; otherwise, false.</returns>
		public static bool StartsWith(this string str, char firstChar)
		{
			if (str == null) throw new ArgumentNullException("str");
			return str.Length > 0 && str[0] == firstChar;
		}

		/// <summary>Determines whether the last character of the specified string is the specified character.</summary>
		/// <param name="str">The string to check.</param>
		/// <param name="lastChar">The character to check with.</param>
		/// <returns>If the specified string ends with the specified character, true; otherwise, false.</returns>
		public static bool EndsWith(this string str, char lastChar)
		{
			if (str == null) throw new ArgumentNullException("str");
			return str.Length > 0 && str[str.Length - 1] == lastChar;
		}
	}
}
