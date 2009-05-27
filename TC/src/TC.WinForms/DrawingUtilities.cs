// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;

namespace TC.WinForms
{
	/// <summary>Provides drawing functionality.</summary>
	public static class DrawingUtilities
	{
		#region DrawShinyVerticalGradient and CreateShinyVerticalGradientBrush

		/// <summary>Draws a shiny vertical gradient.</summary>
		/// <param name="graphics">The drawing surface to draw on.</param>
		/// <param name="totalBounds">The bounds of the gradient.</param>
		/// <param name="clippingBounds">The bounds to draw within.</param>
		/// <param name="lightColor">The light color of the gradient.</param>
		/// <param name="darkColor">The dark color of the gradient.</param>
		public static void DrawShinyVerticalGradient(
			this Graphics graphics,
			Rectangle totalBounds,
			Rectangle clippingBounds,
			Color lightColor,
			Color darkColor)
		{
			if (graphics == null) throw new ArgumentNullException("graphics");

			using (Brush brush = CreateShinyVerticalGradientBrush(totalBounds, lightColor, darkColor))
				graphics.FillRectangle(brush, clippingBounds);
		}

		/// <summary>Creates a <see cref="T:Brush"/> that can be used to draw a shiny vertical gradient.</summary>
		/// <param name="bounds">The bounds of the gradient.</param>
		/// <param name="lightColor">The light color of the gradient.</param>
		/// <param name="darkColor">The dark color of the gradient.</param>
		/// <returns>The created <see cref="T:Brush"/>.</returns>
		public static Brush CreateShinyVerticalGradientBrush(
			Rectangle bounds,
			Color lightColor,
			Color darkColor)
		{
			return new LinearGradientBrush(bounds, lightColor, darkColor, LinearGradientMode.Vertical)
			{
				WrapMode = WrapMode.TileFlipXY,
				Blend = _shinyVerticalGradientBrushBlend
			};
		}

		private static readonly Blend
			_shinyVerticalGradientBrushBlend = CreateShinyVerticalGradientBrushBlend();

		private static Blend CreateShinyVerticalGradientBrushBlend()
		{
			Blend blend = new Blend();
			blend.Factors = new float[] { 0, 0.4F, 0.8F, 1 };
			blend.Positions = new float[] { 0, 0.4F, 0.5F, 1 };
			return blend;
		}

		#endregion

		#region DrawSigmaBellGradient and CreateSigmaBellGradientBrush

		/// <summary>Draws a sigma bell gradient.</summary>
		/// <param name="graphics">The drawing surface to draw on.</param>
		/// <param name="totalBounds">The bounds of the gradient.</param>
		/// <param name="clippingBounds">The bounds to draw within.</param>
		/// <param name="edgeColor">The color of the gradient edges.</param>
		/// <param name="centerColor">The color of the gradient center.</param>
		/// <param name="mode">The gradient mode.</param>
		public static void DrawSigmaBellGradient(
			this Graphics graphics,
			Rectangle totalBounds,
			Rectangle clippingBounds,
			Color edgeColor,
			Color centerColor,
			LinearGradientMode mode)
		{
			if (graphics == null) throw new ArgumentNullException("graphics");

			using (Brush brush = CreateSigmaBellGradientBrush(totalBounds, edgeColor, centerColor, mode))
				graphics.FillRectangle(brush, clippingBounds);
		}

		/// <summary>Creates a <see cref="T:Brush"/> that can be used to draw a sigma bell gradient.</summary>
		/// <param name="bounds">The bounds of the gradient.</param>
		/// <param name="edgeColor">The color of the gradient edges.</param>
		/// <param name="centerColor">The color of the gradient center.</param>
		/// <param name="mode">The gradient mode.</param>
		/// <returns>The created <see cref="T:Brush"/>.</returns>
		public static Brush CreateSigmaBellGradientBrush(
			Rectangle bounds,
			Color edgeColor,
			Color centerColor,
			LinearGradientMode mode)
		{
			LinearGradientBrush brush = new LinearGradientBrush(bounds, edgeColor, centerColor, mode);
			brush.WrapMode = WrapMode.TileFlipXY;
			brush.SetSigmaBellShape(0.5F, 1F);
			return brush;
		}

		#endregion

		#region CreateRoundedRectanglePath

		/// <summary>Creates a <see cref="T:GraphicsPath"/> that represents a rounded rectangle.</summary>
		/// <param name="rectangle">The <see cref="T:Rectangle"/> to create a <see cref="T:GraphicsPath"/> from.</param>
		/// <param name="radius">The radius of the corners.</param>
		/// <returns>The created <see cref="T:GraphicsPath"/>.</returns>
		public static GraphicsPath CreateRoundedRectanglePath(this Rectangle rectangle, int radius)
		{
			return CreateRoundedRectanglePath(
				rectangle.X,
				rectangle.Y,
				rectangle.Width,
				rectangle.Height,
				radius);
		}

