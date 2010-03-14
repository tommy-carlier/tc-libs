﻿// TC Data ADO.NET Bridge
// Copyright © 2009-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
