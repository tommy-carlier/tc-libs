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
	/// <summary>Represents an application command that can be created in the Visual Forms Designer.</summary>
	[DefaultEvent("Executed")]
	public class ApplicationCommand : BaseCommand
	{
		/// <summary>Gets or sets a value indicating whether this command can be executed.</summary>
		/// <value><c>true</c> if this command can be executed; otherwise, <c>false</c>.</value>
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DefaultValue(true), Category("Behavior"), Description("Indicates whether this command can be executed.")]
		public override bool CanExecute
		{
			get { return base.CanExecute; }
			set { base.CanExecute = value; }
		}

		/// <summary>Executes this command.</summary>
		public override void Execute()
		{
			if (CanExecute)
				OnExecuted(EventArgs.Empty);
		}

		#region Executed event

		private static readonly object fEventExecuted = new object();

		/// <summary>Occurs when the command is executed.</summary>
		[Category("Action"), Description("Occurs when the command is executed.")]
		public event EventHandler Executed
		{
			add { Events.AddHandler(fEventExecuted, value); }
			remove { Events.RemoveHandler(fEventExecuted, value); }
		}

		/// <summary>Raises the <see cref="E:Executed"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnExecuted(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventExecuted] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, EventArgs.Empty);
		}

		#endregion
	}
}
