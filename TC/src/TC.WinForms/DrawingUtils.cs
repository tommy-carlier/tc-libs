// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;

namespace TC.WinForms
{
	/// <summary>Provides drawing functionality.</summary>
	public static class DrawingUtils
	{
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

			using (LinearGradientBrush lBrush 
				= new LinearGradientBrush(
					totalBounds, 
					lightColor,
					darkColor,
					LinearGradientMode.Vertical))
			{
				lBrush.WrapMode = WrapMode.TileFlipXY;
				lBrush.Blend = fShinyVerticalGradientBrushBlend;
				graphics.FillRectangle(lBrush, clippingBounds);
			}
		}

		private static readonly Blend 
			fShinyVerticalGradientBrushBlend = CreateShinyVerticalGradientBrushBlend();

		private static Blend CreateShinyVerticalGradientBrushBlend()
		{
			Blend lBlend = new Blend();
			lBlend.Factors = new float[] { 0, 0.4F, 0.8F, 1 };
			lBlend.Positions = new float[] { 0, 0.4F, 0.5F, 1 };
			return lBlend;
		}

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

			ImageAttributes lAttributes = new ImageAttributes();
			ColorMatrix lColorMatrix = new ColorMatrix(new float[][]
				{
					new float[] { 1, 0, 0, 0, 0 },
					new float[] { 0, 1, 0, 0, 0 },
					new float[] { 0, 0, 1, 0, 0 },
					new float[] { 0, 0, 0, translucency, 0 },
					new float[] { lighting, lighting, lighting, 0, 1 }
				});
			lAttributes.SetColorMatrix(lColorMatrix);
			Size lSize = image.Size;

			graphics.DrawImage(
				image,
				destination,
				0,
				0,
				lSize.Width,
				lSize.Height, 
				GraphicsUnit.Pixel, 
				lAttributes);
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

			double lColor2Percentage = 1.0 - color1Percentage;
			
			return Color.FromArgb(
				RoundColorComponent((color1.A * color1Percentage) + (color2.A * lColor2Percentage)),
				RoundColorComponent((color1.R * color1Percentage) + (color2.R * lColor2Percentage)),
				RoundColorComponent((color1.G * color1Percentage) + (color2.G * lColor2Percentage)),
				RoundColorComponent((color1.B * color1Percentage) + (color2.B * lColor2Percentage)));
		}

		private static int RoundColorComponent(double value)
		{
			return (int)Math.Round(value, MidpointRounding.AwayFromZero);
		}
	}
}
