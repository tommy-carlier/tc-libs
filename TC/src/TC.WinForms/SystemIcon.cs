// TC WinForms Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.WinForms
{
	/// <summary>Represents one of the system icons.</summary>
	public enum SystemIcon
	{
		/// <summary>No icon (empty icon).</summary>
		None = 0,

		/// <summary>A custom image.</summary>
		Custom,

		/// <summary>The icon of the current form or dialog.</summary>
		FormIcon,

		/// <summary>The information icon.</summary>
		Information,

		/// <summary>The question icon.</summary>
		Question,

		/// <summary>The warning icon.</summary>
		Warning,

		/// <summary>The error icon.</summary>
		Error
	}
}
