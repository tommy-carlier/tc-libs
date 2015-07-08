// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

using TC.WinForms.Commands;
using TC.WinForms.Controls;
using TC.WinForms.Dialogs;
using TC.WinForms.Settings;

using SWF = System.Windows.Forms;

namespace TC.WinForms.Forms
{
	/// <summary>Represents a window or dialog box.</summary>
	public class TForm : Form, IHasSystemFont
	{
		/// <summary>Initializes a new instance of the <see cref="T:TForm"/> class.</summary>
		public TForm()
		{
			Font = SystemFont.Default.ToFont();
			Icon = TApplication.Icon;
			StartPosition = FormStartPosition.Manual;

			_closeCommand = new SimpleActionCommand(Close);
			_activateCommand = new SimpleActionCommand(Activate);
		}

		#region Commands

		private readonly Collection<ApplicationCommand> _commands = new Collection<ApplicationCommand>();

		/// <summary>Gets the commands of this <see cref="T:TForm"/>.</summary>
		/// <value>The commands of this <see cref="T:TForm"/>.</value>
		/// <remarks>This property enables you to add commands in the Visual Forms Designer.</remarks>
		[Category("Behavior"), Description("The commands of this form.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Collection<ApplicationCommand> Commands { get { return _commands; } }

		/// <summary>Gets the command to display information about the current application.</summary>
		/// <value>The command to display information about the current application.</value>
		[SuppressMessage(
			"Microsoft.Performance",
			"CA1822:MarkMembersAsStatic",
			Justification = "This property is only used to make the TApplication.Current.AboutCommand available in the visual designer.")]
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand ApplicationAboutCommand { get { return TApplication.Current.AboutCommand; } }

		private readonly ICommand _closeCommand;

		/// <summary>Gets the command to close this form.</summary>
		/// <value>The command to close this form.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand CloseCommand { get { return _closeCommand; } }

		private readonly ICommand _activateCommand;

		/// <summary>Gets the command to activate this form.</summary>
		/// <value>The command to activate this form.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand ActivateCommand { get { return _activateCommand; } }

		#endregion

		#region property Application

		/// <summary>Gets the current application.</summary>
		/// <value>The current application.</value>
		[Browsable(false)]
		public static TApplication Application { get { return TApplication.Current; } }

		#endregion

		#region property FirstFocusControl

		/// <summary>Gets or sets the control that gets focus the first time this form is shown.</summary>
		/// <value>The control that gets focus the first time this form is shown.</value>
		[DefaultValue(null), Category("Behavior")]
		[Description("The control that gets focus the first time this form is shown.")]
		public Control FirstFocusControl { get; set; }

		#endregion

		/// <summary>Raises the <see cref="E:Load"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			InitializeDialogSideImageControls();
			SetInitialBounds();
			LoadSettings();
			AdjustBoundsToNotOverlapExistingForms();
		}

		private void InitializeDialogSideImageControls()
		{
			foreach (var control in this.EnumerateDescendants<TSystemIconBox>(false))
				control.InitializeImage();
		}

		private void SetInitialBounds()
		{
			Form owner = Owner;
			if (owner == null && this != Application.MainForm)
				owner = Application.MainForm;

			Rectangle ownerBounds
				= owner != null
				&& owner.Visible
				&& owner.WindowState != FormWindowState.Minimized
					? owner.Bounds
					: Screen.PrimaryScreen.WorkingArea;

			Rectangle formBounds = Bounds;
			formBounds.X = ownerBounds.X + ((ownerBounds.Width - formBounds.Width) / 2);
			formBounds.Y = (int)Math.Round(
				ownerBounds.Y + ((ownerBounds.Height - formBounds.Height) * _heightFactor));

			Bounds = formBounds.AdjustBoundsToWorkingArea();
		}

		private void AdjustBoundsToNotOverlapExistingForms()
		{
			Rectangle bounds = Bounds;
			int captionHeight = SystemInformation.CaptionHeight;
			Rectangle screenBounds = Screen.FromPoint(bounds.Location).WorkingArea;
			int minX = screenBounds.X;
			int minY = screenBounds.Y;
			int deltaX = captionHeight - minX;
			int deltaY = captionHeight - minY;
			int moduloX = screenBounds.Width - bounds.Width;
			int moduloY = screenBounds.Height - bounds.Height;

			FormCollection openForms = SWF.Application.OpenForms;
			for (int i = openForms.Count - 1; i >= 0; i--)
			{
				bool checkAgain = false;

				foreach (Form openForm in openForms)
					if (openForm != this
						&& openForm.Visible
						&& openForm.Location == bounds.Location)
					{
						bounds.X = minX + ((bounds.X + deltaX) % moduloX);
						bounds.Y = minY + ((bounds.Y + deltaY) % moduloY);
						checkAgain = true;
						break;
					}

				if (!checkAgain) break;
			}

			Location = bounds.Location;
		}

		// height factor = 1 / (1 + golden ratio); golden ratio = (1 + SQRT(5)) / 2
		private static readonly double _heightFactor = 2.0 / (3.0 + Math.Sqrt(5.0));

		/// <summary>Raises the <see cref="E:Shown"/> event.</summary>
		/// <param name="e">A <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (FirstFocusControl != null && FirstFocusControl.CanSelect)
				FirstFocusControl.Select();
			else
			{
				foreach (Control control in this.EnumerateDescendants<Control>(false))
					if (control.CanSelect)
					{
						control.Select();
						break;
					}
			}
		}

