// TC Core Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;

namespace TC
{
	/// <summary>Provides functions to convert <see cref="T:Boolean"/> values to
	/// culture-independent string values and back.</summary>
	public static class ConvertBoolean
	{
		#region ToDataString

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this bool value)
		{
			return value ? "true" : "false";
		}

		/// <summary>Converts the specified value to a culture-independent string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted string.</returns>
		public static string ToDataString(this bool? value)
		{
			return value.HasValue ? ToDataString(value.Value) : String.Empty;
		}

		#endregion

		#region TryToBoolean, ToBoolean and ToBooleanOrNull

		/// <summary>Converts the specified value to a <see cref="T:Boolean"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted value, if the conversion succeeds.</param>
		/// <returns>If the conversion succeeds, true; otherwise, false.</returns>
		public static bool TryToBoolean(this string value, out bool result)
		{
			// if value is null or an empty string: conversion fails
			if (value.IsNullOrEmpty())
				return result = false;

			// if value is "true" or "1", the result is "true"
			if (Equals(value, "true", "1"))
				return result = true;

			// if value is "false" or "0", the result is "false"
			if (Equals(value, "false", "0"))
				return !(result = false);

			// any other string: conversion fails
			return result = false;
		}

		private static bool Equals(string value, string booleanValue, string numericValue)
		{
			return
				StringComparer.InvariantCultureIgnoreCase.Equals(value, booleanValue)
				|| StringComparer.Ordinal.Equals(value, numericValue);
		}

		/// <summary>Converts the specified value to a <see cref="T:Boolean"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or false, if the conversion fails.</returns>
		public static bool ToBoolean(this string value)
		{
			return ToBoolean(value, false);
		}

		/// <summary>Converts the specified value to a <see cref="T:Boolean"/> value.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value to return if the conversion fails.</param>
		/// <returns>The converted value; or <paramref name="defaultValue"/>, if the conversion fails.</returns>
		public static bool ToBoolean(this string value, bool defaultValue)
		{
			bool result;
			return TryToBoolean(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified value to a nullable <see cref="T:Boolean"/>.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value; or null, if the conversion fails.</returns>
		public static bool? ToBooleanOrNull(this string value)
		{
			bool result;
			return TryToBoolean(value, out result) ? new bool?(result) : null;
		}

		#endregion
	}
}
