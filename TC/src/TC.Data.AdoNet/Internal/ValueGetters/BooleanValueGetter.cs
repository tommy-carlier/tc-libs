﻿// TC Data ADO.NET Bridge
// Copyright © 2009-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TC.Data.Internal.ValueGetters
{
	internal sealed class BooleanValueGetter :
		StructValueGetter<bool>
	{
		public override bool GetValue(IDataRecord dataRecord, int ordinal)
		{
			return dataRecord.GetBoolean(ordinal);
		}

		protected override string GetStringValue(IDataRecord dataRecord, int ordinal)
		{
			return GetValue(dataRecord, ordinal).ToDataString();
		}
	}
}
