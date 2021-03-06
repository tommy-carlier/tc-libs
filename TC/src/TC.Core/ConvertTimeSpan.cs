﻿// TC Core Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;

namespace TC
{
	/// <summary>Provides functions to convert <see cref="T:TimeSpan"/> values to
	/// culture-independent string values and back.</summary>
	public static class ConvertTimeSpan
	{
		#region ToDataString

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this TimeSpan value)
		{
			return value.ToString();
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this TimeSpan? value)
		{
			return value.HasValue ? ToDataString(value.Value) : String.Empty;
		}

		#endregion

		#region TryToTimeSpan, ToTimeSpan and ToTimeSpanOrNull

		/// <summary>Converts the specified value to a <see cref="T:TimeSpan"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
		/// <returns>If the conversion succeeds, true; otherwise, false.</returns>
		public static bool TryToTimeSpan(this string value, out TimeSpan result)
		{
			if (value.IsNotNullOrEmpty())
				return
					TimeSpan.TryParse(
						value,
						out result);

			result = TimeSpan.Zero;
			return false;
		}

		/// <summary>Converts the specified value to a <see cref="T:TimeSpan"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or <see cref="F:TimeSpan.Zero"/>, if the conversion fails.</returns>
		public static TimeSpan ToTimeSpan(this string value)
		{
			return ToTimeSpan(value, TimeSpan.Zero);
		}

		/// <summary>Converts the specified value to a <see cref="T:TimeSpan"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value to return if the conversion fails.</param>
		/// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
		public static TimeSpan ToTimeSpan(this string value, TimeSpan defaultValue)
		{
            return TryToTimeSpan(value, out TimeSpan result) ? result : defaultValue;
        }

		/// <summary>Converts the specified value to a nullable <see cref="T:TimeSpan"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or null, if the conversion fails.</returns>
		public static TimeSpan? ToTimeSpanOrNull(this string value)
		{
            return TryToTimeSpan(value, out TimeSpan result) ? new TimeSpan?(result) : null;
        }

		#endregion
	}
}
