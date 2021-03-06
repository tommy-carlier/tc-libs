﻿// TC Core Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Globalization;

namespace TC
{
	/// <summary>Provides functions to convert <see cref="T:Byte"/> values to
	/// culture-independent string values and back.</summary>
	public static class ConvertByte
	{
        #region ToDataString

        /// <summary>Converts the specified value to a culture-independent string.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToDataString(this byte value) => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>Converts the specified value to a culture-independent string.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToDataString(this byte? value) => value.HasValue ? ToDataString(value.Value) : String.Empty;

        #endregion

        #region TryToByte, ToByte and ToByteOrNull

        /// <summary>Converts the specified value to a <see cref="T:Byte"/> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
        /// <returns>If the conversion succeeds, true; otherwise, false.</returns>
        public static bool TryToByte(this string value, out byte result)
		{
			if (value.IsNotNullOrEmpty())
				return 
					Byte.TryParse(
						value, 
						NumberStyles.Integer, 
						CultureInfo.InvariantCulture, 
						out result);

			result = 0;
			return false;
		}

        /// <summary>Converts the specified value to a <see cref="T:Byte"/> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value; or 0, if the conversion fails.</returns>
        public static byte ToByte(this string value) => ToByte(value, 0);

        /// <summary>Converts the specified value to a <see cref="T:Byte"/> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value to return if the conversion fails.</param>
        /// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
        public static byte ToByte(this string value, byte defaultValue)
            => TryToByte(value, out byte result) ? result : defaultValue;

        /// <summary>Converts the specified value to a nullable <see cref="T:Byte"/>.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value; or null, if the conversion fails.</returns>
        public static byte? ToByteOrNull(this string value)
            => TryToByte(value, out byte result) ? new byte?(result) : null;

        #endregion
    }
}
