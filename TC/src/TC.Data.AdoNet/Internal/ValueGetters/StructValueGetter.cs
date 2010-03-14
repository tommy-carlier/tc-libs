// TC Data ADO.NET Bridge
// Copyright © 2009-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TC.Data.Internal.ValueGetters
{
	internal abstract class StructValueGetter<TValue> :
		ObjectValueGetter,
		IValueGetter<TValue>,
		IValueGetter<TValue?>
		where TValue : struct
	{
		#region IValueGetter<TValue> Members

		public abstract TValue GetValue(IDataRecord dataRecord, int ordinal);

		#endregion

		#region IValueGetter<TValue?> Members

		TValue? IValueGetter<TValue?>.GetValue(IDataRecord dataRecord, int ordinal)
		{
			return dataRecord.IsDBNull(ordinal)
				? null
				: new TValue?(GetValue(dataRecord, ordinal));
		}

		#endregion
	}
}