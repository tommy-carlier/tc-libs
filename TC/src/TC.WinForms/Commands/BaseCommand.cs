// TC WinForms Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TC.WinForms.Commands
{
	/// <summary>The abstract base implementation of <see cref="T:ICommand"/>.</summary>
	[ToolboxItem(false)]
	public abstract class BaseCommand : Component, ICommand
	{
		#region ICommand Members

		/// <summary>Executes this command.</summary>
		public abstract void Execute();

		private bool _canExecute = true;

		/// <summary>Gets or sets a value indicating whether this command can be executed.</summary>
		/// <value><c>true</c> if this command can be executed; otherwise, <c>false</c>.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool CanExecute
		{
			get
			{
				return _canExecute;
			}

			set
			{
				if (_canExecute != value)
				{
					_canExecute = value;
					OnCanExecuteChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _canExecuteChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:CanExecute"/> property has changed.</summary>
		public event EventHandler CanExecuteChanged
		{
			add { Events.AddHandler(_canExecuteChanged, value); }
			remove { Events.RemoveHandler(_canExecuteChanged, value); }
		}

		/// <summary>Raises the <see cref="E:CanExecuteChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnCanExecuteChanged(EventArgs e)
		{
			EventHandler handler = Events[_canExecuteChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion
	}
}
