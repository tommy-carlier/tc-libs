// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Dialogs
{
	/// <summary>Represents a dialog to display simple messages.</summary>
	public sealed class TMessageDialog : TDialog<TMessageDialogContentControl>
	{
		/// <summary>Prevents a default instance of the <see cref="TMessageDialog"/> class from being created.</summary>
		private TMessageDialog() { }

		private static DialogResult Show(
			Control owner, 
			SystemIcon sideImage, 
			string message, 
			params DialogResultButton[] buttons)
		{
			using (TMessageDialog dialog = new TMessageDialog())
			{
				dialog.ContentControl.SideImage = sideImage;
				dialog.ContentControl.Message = message;
				foreach (DialogResultButton button in buttons)
					if (button != null)
						dialog.ContentControl.DialogResultButtons.Add(button);

				return dialog.ShowDialog(owner);
			}
		}

		/// <summary>Shows an informational message dialog.</summary>
		/// <param name="owner">The control that will own the dialog.</param>
		/// <param name="message">The message to display.</param>
		public static void ShowInfo(Control owner, string message)
		{
			if (message == null) throw new ArgumentNullException("message");
			Show(owner, SystemIcon.Information, message, DialogResultButton.OK);
		}

		/// <summary>Shows an error message dialog.</summary>
		/// <param name="owner">The control that will own the dialog.</param>
		/// <param name="errorMessage">The error message to display.</param>
		public static void ShowError(Control owner, string errorMessage)
		{
			if (errorMessage == null) throw new ArgumentNullException("errorMessage");
			Show(owner, SystemIcon.Error, errorMessage, DialogResultButton.OK);
		}

		/// <summary>Shows an error message dialog.</summary>
		/// <param name="owner">The control that will own the dialog.</param>
		/// <param name="exception">The <see cref="T:Exception"/> to display the message of.</param>
		public static void ShowError(Control owner, Exception exception)
		{
			if (exception == null) throw new ArgumentNullException("exception");
			Show(owner, SystemIcon.Error, exception.Message, DialogResultButton.OK);
		}

		/// <summary>Shows a warning message dialog.</summary>
		/// <param name="owner">The control that will own the dialog.</param>
		/// <param name="warningMessage">The warning message to display.</param>
		public static void ShowWarning(Control owner, string warningMessage)
		{
			if (warningMessage == null) throw new ArgumentNullException("warningMessage");
			Show(owner, SystemIcon.Warning, warningMessage, DialogResultButton.OK);
		}

		/// <summary>Asks the user to confirm an action.</summary>
		/// <param name="owner">The control that will own the dialog.</param>
		/// <param name="question">The question to display.</param>
		/// <param name="confirmButtonCaption">The caption of the button to confirm.</param>
		/// <param name="dangerousAction">Indicates that the action to confirm is potentially dangerous
		/// and a warning icon should be displayed.</param>
		/// <returns>If the user pressed the confirm button, true; otherwise, false.</returns>
		public static bool AskToConfirm(
			Control owner,
			string question,
			string confirmButtonCaption,
			bool dangerousAction)
		{
			if (question == null) throw new ArgumentNullException("question");
			if (confirmButtonCaption == null) throw new ArgumentNullException("confirmButtonCaption");
			if (confirmButtonCaption.Length == 0) 
				throw new ArgumentException("confirmButtonCaption cannot be an empty string.", "confirmButtonCaption");

			return Show(
				owner,
				dangerousAction ? SystemIcon.Warning : SystemIcon.Question,
				question,
				new DialogResultButton(DialogResult.OK, confirmButtonCaption),
				DialogResultButton.Cancel) == DialogResult.OK;
		}

		/// <summary>Asks the user a question where the answer can be yes or no.</summary>
		/// <param name="owner">The control that will own the dialog.</param>
		/// <param name="question">The question to display.</param>
		/// <param name="yesButtonCaption">The caption of the Yes-button.</param>
		/// <param name="noButtonCaption">The caption of the No-button.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous 
		/// and a warning icon should be displayed.</param>
		/// <returns>If the user pressed the Yes-button, true; otherwise, false.</returns>
		public static bool AskYesNo(
			Control owner,
			string question,
			string yesButtonCaption,
			string noButtonCaption,
			bool dangerousAction)
		{
			if (question == null) throw new ArgumentNullException("question");
			if (yesButtonCaption == null) throw new ArgumentNullException("yesButtonCaption");
			if (noButtonCaption == null) throw new ArgumentNullException("noButtonCaption");
			if (yesButtonCaption.Length == 0)
				throw new ArgumentException("yesButtonCaption cannot be an empty string.", "yesButtonCaption");
			if (noButtonCaption.Length == 0)
				throw new ArgumentException("noButtonCaption cannot be an empty string.", "noButtonCaption");

			return Show(
				owner,
				dangerousAction ? SystemIcon.Warning : SystemIcon.Question,
				question,
				new DialogResultButton(DialogResult.Yes, yesButtonCaption),
				new DialogResultButton(DialogResult.No, noButtonCaption)) == DialogResult.Yes;
		}

		/// <summary>Asks the user a question where the answer can be yes or no.</summary>
		/// <param name="owner">The control that will own the dialog.</param>
		/// <param name="question">The question to display.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous 
		/// and a warning icon should be displayed.</param>
		/// <returns>If the user pressed the Yes-button, true; otherwise, false.</returns>
		public static bool AskYesNo(Control owner, string question, bool dangerousAction)
		{
			if (question == null) throw new ArgumentNullException("question");

			return Show(
				owner,
				dangerousAction ? SystemIcon.Warning : SystemIcon.Question,
				question,
				DialogResultButton.Yes,
				DialogResultButton.No) == DialogResult.Yes;
		}

		/// <summary>Asks the user a question where the answer can be yes, no or cancel.</summary>
		/// <param name="owner">The control that will own the dialog.</param>
		/// <param name="question">The question to display.</param>
		/// <param name="yesButtonCaption">The caption of the Yes-button.</param>
		/// <param name="noButtonCaption">The caption of the No-button.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous
		/// and a warning icon should be displayed.</param>
		/// <returns>The dialog result.</returns>
		public static DialogResult AskYesNoCancel(
			Control owner,
			string question,
			string yesButtonCaption, 
			string noButtonCaption,
			bool dangerousAction)
		{
			if (question == null) throw new ArgumentNullException("question");
			if (yesButtonCaption == null) throw new ArgumentNullException("yesButtonCaption");
			if (noButtonCaption == null) throw new ArgumentNullException("noButtonCaption");
			if (yesButtonCaption.Length == 0)
				throw new ArgumentException("yesButtonCaption cannot be an empty string.", "yesButtonCaption");
			if (noButtonCaption.Length == 0)
				throw new ArgumentException("noButtonCaption cannot be an empty string.", "noButtonCaption");

			return Show(
				owner,
				dangerousAction ? SystemIcon.Warning : SystemIcon.Question,
				question,
				new DialogResultButton(DialogResult.Yes, yesButtonCaption),
				new DialogResultButton(DialogResult.No, noButtonCaption),
				DialogResultButton.Cancel);
		}

		/// <summary>Asks the user a question where the answer can be yes, no or cancel.</summary>
		/// <param name="owner">The control that will own the dialog.</param>
		/// <param name="question">The question to display.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous
		/// and a warning icon should be displayed.</param>
		/// <returns>The dialog result.</returns>
		public static DialogResult AskYesNoCancel(Control owner, string question, bool dangerousAction)
		{
			if (question == null) throw new ArgumentNullException("question");

			return Show(
				owner,
				dangerousAction ? SystemIcon.Warning : SystemIcon.Question,
				question,
				DialogResultButton.Yes,
				DialogResultButton.No,
				DialogResultButton.Cancel);
		}
	}
}
