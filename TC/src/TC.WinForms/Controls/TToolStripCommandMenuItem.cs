// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TC.WinForms.Commands;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a <see cref="T:ToolStripMenuItem"/> that can execute a command.</summary>
	[SuppressMessage(
		"Microsoft.Design",
		"CA1063:ImplementIDisposableCorrectly",
		Justification = "IDisposable is already implemented.")]
	[ToolboxBitmap(typeof(ToolStripMenuItem))]
	public class TToolStripCommandMenuItem : ToolStripMenuItem, ICommandControl
	{
		/// <summary>Initializes a new instance of the <see cref="TToolStripCommandMenuItem"/> class.</summary>
		public TToolStripCommandMenuItem()
		{
			_commandBinding = new CommandBinding(this);
		}

		#region Command members

		private readonly CommandBinding _commandBinding;

		/// <summary>Gets or sets the <see cref="T:ICommand"/> that is executed when this menu item is clicked.</summary>
		/// <value>The <see cref="T:ICommand"/> that is executed when this menu item is clicked.</value>
		[Category("Behavior"), Description("The command that is executed when this menu item is clicked."), DefaultValue(null)]
		public ICommand Command
		{
			get { return _commandBinding.Command; }
			set
			{
				if (_commandBinding.Command != value)
				{
					_commandBinding.Command = value;
					OnCommandChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _commandChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:Command"/> property has changed.</summary>
		public event EventHandler CommandChanged
		{
			add { Events.AddHandler(_commandChanged, value); }
			remove { Events.RemoveHandler(_commandChanged, value); }
		}

		/// <summary>Raises the <see cref="E:CommandChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnCommandChanged(EventArgs e)
		{
			EventHandler handler = Events[_commandChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region ICommandControl Members

		/// <summary>Notifies the control that the bound command is enabled or disabled.</summary>
		/// <param name="enabled">if set to <c>true</c> the control should be enabled, otherwise disabled.</param>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "This method is called by CommandBinding and should never be called by derived classes.")]
		void ICommandControl.SetCommandEnabled(bool enabled)
		{
			Enabled = enabled;
		}

		/// <summary>Occurs when the control is activated.</summary>
		/// <remarks>When this event occurs, the bound command will be executed.</remarks>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "This event is used by CommandBinding and is a synonym of the Click-event.")]
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
	}
}
