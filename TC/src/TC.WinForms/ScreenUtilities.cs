// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System.Drawing;
using System.Windows.Forms;

namespace TC.WinForms
{
	/// <summary>Provides utilities that deal with screens.</summary>
	public static class ScreenUtilities
	{
		/// <summary>Calculates the total working area of all screens combined.</summary>
		/// <returns>A union of the working areas of all the screens.</returns>
		public static Rectangle CalculateTotalWorkingArea()
		{
			Rectangle workingArea = Rectangle.Empty;

			foreach (Screen screen in Screen.AllScreens)
				workingArea = Rectangle.Union(workingArea, screen.WorkingArea);

			return workingArea;
		}

		/// <summary>Calculates the total bounds of all screens combined.</summary>
		/// <returns>A union of the bounds of all the screens.</returns>
		public static Rectangle CalculateTotalScreenBounds()
		{
			Rectangle bounds = Rectangle.Empty;

			foreach (Screen screen in Screen.AllScreens)
				bounds = Rectangle.Union(bounds, screen.Bounds);

			return bounds;
		}

		/// <summary>Adjusts the specified bounds to the working area of all screens.</summary>
		/// <param name="bounds">The bounds to adjust.</param>
		/// <returns>The adjusted bounds.</returns>
		public static Rectangle AdjustBoundsToWorkingArea(this Rectangle bounds)
		{
			Rectangle workingArea = CalculateTotalWorkingArea();

			// adjust the location to the total working area
			if (bounds.X < workingArea.X)
				bounds.X = workingArea.X;
			else if (bounds.X >= workingArea.Right)
				bounds.X = workingArea.Right - 10;

			if (bounds.Y < workingArea.Y)
				bounds.Y = workingArea.Y;
			else if (bounds.Y >= workingArea.Bottom)
				bounds.Y = workingArea.Bottom - 10;

			// find the screen that contains the largest area of the bounds
			Rectangle maxWorkingArea = Rectangle.Empty;
			int area, maxArea = 0;
			foreach (Screen screen in Screen.AllScreens)
			{
				Rectangle intersection 
					= Rectangle.Intersect(bounds, workingArea = screen.WorkingArea);

				if ((area = intersection.Width * intersection.Height) > maxArea)
				{
					maxArea = area;
					maxWorkingArea = workingArea;
				}
			}

			// adjust the bounds to the working area of the selected screen
			if (bounds.Width > maxWorkingArea.Width) bounds.Width = maxWorkingArea.Width;
			if (bounds.Height > maxWorkingArea.Height) bounds.Height = maxWorkingArea.Height;

			if (bounds.X < maxWorkingArea.X) 
				bounds.X = maxWorkingArea.X;
			else if (bounds.Right >= maxWorkingArea.Right)
				bounds.X = maxWorkingArea.Right - bounds.Width - 1;

			if (bounds.Y < maxWorkingArea.Y)
				bounds.Y = maxWorkingArea.Y;
			else if (bounds.Bottom >= maxWorkingArea.Bottom)
				bounds.Y = maxWorkingArea.Bottom - bounds.Height - 1;

			return bounds;
		}
	}
}
