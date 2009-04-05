﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a horizontal rule control.</summary>
	[ToolboxItem(true), ToolboxBitmap(typeof(THorizontalRule))]
	public class THorizontalRule : Control
	{
		/// <summary>Initializes a new instance of the <see cref="THorizontalRule"/> class.</summary>
		public THorizontalRule()
		{
			Size = new Size(100, 1);
			Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			ForeColor = SystemColors.ControlDark;
			
			SetStyle(
				ControlStyles.AllPaintingInWmPaint | ControlStyles.FixedHeight 
					| ControlStyles.Opaque | ControlStyles.ResizeRedraw 
					| ControlStyles.UserPaint,
				true);

			SetStyle(ControlStyles.Selectable, false);
		}

		/// <summary>Gets or sets the edges of the container to which a control is bound and
		/// determines how a control is resized with its parent.</summary>
		/// <returns>A bitwise combination of the <see cref="T:AnchorStyles"/> values. The default is Top, Left and Right.</returns>
		[DefaultValue(typeof(AnchorStyles), "Top, Left, Right")]
		public override AnchorStyles Anchor
		{
			get { return base.Anchor; }
			set { base.Anchor = value; }
		}

		/// <summary>Gets or sets the foreground color of the control.</summary>
		/// <returns>The foreground <see cref="T:Color"/> of the control.</returns>
		[DefaultValue(typeof(Color), "ControlDark")]
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
			get { return base.Text; }
			set { base.Text = value; }
		}

		/// <summary>Performs the work of setting the specified bounds of this control.</summary>
		/// <param name="x">The new <see cref="P:Left"/> property value of the control.</param>
		/// <param name="y">The new <see cref="P:Top"/> property value of the control.</param>
		/// <param name="width">The new <see cref="P:Width"/> property value of the control.</param>
		/// <param name="height">The new <see cref="P:Height"/> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:BoundsSpecified"/> values.</param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Height) == BoundsSpecified.Height)
				height = 1;

			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// <summary>Raises the <see cref="E:Paint"/> event.</summary>
		/// <param name="e">A <see cref="T:PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			using (Pen lPen = new Pen(ForeColor))
				e.Graphics.DrawLine(lPen, e.ClipRectangle.Left, 0, e.ClipRectangle.Right, 0);
		}
	}
}