		/// <summary>Raises the <see cref="E:Move"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);
			SaveSettings();
		}

		/// <summary>Raises the <see cref="E:Resize"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			SaveSettings();
		}

		/// <summary>Raises the <see cref="E:Closed"/> event.</summary>
		/// <param name="e">The <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			SaveSettings();
			ExitApplicationWhenLastWindowIsClosed();
		}

		private void ExitApplicationWhenLastWindowIsClosed()
		{
			FormCollection openForms = SWF.Application.OpenForms;
			if (openForms.Count == 0 || (openForms.Count == 1 && openForms[0] == this))
				SWF.Application.Exit();
		}

		#region IHasSystemFont Members

		/// <summary>Gets the <see cref="T:SystemFont"/> of the control.</summary>
		/// <value>The <see cref="T:SystemFont"/> of the control.</value>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual SystemFont SystemFont { get { return SystemFont.Default; } }

		/// <summary>Gets or sets the font of the text displayed by the control.</summary>
		/// <returns>The <see cref="T:Font"/> to apply to the text displayed by the control.</returns>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Font Font
		{
			get { return base.Font; }
			set { base.Font = value; }
		}

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
		/// <param name="dangerousAction">Indicates that the action to confirm is potentially dangerous
		/// and a warning icon should be displayed.</param>
		/// <returns>If the user pressed the confirm button, true; otherwise, false.</returns>
		protected bool AskToConfirm(string question, string confirmButtonCaption, bool dangerousAction)
		{
			return TMessageDialog.AskToConfirm(this, question, confirmButtonCaption, dangerousAction);
		}

		/// <summary>Asks the user a question where the answer can be yes or no.</summary>
		/// <param name="question">The question to display.</param>
		/// <param name="yesButtonCaption">The caption of the Yes-button.</param>
		/// <param name="noButtonCaption">The caption of the No-button.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous 
		/// and a warning icon should be displayed.</param>
		/// <returns>If the user pressed the Yes-button, true; otherwise, false.</returns>
		protected bool AskYesNo(
			string question,
			string yesButtonCaption,
			string noButtonCaption,
			bool dangerousAction)
		{
			return TMessageDialog.AskYesNo(
				this,
				question, 
				yesButtonCaption, 
				noButtonCaption, 
				dangerousAction);
		}

		/// <summary>Asks the user a question where the answer can be yes or no.</summary>
		/// <param name="question">The question to display.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous
		/// and a warning icon should be displayed.</param>
		/// <returns>If the user pressed the Yes-button, true; otherwise, false.</returns>
		protected bool AskYesNo(string question, bool dangerousAction)
		{
			return TMessageDialog.AskYesNo(this, question, dangerousAction);
		}

		/// <summary>Asks the user a question where the answer can be yes, no or cancel.</summary>
		/// <param name="question">The question to display.</param>
		/// <param name="yesButtonCaption">The caption of the Yes-button.</param>
		/// <param name="noButtonCaption">The caption of the No-button.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous
		/// and a warning icon should be displayed.</param>
		/// <returns>The dialog result.</returns>
		protected DialogResult AskYesNoCancel(
			string question,
			string yesButtonCaption,
			string noButtonCaption,
			bool dangerousAction)
		{
			return TMessageDialog.AskYesNoCancel(
				this,
				question,
				yesButtonCaption, 
				noButtonCaption, 
				dangerousAction);
		}

		/// <summary>Asks the user a question where the answer can be yes, no or cancel.</summary>
		/// <param name="question">The question to display.</param>
		/// <param name="dangerousAction">Indicates that the action is potentially dangerous
		/// and a warning icon should be displayed.</param>
		/// <returns>The dialog result.</returns>
		protected DialogResult AskYesNoCancel(string question, bool dangerousAction)
		{
			return TMessageDialog.AskYesNoCancel(this, question, dangerousAction);
		}

		#endregion

		#region loading and saving settings

		private bool _settingsLoaded;

		/// <summary>Loads the settings of this form.</summary>
		public void LoadSettings()
		{
			LoadSettingsCore();
			foreach (var userControl in this.EnumerateDescendants<TUserControl>(false))
				userControl.LoadSettings();
			_settingsLoaded = true;
		}

		/// <summary>When overriden in a derived class, loads the settings of this form.</summary>
		protected virtual void LoadSettingsCore()
		{
		}

		/// <summary>Loads the base form settings.</summary>
		/// <param name="settings">The settings to load.</param>
		protected void LoadBaseFormSettings(BaseFormSettings settings)
		{
			if (settings == null) throw new ArgumentNullException("settings");

			Rectangle bounds = Bounds;
			if (settings.X >= 0) bounds.X = settings.X;
			if (settings.Y >= 0) bounds.Y = settings.Y;
			if (settings.Width >= 0) bounds.Width = settings.Width;
			if (settings.Height >= 0) bounds.Height = settings.Height;
			Bounds = bounds.AdjustBoundsToWorkingArea();

			WindowState = settings.IsMaximized ? FormWindowState.Maximized : FormWindowState.Normal;
		}

		/// <summary>Saves the settings of this form.</summary>
		public void SaveSettings()
		{
			if (_settingsLoaded)
			{
				SaveSettingsCore();
				foreach (var userControl in this.EnumerateDescendants<TUserControl>(false))
					userControl.SaveSettings();
			}
		}

		/// <summary>When overriden in a derived class, saves the settings of this form.</summary>
		protected virtual void SaveSettingsCore()
		{
		}

		/// <summary>Saves the base form settings.</summary>
		/// <param name="settings">The settings to save.</param>
		protected void SaveBaseFormSettings(BaseFormSettings settings)
		{
			if (settings == null) throw new ArgumentNullException("settings");

			switch (WindowState)
			{
				case FormWindowState.Normal:
					Rectangle bounds = Bounds;
					settings.X = bounds.X;
					settings.Y = bounds.Y;
					settings.Width = bounds.Width;
					settings.Height = bounds.Height;
					settings.IsMaximized = false;
					break;
				case FormWindowState.Maximized:
					settings.IsMaximized = true;
					break;
			}
		}

		#endregion
	}
}
