// TC Data Library
// Copyright © 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TC.Data.Internal;

namespace TC.Data
{
	/// <summary>Provides some utility methods for the base record types.</summary>
	public static class RecordUtilities
	{
		#region IRecordDescriptor extensions

		/// <summary>Gets the type of the field with the specified name.</summary>
		/// <param name="descriptor">The source <see cref="T:IRecordDescriptor"/>.</param>
		/// <param name="name">The name of the field.</param>
		/// <returns>The type of the field with the specified name.</returns>
		public static Type GetFieldType(this IRecordDescriptor descriptor, string name)
		{
			if (descriptor == null) throw new ArgumentNullException("descriptor");
			return descriptor.GetFieldType(descriptor.GetFieldOrdinal(name));
		}

		#region GetFieldNames

		/// <summary>Gets the names of the fields of the specified <see cref="T:IRecordDescriptor"/>.</summary>
		/// <param name="descriptor">The <see cref="T:IRecordDescriptor"/> to get the field names of.</param>
		/// <returns>A collection with all the names of the fields.</returns>
		public static IEnumerable<string> GetFieldNames(this IRecordDescriptor descriptor)
		{
			if (descriptor == null) throw new ArgumentNullException("descriptor");
			return GetFieldNamesCore(descriptor);
		}

		private static IEnumerable<string> GetFieldNamesCore(IRecordDescriptor descriptor)
		{
			int fieldCount = descriptor.FieldCount;
			for (int i = 0; i < fieldCount; i++)
				yield return descriptor.GetFieldName(i);
		}

		/// <summary>Copies the names of the fields of the specified <see cref="T:IRecordDescriptor"/> to the specified array.</summary>
		/// <param name="descriptor">The <see cref="T:IRecordDescriptor"/> to get the field names of.</param>
		/// <param name="array">The array to copy the field names to.</param>
		public static void GetFieldNames(this IRecordDescriptor descriptor, string[] array)
		{
			if (descriptor == null) throw new ArgumentNullException("descriptor");
			if (array == null) throw new ArgumentNullException("array");

			for (int i = Math.Min(descriptor.FieldCount, array.Length) - 1; i >= 0; i--)
				array[i] = descriptor.GetFieldName(i);
		}

		#endregion

		#region GetFieldTypes

		/// <summary>Gets the types of the fields of the specified <see cref="T:IRecordDescriptor"/>.</summary>
		/// <param name="descriptor">The <see cref="T:IRecordDescriptor"/> to get the field types of.</param>
		/// <returns>A collection with all the types of the fields.</returns>
		public static IEnumerable<Type> GetFieldTypes(this IRecordDescriptor descriptor)
		{
			if (descriptor == null) throw new ArgumentNullException("descriptor");
			return GetFieldTypesCore(descriptor);
		}

		private static IEnumerable<Type> GetFieldTypesCore(IRecordDescriptor descriptor)
		{
			int fieldCount = descriptor.FieldCount;
			for (int i = 0; i < fieldCount; i++)
				yield return descriptor.GetFieldType(i);
		}

		/// <summary>Copies the types of the fields of the specified <see cref="T:IRecordDescriptor"/> to the specified array.</summary>
		/// <param name="descriptor">The <see cref="T:IRecordDescriptor"/> to get the field types of.</param>
		/// <param name="array">The array to copy the field types to.</param>
		public static void GetFieldTypes(this IRecordDescriptor descriptor, Type[] array)
		{
			if (descriptor == null) throw new ArgumentNullException("descriptor");
			if (array == null) throw new ArgumentNullException("array");

			for (int i = Math.Min(descriptor.FieldCount, array.Length) - 1; i >= 0; i--)
				array[i] = descriptor.GetFieldType(i);
		}

		#endregion

		/// <summary>Creates a copy of the specified <see cref="T:IRecordDescriptor"/>.</summary>
		/// <param name="descriptor">The <see cref="T:IRecordDescriptor"/> to copy.</param>
		/// <returns>The copied <see cref="T:IRecordDescriptor"/>.</returns>
		public static IRecordDescriptor Copy(this IRecordDescriptor descriptor)
		{
			if (descriptor == null) throw new ArgumentNullException("descriptor");
			return RecordDescriptorBuilder.Copy(descriptor);
		}

