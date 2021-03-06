﻿// TC Core Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Globalization;

namespace TC
{
	/// <summary>Provides functions to convert <see cref="T:Double"/> values to
	/// culture-independent string values and back.</summary>
	public static class ConvertDouble
	{
		#region ToDataString

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this double value)
		{
			return value.ToString("R", CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this double? value)
		{
			return value.HasValue ? ToDataString(value.Value) : String.Empty;
		}

		#endregion

		#region TryToDouble, ToDouble and ToDoubleOrNull

		/// <summary>Converts the specified value to a <see cref="T:Double"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
		/// <returns>If the conversion succeeds, true; otherwise, false.</returns>
		public static bool TryToDouble(this string value, out double result)
		{
			if (value.IsNotNullOrEmpty())
				return
					Double.TryParse(
						value,
						NumberStyles.Float,
						CultureInfo.InvariantCulture,
						out result);

			result = 0;
			return false;
		}

		/// <summary>Converts the specified value to a <see cref="T:Double"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or 0, if the conversion fails.</returns>
		public static double ToDouble(this string value)
		{
			return ToDouble(value, 0);
		}

		/// <summary>Converts the specified value to a <see cref="T:Double"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value to return if the conversion fails.</param>
		/// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
		public static double ToDouble(this string value, double defaultValue)
		{
            return TryToDouble(value, out double result) ? result : defaultValue;
        }

		/// <summary>Converts the specified value to a nullable <see cref="T:Double"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or null, if the conversion fails.</returns>
		public static double? ToDoubleOrNull(this string value)
		{
            return TryToDouble(value, out double result) ? new double?(result) : null;
        }

		#endregion
	}
}
