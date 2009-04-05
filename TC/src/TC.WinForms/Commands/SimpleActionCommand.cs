﻿// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Commands
{
	/// <summary>Represents a command that invokes a simple action.</summary>
	public class SimpleActionCommand : BaseCommand
	{
		/// <summary>Initializes a new instance of the <see cref="SimpleActionCommand"/> class.</summary>
		/// <param name="action">The action to invoke when this command is executed.</param>
		public SimpleActionCommand(Action action)
		{
			if (action == null) throw new ArgumentNullException("action");
			fAction = action;
		}

		private readonly Action fAction;

		/// <summary>Executes this command.</summary>
		public override void Execute()
		{
			if (CanExecute) fAction();
		}
	}
}
