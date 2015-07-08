﻿// TC Data ADO.NET Bridge
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TC.Data.Internal.ValueGetters
{
	internal sealed class SingleValueGetter :
		StructValueGetter<float>
	{
		public override float GetValue(IDataRecord dataRecord, int ordinal)
		{
			return dataRecord.GetFloat(ordinal);
		}

		protected override string GetStringValue(IDataRecord dataRecord, int ordinal)
		{
			return GetValue(dataRecord, ordinal).ToDataString();
		}
	}
}
