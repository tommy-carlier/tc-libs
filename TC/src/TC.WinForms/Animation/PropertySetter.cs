// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.WinForms.Animation
{
	/// <summary>Represents a method that sets the value of a property.</summary>
	/// <typeparam name="TTarget">The type of the object to set the property value of.</typeparam>
	/// <typeparam name="TValue">The type of the property value.</typeparam>
	public delegate void PropertySetter<TTarget, TValue>(TTarget target, TValue value);
}
