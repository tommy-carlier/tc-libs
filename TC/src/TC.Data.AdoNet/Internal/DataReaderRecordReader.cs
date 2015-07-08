// TC Data ADO.NET Bridge
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Data;

using TC.Data.Internal.ValueGetters;

namespace TC.Data.Internal
{
	internal sealed class DataReaderRecordReader : IRecordReader
	{
		private readonly IDataReader _dataReader;
		private readonly DataRecordDescriptor _descriptor;
		private readonly object[] _valueGetters;
		private readonly bool _disposeDataReader;

		internal DataReaderRecordReader(IDataReader dataReader, bool disposeDataReader)
		{
			_dataReader = dataReader;
			_descriptor = new DataRecordDescriptor(dataReader);
			_valueGetters = InitializeValueGetters(dataReader);
			_disposeDataReader = disposeDataReader;
		}

		~DataReaderRecordReader()
		{
			Dispose(false);
		}

		#region IRecordReader Members

		bool IRecordReader.Read()
		{
			return _dataReader.Read();
		}

		#endregion

		#region IRecord Members

		IRecordDescriptor IRecord.Descriptor
		{
			get { return _descriptor; }
		}

		IRecordMetadata IRecord.Metadata
		{
			get { return EmptyRecordMetadata.Instance; }
		}

		TValue IRecord.GetValue<TValue>(int ordinal)
		{
			return
				GetValueGetter<TValue>(ordinal)
				.GetValue(_dataReader, ordinal);
		}

		bool IRecord.IsNull(int ordinal)
		{
			return _dataReader.IsDBNull(ordinal);
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing && _disposeDataReader)
				_dataReader.Dispose();
		}

		#endregion

		#region private ValueGetter methods

		private IValueGetter<TValue> GetValueGetter<TValue>(int ordinal)
		{
			var getter = _valueGetters[ordinal] as IValueGetter<TValue>;
			if (getter != null)
				return getter;

			throw new ArgumentException(
				"The valud of the specified field cannot be converted to type " + typeof(TValue).Name);
		}

		private static object[] InitializeValueGetters(IDataRecord dataRecord)
		{
			object[] getters = new object[dataRecord.FieldCount];

			for (int i = 0; i < getters.Length; i++)
				getters[i] =
					_typeValueGetters.GetValue(
						dataRecord.GetFieldType(i),
						_defaultTypeValueGetter);

			return getters;
		}

		private static readonly IDictionary<Type, object>
			_typeValueGetters = InitializeTypeValueGetters();

		private static readonly object
			_defaultTypeValueGetter = new ObjectValueGetter();

		private static IDictionary<Type, object> InitializeTypeValueGetters()
		{
			var getters = new Dictionary<Type, object>(15);

			getters.Add(typeof(string), new StringValueGetter());
			getters.Add(typeof(bool), new BooleanValueGetter());
			getters.Add(typeof(byte), new ByteValueGetter());
			getters.Add(typeof(char), new CharValueGetter());
			getters.Add(typeof(short), new Int16ValueGetter());
			getters.Add(typeof(int), new Int32ValueGetter());
			getters.Add(typeof(long), new Int64ValueGetter());
			getters.Add(typeof(float), new SingleValueGetter());
			getters.Add(typeof(double), new DoubleValueGetter());
			getters.Add(typeof(decimal), new DecimalValueGetter());
			getters.Add(typeof(DateTime), new DateTimeValueGetter());
			getters.Add(typeof(Guid), new GuidValueGetter());
			getters.Add(typeof(byte[]), new BytesValueGetter());

			return getters;
		}

		#endregion
	}
}
