// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

		/// <summary>The fixed-width or non-proportional font.</summary>
		/// <remarks>See http://en.wikipedia.org/wiki/Monospaced_font </remarks>
		[SuppressMessage(
			"Microsoft.Naming",
			"CA1704:IdentifiersShouldBeSpelledCorrectly",
			MessageId = "Monospace",
			Justification = "Monospace is a term that is used for fixed-width or non-proportional fonts.")]
		Monospace
	}
}