		#endregion

		#region IRecordMetadata extensions

		/// <summary>Creates a copy of the specified <see cref="T:IRecordMetadata"/>.</summary>
		/// <param name="metadata">The <see cref="T:IRecordMetadata"/> to copy.</param>
		/// <returns>The copied <see cref="T:IRecordMetadata"/>.</returns>
		public static IRecordMetadata Copy(this IRecordMetadata metadata)
		{
			if (metadata == null) throw new ArgumentNullException("metadata");
			return RecordMetadataBuilder.Copy(metadata);
		}

		#endregion

		#region IRecord extensions

		/// <summary>Gets the value of the field with the specified name.</summary>
		/// <typeparam name="TValue">The type of the value to get.</typeparam>
		/// <param name="record">The <see cref="T:IRecord"/> to get a value of.</param>
		/// <param name="name">The name of the field.</param>
		/// <returns>The value of the field with the specified name.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type TValue is an important parameter and knowledge of generics is essential for using this function.")]
		public static TValue GetValue<TValue>(this IRecord record, string name)
		{
			if (record == null) throw new ArgumentNullException("record");
			return record.GetValue<TValue>(record.Descriptor.GetFieldOrdinal(name));
		}

		/// <summary>Determines whether the value of the field with the specified name is null.</summary>
		/// <param name="record">The <see cref="T:IRecord"/> to check a field value of.</param>
		/// <param name="name">The name of the field.</param>
		/// <returns>If the value of the specified field is null, <c>true</c>; otherwise, <c>false</c>.</returns>
		public static bool IsNull(this IRecord record, string name)
		{
			if (record == null) throw new ArgumentNullException("record");
			return record.IsNull(record.Descriptor.GetFieldOrdinal(name));
		}

		/// <summary>Copies the values of the fields to the specified array.</summary>
		/// <typeparam name="TValue">The type of the items in the array.</typeparam>
		/// <param name="record">The <see cref="T:IRecord"/> to copy the values of.</param>
		/// <param name="array">The array to copy the values to.</param>
		public static void CopyTo<TValue>(this IRecord record, TValue[] array)
		{
			if (record == null) throw new ArgumentNullException("record");
			if (array == null) throw new ArgumentNullException("array");

			for (int i = Math.Min(record.Descriptor.FieldCount, array.Length) - 1; i >= 0; i--)
				array[i] = record.GetValue<TValue>(i);
		}

		#endregion

		#region IWritableRecord extensions

		/// <summary>Sets the value of the field with the specified name.</summary>
		/// <typeparam name="TValue">The type of the value to set.</typeparam>
		/// <param name="record">The <see cref="T:IWritableRecord"/> to set a field value of.</param>
		/// <param name="name">The name of the field.</param>
		/// <param name="value">The value to assign to the specified field.</param>
		public static void SetValue<TValue>(this IWritableRecord record, string name, TValue value)
		{
			if (record == null) throw new ArgumentNullException("record");
			record.SetValue(record.Descriptor.GetFieldOrdinal(name), value);
		}

		/// <summary>Sets the value of the field with the specified name to null.</summary>
		/// <param name="record">The <see cref="T:IWritableRecord"/> to set a field value of.</param>
		/// <param name="name">The name of the field.</param>
		public static void SetNull(this IWritableRecord record, string name)
		{
			if (record == null) throw new ArgumentNullException("record");
			record.SetNull(record.Descriptor.GetFieldOrdinal(name));
		}

		/// <summary>Sets the value of all the fields to null.</summary>
		/// <param name="record">The <see cref="T:IWritableRecord"/> to clear.</param>
		public static void Clear(this IWritableRecord record)
		{
			if (record == null) throw new ArgumentNullException("record");
			for (int i = record.Descriptor.FieldCount - 1; i >= 0; i--)
				record.SetNull(i);
		}

