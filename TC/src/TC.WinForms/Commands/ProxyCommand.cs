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
	/// <summary>Represents a command with a pluggable implementation.</summary>
	[ToolboxItem(false)]
	public sealed class ProxyCommand : Component, ICommand
	{
		#region Implementation members

		private ICommand fImplementation;

		/// <summary>Gets or sets the <see cref="T:ICommand"/> that implements the functionality of this <see cref="T:ProxyCommand"/>.</summary>
		/// <value>The <see cref="T:ICommand"/> that implements the functionality of this <see cref="T:ProxyCommand"/>.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ICommand Implementation
		{
			get { return fImplementation; }
			set
			{
				if (fImplementation != value)
				{
					if (fImplementation != null)
						fImplementation.CanExecuteChanged -= HandlerImplementationCanExecuteChanged;

					if (value != null)
						value.CanExecuteChanged += HandlerImplementationCanExecuteChanged;

					fImplementation = value;
					RaiseCanExecuteChanged();
				}
			}
		}

		#endregion

		private void HandlerImplementationCanExecuteChanged(object sender, EventArgs e)
		{
			RaiseCanExecuteChanged();
		}

		private void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
				CanExecuteChanged(this, EventArgs.Empty);
		}

		#region ICommand Members

		/// <summary>Executes this command.</summary>
		public void Execute()
		{
			if (fImplementation != null && fImplementation.CanExecute)
				fImplementation.Execute();
		}

		/// <summary>Gets a value indicating whether this command can be executed.</summary>
		/// <value><c>true</c> if this command can be executed; otherwise, <c>false</c>.</value>
		public bool CanExecute
		{
			get { return fImplementation != null && fImplementation.CanExecute; }
		}

		/// <summary>Occurs when the value of the <see cref="P:CanExecute"/> property has changed.</summary>
		public event EventHandler CanExecuteChanged;

		#endregion

		/// <summary>Releases the unmanaged resources used by the <see cref="T:Component"/> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (fImplementation != null)
				fImplementation.CanExecuteChanged -= HandlerImplementationCanExecuteChanged;

			base.Dispose(disposing);
		}
	}
}
