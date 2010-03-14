﻿// TC Data ADO.NET Bridge
// Copyright © 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TC.Data.Internal.ValueGetters
{
	internal sealed class BytesValueGetter : 
		ObjectValueGetter,
		IValueGetter<byte[]>
	{
		#region IValueGetter<byte[]> Members

		byte[] IValueGetter<byte[]>.GetValue(IDataRecord dataRecord, int ordinal)
		{
			return dataRecord.IsDBNull(ordinal)
				? null
				: dataRecord.GetValue(ordinal) as byte[];
		}

		#endregion

		protected override string GetStringValue(IDataRecord dataRecord, int ordinal)
		{
			byte[] value = dataRecord.GetValue(ordinal) as byte[];
			return value != null
				? Convert.ToBase64String(value, Base64FormattingOptions.None)
				: null;
		}
	}
}