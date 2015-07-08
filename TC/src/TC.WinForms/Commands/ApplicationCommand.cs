// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.ComponentModel;

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

		private static readonly object _executed = new object();

		/// <summary>Occurs when the command is executed.</summary>
		[Category("Action"), Description("Occurs when the command is executed.")]
		public event EventHandler Executed
		{
			add { Events.AddHandler(_executed, value); }
			remove { Events.RemoveHandler(_executed, value); }
		}

		/// <summary>Raises the <see cref="E:Executed"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnExecuted(EventArgs e)
		{
			EventHandler handler = Events[_executed] as EventHandler;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion
	}
}