		#endregion

		#region IRecordReader extensions

		#region ReadColumn

		/// <summary>Reads the values of the specified column.</summary>
		/// <typeparam name="TValue">The type of the values to read.</typeparam>
		/// <param name="reader">The <see cref="T:IRecordReader"/> to read values of.</param>
		/// <param name="columnOrdinal">The zero-based index of the column to read the values of.</param>
		/// <returns>A collection with all the values of the specified column.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type TValue is an important parameter and knowledge of generics is essential for using this function.")]
		public static IEnumerable<TValue> ReadColumn<TValue>(this IRecordReader reader, int columnOrdinal)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			VerifyFieldOrdinal(reader, columnOrdinal, "columnOrdinal");

			return ReadColumnCore<TValue>(reader, columnOrdinal);
		}

		/// <summary>Reads the values of the specified column.</summary>
		/// <typeparam name="TValue">The type of the values to read.</typeparam>
		/// <param name="reader">The <see cref="T:IRecordReader"/> to read values of.</param>
		/// <param name="columnName">The name of the column to read the values of.</param>
		/// <returns>A collection with all the values of the specified column.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type TValue is an important parameter and knowledge of generics is essential for using this function.")]
		public static IEnumerable<TValue> ReadColumn<TValue>(this IRecordReader reader, string columnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			return ReadColumnCore<TValue>(
				reader, 
				VerifyFieldNameAndGetOrdinal(reader, columnName, "columnName"));
		}

		/// <summary>Reads the values of the first column.</summary>
		/// <typeparam name="TValue">The type of the values to read.</typeparam>
		/// <param name="reader">The <see cref="T:IRecordReader"/> to read values of.</param>
		/// <returns>A collection with all the values of the first column.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type TValue is an important parameter and knowledge of generics is essential for using this function.")]
		public static IEnumerable<TValue> ReadColumn<TValue>(this IRecordReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			if (reader.Descriptor.FieldCount == 0)
				throw new ArgumentException("reader does not contain any columns", "reader");

			return ReadColumnCore<TValue>(reader, 0);
		}

		private static IEnumerable<TValue> ReadColumnCore<TValue>(IRecordReader reader, int columnOrdinal)
		{
			while (reader.Read())
				yield return reader.GetValue<TValue>(columnOrdinal);
		}

		#endregion

		#region ReadColumns

		/// <summary>Reads the values of the 2 specified columns.</summary>
		/// <typeparam name="TKey">The type of the values of the key column.</typeparam>
		/// <typeparam name="TValue">The type of the values of the value column.</typeparam>
		/// <param name="reader">The <see cref="T:IRecordReader"/> to read values of.</param>
		/// <param name="keyColumnOrdinal">The zero-based index of the key column.</param>
		/// <param name="valueColumnOrdinal">The zero-based index of the value column.</param>
		/// <returns>A collection of pairs with all the values of the specified columns.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The types TKey and TValue is an important parameter and knowledge of generics is essential for using this function.")]
		[SuppressMessage(
			"Microsoft.Design",
			"CA1006:DoNotNestGenericTypesInMemberSignatures",
			Justification = "There is no other general way of returning a collection of pairs.")]
		public static IEnumerable<KeyValuePair<TKey, TValue>>
			ReadColumns<TKey, TValue>(
				this IRecordReader reader,
				int keyColumnOrdinal,
				int valueColumnOrdinal)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			VerifyFieldOrdinal(reader, keyColumnOrdinal, "keyColumnOrdinal");
			VerifyFieldOrdinal(reader, valueColumnOrdinal, "valueColumnOrdinal");

