﻿// TC Core Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Globalization;

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
        public static string ToDataString(this DateTime value) => value.ToString(GetFormat(value), CultureInfo.InvariantCulture);

        /// <summary>Converts the specified value to a culture-independent string.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToDataString(this DateTime? value) => value.HasValue ? ToDataString(value.Value) : String.Empty;

        private const string
			DateFormat = "yyyy-MM-dd",
			TimeFormat = "HH:mm:ss.fffffff",
			DateAndTimeFormat = DateFormat + "T" + TimeFormat,
			LocalSuffix = "zzzz",
			UtcSuffix = "Z";

		private static string GetFormat(DateTime value)
		{
			if (value.TimeOfDay == TimeSpan.Zero)
				return DateFormat;

			switch (value.Kind)
			{
				case DateTimeKind.Utc: return DateAndTimeFormat + UtcSuffix;
				case DateTimeKind.Local: return DateAndTimeFormat + LocalSuffix;
				default: return DateAndTimeFormat;
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
			if (value.IsNotNullOrEmpty())
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
        public static DateTime ToDateTime(this string value) => ToDateTime(value, DateTime.MinValue);

        /// <summary>Converts the specified value to a <see cref="T:DateTime"/> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value to return if the conversion fails.</param>
        /// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
        public static DateTime ToDateTime(this string value, DateTime defaultValue)
			=> TryToDateTime(value, out DateTime result) ? result : defaultValue;

        /// <summary>Converts the specified value to a nullable <see cref="T:DateTime"/>.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value; or null, if the conversion fails.</returns>
        public static DateTime? ToDateTimeOrNull(this string value)
			=> TryToDateTime(value, out DateTime result) ? new DateTime?(result) : null;

        #endregion
    }
}
