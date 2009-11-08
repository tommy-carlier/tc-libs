// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC
{
	/// <summary>Specifies the components of a <see cref="T:DateTime"/> value to use.</summary>
	[Flags]
	public enum DateTimeComponents
	{
		/// <summary>No component.</summary>
		None = 0,

		/// <summary>The date-component.</summary>
		Date = 1,

		/// <summary>The time-component.</summary>
		Time = 2,

		/// <summary>Both the date- and time-component.</summary>
		DateAndTime = Date | Time
	}
}
