﻿// TC Data ADO.NET Bridge
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System.Data;

namespace TC.Data.Internal.ValueGetters
{
	internal interface IValueGetter<TValue>
	{
		TValue GetValue(IDataRecord dataRecord, int ordinal);
	}
}
