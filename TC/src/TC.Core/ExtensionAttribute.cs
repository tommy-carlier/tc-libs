// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tccore
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tccore/license

using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices
{
	/// <summary>Annotates extension methods.</summary>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class ExtensionAttribute : Attribute
	{
		// This class is present to add extension method capabilities to this
		// .NET 2.0 project.

		// see http://www.danielmoth.com/Blog/2007/05/using-extension-methods-in-fx-20.html
	}
}
