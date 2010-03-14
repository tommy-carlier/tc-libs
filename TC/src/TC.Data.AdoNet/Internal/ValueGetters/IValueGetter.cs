// TC Data ADO.NET Bridge
// Copyright © 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TC.Data.Internal.ValueGetters
{
	internal interface IValueGetter<TValue>
	{
		TValue GetValue(IDataRecord dataRecord, int ordinal);
	}
}
