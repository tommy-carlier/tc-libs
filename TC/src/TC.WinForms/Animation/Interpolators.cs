// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Animation
{
	/// <summary>Provides default implementations of the <see cref="T:Interpolator"/> delegate.</summary>
	public static class Interpolators
	{
		#region initialization

		private static readonly Dictionary<Type, Delegate> fDefaultInterpolators = new Dictionary<Type, Delegate>();
		static Interpolators()
		{
			fDefaultInterpolators[typeof(bool)] = new Interpolator<bool>(Interpolate);
			fDefaultInterpolators[typeof(byte)] = new Interpolator<byte>(Interpolate);
			fDefaultInterpolators[typeof(short)] = new Interpolator<short>(Interpolate);
			fDefaultInterpolators[typeof(int)] = new Interpolator<int>(Interpolate);
			fDefaultInterpolators[typeof(long)] = new Interpolator<long>(Interpolate);
			fDefaultInterpolators[typeof(float)] = new Interpolator<float>(Interpolate);
			fDefaultInterpolators[typeof(double)] = new Interpolator<double>(Interpolate);
			fDefaultInterpolators[typeof(decimal)] = new Interpolator<decimal>(Interpolate);
			fDefaultInterpolators[typeof(Color)] = new Interpolator<Color>(Interpolate);
			fDefaultInterpolators[typeof(Point)] = new Interpolator<Point>(Interpolate);
			fDefaultInterpolators[typeof(PointF)] = new Interpolator<PointF>(Interpolate);
			fDefaultInterpolators[typeof(Size)] = new Interpolator<Size>(Interpolate);
			fDefaultInterpolators[typeof(SizeF)] = new Interpolator<SizeF>(Interpolate);
			fDefaultInterpolators[typeof(Rectangle)] = new Interpolator<Rectangle>(Interpolate);
			fDefaultInterpolators[typeof(RectangleF)] = new Interpolator<RectangleF>(Interpolate);
			fDefaultInterpolators[typeof(Padding)] = new Interpolator<Padding>(Interpolate);
		}

		#endregion

		/// <summary>Gets the default interpolator for the specified type T.</summary>
		/// <typeparam name="T">The type of the values to interpolate.</typeparam>
		/// <returns>The default interpolator for the specified type T, or null if no default interpolator exists for T.</returns>
		public static Interpolator<T> GetInterpolator<T>()
		{
			Delegate lInterpolator;
			if (fDefaultInterpolators.TryGetValue(typeof(T), out lInterpolator))
				return lInterpolator as Interpolator<T>;
			else return null;
		}

		#region interpolator implementations

		private static bool Interpolate(bool startValue, bool endValue, long progress, long maximum)
		{
			if (startValue == endValue) return startValue;
			else return progress * 2 >= maximum ? endValue : startValue;
		}

		private static byte Interpolate(byte startValue, byte endValue, long progress, long maximum)
		{
			if (startValue == endValue) return startValue;
			else return (byte)(startValue + (((long)endValue - startValue) * progress / maximum));
		}

		private static short Interpolate(short startValue, short endValue, long progress, long maximum)
		{
			if (startValue == endValue) return startValue;
			else return (short)(startValue + (((double)endValue - startValue) * progress / maximum));
		}

		private static int Interpolate(int startValue, int endValue, long progress, long maximum)
		{
			if (startValue == endValue) return startValue;
			else return (int)(startValue + (((double)endValue - startValue) * progress / maximum));
		}

		private static long Interpolate(long startValue, long endValue, long progress, long maximum)
		{
			if (startValue == endValue) return startValue;
			else return (long)(startValue + ((endValue - startValue) * progress / maximum));
		}

		private static float Interpolate(float startValue, float endValue, long progress, long maximum)
		{
			if (startValue == endValue) return startValue;
			else return (float)Math.Round(startValue + (((double)endValue - startValue) * progress / maximum));
		}

		private static double Interpolate(double startValue, double endValue, long progress, long maximum)
		{
			if (startValue == endValue) return startValue;
			else return Math.Round(startValue + ((endValue - startValue) * progress / maximum));
		}

		private static decimal Interpolate(decimal startValue, decimal endValue, long progress, long maximum)
		{
			if (startValue == endValue) return startValue;
			else return Math.Round(startValue + ((endValue - startValue) * progress / maximum));
		}

		private static Color Interpolate(Color startValue, Color endValue, long progress, long maximum)
		{
			return Color.FromArgb(
				Interpolate(startValue.A, endValue.A, progress, maximum),
				Interpolate(startValue.R, endValue.R, progress, maximum),
				Interpolate(startValue.G, endValue.G, progress, maximum),
				Interpolate(startValue.B, endValue.B, progress, maximum));
		}

		private static Point Interpolate(Point startValue, Point endValue, long progress, long maximum)
		{
			return new Point(
				Interpolate(startValue.X, endValue.X, progress, maximum),
				Interpolate(startValue.Y, endValue.Y, progress, maximum));
		}

		private static PointF Interpolate(PointF startValue, PointF endValue, long progress, long maximum)
		{
			return new PointF(
				Interpolate(startValue.X, endValue.X, progress, maximum),
				Interpolate(startValue.Y, endValue.Y, progress, maximum));
		}

		private static Size Interpolate(Size startValue, Size endValue, long progress, long maximum)
		{
			return new Size(
				Interpolate(startValue.Width, endValue.Width, progress, maximum),
				Interpolate(startValue.Height, endValue.Height, progress, maximum));
		}

		private static SizeF Interpolate(SizeF startValue, SizeF endValue, long progress, long maximum)
		{
			return new SizeF(
				Interpolate(startValue.Width, endValue.Width, progress, maximum),
				Interpolate(startValue.Height, endValue.Height, progress, maximum));
		}

		private static Rectangle Interpolate(Rectangle startValue, Rectangle endValue, long progress, long maximum)
		{
			return new Rectangle(
				Interpolate(startValue.X, endValue.X, progress, maximum),
				Interpolate(startValue.Y, endValue.Y, progress, maximum),
				Interpolate(startValue.Width, endValue.Width, progress, maximum),
				Interpolate(startValue.Height, endValue.Height, progress, maximum));
		}

		private static RectangleF Interpolate(RectangleF startValue, RectangleF endValue, long progress, long maximum)
		{
			return new RectangleF(
				Interpolate(startValue.X, endValue.X, progress, maximum),
				Interpolate(startValue.Y, endValue.Y, progress, maximum),
				Interpolate(startValue.Width, endValue.Width, progress, maximum),
				Interpolate(startValue.Height, endValue.Height, progress, maximum));
		}

		private static Padding Interpolate(Padding startValue, Padding endValue, long progress, long maximum)
		{
			return new Padding(
				Interpolate(startValue.Left, endValue.Left, progress, maximum),
				Interpolate(startValue.Top, endValue.Top, progress, maximum),
				Interpolate(startValue.Right, endValue.Right, progress, maximum),
				Interpolate(startValue.Bottom, endValue.Bottom, progress, maximum));
		}

		#endregion
	}
}
