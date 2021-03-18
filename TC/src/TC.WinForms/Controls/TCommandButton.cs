// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using TC.WinForms.Commands;

namespace TC.WinForms.Controls
{
    /// <summary>Represents a <see cref="T:Button"/> control that can execute a <see cref="T:ICommand"/>.</summary>
    [DefaultProperty("Command")]
	public class TCommandButton : TButton, ICommandControl
	{
		/// <summary>Initializes a new instance of the <see cref="TCommandButton"/> class.</summary>
		public TCommandButton()
		{
			_commandBinding = new CommandBinding(this);
		}

		#region Command members

		private readonly CommandBinding _commandBinding;

		/// <summary>Gets or sets the <see cref="T:ICommand"/> that is executed when this button is clicked.</summary>
		/// <value>The <see cref="T:ICommand"/> that is executed when this button is clicked.</value>
		[Category("Behavior"), DefaultValue(null)]
		[Description("The command that is executed when this button is clicked.")]
		public ICommand Command
		{
			get
			{
				return _commandBinding.Command;
			}

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
			this.TriggerEvent(Events, _commandChanged, e);
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
	}
}
