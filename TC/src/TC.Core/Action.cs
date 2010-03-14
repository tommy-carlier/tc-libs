// TC Core Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC
{
	/// <summary>Encapsulates a method that takes no arguments and does not return a value.</summary>
	public delegate void Action();

	/// <summary>Encapsulates a method that takes 2 arguments and does not return a value.</summary>
	/// <param name="arg1">The first argument.</param>
	/// <param name="arg2">The second argument.</param>
	/// <typeparam name="TArg1">The type of the first argument.</typeparam>
	/// <typeparam name="TArg2">The type of the second argument.</typeparam>
	public delegate void Action<TArg1, TArg2>(TArg1 arg1, TArg2 arg2);
}
