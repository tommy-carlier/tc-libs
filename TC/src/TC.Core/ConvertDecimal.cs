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
	/// <summary>Provides functions to convert <see cref="T:Decimal"/> values to
	/// culture-independent string values and back.</summary>
	public static class ConvertDecimal
	{
		#region ToDataString

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this decimal value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this decimal? value)
		{
			return value.HasValue ? ToDataString(value.Value) : String.Empty;
		}

		#endregion

		#region TryToDecimal, ToDecimal and ToDecimalOrNull

		/// <summary>Converts the specified value to a <see cref="T:Decimal"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
		/// <returns>If the conversion succeeds, true; otherwise, false.</returns>
		public static bool TryToDecimal(this string value, out decimal result)
		{
			if (value.IsNotEmpty())
				return
					Decimal.TryParse(
						value,
						NumberStyles.Number,
						CultureInfo.InvariantCulture,
						out result);

			result = 0;
			return false;
		}

		/// <summary>Converts the specified value to a <see cref="T:Decimal"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or 0, if the conversion fails.</returns>
		public static decimal ToDecimal(this string value)
		{
			return ToDecimal(value, 0);
		}

		/// <summary>Converts the specified value to a <see cref="T:Decimal"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value to return if the conversion fails.</param>
		/// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
		public static decimal ToDecimal(this string value, decimal defaultValue)
		{
			decimal result;
			return TryToDecimal(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified value to a nullable <see cref="T:Decimal"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or null, if the conversion fails.</returns>
		public static decimal? ToDecimalOrNull(this string value)
		{
			decimal result;
			return TryToDecimal(value, out result) ? new decimal?(result) : null;
		}

		#endregion
	}
}
