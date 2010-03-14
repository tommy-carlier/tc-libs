// TC Core Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TC
{
	/// <summary>Provides functions to convert <see cref="T:Int16"/> values to
	/// culture-independent string values and back.</summary>
	public static class ConvertInt16
	{
		#region ToDataString

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this short value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this short? value)
		{
			return value.HasValue ? ToDataString(value.Value) : String.Empty;
		}

		#endregion

		#region TryToInt16, ToInt16 and ToInt16OrNull

		/// <summary>Converts the specified value to a <see cref="T:Int16"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
		/// <returns>If the conversion succeeds, true; otherwise, false.</returns>
		public static bool TryToInt16(this string value, out short result)
		{
			if (value.IsNotNullOrEmpty())
				return
					Int16.TryParse(
						value,
						NumberStyles.Integer,
						CultureInfo.InvariantCulture,
						out result);

			result = 0;
			return false;
		}

		/// <summary>Converts the specified value to a <see cref="T:Int16"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or 0, if the conversion fails.</returns>
		public static short ToInt16(this string value)
		{
			return ToInt16(value, 0);
		}

		/// <summary>Converts the specified value to a <see cref="T:Int16"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value to return if the conversion fails.</param>
		/// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
		public static short ToInt16(this string value, short defaultValue)
		{
			short result;
			return TryToInt16(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified value to a nullable <see cref="T:Int16"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or null, if the conversion fails.</returns>
		public static short? ToInt16OrNull(this string value)
		{
			short result;
			return TryToInt16(value, out result) ? new short?(result) : null;
		}

		#endregion
	}
}
