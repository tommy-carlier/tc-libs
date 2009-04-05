// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
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
	/// <summary>Represents a standard label control.</summary>
	[ToolboxBitmap(typeof(Label))]
	public class TLabel : Label, IHasSystemFont
	{
		#region IHasSystemFont Members

		private SystemFont fSystemFont;

		/// <summary>Gets the <see cref="T:SystemFont"/> of the control.</summary>
		/// <value>The <see cref="T:SystemFont"/> of the control.</value>
		[DefaultValue(typeof(SystemFont), "Default"), Category("Appearance")]
		[Description("The font that is used to display the text.")]
		public SystemFont SystemFont
		{
			get { return fSystemFont; }
			set
			{
				if (fSystemFont != value)
				{
					fSystemFont = value;
					Font = value.ToFont();
				}
			}
		}

		/// <summary>Gets or sets the font of the text displayed by the control.</summary>
		/// <returns>The <see cref="T:Font"/> to apply to the text displayed by the control.</returns>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Font Font
		{
			get { return base.Font; }
			set { base.Font = value; }
		}

		#endregion

		/// <summary>Raises the <see cref="E:Click"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);

			// select the next control when this label is clicked
			Control lParent = Parent;
			if (lParent != null)
			{
				if (lParent.SelectNextControl(this, true, true, true, true) && !lParent.ContainsFocus)
					lParent.Focus();
			}
		}
	}
}
