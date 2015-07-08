// TC Data Library
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;

namespace TC.Data
{
	/// <summary>Provides a means of writing a forward-only stream of records.</summary>
	public interface IRecordWriter : IWritableRecord, IDisposable
	{
		/// <summary>Writes the current record to the underlying stream.</summary>
		void Write();
	}
}
