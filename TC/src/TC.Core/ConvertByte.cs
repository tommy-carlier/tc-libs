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
	/// <summary>Provides functions to convert <see cref="T:Byte"/> values to
	/// culture-independent string values and back.</summary>
	public static class ConvertByte
	{
		#region ToDataString

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this byte value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this byte? value)
		{
			return value.HasValue ? ToDataString(value.Value) : String.Empty;
		}

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
		public static byte ToByte(this string value)
		{
			return ToByte(value, 0);
		}

		/// <summary>Converts the specified value to a <see cref="T:Byte"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value to return if the conversion fails.</param>
		/// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
		public static byte ToByte(this string value, byte defaultValue)
		{
			byte result;
			return TryToByte(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified value to a nullable <see cref="T:Byte"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or null, if the conversion fails.</returns>
		public static byte? ToByteOrNull(this string value)
		{
			byte result;
			return TryToByte(value, out result) ? new byte?(result) : null;
		}

		#endregion
	}
}
