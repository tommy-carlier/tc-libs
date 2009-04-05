// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms
{
	/// <summary>Provides utilities that deal with screens.</summary>
	public static class ScreenUtils
	{
		/// <summary>Calculates the total working area of all screens combined.</summary>
		/// <returns>A union of the working areas of all the screens.</returns>
		public static Rectangle CalculateTotalWorkingArea()
		{
			Rectangle lWorkingArea = Rectangle.Empty;

			foreach (Screen lScreen in Screen.AllScreens)
				lWorkingArea = Rectangle.Union(lWorkingArea, lScreen.WorkingArea);

			return lWorkingArea;
		}

		/// <summary>Calculates the total bounds of all screens combined.</summary>
		/// <returns>A union of the bounds of all the screens.</returns>
		public static Rectangle CalculateTotalScreenBounds()
		{
			Rectangle lBounds = Rectangle.Empty;

			foreach (Screen lScreen in Screen.AllScreens)
				lBounds = Rectangle.Union(lBounds, lScreen.Bounds);

			return lBounds;
		}

		/// <summary>Adjusts the specified bounds to the working area of all screens.</summary>
		/// <param name="bounds">The bounds to adjust.</param>
		/// <returns>The adjusted bounds.</returns>
		public static Rectangle AdjustBoundsToWorkingArea(this Rectangle bounds)
		{
			Rectangle lWorkingArea = CalculateTotalWorkingArea();

			// adjust the location to the total working area
			if (bounds.X < lWorkingArea.X)
				bounds.X = lWorkingArea.X;
			else if (bounds.X >= lWorkingArea.Right)
				bounds.X = lWorkingArea.Right - 10;

			if (bounds.Y < lWorkingArea.Y)
				bounds.Y = lWorkingArea.Y;
			else if (bounds.Y >= lWorkingArea.Bottom)
				bounds.Y = lWorkingArea.Bottom - 10;

			// find the screen that contains the largest area of the bounds
			Rectangle lMaxWorkingArea = Rectangle.Empty;
			int lArea, lMaxArea = 0;
			foreach (Screen lScreen in Screen.AllScreens)
			{
				Rectangle lIntersection 
					= Rectangle.Intersect(bounds, lWorkingArea = lScreen.WorkingArea);

				if ((lArea = lIntersection.Width * lIntersection.Height) > lMaxArea)
				{
					lMaxArea = lArea;
					lMaxWorkingArea = lWorkingArea;
				}
			}

			// adjust the bounds to the working area of the selected screen
			if (bounds.Width > lMaxWorkingArea.Width) bounds.Width = lMaxWorkingArea.Width;
			if (bounds.Height > lMaxWorkingArea.Height) bounds.Height = lMaxWorkingArea.Height;

			if (bounds.X < lMaxWorkingArea.X) 
				bounds.X = lMaxWorkingArea.X;
			else if (bounds.Right >= lMaxWorkingArea.Right)
				bounds.X = lMaxWorkingArea.Right - bounds.Width - 1;

			if (bounds.Y < lMaxWorkingArea.Y)
				bounds.Y = lMaxWorkingArea.Y;
			else if (bounds.Bottom >= lMaxWorkingArea.Bottom)
				bounds.Y = lMaxWorkingArea.Bottom - bounds.Height - 1;

			return bounds;
		}
	}
}
