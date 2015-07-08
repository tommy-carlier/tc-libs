// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System.Drawing;

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
