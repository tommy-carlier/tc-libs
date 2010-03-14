// TC Data Library
// Copyright © 2009-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Data
{
	/// <summary>Provides a means of writing a forward-only stream of records.</summary>
	public interface IRecordWriter : IWritableRecord, IDisposable
	{
		/// <summary>Writes the current record to the underlying stream.</summary>
		void Write();
	}
}
