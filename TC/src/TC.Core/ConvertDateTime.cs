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
	/// <summary>Provides functions to convert <see cref="T:DateTime"/> values to
	/// culture-independent string values and back.</summary>
	public static class ConvertDateTime
	{
		#region ToDataString

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this DateTime value)
		{
			return ToDataString(value, DateTimeComponents.DateAndTime);
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="components">The <see cref="T:DateTimeComponents"/> to include in the string.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this DateTime value, DateTimeComponents components)
		{
			return value.ToString(GetFormat(components), CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this DateTime? value)
		{
			return value.HasValue ? ToDataString(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="components">The <see cref="T:DateTimeComponents"/> to include in the string.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this DateTime? value, DateTimeComponents components)
		{
			return value.HasValue ? ToDataString(value.Value, components) : String.Empty;
		}

		private const string
			DateFormat = "yyyy-MM-dd",
			TimeFormat = "HH:mm:ss.fffffffzzzz",
			DateAndTimeFormat = DateFormat + "T" + TimeFormat;

		private static string GetFormat(DateTimeComponents components)
		{
			switch (components)
			{
				case DateTimeComponents.Date: return DateFormat;
				case DateTimeComponents.Time: return TimeFormat;
				case DateTimeComponents.DateAndTime: return DateAndTimeFormat;
				default: return String.Empty;
			}
		}

		#endregion

		#region TryToDateTime, ToDateTime and ToDateTimeOrNull

		/// <summary>Converts the specified value to a <see cref="T:DateTime"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
		/// <returns>If the conversion succeeds, true; otherwise, false.</returns>
		public static bool TryToDateTime(this string value, out DateTime result)
		{
			if (value.IsNotEmpty())
				return
					DateTime.TryParse(
						value,
						CultureInfo.InvariantCulture,
						DateTimeStyles.None,
						out result);

			result = DateTime.MinValue;
			return false;
		}

		/// <summary>Converts the specified value to a <see cref="T:DateTime"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or <see cref="F:DateTime.MinValue"/>, if the conversion fails.</returns>
		public static DateTime ToDateTime(this string value)
		{
			return ToDateTime(value, DateTime.MinValue);
		}

		/// <summary>Converts the specified value to a <see cref="T:DateTime"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value to return if the conversion fails.</param>
		/// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
		public static DateTime ToDateTime(this string value, DateTime defaultValue)
		{
			DateTime result;
			return TryToDateTime(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified value to a nullable <see cref="T:DateTime"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or null, if the conversion fails.</returns>
		public static DateTime? ToDateTimeOrNull(this string value)
		{
			DateTime result;
			return TryToDateTime(value, out result) ? new DateTime?(result) : null;
		}

		#endregion
	}
}