			return ReadColumnsCore<TKey, TValue>(reader, keyColumnOrdinal, valueColumnOrdinal);
		}

		/// <summary>Reads the values of the 2 specified columns.</summary>
		/// <typeparam name="TKey">The type of the values of the key column.</typeparam>
		/// <typeparam name="TValue">The type of the values of the value column.</typeparam>
		/// <param name="reader">The <see cref="T:IRecordReader"/> to read values of.</param>
		/// <param name="keyColumnName">The name of the key column.</param>
		/// <param name="valueColumnName">The name of the value column.</param>
		/// <returns>A collection of pairs with all the values of the specified columns.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The types TKey and TValue is an important parameter and knowledge of generics is essential for using this function.")]
		[SuppressMessage(
			"Microsoft.Design",
			"CA1006:DoNotNestGenericTypesInMemberSignatures",
			Justification = "There is no other general way of returning a collection of pairs.")]
		public static IEnumerable<KeyValuePair<TKey, TValue>>
			ReadColumns<TKey, TValue>(
				this IRecordReader reader,
				string keyColumnName,
				string valueColumnName)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			return ReadColumnsCore<TKey, TValue>(
				reader,
				VerifyFieldNameAndGetOrdinal(reader, keyColumnName, "keyColumnName"),
				VerifyFieldNameAndGetOrdinal(reader, valueColumnName, "valueColumnName"));
		}

		/// <summary>Reads the values of the first 2 columns.</summary>
		/// <typeparam name="TKey">The type of the values of the first column.</typeparam>
		/// <typeparam name="TValue">The type of the values of the second column.</typeparam>
		/// <param name="reader">The <see cref="T:IRecordReader"/> to read values of.</param>
		/// <returns>A collection of pairs with all the values of the first 2 columns.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The types TKey and TValue is an important parameter and knowledge of generics is essential for using this function.")]
		[SuppressMessage(
			"Microsoft.Design",
			"CA1006:DoNotNestGenericTypesInMemberSignatures",
			Justification = "There is no other general way of returning a collection of pairs.")]
		public static IEnumerable<KeyValuePair<TKey, TValue>>
			ReadColumns<TKey, TValue>(
				this IRecordReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			if (reader.Descriptor.FieldCount < 2)
				throw new ArgumentException("reader contains less than 2 columns", "reader");

			return ReadColumnsCore<TKey, TValue>(reader, 0, 1);
		}

		private static IEnumerable<KeyValuePair<TKey, TValue>>
			ReadColumnsCore<TKey, TValue>(
				IRecordReader reader,
				int keyColumnOrdinal,
				int valueColumnOrdinal)
		{
			while (reader.Read())
				yield return
					new KeyValuePair<TKey, TValue>(
						reader.GetValue<TKey>(keyColumnOrdinal),
						reader.GetValue<TValue>(valueColumnOrdinal));
		}

		#endregion

		#region Where

		/// <summary>Creates a new <see cref="T:IRecordReader"/> from the specified <see cref="T:IRecordReader"/>
		/// that skips records that don't match the specified condition.</summary>
		/// <param name="reader">The <see cref="T:IRecordReader"/> to read records from.</param>
		/// <param name="condition">The condition that the records must match.</param>
		/// <returns>The created <see cref="T:IRecordReader"/>.</returns>
		public static IRecordReader Where(
			this IRecordReader reader, 
			Predicate<IRecord> condition)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (condition == null) throw new ArgumentNullException("condition");

			return new FilteredRecordReader(reader, condition);
		}

		#endregion

		#endregion

		#region private helper methods

		private static void VerifyFieldOrdinal(IRecord record, int fieldOrdinal, string argumentName)
		{
			if (fieldOrdinal < 0)
				throw new ArgumentOutOfRangeException(
					argumentName,
					argumentName + " cannot be negative");

			if (fieldOrdinal >= record.Descriptor.FieldCount)
				throw new ArgumentOutOfRangeException(
					argumentName,
					argumentName + " cannot be greater than or equal to the number of fields in the IRecord");
		}

		private static int VerifyFieldNameAndGetOrdinal(IRecord record, string fieldName, string argumentName)
		{
			int ordinal = record.Descriptor.GetFieldOrdinal(fieldName);
			if (ordinal >= 0)
				return ordinal;
			else
				throw new ArgumentException(
					"The record does not contain a column with the specified name",
					argumentName);
		}

		#endregion
	}
}