// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

namespace TC.WinForms.Animation
{
	/// <summary>Represents a method that sets the value of a property.</summary>
	/// <typeparam name="TTarget">The type of the object to set the property value of.</typeparam>
	/// <typeparam name="TValue">The type of the property value.</typeparam>
	/// <param name="target">The object to set the property value of.</param>
	/// <param name="value">The property value.</param>
	public delegate void PropertySetter<TTarget, TValue>(TTarget target, TValue value);
}
