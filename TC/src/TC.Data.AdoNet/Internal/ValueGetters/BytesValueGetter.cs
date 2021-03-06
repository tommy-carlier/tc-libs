﻿// TC Data ADO.NET Bridge
// Copyright © 2009-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Data;

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
            return dataRecord.GetValue(ordinal) is byte[] value
                ? Convert.ToBase64String(value, Base64FormattingOptions.None)
                : null;
        }
	}
}
