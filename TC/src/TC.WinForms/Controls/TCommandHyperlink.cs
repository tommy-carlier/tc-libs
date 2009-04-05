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
	/// <summary>Represents a hyperlink that can execute a <see cref="T:ICommand"/>.</summary>
	[SuppressMessage(
		"Microsoft.Design",
		"CA1063:ImplementIDisposableCorrectly",
		Justification = "IDisposable is already implemented.")]
	[DefaultProperty("Command"), ToolboxBitmap(typeof(LinkLabel))]
	public class TCommandHyperlink : LinkLabel, ICommandControl
	{
		/// <summary>Initializes a new instance of the <see cref="TCommandHyperlink"/> class.</summary>
		public TCommandHyperlink()
		{
			fCommandBinding = new CommandBinding(this);
		}

		#region Command members

		private readonly CommandBinding fCommandBinding;

		/// <summary>Gets or sets the <see cref="T:ICommand"/> that is executed when this hyperlink is clicked.</summary>
		/// <value>The <see cref="T:ICommand"/> that is executed when this hyperlink is clicked.</value>
		[Category("Behavior"), DefaultValue(null)]
		[Description("The command that is executed when this hyperlink is clicked.")]
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
		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "This method is called by CommandBinding and should never be called by derived classes.")]
		void ICommandControl.SetCommandEnabled(bool enabled)
		{
			Enabled = enabled;
		}

		private static readonly object fEventActivated = new object();

		/// <summary>Occurs when the control is activated.</summary>
		/// <remarks>When this event occurs, the bound command will be executed.</remarks>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "This event is used by CommandBinding and is almost a synonym of the LinkClicked-event.")]
		event EventHandler ICommandControl.Activated
		{
			add { Events.AddHandler(fEventActivated, value); }
			remove { Events.RemoveHandler(fEventActivated, value); }
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

		/// <summary>Raises the <see cref="E:LinkClicked"/> event.</summary>
		/// <param name="e">A <see cref="T:LinkLabelLinkClickedEventArgs"/> that contains the event data.</param>
		protected override void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
		{
			base.OnLinkClicked(e);

			if (e.Button == MouseButtons.Left)
			{
				EventHandler lEventHandler = Events[fEventActivated] as EventHandler;
				if (lEventHandler != null)
					lEventHandler(this, EventArgs.Empty);
			}
		}
	}
}
