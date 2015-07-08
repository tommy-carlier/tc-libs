// TC Data Library
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;

namespace TC.Data
{
	/// <summary>Provides a means of reading a forward-only stream of records.</summary>
	public interface IRecordReader : IRecord, IDisposable
	{
		/// <summary>Reads the next record.</summary>
		/// <returns>If a record was read, <c>true</c>; if the end of the stream was reached, <c>false</c>.</returns>
		/// <remarks>This method must also be called to read the very first record.</remarks>
		bool Read();
	}
}
