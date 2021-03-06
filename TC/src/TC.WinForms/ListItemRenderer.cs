﻿// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

namespace TC.WinForms
{
    /// <summary>Represents an object that can render items in list controls.</summary>
    public class ListItemRenderer
	{
		/// <summary>Initializes a new instance of the <see cref="ListItemRenderer"/> class.</summary>
		/// <param name="control">The control that owns this renderer.</param>
		/// <param name="defaultImage">The default image for each item.</param>
		public ListItemRenderer(ListControl control, Image defaultImage)
		{
            _control = control ?? throw new ArgumentNullException("control");
			_defaultImage = defaultImage;
		}

		private readonly ListControl _control;
		private readonly Image _defaultImage;

		internal void MeasureItem(MeasureItemEventArgs context, object item)
		{
			if (item != null)
			{
				Size itemSize = MeasureItemCore(context, item);
				context.ItemWidth = itemSize.Width;
				context.ItemHeight = itemSize.Height;
			}
		}

		/// <summary>Measures the specified item.</summary>
		/// <param name="context">The <see cref="MeasureItemEventArgs"/> instance that represents the measuring context.</param>
		/// <param name="item">The item to measure.</param>
		/// <returns>The size of the specified item.</returns>
		protected virtual Size MeasureItemCore(MeasureItemEventArgs context, object item)
		{
			Size size = Size.Empty;

			// measure the image of the specified item
			Image image = GetItemImage(item) ?? _defaultImage;
			if (image != null)
			{
				size.Width += image.Width;
				if (image.Height > size.Height)
					size.Height = image.Height;
			}

			// measure the text of the specified item
			string text = GetItemText(item);
			if (text.IsNotNullOrEmpty())
			{
				Size textSize = TextRenderer.MeasureText(
					context.Graphics,
					text,
					_control.Font,
					Size.Empty,
					ItemTextFormatFlags);

				textSize.Height += 6;
				size.Width += textSize.Width;
				if (textSize.Height > size.Height)
					size.Height = textSize.Height;
			}

			size.Height += 2;
			return size;
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
			Rectangle bounds = context.Bounds;

			// draw the image
			Image image = GetItemImage(item) ?? _defaultImage;
			Rectangle imageBounds = Rectangle.Empty;
			if (image != null)
			{
				imageBounds = new Rectangle(
					bounds.Left,
					bounds.Top + ((bounds.Height - image.Height) / 2),
					image.Width,
					image.Height);

				context.Graphics.DrawImage(
					image,
					imageBounds,
					1F,
					IsSelected(context) ? 0.15F : 0F);
			}

			// draw the text
			string text = GetItemText(item);
			if (text.IsNotNullOrEmpty())
			{
				Rectangle textBounds = new Rectangle(
					imageBounds.Right,
					bounds.Top,
					bounds.Width - imageBounds.Width,
					bounds.Height);

				TextRenderer.DrawText(
					context.Graphics,
					text,
					context.Font,
					textBounds,
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
			=> (context.State & DrawItemState.Selected) == DrawItemState.Selected;

        private static void DrawShinySelectedItemBackground(DrawItemEventArgs context)
		{
			context.Graphics.DrawShinyVerticalGradient(
				context.Bounds,
				context.Bounds,
				CalculateShinySelectedItemLightColor(),
				SystemColors.Highlight);
		}

        private static Color CalculateShinySelectedItemLightColor()
			=> DrawingUtilities.MixColors(SystemColors.Highlight, SystemColors.Window, 0.7);

        private static void DrawSolidItemBackground(DrawItemEventArgs context)
		{
			using (SolidBrush brush = new SolidBrush(context.BackColor))
				context.Graphics.FillRectangle(brush, context.Bounds);
		}

        /// <summary>Gets the text to display for the specified item.</summary>
        /// <param name="item">The item to get the text representation of.</param>
        /// <returns>The text to display for the specified item.</returns>
        protected virtual string GetItemText(object item) => _control.GetItemText(item);

        /// <summary>Gets the image to display for the specified item.</summary>
        /// <param name="item">The item to get the display image of.</param>
        /// <returns>The image to display for the specified item.</returns>
        protected virtual Image GetItemImage(object item) => null;
    }
}
