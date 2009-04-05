// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TC.WinForms
{
	/// <summary>Represents a control that uses a <see cref="T:SystemFont"/>.</summary>
	public interface IHasSystemFont
	{
		/// <summary>Gets the <see cref="T:SystemFont"/> of the control.</summary>
		/// <value>The <see cref="T:SystemFont"/> of the control.</value>
		SystemFont SystemFont { get; }

		/// <summary>Gets or sets the <see cref="T:Font"/> of the control.</summary>
		/// <value>The <see cref="T:Font"/> of the control.</value>
		Font Font { get; set; }
	}
}
