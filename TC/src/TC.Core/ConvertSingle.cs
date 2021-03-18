// TC Core Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Globalization;

namespace TC
{
	/// <summary>Provides functions to convert <see cref="T:Single"/> values to
	/// culture-independent string values and back.</summary>
	public static class ConvertSingle
	{
		#region ToDataString

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this float value)
		{
			return value.ToString("R", CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this float? value)
		{
			return value.HasValue ? ToDataString(value.Value) : String.Empty;
		}

		#endregion

		#region TryToSingle, ToSingle and ToSingleOrNull

		/// <summary>Converts the specified value to a <see cref="T:Single"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
		/// <returns>If the conversion succeeds, true; otherwise, false.</returns>
		public static bool TryToSingle(this string value, out float result)
		{
			if (value.IsNotNullOrEmpty())
				return
					Single.TryParse(
						value,
						NumberStyles.Float,
						CultureInfo.InvariantCulture,
						out result);

			result = 0;
			return false;
		}

		/// <summary>Converts the specified value to a <see cref="T:Single"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or 0, if the conversion fails.</returns>
		public static float ToSingle(this string value)
		{
			return ToSingle(value, 0);
		}

		/// <summary>Converts the specified value to a <see cref="T:Single"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value to return if the conversion fails.</param>
		/// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
		public static float ToSingle(this string value, float defaultValue)
		{
            return TryToSingle(value, out float result) ? result : defaultValue;
        }

		/// <summary>Converts the specified value to a nullable <see cref="T:Single"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or null, if the conversion fails.</returns>
		public static float? ToSingleOrNull(this string value)
		{
            return TryToSingle(value, out float result) ? new float?(result) : null;
        }

		#endregion
	}
}
