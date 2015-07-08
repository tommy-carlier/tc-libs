// TC Data ADO.NET Bridge
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System.Data;

namespace TC.Data.Internal.ValueGetters
{
	internal sealed class Int32ValueGetter :
		StructValueGetter<int>
	{
		public override int GetValue(IDataRecord dataRecord, int ordinal)
		{
			return dataRecord.GetInt32(ordinal);
		}

		protected override string GetStringValue(IDataRecord dataRecord, int ordinal)
		{
			return GetValue(dataRecord, ordinal).ToDataString();
		}
	}
}
