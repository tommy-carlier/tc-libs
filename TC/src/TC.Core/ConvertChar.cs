// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC
{
	/// <summary>Provides functions to convert <see cref="T:Char"/> values to
	/// culture-independent string values and back.</summary>
	public static class ConvertChar
	{
		#region ToDataString

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this char value)
		{
			return new string(value, 1);
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this char? value)
		{
			return value.HasValue ? ToDataString(value.Value) : String.Empty;
		}

		#endregion

		#region TryToChar, ToChar and ToCharOrNull

		/// <summary>Converts the specified value to a <see cref="T:Char"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
		/// <returns>If the conversion succeeds, true; otherwise, false.</returns>
		public static bool TryToChar(this string value, out char result)
		{
			if (value.IsNotEmpty())
				return Char.TryParse(value, out result);

			result = '\0';
			return false;
		}

		/// <summary>Converts the specified value to a <see cref="T:Char"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or '\0', if the conversion fails.</returns>
		public static char ToChar(this string value)
		{
			return ToChar(value, '\0');
		}

		/// <summary>Converts the specified value to a <see cref="T:Char"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value to return if the conversion fails.</param>
		/// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
		public static char ToChar(this string value, char defaultValue)
		{
			char result;
			return TryToChar(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified value to a nullable <see cref="T:Char"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or null, if the conversion fails.</returns>
		public static char? ToCharOrNull(this string value)
		{
			char result;
			return TryToChar(value, out result) ? new char?(result) : null;
		}

		#endregion
	}
}
