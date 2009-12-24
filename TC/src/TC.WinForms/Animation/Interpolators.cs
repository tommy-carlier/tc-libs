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

namespace TC.WinForms.Animation
{
	/// <summary>Provides default implementations of the <see cref="T:Interpolator"/> delegate.</summary>
	public static class Interpolators
	{
		#region initialization

		private static readonly IDictionary<Type, Delegate> _defaultInterpolators = InitializeInterpolators();

		private static IDictionary<Type, Delegate> InitializeInterpolators()
		{
			var interpolators = new Dictionary<Type, Delegate>(20);

			interpolators.Add<bool>(Interpolate);
			interpolators.Add<byte>(Interpolate);
			interpolators.Add<short>(Interpolate);
			interpolators.Add<int>(Interpolate);
			interpolators.Add<long>(Interpolate);
			interpolators.Add<float>(Interpolate);
			interpolators.Add<double>(Interpolate);
			interpolators.Add<decimal>(Interpolate);
			interpolators.Add<Color>(Interpolate);
			interpolators.Add<Point>(Interpolate);
			interpolators.Add<PointF>(Interpolate);
			interpolators.Add<Size>(Interpolate);
			interpolators.Add<SizeF>(Interpolate);
			interpolators.Add<Rectangle>(Interpolate);
			interpolators.Add<RectangleF>(Interpolate);
			interpolators.Add<Padding>(Interpolate);

			return interpolators;
		}

		private static void Add<T>(this IDictionary<Type, Delegate> interpolators, Interpolator<T> interpolator)
		{
			interpolators[typeof(T)] = interpolator;
		}

		#endregion

		/// <summary>Gets the default interpolator for the specified type T.</summary>
		/// <typeparam name="T">The type of the values to interpolate.</typeparam>
		/// <returns>The default interpolator for the specified type T, or null if no default interpolator exists for T.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type T is an important parameter and knowledge of generics is essential for using this function.")]
		public static Interpolator<T> GetInterpolator<T>()
		{
			return _defaultInterpolators.GetValue(typeof(T)) as Interpolator<T>;
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
