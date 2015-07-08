// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a Windows button control.</summary>
	[ToolboxBitmap(typeof(Button))]
	public class TButton : Button
	{
		/// <summary>Initializes a new instance of the <see cref="TButton"/> class.</summary>
		public TButton()
		{
			// let the OS draw the button:
			// this will enable Vista to use the fading effect
			FlatStyle = FlatStyle.System;
		}

		/// <summary>Gets or sets the flat style appearance of the button control.</summary>
		/// <returns>One of the <see cref="T:FlatStyle"/> values. The default value is System.</returns>
		[DefaultValue(typeof(FlatStyle), "System")]
		public new FlatStyle FlatStyle
		{
			get { return base.FlatStyle; }
			set { base.FlatStyle = value; }
		}
	}
}
