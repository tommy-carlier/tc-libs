// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms
{
	/// <summary>Represents an object that can render items in list controls.</summary>
	[SuppressMessage(
		"Microsoft.Naming",
		"CA1704:IdentifiersShouldBeSpelledCorrectly",
		MessageId = "Renderer",
		Justification = "Renderer is a term that describes an object that handles visual rendering.")]
	public class ListItemRenderer
	{
		/// <summary>Initializes a new instance of the <see cref="ListItemRenderer"/> class.</summary>
		/// <param name="control">The control that owns this renderer.</param>
		/// <param name="defaultImage">The default image for each item.</param>
		public ListItemRenderer(ListControl control, Image defaultImage)
		{
			if (control == null) throw new ArgumentNullException("control");

			fControl = control;
			fDefaultImage = defaultImage;
		}

		private readonly ListControl fControl;
		private readonly Image fDefaultImage;

		internal void MeasureItem(MeasureItemEventArgs context, object item)
		{
			if (item != null)
			{
				Size lItemSize = MeasureItemCore(context, item);
				context.ItemWidth = lItemSize.Width;
				context.ItemHeight = lItemSize.Height;
			}
		}

		/// <summary>Measures the specified item.</summary>
		/// <param name="context">The <see cref="MeasureItemEventArgs"/> instance that represents the measuring context.</param>
		/// <param name="item">The item to measure.</param>
		/// <returns>The size of the specified item.</returns>
		protected virtual Size MeasureItemCore(MeasureItemEventArgs context, object item)
		{
			Size lSize = Size.Empty;

			// measure the image of the specified image
			Image lImage = GetItemImage(item) ?? fDefaultImage;
			if (lImage != null)
			{
				lSize.Width += lImage.Width;
				if (lSize.Height > lImage.Height)
					lSize.Height = lImage.Height;
			}

			// measure the text of the specified image
			string lText = GetItemText(item);
			if (!string.IsNullOrEmpty(lText))
			{
				Size lTextSize = TextRenderer.MeasureText(
					context.Graphics,
					lText,
					fControl.Font,
					Size.Empty,
					ItemTextFormatFlags);

				lTextSize.Height += 6;
				lSize.Width += lTextSize.Width;
				if (lTextSize.Height > lSize.Height)
					lSize.Height = lTextSize.Height;
			}

			lSize.Height += 2;
			return lSize;
		}

		internal void DrawItem(DrawItemEventArgs context, object item)
		{
			DrawItemBackground(context);
			
			if (item != null)
				DrawItemCore(context, item);

			context.DrawFocusRectangle();
		}

		/// <summary>Draws the specified item.</summary>
		/// <param name="context">The <see cref="DrawItemEventArgs"/> instance that represents the drawing context.</param>
		/// <param name="item">The item to draw.</param>
		protected virtual void DrawItemCore(DrawItemEventArgs context, object item)
		{
			Rectangle lBounds = context.Bounds;

			// draw the image
			Image lImage = GetItemImage(item) ?? fDefaultImage;
			Rectangle lImageBounds = Rectangle.Empty;
			if (lImage != null)
			{
				lImageBounds = new Rectangle(
					lBounds.Left,
					lBounds.Top + ((lBounds.Height - lImage.Height) / 2),
					lImage.Width,
					lImage.Height);

				context.Graphics.DrawImage(
					lImage,
					lImageBounds,
					1F,
					IsSelected(context) ? 0.15F : 0F);
			}

			// draw the text
			string lText = GetItemText(item);
			if (!string.IsNullOrEmpty(lText))
			{
				Rectangle lTextBounds = new Rectangle(
					lImageBounds.Right,
					lBounds.Top,
					lBounds.Width - lImageBounds.Width,
					lBounds.Height);

				TextRenderer.DrawText(
					context.Graphics,
					lText,
					context.Font,
					lTextBounds,
					context.ForeColor,
					ItemTextFormatFlags);
			}
		}

		private const TextFormatFlags ItemTextFormatFlags
			= TextFormatFlags.Left | TextFormatFlags.VerticalCenter
			| TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine;

		private static void DrawItemBackground(DrawItemEventArgs context)
		{
			if (IsSelected(context) && !SystemInformation.HighContrast)
				DrawShinySelectedItemBackground(context);
			else DrawSolidItemBackground(context);
		}

		private static bool IsSelected(DrawItemEventArgs context)
		{
			return (context.State & DrawItemState.Selected) == DrawItemState.Selected;
		}

		private static void DrawShinySelectedItemBackground(DrawItemEventArgs context)
		{
			context.Graphics.DrawShinyVerticalGradient(
				context.Bounds,
				context.Bounds,
				DrawingUtilities.MixColors(
					SystemColors.Highlight,
					SystemColors.Window,
					0.7),
				SystemColors.Highlight);
		}

		private static void DrawSolidItemBackground(DrawItemEventArgs context)
		{
			using (SolidBrush lBrush = new SolidBrush(context.BackColor))
				context.Graphics.FillRectangle(lBrush, context.Bounds);
		}

		/// <summary>Gets the text to display for the specified item.</summary>
		/// <param name="item">The item to get the text representation of.</param>
		/// <returns>The text to display for the specified item.</returns>
		protected virtual string GetItemText(object item)
		{
			return fControl.GetItemText(item);
		}

		/// <summary>Gets the image to display for the specified item.</summary>
		/// <param name="item">The item to get the display image of.</param>
		/// <returns>The image to display for the specified item.</returns>
		protected virtual Image GetItemImage(object item)
		{
			return null;
		}
	}
}
