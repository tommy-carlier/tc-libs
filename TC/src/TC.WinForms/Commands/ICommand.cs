// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.WinForms.Commands
{
	/// <summary>Represents a command that can be executed.</summary>
	public interface ICommand
	{
		/// <summary>Executes this command.</summary>
		void Execute();

		/// <summary>Gets a value indicating whether this command can be executed.</summary>
		/// <value><c>true</c> if this command can be executed; otherwise, <c>false</c>.</value>
		bool CanExecute { get; }

		/// <summary>Occurs when the value of the <see cref="P:CanExecute"/> property has changed.</summary>
		event EventHandler CanExecuteChanged;
	}
}
