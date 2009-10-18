// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Text;

namespace TC
{
	/// <summary>Provides functions to convert values of the basic data types
	/// from and to string in a culture-independent way.</summary>
	public static class ConvertString
	{
		private static readonly CultureInfo _invariantCulture = CultureInfo.InvariantCulture;

		#region convert from and to Boolean

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromBoolean(bool value)
		{
			return value ? "true" : "false";
		}

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromBoolean(bool? value)
		{
			return value.HasValue ? FromBoolean(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a boolean.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a boolean,
		/// or false if the value could not be converted.</returns>
		public static bool ToBoolean(string value) { return ToBoolean(value, false); }

		/// <summary>Converts the specified string to a boolean.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a boolean, or <paramref name="defaultValue"/>
		/// if the value could not be converted.</returns>
		public static bool ToBoolean(string value, bool defaultValue)
		{
			bool result;
			return TryToBoolean(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable boolean.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a boolean, or null if the value could not be converted.</returns>
		public static bool? ToBooleanOrNull(string value)
		{
			bool result;
			return TryToBoolean(value, out result) ? new bool?(result) : null;
		}

		/// <summary>Converts the specified string to a boolean.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToBoolean(string value, out bool result)
		{
			// if value is null or the trimmed value is empty: conversion fails
			if (value == null || (value = value.Trim()).Length == 0)
				return result = false;

			// if value is "true" or "1", the result is "true"
			if (BooleanEquals(value, "true", "1"))
				return result = true;

			// if value is "false" or "0", the result is "false"
			if (BooleanEquals(value, "false", "0"))
				return !(result = false);

			// any other string: conversion fails
			return result = false;
		}

		private static bool BooleanEquals(string value, string booleanValue, string numericValue)
		{
			return StringComparer.InvariantCultureIgnoreCase.Equals(value, booleanValue)
				|| StringComparer.Ordinal.Equals(value, numericValue);
		}

		#endregion

		#region convert from and to Byte

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromByte(byte value) { return value.ToString(_invariantCulture); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromByte(byte? value)
		{
			return value.HasValue ? FromByte(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a byte.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a byte, or 0 if the value could not be converted.</returns>
		public static byte ToByte(string value) { return ToByte(value, 0); }

		/// <summary>Converts the specified string to a byte.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a byte, or <paramref name="defaultValue"/> 
		/// if the value could not be converted.</returns>
		public static byte ToByte(string value, byte defaultValue)
		{
			byte result;
			return TryToByte(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable byte.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a byte, or null if the value could not be converted.</returns>
		public static byte? ToByteOrNull(string value)
		{
			byte result;
			return TryToByte(value, out result) ? new byte?(result) : null;
		}

		/// <summary>Converts the specified string to a byte.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToByte(string value, out byte result)
		{
			if (!string.IsNullOrEmpty(value))
				return byte.TryParse(value, NumberStyles.Integer, _invariantCulture, out result);
			else
			{
				result = 0;
				return false;
			}
		}

		#endregion

		#region convert from and to Int16

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromInt16(short value) { return value.ToString(_invariantCulture); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromInt16(short? value)
		{
			return value.HasValue ? FromInt16(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a short integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a short integer, or 0 if the value could not be converted.</returns>
		public static short ToInt16(string value) { return ToInt16(value, 0); }

		/// <summary>Converts the specified string to a short integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a short integer, or <paramref name="defaultValue"/>
		/// if the value could not be converted.</returns>
		public static short ToInt16(string value, short defaultValue)
		{
			short result;
			return TryToInt16(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable short integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a short integer, or null if the value could not be converted.</returns>
		public static short? ToInt16OrNull(string value)
		{
			short result;
			return TryToInt16(value, out result) ? new short?(result) : null;
		}

		/// <summary>Converts the specified string to a short integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToInt16(string value, out short result)
		{
			if (!string.IsNullOrEmpty(value))
				return short.TryParse(value, NumberStyles.Integer, _invariantCulture, out result);
			else
			{
				result = 0;
				return false;
			}
		}

		#endregion

		#region convert from and to Int32

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromInt32(int value) { return value.ToString(_invariantCulture); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromInt32(int? value)
		{
			return value.HasValue ? FromInt32(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to an integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to an integer, or 0 if the value could not be converted.</returns>
		public static int ToInt32(string value) { return ToInt32(value, 0); }

		/// <summary>Converts the specified string to an integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to an integer, or <paramref name="defaultValue"/> 
		/// if the value could not be converted.</returns>
		public static int ToInt32(string value, int defaultValue)
		{
			int result;
			return TryToInt32(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable int integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to an integer, or null if the value could not be converted.</returns>
		public static int? ToInt32OrNull(string value)
		{
			int result;
			return TryToInt32(value, out result) ? new int?(result) : null;
		}

		/// <summary>Converts the specified string to an integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToInt32(string value, out int result)
		{
			if (!string.IsNullOrEmpty(value))
				return int.TryParse(value, NumberStyles.Integer, _invariantCulture, out result);
			else
			{
				result = 0;
				return false;
			}
		}

		#endregion

		#region convert from and to Int64

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromInt64(long value) { return value.ToString(_invariantCulture); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromInt64(long? value)
		{
			return value.HasValue ? FromInt64(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a long integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a long integer, or 0 if the value could not be converted.</returns>
		public static long ToInt64(string value) { return ToInt64(value, 0); }

		/// <summary>Converts the specified string to a long integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a long integer, or <paramref name="defaultValue"/>
		/// if the value could not be converted.</returns>
		public static long ToInt64(string value, long defaultValue)
		{
			long result;
			return TryToInt64(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable long integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a long integer, or null if the value could not be converted.</returns>
		public static long? ToInt64OrNull(string value)
		{
			long result;
			return TryToInt64(value, out result) ? new long?(result) : null;
		}

		/// <summary>Converts the specified string to a long integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToInt64(string value, out long result)
		{
			if (!string.IsNullOrEmpty(value))
				return long.TryParse(value, NumberStyles.Integer, _invariantCulture, out result);
			else
			{
				result = 0;
				return false;
			}
		}

		#endregion

		#region convert from and to Single

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromSingle(float value) { return value.ToString("R", _invariantCulture); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromSingle(float? value)
		{
			return value.HasValue ? FromSingle(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a single-precision floating point number.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a single-precision floating point number, 
		/// or 0 if the value could not be converted.</returns>
		public static float ToSingle(string value) { return ToSingle(value, 0); }

		/// <summary>Converts the specified string to a single-precision floating point number.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a single-precision floating point number, 
		/// or <paramref name="defaultValue"/> if the value could not be converted.</returns>
		public static float ToSingle(string value, float defaultValue)
		{
			float result;
			return TryToSingle(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable float integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a single-precision floating point number, 
		/// or null if the value could not be converted.</returns>
		public static float? ToSingleOrNull(string value)
		{
			float result;
			return TryToSingle(value, out result) ? new float?(result) : null;
		}

		/// <summary>Converts the specified string to a single-precision floating point number.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToSingle(string value, out float result)
		{
			if (!string.IsNullOrEmpty(value))
				return float.TryParse(value, NumberStyles.Float, _invariantCulture, out result);
			else
			{
				result = 0;
				return false;
			}
		}

		#endregion

		#region convert from and to Double

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromDouble(double value) { return value.ToString("R", _invariantCulture); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromDouble(double? value)
		{
			return value.HasValue ? FromDouble(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a double-precision floating point number.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a double-precision floating point number, 
		/// or 0 if the value could not be converted.</returns>
		public static double ToDouble(string value) { return ToDouble(value, 0); }

		/// <summary>Converts the specified string to a double-precision floating point number.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a double-precision floating point number, 
		/// or <paramref name="defaultValue"/> if the value could not be converted.</returns>
		public static double ToDouble(string value, double defaultValue)
		{
			double result;
			return TryToDouble(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable double integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a double-precision floating point number,
		/// or null if the value could not be converted.</returns>
		public static double? ToDoubleOrNull(string value)
		{
			double result;
			return TryToDouble(value, out result) ? new double?(result) : null;
		}

		/// <summary>Converts the specified string to a double-precision floating point number.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToDouble(string value, out double result)
		{
			if (!string.IsNullOrEmpty(value))
				return double.TryParse(value, NumberStyles.Float, _invariantCulture, out result);
			else
			{
				result = 0;
				return false;
			}
		}

		#endregion

		#region convert from and to Decimal

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromDecimal(decimal value) { return value.ToString(_invariantCulture); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromDecimal(decimal? value)
		{
			return value.HasValue ? FromDecimal(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a decimal number.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a decimal number, or 0 if the value could not be converted.</returns>
		public static decimal ToDecimal(string value) { return ToDecimal(value, 0); }

		/// <summary>Converts the specified string to a decimal number.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a decimal number, or <paramref name="defaultValue"/> 
		/// if the value could not be converted.</returns>
		public static decimal ToDecimal(string value, decimal defaultValue)
		{
			decimal result;
			return TryToDecimal(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable decimal integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a decimal number, or null if the value could not be converted.</returns>
		public static decimal? ToDecimalOrNull(string value)
		{
			decimal result;
			return TryToDecimal(value, out result) ? new decimal?(result) : null;
		}

		/// <summary>Converts the specified string to a decimal number.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToDecimal(string value, out decimal result)
		{
			if (!string.IsNullOrEmpty(value))
				return decimal.TryParse(value, NumberStyles.Number, _invariantCulture, out result);
			else
			{
				result = 0;
				return false;
			}
		}

		#endregion

		#region convert from and to Char

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromChar(char value) { return new string(value, 1); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromChar(char? value)
		{
			return value.HasValue ? FromChar(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a character.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a character, or (char)0 if the value could not be converted.</returns>
		public static char ToChar(string value) { return ToChar(value, '\0'); }

		/// <summary>Converts the specified string to a character.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a character, or <paramref name="defaultValue"/>
		/// if the value could not be converted.</returns>
		public static char ToChar(string value, char defaultValue)
		{
			char result;
			return TryToChar(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable char integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a character, or null if the value could not be converted.</returns>
		public static char? ToCharOrNull(string value)
		{
			char result;
			return TryToChar(value, out result) ? new char?(result) : null;
		}

		/// <summary>Converts the specified string to a character.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToChar(string value, out char result)
		{
			return char.TryParse(value, out result);
		}

		#endregion

		#region convert from and to DateTime

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromDateTime(DateTime value) { return value.ToString("O", _invariantCulture); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromDateTime(DateTime? value)
		{
			return value.HasValue ? FromDateTime(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a date and time.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a date and time, or <see cref="F:DateTime.MinValue"/>
		/// if the value could not be converted.</returns>
		public static DateTime ToDateTime(string value) { return ToDateTime(value, DateTime.MinValue); }

		/// <summary>Converts the specified string to a date and time.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a date and time, or <paramref name="defaultValue"/> 
		/// if the value could not be converted.</returns>
		public static DateTime ToDateTime(string value, DateTime defaultValue)
		{
			DateTime result;
			return TryToDateTime(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable date and time.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a date and time, or null if the value could not be converted.</returns>
		public static DateTime? ToDateTimeOrNull(string value)
		{
			DateTime result;
			return TryToDateTime(value, out result) ? new DateTime?(result) : null;
		}

		/// <summary>Converts the specified string to a date and time.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToDateTime(string value, out DateTime result)
		{
			if (!string.IsNullOrEmpty(value))
				return DateTime.TryParse(value, _invariantCulture, DateTimeStyles.None, out result);
			else
			{
				result = DateTime.MinValue;
				return false;
			}
		}

		#endregion

		#region convert from and to Date

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromDate(DateTime value) { return value.ToString("d", _invariantCulture); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromDate(DateTime? value)
		{
			return value.HasValue ? FromDate(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a date.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a date, or <see cref="F:Date.MinValue"/>
		/// if the value could not be converted.</returns>
		public static DateTime ToDate(string value) { return ToDate(value, DateTime.MinValue.Date); }

		/// <summary>Converts the specified string to a date.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a date, or <paramref name="defaultValue"/>
		/// if the value could not be converted.</returns>
		public static DateTime ToDate(string value, DateTime defaultValue)
		{
			DateTime result;
			return TryToDate(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable date.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a date, or null if the value could not be converted.</returns>
		public static DateTime? ToDateOrNull(string value)
		{
			DateTime result;
			return TryToDate(value, out result) ? new DateTime?(result) : null;
		}

		/// <summary>Converts the specified string to a date.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToDate(string value, out DateTime result)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (DateTime.TryParse(value, _invariantCulture, DateTimeStyles.None, out result))
				{
					result = result.Date;
					return true;
				}
				else return false;
			}
			else
			{
				result = DateTime.MinValue;
				return false;
			}
		}

		#endregion

		#region convert from and to TimeSpan

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromTimeSpan(TimeSpan value) { return value.ToString(); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromTimeSpan(TimeSpan? value)
		{
			return value.HasValue ? FromTimeSpan(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a time span.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a time span, or <see cref="F:TimeSpan.Zero"/>
		/// if the value could not be converted.</returns>
		public static TimeSpan ToTimeSpan(string value) { return ToTimeSpan(value, TimeSpan.Zero); }

		/// <summary>Converts the specified string to a time span.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a time span, or <paramref name="defaultValue"/> 
		/// if the value could not be converted.</returns>
		public static TimeSpan ToTimeSpan(string value, TimeSpan defaultValue)
		{
			TimeSpan result;
			return TryToTimeSpan(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable TimeSpan integer.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a time span, or null if the value could not be converted.</returns>
		public static TimeSpan? ToTimeSpanOrNull(string value)
		{
			TimeSpan result;
			return TryToTimeSpan(value, out result) ? new TimeSpan?(result) : null;
		}

		/// <summary>Converts the specified string to a time span.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToTimeSpan(string value, out TimeSpan result)
		{
			if (!string.IsNullOrEmpty(value))
				return TimeSpan.TryParse(value, out result);
			else
			{
				result = TimeSpan.Zero;
				return false;
			}
		}

		#endregion

		#region convert from and to Bytes

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromBytes(byte[] value) { return FromBytes(value, Base64FormattingOptions.None); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="options">The <see cref="T:Base64FormattingOptions"/> that determine whether to insert line breaks or not.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromBytes(byte[] value, Base64FormattingOptions options)
		{
			return value != null && value.Length > 0
				? Convert.ToBase64String(value, options)
				: String.Empty;
		}

		/// <summary>Converts the specified string to a byte array.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a byte array, or null if the value could not be converted.</returns>
		public static byte[] ToBytes(string value)
		{
			byte[] result;
			return TryToBytes(value, out result) ? result : null;
		}

		/// <summary>Converts the specified string to a byte array.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToBytes(string value, out byte[] result)
		{
			if (!string.IsNullOrEmpty(value))
				try
				{
					result = Convert.FromBase64String(value.Trim());
					return true;
				}
				catch (FormatException) { }

			result = null;
			return false;
		}

		#endregion

		#region convert from and to Guid

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromGuid(Guid value) { return value.ToString("D", _invariantCulture); }

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromGuid(Guid? value)
		{
			return value.HasValue ? FromGuid(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to a GUID.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a GUID, or <see cref="F:Guid.Empty"/>
		/// if the value could not be converted.</returns>
		public static Guid ToGuid(string value) { return ToGuid(value, Guid.Empty); }

		/// <summary>Converts the specified string to a GUID.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a GUID, or <paramref name="defaultValue"/>
		/// if the value could not be converted.</returns>
		public static Guid ToGuid(string value, Guid defaultValue)
		{
			Guid result;
			return TryToGuid(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to a nullable GUID.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a GUID, or null if the value could not be converted.</returns>
		public static Guid? ToGuidOrNull(string value)
		{
			Guid result;
			return TryToGuid(value, out result) ? new Guid?(result) : null;
		}

		/// <summary>Converts the specified string to a GUID.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToGuid(string value, out Guid result)
		{
			if (!string.IsNullOrEmpty(value))
				try
				{
					result = new Guid(value.Trim());
					return true;
				}
				catch (FormatException) { }

			result = Guid.Empty;
			return false;
		}

		#endregion

		#region convert from and to CultureInfo

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromCultureInfo(CultureInfo value)
		{
			return value != null ? value.ToString() : String.Empty;
		}

		/// <summary>Converts the specified string to a culture.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a culture, or null if the value could not be converted.</returns>
		public static CultureInfo ToCultureInfo(string value)
		{
			CultureInfo result;
			return TryToCultureInfo(value, out result) ? result : null;
		}

		/// <summary>Converts the specified string to a culture.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToCultureInfo(string value, out CultureInfo result)
		{
			if (!string.IsNullOrEmpty(value))
				try
				{
					result = CultureInfo.GetCultureInfo(value.Trim());
					return true;
				}
				catch (ArgumentException) { }

			result = null;
			return false;
		}

		#endregion

		#region convert from and to Uri

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromUri(Uri value)
		{
			return value != null ? value.ToString() : String.Empty;
		}

		/// <summary>Converts the specified string to a URI.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a URI, or null if the value could not be converted.</returns>
		public static Uri ToUri(string value)
		{
			Uri result;
			return TryToUri(value, out result) ? result : null;
		}

		/// <summary>Converts the specified string to a URI.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToUri(string value, out Uri result)
		{
			if (!string.IsNullOrEmpty(value))
				return Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out result);
			else
			{
				result = null;
				return false;
			}
		}

		#endregion

		#region convert from and to IPAddress

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromIPAddress(IPAddress value)
		{
			return value != null ? value.ToString() : String.Empty;
		}

		/// <summary>Converts the specified string to a IP-address.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a IP-address, or null if the value could not be converted.</returns>
		public static IPAddress ToIPAddress(string value)
		{
			IPAddress result;
			return TryToIPAddress(value, out result) ? result : null;
		}

		/// <summary>Converts the specified string to a IP-address.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToIPAddress(string value, out IPAddress result)
		{
			if (!string.IsNullOrEmpty(value))
				return IPAddress.TryParse(value, out result);
			else
			{
				result = null;
				return false;
			}
		}

		#endregion

		#region convert from and to Enum

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromEnum<TEnum>(TEnum value) where TEnum : struct
		{
			return value.ToString();
		}

		/// <summary>Converts the specified value to a string.</summary>
		/// <typeparam name="TEnum">The type of the Enum.</typeparam>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromEnum<TEnum>(TEnum? value) where TEnum : struct
		{
			return value.HasValue ? FromEnum(value.Value) : String.Empty;
		}

		/// <summary>Converts the specified string to an enum of type <typeparamref name="TEnum"/>.</summary>
		/// <typeparam name="TEnum">The type of the Enum.</typeparam>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a <typeparamref name="TEnum"/>,
		/// or the default value of <typeparamref name="TEnum"/> if the value could not be converted.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type TEnum is an important parameter and knowledge of generics is essential for using this function.")]
		public static TEnum ToEnum<TEnum>(string value) where TEnum : struct
		{
			return ToEnum(value, default(TEnum));
		}

		/// <summary>Converts the specified string to an enum of type <typeparamref name="TEnum"/>.</summary>
		/// <typeparam name="TEnum">The type of the Enum.</typeparam>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value that is returned if the value could not be converted.</param>
		/// <returns>The specified value, converted to a <typeparamref name="TEnum"/>,
		/// or <paramref name="defaultValue"/> if the value could not be converted.</returns>
		public static TEnum ToEnum<TEnum>(string value, TEnum defaultValue) where TEnum : struct
		{
			TEnum result;
			return TryToEnum(value, out result) ? result : defaultValue;
		}

		/// <summary>Converts the specified string to an enum of type <typeparamref name="TEnum"/>.</summary>
		/// <typeparam name="TEnum">The type of the Enum.</typeparam>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a <typeparamref name="TEnum"/>,
		/// or null if the value could not be converted.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type TEnum is an important parameter and knowledge of generics is essential for using this function.")]
		public static TEnum? ToEnumOrNull<TEnum>(string value) where TEnum : struct
		{
			TEnum result;
			return TryToEnum(value, out result) ? new TEnum?(result) : null;
		}

		/// <summary>Converts the specified string to an enum of type <typeparamref name="TEnum"/>.</summary>
		/// <typeparam name="TEnum">The type of the Enum.</typeparam>
		/// <param name="value">The value to convert.</param>
		/// <param name="result">A reference to the variable that receives the converted result.</param>
		/// <returns>If the conversion succeeded, true; otherwise, false.</returns>
		public static bool TryToEnum<TEnum>(string value, out TEnum result) where TEnum : struct
		{
			if (!string.IsNullOrEmpty(value))
				try
				{
					result = (TEnum)Enum.Parse(typeof(TEnum), value);
					return true;
				}
				catch (ArgumentException) { }

			result = default(TEnum);
			return false;
		}

		#endregion

		#region convert object to string

		/// <summary>Converts the specified value to a string.</summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The specified value, converted to a string.</returns>
		public static string FromObject(object value)
		{
			if (value != null)
			{
				Converter<object, string> converter;
				if (_objectToStringConverters.TryGetValue(value.GetType(), out converter))
					return converter(value);
			}

			return String.Empty;
		}

		private static readonly Dictionary<Type, Converter<object, string>>
			_objectToStringConverters = CreateObjectToStringConverters();

		[SuppressMessage(
			"Microsoft.Maintainability",
			"CA1502:AvoidExcessiveComplexity",
			Justification = "Although the cyclomatic complexity is high, this is not a complex function and it's only called once.")]
		private static Dictionary<Type, Converter<object, string>> CreateObjectToStringConverters()
		{
			var converters = new Dictionary<Type, Converter<object, string>>();

			converters[typeof(DBNull)] = value => String.Empty;
			converters[typeof(string)] = value => value as string;
			converters[typeof(bool)] = value => FromBoolean((bool)value);
			converters[typeof(byte)] = value => FromByte((byte)value);
			converters[typeof(short)] = value => FromInt16((short)value);
			converters[typeof(int)] = value => FromInt32((int)value);
			converters[typeof(long)] = value => FromInt64((long)value);
			converters[typeof(float)] = value => FromSingle((float)value);
			converters[typeof(double)] = value => FromDouble((double)value);
			converters[typeof(decimal)] = value => FromDecimal((decimal)value);
			converters[typeof(char)] = value => FromChar((char)value);
			converters[typeof(DateTime)] = value => FromDateTime((DateTime)value);
			converters[typeof(TimeSpan)] = value => FromTimeSpan((TimeSpan)value);
			converters[typeof(byte[])] = value => FromBytes(value as byte[]);
			converters[typeof(Guid)] = value => FromGuid((Guid)value);
			converters[typeof(CultureInfo)] = value => FromCultureInfo(value as CultureInfo);
			converters[typeof(Uri)] = value => FromUri(value as Uri);
			converters[typeof(IPAddress)] = value => FromIPAddress(value as IPAddress);

			return converters;
		}

		#endregion
	}
}
