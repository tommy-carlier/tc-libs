// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

namespace TC.WinForms.Animation
{
	/// <summary>Represents a function that calculates intermediate value between 2 values.</summary>
	/// <typeparam name="T">The type of values to interpolate.</typeparam>
	/// <param name="startValue">The start-value.</param>
	/// <param name="endValue">The end-value.</param>
	/// <param name="progress">The interpolation progress: a <see cref="T:Int64"/> between 0 and maximum.</param>
	/// <param name="maximum">The maximum value of progress.</param>
	/// <returns>The calculated intermediate value.</returns>
	public delegate T Interpolator<T>(T startValue, T endValue, long progress, long maximum);
}
