// TC Core Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

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
