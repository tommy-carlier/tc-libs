// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC
{
	/// <summary>Provides functions to convert <see cref="T:Byte"/> arrays to
	/// Base64 string values and back.</summary>
	public static class ConvertBytes
	{
		#region ToDataString

		/// <summary>Converts the specified array to a Base64 string.</summary>
		/// <param name="value">The array to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this byte[] value)
		{
			return ToDataString(value, Base64FormattingOptions.None);
		}

		/// <summary>Converts the specified array to a Base64 string.</summary>
		/// <param name="value">The array to convert.</param>
		/// <param name="options">The <see cref="T:Base64FormattingOptions"/> to use.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this byte[] value, Base64FormattingOptions options)
		{
			return value.HasItems()
				? Convert.ToBase64String(value, options)
				: String.Empty;
		}

		#endregion

		#region TryToBytes and ToBytes

		/// <summary>Converts the specified value to a <see cref="T:Byte"/> array.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
		/// <returns>If the conversion succeeds, true; otherwise, false.</returns>
		public static bool TryToBytes(this string value, out byte[] result)
		{
			if (value.IsNotNullOrEmpty())
				try
				{
					result = Convert.FromBase64String(value.Trim());
					return true;
				}
				catch (FormatException)
				{
					// the conversion from Base64 failed
				}

			result = null;
			return false;
		}

		/// <summary>Converts the specified value to a <see cref="T:Byte"/> array.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or null, if the conversion fails.</returns>
		public static byte[] ToBytes(this string value)
		{
			byte[] result;
			return TryToBytes(value, out result) ? result : null;
		}

		#endregion
	}
}
