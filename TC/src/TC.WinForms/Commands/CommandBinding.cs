// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.WinForms.Commands
{
	/// <summary>Manages the binding between a <see cref="T:ICommand"/> and a <see cref="T:ICommandControl"/>.</summary>
	public sealed class CommandBinding
	{
		/// <summary>Initializes a new instance of the <see cref="T:CommandBinding"/> class.</summary>
		/// <param name="control">The <see cref="T:ICommandControl"/> that can be bound to a <see cref="T:ICommand"/>.</param>
		public CommandBinding(ICommandControl control)
		{
			if (control == null) throw new ArgumentNullException("control");

			fControl = control;
			control.Activated += HandlerControlActivated;
			control.Disposed += HandlerControlDisposed;
			control.SetCommandEnabled(false);
		}

		private readonly ICommandControl fControl;
		private ICommand fCommand;

		/// <summary>Gets or sets the <see cref="T:ICommand"/> to bind to the <see cref="T:ICommandControl"/> of this <see cref="T:CommandBinding"/>.</summary>
		/// <value>The <see cref="T:ICommand"/> to bind to the <see cref="T:ICommandControl"/> of this <see cref="T:CommandBinding"/>.</value>
		public ICommand Command
		{
			get { return fCommand; }
			set
			{
				if (fCommand != value)
				{
					if (fCommand != null) fCommand.CanExecuteChanged -= HandlerCommandCanExecuteChanged;
					if (value != null) value.CanExecuteChanged += HandlerCommandCanExecuteChanged;

					fCommand = value;
					fControl.SetCommandEnabled(CanExecute);
				}
			}
		}

		private bool CanExecute { get { return fCommand != null && fCommand.CanExecute; } }

		private void HandlerCommandCanExecuteChanged(object sender, EventArgs e)
		{
			fControl.SetCommandEnabled(CanExecute);
		}

		private void HandlerControlActivated(object sender, EventArgs e)
		{
			if (CanExecute)
				fCommand.Execute();
		}

		private void HandlerControlDisposed(object sender, EventArgs e)
		{
			fControl.Activated -= HandlerControlActivated;
			fControl.Disposed -= HandlerControlDisposed;
			if (fCommand != null)
			{
				fCommand.CanExecuteChanged -= HandlerCommandCanExecuteChanged;
				fCommand = null;
			}
		}
	}
}
