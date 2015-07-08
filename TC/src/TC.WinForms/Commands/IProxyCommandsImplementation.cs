// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.WinForms.Commands
{
	/// <summary>Represents an object that implements all or some of the
	/// <see cref="T:TCommandIdentifier">command identifiers</see>.</summary>
	/// <typeparam name="TCommandIdentifier">The type of the command identifiers to implement.</typeparam>
	public interface IProxyCommandsImplementation<TCommandIdentifier>
	{
		/// <summary>Gets the <see cref="T:ICommand"/> that implements the specified <see cref="T:TCommandIdentifier"/>.</summary>
		/// <param name="identifier">The <see cref="T:TCommandIdentifier"/> to get the <see cref="T:ICommand"/> of.</param>
		/// <returns>The <see cref="T:ICommand"/> that implements the specified <see cref="T:TCommandIdentifier"/>.</returns>
		ICommand GetCommand(TCommandIdentifier identifier);
	}
}