		/// <summary>Creates a <see cref="T:GraphicsPath"/> that represents a rounded rectangle.</summary>
		/// <param name="x">The X-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="width">The width of the rectangle.</param>
		/// <param name="height">The height of the rectangle.</param>
		/// <param name="radius">The radius of the corners.</param>
		/// <returns>The created <see cref="T:GraphicsPath"/>.</returns>
		[SuppressMessage(
			"Microsoft.Naming",
			"CA1704:IdentifiersShouldBeSpelledCorrectly",
			Justification = "x and y are valid identifiers in this context.")]
		public static GraphicsPath CreateRoundedRectanglePath(int x, int y, int width, int height, int radius)
		{
			if (radius < 0)
				throw new ArgumentOutOfRangeException("radius", "radius cannot be negative");
			else if (radius > int.MaxValue / 2)
				throw new ArgumentOutOfRangeException(
					"radius",
					"radius cannot be greater than " + ConvertString.FromInt32(int.MaxValue / 2));

			if (width < 0) throw new ArgumentOutOfRangeException("width", "width cannot be negative");
			if (height < 0) throw new ArgumentOutOfRangeException("height", "height cannot be negative");

			GraphicsPath path = new GraphicsPath();
			path.StartFigure();

			int cornerSize = 2 * radius;

			// top left corner
			if (radius > 0)
				path.AddArc(x, y, cornerSize, cornerSize, 180, 90);

			// top edge
			path.AddLine(x + radius, y, x + width - radius, y);

			// top right corner
			if (radius > 0)
				path.AddArc(x + width - cornerSize, y, cornerSize, cornerSize, 290, 90);

			// right edge
			path.AddLine(x + width, y + radius, x + width, y + height - radius);

			// bottom right corner
			if (radius > 0)
				path.AddArc(x + width - cornerSize, y + height - cornerSize, cornerSize, cornerSize, 0, 90);

			// bottom edge
			path.AddLine(x + width - radius, y + height, x + radius, y + height);

			// bottom left corner
			if (radius > 0)
				path.AddArc(x, y + height - cornerSize, cornerSize, cornerSize, 90, 90);

			// left edge
			path.AddLine(x, y + height - radius, x, y + radius);

			path.CloseFigure();
			return path;
		}

		#endregion

		/// <summary>Draws the specified image translucently.</summary>
		/// <param name="graphics">The drawing surface to draw the image on.</param>
		/// <param name="image">The image to draw.</param>
		/// <param name="destination">The destination rectange on the drawing surface.</param>
		/// <param name="translucency">The translucency of the image (between 0 and 1: 0 = fully transparent; 1 = fully opaque).</param>
		/// <param name="lighting">The lighting of the image (between -1 and 1: -1 = darkest, 0 = normal, 1 = lightest).</param>
		public static void DrawImage(
			this Graphics graphics,
			Image image,
			Rectangle destination,
			float translucency,
			float lighting)
		{
			if (graphics == null) throw new ArgumentNullException("graphics");
			if (image == null) throw new ArgumentNullException("image");
			if (translucency < 0F || translucency > 1F)
				throw new ArgumentOutOfRangeException("translucency", "translucency has to be between 0 and 1.");
			if (lighting < -1F || lighting > 1F)
				throw new ArgumentOutOfRangeException("lighting", "lighting has to be between -1 and 1.");

			ImageAttributes attributes = new ImageAttributes();
			ColorMatrix colorMatrix = new ColorMatrix(
				new float[][]
				{
					new float[] { 1, 0, 0, 0, 0 },
					new float[] { 0, 1, 0, 0, 0 },
					new float[] { 0, 0, 1, 0, 0 },
					new float[] { 0, 0, 0, translucency, 0 },
					new float[] { lighting, lighting, lighting, 0, 1 }
				});
			attributes.SetColorMatrix(colorMatrix);
			Size size = image.Size;

			graphics.DrawImage(
				image,
				destination,
				0,
				0,
				size.Width,
				size.Height,
				GraphicsUnit.Pixel,
				attributes);
		}

		/// <summary>Gets the average of 2 colors.</summary>
		/// <param name="color1">The first color.</param>
		/// <param name="color2">The second color.</param>
		/// <returns>The average of the 2 specified colors.</returns>
		public static Color GetAverageColor(Color color1, Color color2)
		{
			return Color.FromArgb(
				(color1.A + color2.A) / 2,
				(color1.R + color2.R) / 2,
				(color1.G + color2.G) / 2,
				(color1.B + color2.B) / 2);
		}

		/// <summary>Mixes the specified colors.</summary>
		/// <param name="color1">The first color.</param>
		/// <param name="color2">The second color.</param>
		/// <param name="color1Percentage">The percentage of the first color to use (between 0.0 and 1.0).</param>
		/// <returns>The mixed color.</returns>
		public static Color MixColors(Color color1, Color color2, double color1Percentage)
		{
			if (color1Percentage <= 0.0) return color2;
			if (color1Percentage >= 1.0) return color1;

			double color2Percentage = 1.0 - color1Percentage;

			return Color.FromArgb(
				RoundColorComponent((color1.A * color1Percentage) + (color2.A * color2Percentage)),
				RoundColorComponent((color1.R * color1Percentage) + (color2.R * color2Percentage)),
				RoundColorComponent((color1.G * color1Percentage) + (color2.G * color2Percentage)),
				RoundColorComponent((color1.B * color1Percentage) + (color2.B * color2Percentage)));
		}

		private static int RoundColorComponent(double value)
		{
			return (int)Math.Round(value, MidpointRounding.AwayFromZero);
		}
	}
}
