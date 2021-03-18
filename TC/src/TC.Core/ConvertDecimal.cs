// TC Core Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Globalization;

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
        public static string ToDataString(this decimal value) => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>Converts the specified value to a culture-independent string.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToDataString(this decimal? value) => value.HasValue ? ToDataString(value.Value) : String.Empty;

        #endregion

        #region TryToDecimal, ToDecimal and ToDecimalOrNull

        /// <summary>Converts the specified value to a <see cref="T:Decimal"/> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
        /// <returns>If the conversion succeeds, true; otherwise, false.</returns>
        public static bool TryToDecimal(this string value, out decimal result)
		{
			if (value.IsNotNullOrEmpty())
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
        public static decimal ToDecimal(this string value) => ToDecimal(value, 0);

        /// <summary>Converts the specified value to a <see cref="T:Decimal"/> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value to return if the conversion fails.</param>
        /// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
        public static decimal ToDecimal(this string value, decimal defaultValue)
            => TryToDecimal(value, out decimal result) ? result : defaultValue;

        /// <summary>Converts the specified value to a nullable <see cref="T:Decimal"/>.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value; or null, if the conversion fails.</returns>
        public static decimal? ToDecimalOrNull(this string value)
            => TryToDecimal(value, out decimal result) ? new decimal?(result) : null;

        #endregion
    }
}
