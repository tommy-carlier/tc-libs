// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

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

		private bool fCanExecute = true;

		/// <summary>Gets or sets a value indicating whether this command can be executed.</summary>
		/// <value><c>true</c> if this command can be executed; otherwise, <c>false</c>.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool CanExecute
		{
			get { return fCanExecute; }
			set
			{
				if (fCanExecute != value)
				{
					fCanExecute = value;
					OnCanExecuteChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object fEventCanExecuteChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:CanExecute"/> property has changed.</summary>
		public event EventHandler CanExecuteChanged
		{
			add { Events.AddHandler(fEventCanExecuteChanged, value); }
			remove { Events.RemoveHandler(fEventCanExecuteChanged, value); }
		}

		/// <summary>Raises the <see cref="E:CanExecuteChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnCanExecuteChanged(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventCanExecuteChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		#endregion
	}
}
