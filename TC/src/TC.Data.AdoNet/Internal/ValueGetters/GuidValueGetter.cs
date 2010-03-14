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
	internal sealed class GuidValueGetter :
		StructValueGetter<Guid>
	{
		public override Guid GetValue(IDataRecord dataRecord, int ordinal)
		{
			return dataRecord.GetGuid(ordinal);
		}

		protected override string GetStringValue(IDataRecord dataRecord, int ordinal)
		{
			return GetValue(dataRecord, ordinal).ToDataString();
		}
	}
}
