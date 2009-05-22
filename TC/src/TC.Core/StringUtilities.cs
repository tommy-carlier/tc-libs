// TC Core Library
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

			StringBuilder lBuilder = new StringBuilder();
			JoinInternal(lBuilder, collection);
			return lBuilder.ToString();
		}

		/// <summary>Concatenates the specified separator between each element of the specified collection of strings, yielding a single concatenated string.</summary>
		/// <param name="collection">The collection of elements to join.</param>
		/// <param name="separator">The separator to concatenate between each element.</param>
		/// <returns>The single concatenated string.</returns>
		public static string Join(this IEnumerable<string> collection, string separator)
		{
			if (collection == null) throw new ArgumentNullException("collection");

			StringBuilder lBuilder = new StringBuilder();

			if (string.IsNullOrEmpty(separator))
				JoinInternal(lBuilder, collection);
			else JoinInternal(lBuilder, collection, separator);

			return lBuilder.ToString();
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

			StringBuilder lBuilder = new StringBuilder();

			AppendIfNotEmpty(lBuilder, prefix);
			if (string.IsNullOrEmpty(separator))
				JoinInternal(lBuilder, collection);
			else JoinInternal(lBuilder, collection, separator);
			AppendIfNotEmpty(lBuilder, suffix);

			return lBuilder.ToString();
		}

		private static void JoinInternal(StringBuilder builder, IEnumerable<string> collection)
		{
			foreach (string lItem in collection)
				AppendIfNotEmpty(builder, lItem);
		}

		private static void JoinInternal(StringBuilder builder, IEnumerable<string> collection, string separator)
		{
			using (var lEnumerator = collection.GetEnumerator())
				if (lEnumerator.MoveNext())
				{
					AppendIfNotEmpty(builder, lEnumerator.Current);
					while (lEnumerator.MoveNext())
					{
						builder.Append(separator);
						AppendIfNotEmpty(builder, lEnumerator.Current);
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
			// lOffset = the start index of each found substring
			int lOffset = 0;

			// lNextIndex = the index of the next separator
			int lNextIndex = value.IndexOf(separator);
			while (lNextIndex >= 0)
			{
				// lLength = the length of each found substring
				int lLength = lNextIndex - lOffset;

				// if lLength is positive, yield the found substring, else yield an empty string
				yield return lLength > 0 ? value.Substring(lOffset, lLength) : String.Empty;

				// check if there is still text after the separator
				lOffset = lNextIndex + 1;
				if (lOffset >= value.Length)
				{
					yield return String.Empty;
					break;
				}

				// find the next separator (start searching just after the previous one)
				lNextIndex = value.IndexOf(separator, lOffset);
			}

			// check if there is still text after the last separator
			if (lOffset < value.Length) yield return value.Substring(lOffset);
		}

		private static IEnumerable<string> LazySplitInternal(string value, char separator)
		{
			// lOffset = the start index of each found substring
			int lOffset = 0;

			// lNextIndex = the index of the next separator
			int lNextIndex = value.IndexOf(separator);
			while (lNextIndex >= 0)
			{
				// lLength = the length of each found substring
				int lLength = lNextIndex - lOffset;

				// if lLength is positive, yield the found substring, else yield an empty string
				yield return lLength > 0 ? value.Substring(lOffset, lLength) : String.Empty;

				// check if there is still text after the separator
				lOffset = lNextIndex + 1;
				if (lOffset >= value.Length)
				{
					yield return String.Empty;
					break;
				}

				// find the next separator (start searching just after the previous one)
				lNextIndex = value.IndexOf(separator, lOffset);
			}

			// check if there is still text after the last separator
			if (lOffset < value.Length) yield return value.Substring(lOffset);
		}

		private static IEnumerable<string> LazySplitInternal(string value, char[] separators)
		{
			// lOffset = the start index of each found substring
			int lOffset = 0;

			// lNextIndex = the index of the next separator
			int lNextIndex = value.IndexOfAny(separators);
			while (lNextIndex >= 0)
			{
				// lLength = the length of each found substring
				int lLength = lNextIndex - lOffset;

				// if lLength is positive, yield the found substring, else yield an empty string
				yield return lLength > 0 ? value.Substring(lOffset, lLength) : String.Empty;

				// check if there is still text after the separator
				lOffset = lNextIndex + 1;
				if (lOffset >= value.Length)
				{
					yield return String.Empty;
					break;
				}

				// find the next separator (start searching just after the previous one)
				lNextIndex = value.IndexOfAny(separators, lOffset);
			}

			// check if there is still text after the last separator
			if (lOffset < value.Length) yield return value.Substring(lOffset);
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

			foreach (string lItem in collection)
				if (!string.IsNullOrEmpty(lItem))
					yield return lItem;
		}

		/// <summary>Returns all the items of the specified collection, trimming the leading and trailing white-space from each element.</summary>
		/// <param name="collection">The collection of strings to enumerate.</param>
		/// <returns>The collection of all the trimmed elements.</returns>
		public static IEnumerable<string> Trim(this IEnumerable<string> collection)
		{
			if (collection == null) throw new ArgumentNullException("collection");

			foreach (string lItem in collection)
				yield return string.IsNullOrEmpty(lItem) ? String.Empty : lItem.Trim();
		}

		/// <summary>Determines whether the first character of the specified string is the specified character.</summary>
		/// <param name="s">The string to check.</param>
		/// <param name="firstChar">The character to check with.</param>
		/// <returns>If the specified string starts with the specified character, true; otherwise, false.</returns>
		public static bool StartsWith(this string s, char firstChar)
		{
			if (s == null) throw new ArgumentNullException("s");
			return s.Length > 0 && s[0] == firstChar;
		}

		/// <summary>Determines whether the last character of the specified string is the specified character.</summary>
		/// <param name="s">The string to check.</param>
		/// <param name="lastChar">The character to check with.</param>
		/// <returns>If the specified string ends with the specified character, true; otherwise, false.</returns>
		public static bool EndsWith(this string s, char lastChar)
		{
			if (s == null) throw new ArgumentNullException("s");
			return s.Length > 0 && s[s.Length - 1] == lastChar;
		}
	}
}
