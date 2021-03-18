// TC Data ADO.NET Bridge
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System.Data;

namespace TC.Data.Internal.ValueGetters
{
	internal class ObjectValueGetter :
		IValueGetter<object>,
		IValueGetter<string>
	{
		#region IValueGetter<object> Members

		object IValueGetter<object>.GetValue(IDataRecord dataRecord, int ordinal)
		{
			return dataRecord.IsDBNull(ordinal)
				? null
				: dataRecord.GetValue(ordinal);
		}

		#endregion

		#region IValueGetter<string> Members

		string IValueGetter<string>.GetValue(IDataRecord dataRecord, int ordinal)
		{
			return dataRecord.IsDBNull(ordinal)
				? null
				: GetStringValue(dataRecord, ordinal);
		}

		#endregion

		protected virtual string GetStringValue(IDataRecord dataRecord, int ordinal)
		{
			return dataRecord.GetValue(ordinal)?.ToString();
		}
	}
}
