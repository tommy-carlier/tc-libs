// TC Core Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TC
{
	/// <summary>Provides functions to convert <see cref="T:Guid"/> values to
	/// culture-independent string values and back.</summary>
	public static class ConvertGuid
	{
		#region ToDataString

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this Guid value)
		{
			return value.ToString("N", CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this Guid? value)
		{
			return value.HasValue ? ToDataString(value.Value) : String.Empty;
		}

		#endregion

		#region TryToGuid, ToGuid and ToGuidOrNull

		private const string
			GuidRegexDigit = "[0-9A-Fa-f]",
			GuidRegexContiguousDigits = GuidRegexDigit + "{32}",
			GuidRegexDigitsWithHyphens =
				GuidRegexDigit + "{8}"
				+ "-" + GuidRegexDigit + "{4}"
				+ "-" + GuidRegexDigit + "{4}"
				+ "-" + GuidRegexDigit + "{4}"
				+ "-" + GuidRegexDigit + "{12}";

		private static readonly Regex
			_guidRegex = new Regex(
				"(?:" + GuidRegexContiguousDigits + ")"
				+ "|(?:" + GuidRegexDigitsWithHyphens + ")"
				+ @"|(?:\{" + GuidRegexDigitsWithHyphens + @"\})"
				+ @"|(?:\(" + GuidRegexDigitsWithHyphens + @"\))",
				RegexOptions.Compiled);

		/// <summary>Converts the specified value to a <see cref="T:Guid"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
		/// <returns>If the conversion succeeds, true; otherwise, false.</returns>
		public static bool TryToGuid(this string value, out Guid result)
		{
			if (value.IsNotNullOrEmpty() && _guidRegex.IsMatch(value))
			{
				try
				{
					result = new Guid(value);
					return true;
				}
				catch (FormatException)
				{
					return Fail(out result);
				}
				catch (OverflowException)
				{
					return Fail(out result);
				}
			}

			return Fail(out result);
		}

		private static bool Fail(out Guid result)
		{
			result = Guid.Empty;
			return false;
		}

		/// <summary>Converts the specified value to a <see cref="T:Guid"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or 0, if the conversion fails.</returns>
		public static Guid ToGuid(this string value)
		{
			return ToGuid(value, Guid.Empty);
		}

		/// <summary>Converts the specified value to a <see cref="T:Guid"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value to return if the conversion fails.</param>
		/// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
		public static Guid ToGuid(this string value, Guid defaultValue)
		{
			Guid result;
			return TryToGuid(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified value to a nullable <see cref="T:Guid"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or null, if the conversion fails.</returns>
		public static Guid? ToGuidOrNull(this string value)
		{
			Guid result;
			return TryToGuid(value, out result) ? new Guid?(result) : null;
		}

		#endregion
	}
}
