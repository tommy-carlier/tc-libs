// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms
{
	/// <summary>Handles the painting of toolstrips.</summary>
	[SuppressMessage(
		"Microsoft.Naming",
		"CA1704:IdentifiersShouldBeSpelledCorrectly",
		MessageId = "Renderer",
		Justification = "Renderer is a term that describes an object that handles visual rendering.")]
	public class TToolStripRenderer : ToolStripProfessionalRenderer
	{
		/// <summary>Initializes a new instance of the <see cref="TToolStripRenderer"/> class.</summary>
		public TToolStripRenderer()
		{
			RoundedEdges = false;
		}

		/// <summary>Renders the ToolStrip background.</summary>
		/// <param name="e">A <see cref="T:ToolStripRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			if (e.ToolStrip is ToolStripDropDownMenu)
			{
				// Drop-down menus are rendered by the base class
				base.OnRenderToolStripBackground(e);
			}
			else if (e.ToolStrip is MenuStrip)
			{
				// MenuStrips are rendered in a solid color
				e.Graphics.FillRectangle(SystemBrushes.ControlLightLight, e.AffectedBounds);
			}
			else
			{
				// ToolStrips are rendered as a shiny vertical gradient
				e.Graphics.DrawShinyVerticalGradient(
					new Rectangle(Point.Empty, e.ToolStrip.Size),
					e.AffectedBounds,
					SystemColors.ControlLightLight,
					SystemColors.Control);
			}
		}

		/// <summary>Renders the ToolStrip border.</summary>
		/// <param name="e">A <see cref="T:ToolStripRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			// only draw the border of the drop-down menu, not of the regular ToolStrip and MenuStrip
			if (e.ToolStrip is ToolStripDropDownMenu)
				base.OnRenderToolStripBorder(e);
		}

		/// <summary>Renders the image margin.</summary>
		/// <param name="e">A <see cref="T:ToolStripRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
			// no image margin
		}

		/// <summary>Renders a button background.</summary>
		/// <param name="e">A <see cref="T:ToolStripRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (e.Item.Enabled && (e.Item.Pressed || e.Item.Selected))
				DrawSelectedItemBackground(e);
		}

		/// <summary>Renders a menu-item background.</summary>
		/// <param name="e">A <see cref="T:ToolStripItemRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			if (e.Item.Enabled)
			{
				if (e.Item.Pressed && e.Item.OwnerItem == null)
					base.OnRenderMenuItemBackground(e);
				else if (e.Item.Selected)
					DrawSelectedItemBackground(e);
			}
		}

		private static void DrawSelectedItemBackground(ToolStripItemRenderEventArgs e)
		{
			Rectangle lBounds = new Rectangle(Point.Empty, e.Item.Size);
			e.Graphics.DrawShinyVerticalGradient(
				lBounds,
				lBounds,
				Color.FromArgb(20, SystemColors.Highlight),
				Color.FromArgb(70, SystemColors.Highlight));
		}

		/// <summary>Renders an item text.</summary>
		/// <param name="e">A <see cref="T:ToolStripItemTextRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Text))
				TextRenderer.DrawText(
					e.Graphics,
					e.Text,
					e.TextFont,
					e.TextRectangle,
					GetItemTextColor(e.Item),
					e.TextFormat);
		}

		private static Color GetItemTextColor(ToolStripItem item)
		{
			return
				item.Enabled
					? SystemColors.ControlText
					: SystemColors.GrayText;
		}

		/// <summary>Renders an item image.</summary>
		/// <param name="e">A <see cref="T:ToolStripItemImageRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			if (e.Image != null)
			{
				bool lIsEnabled = e.Item.Enabled;
				bool lIsPressed = lIsEnabled && e.Item.Pressed;
				bool lIsSelected = lIsEnabled && !lIsPressed && e.Item.Selected;

				e.Graphics.DrawImage(
					e.Image,
					e.ImageRectangle,
					lIsEnabled ? 1F : 0.3F,
					lIsPressed ? -0.15F : lIsSelected ? 0.15F : 0F);
			}
		}

		/// <summary>Renders an item checkbox.</summary>
		/// <param name="e">A <see cref="T:ToolStripItemImageRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
			if (e.Image != null)
			{
				if (e.Item.Enabled && !e.Item.Pressed && e.Item.Selected)
				{
					Rectangle lRectangle = e.ImageRectangle;
					e.Graphics.FillRectangle(SystemBrushes.ControlLightLight, lRectangle);
					e.Graphics.DrawRectangle(
						SystemPens.Highlight,
						lRectangle.X,
						lRectangle.Y,
						lRectangle.Width - 1,
						lRectangle.Height - 1);
				}

				OnRenderItemImage(e);
			}
		}
	}
}
