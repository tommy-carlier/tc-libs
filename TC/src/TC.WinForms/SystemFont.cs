// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.WinForms
{
	/// <summary>Represents one of the system fonts.</summary>
	public enum SystemFont
	{
		/// <summary>The default UI font.</summary>
		Default = 0,

		/// <summary>The default UI font, but bold.</summary>
		Bold,

		/// <summary>The default UI font, but italic.</summary>
		Italic,

		/// <summary>The font for headers.</summary>
		Header,

		/// <summary>The monospace font.</summary>
		Monospace
	}
}
