// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Forms;

using TC.WinForms.Commands;
using TC.WinForms.Dialogs;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a container of other controls, to be used as a part of a Form.</summary>
	[ToolboxItem(false)]
	public class TUserControl : UserControl
	{
		#region Commands

		private readonly Collection<ApplicationCommand>
			fCommands = new Collection<ApplicationCommand>();

		/// <summary>Gets the commands of this <see cref="T:TUserControl"/>.</summary>
		/// <value>The commands of this <see cref="T:TUserControl"/>.</value>
		/// <remarks>This property enables you to add commands in the Visual Forms Designer.</remarks>
		[Category("Behavior"), Description("The commands of this control.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Collection<ApplicationCommand> Commands { get { return fCommands; } }

		/// <summary>Gets the command to display information about the current application.</summary>
		/// <value>The command to display information about the current application.</value>
		[SuppressMessage(
			"Microsoft.Performance",
			"CA1822:MarkMembersAsStatic",
			Justification = "This property is only used to make the TApplication.Current.AboutCommand available in the visual designer.")]
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand ApplicationAboutCommand { get { return TApplication.Current.AboutCommand; } }

		#endregion

		#region property Application

		/// <summary>Gets the current application.</summary>
		/// <value>The current application.</value>
		[Browsable(false)]
		public static TApplication Application { get { return TApplication.Current; } }

		#endregion

		#region message dialog methods

		/// <summary>Shows an informational message dialog.</summary>
		/// <param name="message">The message to display.</param>
		protected void ShowInfo(string message)
		{
			TMessageDialog.ShowInfo(this, message);
		}

		/// <summary>Shows an error message dialog.</summary>
		/// <param name="errorMessage">The error message to display.</param>
		protected void ShowError(string errorMessage)
		{
			TMessageDialog.ShowError(this, errorMessage);
		}

		/// <summary>Shows an error message dialog.</summary>
		/// <param name="exception">The <see cref="T:Exception"/> to display the message of.</param>
		protected void ShowError(Exception exception)
		{
			TMessageDialog.ShowError(this, exception);
		}

		/// <summary>Shows a warning message dialog.</summary>
		/// <param name="warningMessage">The warning message to display.</param>
		protected void ShowWarning(string warningMessage)
		{
			TMessageDialog.ShowWarning(this, warningMessage);
		}

		/// <summary>Asks the user to confirm an action.</summary>
		/// <param name="question">The question to display.</param>
		/// <param name="confirmButtonCaption">The caption of the button to confirm.</param>
		/// <param name="dangerousAction">Indicates that the action to confirm is potentially dangerous and a warning icon should be displayed.</param>
		/// <returns>If the user pressed the confirm button, true; otherwise, false.</returns>
		protected bool AskToConfirm(string question, string confirmButtonCaption, bool dangerousAction)
		{
			return TMessageDialog.AskToConfirm(this, question, confirmButtonCaption, dangerousAction);
		}

		/// <summary>Asks the user a question where the answer can be yes or no.</summary>
		/// <param name="question">The question to display.</param>
		/// <param name="yesButtonCaption">The caption of the Yes-button.</param>
		/// <param name="noButtonCaption">The caption of the No-button.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous and a warning icon should be displayed.</param>
		/// <returns>If the user pressed the Yes-button, true; otherwise, false.</returns>
		protected bool AskYesNo(string question, string yesButtonCaption, string noButtonCaption, bool dangerousAction)
		{
			return TMessageDialog.AskYesNo(this, question, yesButtonCaption, noButtonCaption, dangerousAction);
		}

		/// <summary>Asks the user a question where the answer can be yes or no.</summary>
		/// <param name="question">The question to display.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous and a warning icon should be displayed.</param>
		/// <returns>If the user pressed the Yes-button, true; otherwise, false.</returns>
		protected bool AskYesNo(string question, bool dangerousAction)
		{
			return TMessageDialog.AskYesNo(this, question, dangerousAction);
		}

		/// <summary>Asks the user a question where the answer can be yes, no or cancel.</summary>
		/// <param name="question">The question to display.</param>
		/// <param name="yesButtonCaption">The caption of the Yes-button.</param>
		/// <param name="noButtonCaption">The caption of the No-button.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous and a warning icon should be displayed.</param>
		/// <returns>The dialog result.</returns>
		protected DialogResult AskYesNoCancel(string question, string yesButtonCaption, string noButtonCaption, bool dangerousAction)
		{
			return TMessageDialog.AskYesNoCancel(this, question, yesButtonCaption, noButtonCaption, dangerousAction);
		}

		/// <summary>Asks the user a question where the answer can be yes, no or cancel.</summary>
		/// <param name="question">The question to display.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous and a warning icon should be displayed.</param>
		/// <returns>The dialog result.</returns>
		protected DialogResult AskYesNoCancel(string question, bool dangerousAction)
		{
			return TMessageDialog.AskYesNoCancel(this, question, dangerousAction);
		}

		#endregion

		#region loading and saving settings

		/// <summary>Loads the settings of this control.</summary>
		internal void LoadSettings()
		{
			// LoadSettings is called by TForm.LoadSettings
			LoadSettingsCore();
		}

		/// <summary>When overriden in a derived class, loads the settings of this control.</summary>
		protected virtual void LoadSettingsCore()
		{
		}

		/// <summary>Saves the settings of this control.</summary>
		internal void SaveSettings()
		{
			// SaveSettings is called by TForm.SaveSettings
			SaveSettingsCore();
		}

		/// <summary>When overriden in a derived class, saves the settings of this control.</summary>
		protected virtual void SaveSettingsCore()
		{
		}

		#endregion
	}
}
