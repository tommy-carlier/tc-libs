﻿// TC Data Library
// Copyright © 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;

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
