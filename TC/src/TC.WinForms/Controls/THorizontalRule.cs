// TC WinForms Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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
			Size = new Size(100, 2);
			Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			
			SetStyle(
				ControlStyles.AllPaintingInWmPaint 
				| ControlStyles.FixedHeight 
				| ControlStyles.Opaque 
				| ControlStyles.ResizeRedraw 
				| ControlStyles.UserPaint,
				true);

			SetStyle(ControlStyles.Selectable, false);
		}

		/// <summary>Gets or sets the edges of the container to which a control is bound and
		/// determines how a control is resized with its parent.</summary>
		/// <returns>A bitwise combination of the <see cref="T:AnchorStyles"/> values.
		/// The default is Top, Left and Right.</returns>
		[DefaultValue(typeof(AnchorStyles), "Top, Left, Right")]
		public override AnchorStyles Anchor
		{
			get { return base.Anchor; }
			set { base.Anchor = value; }
		}

		/// <summary>Gets or sets the foreground color of the control.</summary>
		/// <returns>The foreground <see cref="T:Color"/> of the control.</returns>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}

		/// <summary>Gets or sets the background color for the control.</summary>
		/// <returns>A <see cref="T:Color"/> that represents the background color of the control.</returns>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
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
				height = 2;

			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// <summary>Raises the <see cref="E:Paint"/> event.</summary>
		/// <param name="e">A <see cref="T:PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle 
				totalBounds = new Rectangle(Point.Empty, Size),
				clippingBounds = new Rectangle(e.ClipRectangle.Left, 0, e.ClipRectangle.Width, 1);

			e.Graphics.DrawSigmaBellGradient(
				totalBounds,
				clippingBounds,
				DrawingUtilities.GetAverageColor(SystemColors.Control, SystemColors.ControlDark),
				SystemColors.ControlDark,
				LinearGradientMode.Horizontal);

			clippingBounds.Offset(0, 1);

			e.Graphics.DrawSigmaBellGradient(
				totalBounds,
				clippingBounds,
				DrawingUtilities.GetAverageColor(SystemColors.Control, SystemColors.ControlLightLight),
				SystemColors.ControlLightLight,
				LinearGradientMode.Horizontal);
		}
	}
}
