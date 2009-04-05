// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TC.WinForms.Commands;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a <see cref="T:Button"/> control that can execute a <see cref="T:ICommand"/>.</summary>
	[DefaultProperty("Command"), ToolboxBitmap(typeof(Button))]
	public class TCommandButton : Button, ICommandControl
	{
		/// <summary>Initializes a new instance of the <see cref="TCommandButton"/> class.</summary>
		public TCommandButton()
		{
			fCommandBinding = new CommandBinding(this);
		}

		#region Command members

		private readonly CommandBinding fCommandBinding;

		/// <summary>Gets or sets the <see cref="T:ICommand"/> that is executed when this button is clicked.</summary>
		/// <value>The <see cref="T:ICommand"/> that is executed when this button is clicked.</value>
		[Category("Behavior"), DefaultValue(null)]
		[Description("The command that is executed when this button is clicked.")]
		public ICommand Command
		{
			get { return fCommandBinding.Command; }
			set
			{
				if (fCommandBinding.Command != value)
				{
					fCommandBinding.Command = value;
					OnCommandChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object fEventCommandChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:Command"/> property has changed.</summary>
		public event EventHandler CommandChanged
		{
			add { Events.AddHandler(fEventCommandChanged, value); }
			remove { Events.RemoveHandler(fEventCommandChanged, value); }
		}

		/// <summary>Raises the <see cref="E:CommandChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnCommandChanged(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventCommandChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		#endregion

		#region ICommandControl Members

		/// <summary>Notifies the control that the bound command is enabled or disabled.</summary>
		/// <param name="enabled">if set to <c>true</c> the control should be enabled, otherwise disabled.</param>
		void ICommandControl.SetCommandEnabled(bool enabled)
		{
			Enabled = enabled;
		}

		/// <summary>Occurs when the control is activated.</summary>
		/// <remarks>When this event occurs, the bound command will be executed.</remarks>
		event EventHandler ICommandControl.Activated
		{
			add { Click += value; }
			remove { Click -= value; }
		}

		#endregion

		#region shadowing Enabled: is now controlled by the bound Command

		/// <summary>Gets or sets a value indicating whether the control can respond to user interaction.</summary>
		/// <returns>true if the control can respond to user interaction; otherwise, false. The default is true.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public new bool Enabled
		{
			get { return base.Enabled; }
			set { base.Enabled = value; }
		}

		#endregion

		/// <summary>Raises the <see cref="E:BackColorChanged"/> event when the <see cref="P:BackColor"/> property value of the control's container changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnParentBackColorChanged(EventArgs e)
		{
			base.OnParentBackColorChanged(e);
			if (BackColor == SystemColors.Window)
				BackColor = SystemColors.Control;
		}

		/// <summary>Raises the <see cref="E:ForeColorChanged"/> event when the <see cref="P:ForeColor"/> property value of the control's container changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnParentForeColorChanged(EventArgs e)
		{
			base.OnParentForeColorChanged(e);
			if (ForeColor == SystemColors.WindowText)
				ForeColor = SystemColors.ControlText;
		}
	}
}
