// TC WinForms Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a Windows checkbox control.</summary>
	[ToolboxBitmap(typeof(CheckBox))]
	public class TCheckBox : CheckBox
	{
		/// <summary>Initializes a new instance of the <see cref="TCheckBox"/> class.</summary>
		public TCheckBox()
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
