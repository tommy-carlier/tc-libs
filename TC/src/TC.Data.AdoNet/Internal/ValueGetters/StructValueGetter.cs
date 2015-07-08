// TC Data ADO.NET Bridge
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System.Data;

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