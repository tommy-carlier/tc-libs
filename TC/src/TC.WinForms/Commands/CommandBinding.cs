﻿// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;

namespace TC.WinForms.Commands
{
	/// <summary>Manages the binding between a <see cref="T:ICommand"/> and a <see cref="T:ICommandControl"/>.</summary>
	public sealed class CommandBinding
	{
		/// <summary>Initializes a new instance of the <see cref="T:CommandBinding"/> class.</summary>
		/// <param name="control">The <see cref="T:ICommandControl"/> that can be bound to a <see cref="T:ICommand"/>.</param>
		public CommandBinding(ICommandControl control)
		{
            _control = control ?? throw new ArgumentNullException("control");
			control.Activated += HandlerControlActivated;
			control.Disposed += HandlerControlDisposed;
			control.SetCommandEnabled(false);
		}

		private readonly ICommandControl _control;
		private ICommand _command;

		/// <summary>Gets or sets the <see cref="T:ICommand"/> to bind to the <see cref="T:ICommandControl"/>
		/// of this <see cref="T:CommandBinding"/>.</summary>
		/// <value>The <see cref="T:ICommand"/> to bind to the <see cref="T:ICommandControl"/> 
		/// of this <see cref="T:CommandBinding"/>.</value>
		public ICommand Command
		{
			get
			{
				return _command;
			}

			set
			{
				if (_command != value)
				{
					if (_command != null)
						_command.CanExecuteChanged -= HandlerCommandCanExecuteChanged;

					if (value != null)
						value.CanExecuteChanged += HandlerCommandCanExecuteChanged;

					_command = value;
					_control.SetCommandEnabled(CanExecute);
				}
			}
		}

		private bool CanExecute
		{
			get { return _command != null && _command.CanExecute; }
		}

		private void HandlerCommandCanExecuteChanged(object sender, EventArgs e)
		{
			_control.SetCommandEnabled(CanExecute);
		}

		private void HandlerControlActivated(object sender, EventArgs e)
		{
			if (CanExecute)
				_command.Execute();
		}

		private void HandlerControlDisposed(object sender, EventArgs e)
		{
			_control.Activated -= HandlerControlActivated;
			_control.Disposed -= HandlerControlDisposed;

			if (_command != null)
			{
				_command.CanExecuteChanged -= HandlerCommandCanExecuteChanged;
				_command = null;
			}
		}
	}
}
