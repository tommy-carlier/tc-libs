// TC Data Library
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Data.Internal
{
	internal sealed class FilteredRecordReader : WrappedRecordReader
	{
		private readonly Predicate<IRecord> _condition;

		internal FilteredRecordReader(
			IRecordReader reader,
			Predicate<IRecord> condition)
			: base(reader)
		{
			_condition = condition;
		}

		public override bool Read()
		{
			while (base.Read())
				if (_condition(Reader))
					return true;

			return false;
		}
	}
}
