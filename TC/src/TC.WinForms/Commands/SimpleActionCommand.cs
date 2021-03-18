// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;

namespace TC.WinForms.Commands
{
	/// <summary>Represents a command that invokes a simple action.</summary>
	public class SimpleActionCommand : BaseCommand
	{
		/// <summary>Initializes a new instance of the <see cref="SimpleActionCommand"/> class.</summary>
		/// <param name="action">The action to invoke when this command is executed.</param>
		public SimpleActionCommand(Action action)
		{
            _action = action ?? throw new ArgumentNullException("action");
		}

		private readonly Action _action;

		/// <summary>Executes this command.</summary>
		public override void Execute()
		{
			if (CanExecute) _action();
		}
	}
}
