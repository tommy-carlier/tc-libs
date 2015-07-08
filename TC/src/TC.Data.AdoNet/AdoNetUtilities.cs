// TC Data ADO.NET Bridge
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

using TC.Data.Internal;

namespace TC.Data
{
	/// <summary>Provides some utility methods for ADO.NET objects.</summary>
	public static class AdoNetUtilities
	{
		/// <summary>Gets the <see cref="T:IRecordDescriptor"/> of the specified <see cref="T:IDataRecord"/>.</summary>
		/// <param name="dataRecord">The <see cref="T:IDataRecord"/> to get metadata of.</param>
		/// <returns>The <see cref="T:IRecordDescriptor"/> of the specified <see cref="T:IDataRecord"/>.</returns>
		public static IRecordDescriptor GetDescriptor(this IDataRecord dataRecord)
		{
			if (dataRecord == null) throw new ArgumentNullException("dataRecord");

			return new DataRecordDescriptor(dataRecord);
		}

		#region ToRecordReader and ToRecordReaders

		/// <summary>Converts the specified <see cref="T:IDataReader"/> to a <see cref="T:IRecordReader"/>.</summary>
		/// <param name="dataReader">The <see cref="T:IDataReader"/> to convert.</param>
		/// <returns>The converted <see cref="T:IRecordReader"/>.</returns>
		public static IRecordReader ToRecordReader(this IDataReader dataReader)
		{
			if (dataReader == null) throw new ArgumentNullException("dataReader");

			return new DataReaderRecordReader(dataReader, true);
		}

		/// <summary>Converts the specified <see cref="T:IDataReader"/> to a collection of <see cref="T:IRecordReader"/> instances.</summary>
		/// <param name="dataReader">The <see cref="T:IDataReader"/> to convert.</param>
		/// <returns>A collection of <see cref="IRecordReader"/> instances, one for each result set.</returns>
		public static IEnumerable<IRecordReader> ToRecordReaders(this IDataReader dataReader)
		{
			if (dataReader == null) throw new ArgumentNullException("dataReader");

			return ToRecordReadersCore(dataReader);
		}

		private static IEnumerable<IRecordReader> ToRecordReadersCore(IDataReader dataReader)
		{
			using (dataReader)
				do
				{
					yield return new DataReaderRecordReader(dataReader, false);
				}
				while (dataReader.NextResult());
		}

		#endregion

		/// <summary>Creates an empty <see cref="T:DataTable"/> from the specified <see cref="T:IRecordDescriptor"/>.</summary>
		/// <param name="descriptor">The <see cref="T:IRecordDescriptor"/> that define the columns in the table.</param>
		/// <returns>The created <see cref="T:DataTable"/>.</returns>
		public static DataTable CreateDataTable(this IRecordDescriptor descriptor)
		{
			if (descriptor == null) throw new ArgumentNullException("descriptor");
			
			return CreateDataTableCore(descriptor);
		}

		private static DataTable CreateDataTableCore(IRecordDescriptor descriptor)
		{
			DataTable table = new DataTable();
			table.Locale = CultureInfo.CurrentCulture;
			table.ExtendedProperties.Add(typeof(IRecordDescriptor), descriptor.Copy());

			int fieldCount = descriptor.FieldCount;

			for (int i = 0; i < fieldCount; i++)
				table.Columns.Add(
					descriptor.GetFieldName(i),
					descriptor.GetFieldType(i));

			return table;
		}

		/// <summary>Reads all the records from the specified <see cref="T:IRecordReader"/> into a <see cref="T:DataTable"/>.</summary>
		/// <param name="reader">The <see cref="T:IRecordReader"/> to read records from.</param>
		/// <returns>The created <see cref="T:DataTable"/>.</returns>
		public static DataTable ToDataTable(this IRecordReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			DataTable table = CreateDataTableCore(reader.Descriptor);
			object[] values = new object[reader.Descriptor.FieldCount];

			using (reader)
				while (reader.Read())
				{
					reader.CopyTo(values);
					table.Rows.Add(values);
				}

			return table;
		}
	}
}
