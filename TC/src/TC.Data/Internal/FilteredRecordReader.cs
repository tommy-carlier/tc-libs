// TC Data Library
// Copyright © 2009-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

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
