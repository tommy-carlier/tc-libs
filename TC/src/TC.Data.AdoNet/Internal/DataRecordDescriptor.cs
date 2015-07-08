// TC Data ADO.NET Bridge
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Data;

namespace TC.Data.Internal
{
	internal sealed class DataRecordDescriptor : IRecordDescriptor
	{
		private readonly IDataRecord _dataRecord;

		internal DataRecordDescriptor(IDataRecord dataRecord)
		{
			_dataRecord = dataRecord;
		}

		#region IRecordDescriptor Members

		int IRecordDescriptor.FieldCount
		{
			get { return _dataRecord.FieldCount; }
		}

		string IRecordDescriptor.GetFieldName(int ordinal)
		{
			return _dataRecord.GetName(ordinal);
		}

		Type IRecordDescriptor.GetFieldType(int ordinal)
		{
			return _dataRecord.GetFieldType(ordinal);
		}

		int IRecordDescriptor.GetFieldOrdinal(string name)
		{
			return _dataRecord.GetOrdinal(name);
		}

		#endregion
	}
}
