// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
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
			ColorTable.UseSystemColors = true;
		}

		/// <summary>Renders the ToolStrip background.</summary>
		/// <param name="e">A <see cref="T:ToolStripRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			if (e.ToolStrip is ToolStripDropDownMenu
				|| e.ToolStrip is MenuStrip)
			{
				// Drop-down menus and menu strips are rendered by the base class
				base.OnRenderToolStripBackground(e);
			}
			else
			{
				// ToolStrips are rendered as a shiny vertical gradient
				e.Graphics.DrawShinyVerticalGradient(
					new Rectangle(Point.Empty, e.ToolStrip.Size),
					e.AffectedBounds,
					ColorTable.ToolStripGradientBegin,
					ColorTable.ToolStripGradientEnd);
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
			// render a background only when the button is pressed or highlighted
			ToolStripItemState state = GetState(e.Item);
			if (state == ToolStripItemState.HighlightedButton
				|| state == ToolStripItemState.PressedButton)
			{
				Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);
				bounds.Width -= 1;
				bounds.Height -= 1;

				e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

				FillAndStrokeButtonBackground(
					e.Graphics,
					bounds,
					ColorTable.ButtonSelectedHighlight,
					ColorTable.ButtonSelectedBorder,
					state == ToolStripItemState.PressedButton);
			}
		}

		private static void FillAndStrokeButtonBackground(
			Graphics g,
			Rectangle bounds,
			Color backColor,
			Color borderColor,
			bool pressed)
		{
			// the background is filled and stroked as a rounded rectangle
			using (GraphicsPath path = bounds.CreateRoundedRectanglePath(1))
			{
				// reset the background to white
				g.FillPath(Brushes.White, path);

				// fill the rounded rectangle with a shiny vertical gradient
				using (Brush brush = CreateButtonBackgroundBrush(bounds, backColor, pressed ? 32 : 64))
					g.FillPath(brush, path);

				// stroke the rounded rectangle with a subtle shiny vertical gradient
				using (Brush brush = CreateButtonBackgroundBrush(bounds, borderColor, pressed ? 192 : 128))
				using (Pen pen = new Pen(brush))
					g.DrawPath(pen, path);
			}
		}

		private static Brush CreateButtonBackgroundBrush(Rectangle bounds, Color color, int lightAlpha)
		{
			return DrawingUtilities.CreateShinyVerticalGradientBrush(
				bounds, Color.FromArgb(lightAlpha, color), color);
		}

		/// <summary>Renders a menu-item background.</summary>
		/// <param name="e">A <see cref="T:ToolStripItemRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			switch (GetState(e.Item))
			{
				case ToolStripItemState.NormalMenuItem:
					base.OnRenderMenuItemBackground(e);
					break;

				case ToolStripItemState.HighlightedMenuItem:
					Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);
					e.Graphics.DrawShinyVerticalGradient(
						bounds,
						bounds,
						Color.FromArgb(180, SystemColors.Highlight),
						SystemColors.Highlight);
					break;
			}
		}

		/// <summary>Renders an item text.</summary>
		/// <param name="e">A <see cref="T:ToolStripItemTextRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if (e.Text.IsNotNullOrEmpty())
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
			switch (GetState(item))
			{
				case ToolStripItemState.HighlightedMenuItem:
					return SystemColors.HighlightText;

				case ToolStripItemState.DisabledMenuItem:
				case ToolStripItemState.DisabledButton:
					return DrawingUtilities.MixColors(
						SystemColors.ControlText,
						SystemColors.Control,
						0.3);

				default:
					return SystemColors.ControlText;
			}
		}

		#region ToolStripItem-state

		private enum ToolStripItemState
		{
			NormalMenuItem,
			HighlightedMenuItem,
			DisabledMenuItem,
			NormalButton,
			HighlightedButton,
			PressedButton,
			DisabledButton
		}

		private static ToolStripItemState GetState(ToolStripItem item)
		{
			return item is ToolStripMenuItem
				? GetMenuItemState(item)
				: GetButtonState(item);
		}

		private static ToolStripItemState GetMenuItemState(ToolStripItem item)
		{
			return item.Enabled
				? GetEnabledMenuItemState(item)
				: ToolStripItemState.DisabledMenuItem;
		}

		private static ToolStripItemState GetEnabledMenuItemState(ToolStripItem item)
		{
			return item.Selected && !(item.Pressed && item.OwnerItem == null)
				? ToolStripItemState.HighlightedMenuItem
				: ToolStripItemState.NormalMenuItem;
		}

		private static ToolStripItemState GetButtonState(ToolStripItem item)
		{
			return item.Enabled
				? GetEnabledButtonState(item)
				: ToolStripItemState.DisabledButton;
		}

		private static ToolStripItemState GetEnabledButtonState(ToolStripItem item)
		{
			return item.Pressed
				? ToolStripItemState.PressedButton
				: item.Selected
					? ToolStripItemState.HighlightedButton
					: ToolStripItemState.NormalButton;
		}

		#endregion

		/// <summary>Renders an item image.</summary>
		/// <param name="e">A <see cref="T:ToolStripItemImageRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			if (e.Image != null)
			{
				float translucency = 1F, lighting = 0F;

				switch (GetState(e.Item))
				{
					case ToolStripItemState.DisabledMenuItem:
					case ToolStripItemState.DisabledButton:
						// disabled images are drawn translucently
						translucency = 0.3F;
						break;

					case ToolStripItemState.HighlightedMenuItem:
					case ToolStripItemState.HighlightedButton:
						// highlighted images are a drawn lighter
						lighting = 0.15F;
						break;

					case ToolStripItemState.PressedButton:
						// pressed images are drawn darker
						lighting = -0.15F;
						break;
				}

				e.Graphics.DrawImage(
					e.Image,
					e.ImageRectangle,
					translucency,
					lighting);
			}
		}

		/// <summary>Renders a separator.</summary>
		/// <param name="e">A <see cref="T:ToolStripSeparatorRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);
			DrawSeparatorPart(e.Graphics, bounds, e.Vertical, false);
			DrawSeparatorPart(e.Graphics, bounds, e.Vertical, true);
		}

		private void DrawSeparatorPart(Graphics g, Rectangle bounds, bool vertical, bool light)
		{
			using (Brush brush = CreateSeparatorBrush(bounds, vertical, light))
				g.FillRectangle(brush, CalculateSeparatorPartBounds(bounds, vertical, light));
		}

		private static Rectangle CalculateSeparatorPartBounds(Rectangle bounds, bool vertical, bool light)
		{
			int offset = light ? 1 : 0;
			return vertical
				? new Rectangle((bounds.Width / 2) + offset, 4, 1, bounds.Height - 8)
				: new Rectangle(4, (bounds.Height / 2) + offset, bounds.Width - 8, 1);
		}

		private Brush CreateSeparatorBrush(Rectangle bounds, bool vertical, bool light)
		{
			return new LinearGradientBrush(
				bounds,
				light ? ColorTable.SeparatorLight : Color.FromArgb(64, ColorTable.SeparatorDark),
				light ? Color.FromArgb(64, ColorTable.SeparatorLight) : ColorTable.SeparatorDark,
				vertical ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal);
		}

		/// <summary>Renders an item checkbox.</summary>
		/// <param name="e">A <see cref="T:ToolStripItemImageRenderEventArgs"/> that contains the event data.</param>
		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
			if (e.Image != null)
			{
				if (e.Item.Enabled)
				{
					Rectangle bounds = e.ImageRectangle;
					bounds.Inflate(1, 1);

					e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

					FillAndStrokeButtonBackground(
						e.Graphics,
						bounds,
						ColorTable.ButtonCheckedHighlight,
						ColorTable.ButtonCheckedHighlightBorder,
						e.Item.Pressed);
				}

				OnRenderItemImage(e);
			}
		}
	}
}
