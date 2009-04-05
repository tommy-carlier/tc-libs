// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.WinForms.Commands
{
	/// <summary>Represents an object that implements all or some of the <see cref="T:TCommandIdentifier">command identifiers</see>.</summary>
	/// <typeparam name="TCommandIdentifier">The type of the command identifiers to implement.</typeparam>
	public interface IProxyCommandsImplementation<TCommandIdentifier>
	{
		/// <summary>Gets the <see cref="T:ICommand"/> that implements the specified <see cref="T:TCommandIdentifier"/>.</summary>
		/// <param name="identifier">The <see cref="T:TCommandIdentifier"/> to get the <see cref="T:ICommand"/> of.</param>
		/// <returns>The <see cref="T:ICommand"/> that implements the specified <see cref="T:TCommandIdentifier"/>.</returns>
		ICommand GetCommand(TCommandIdentifier identifier);
	}
}
