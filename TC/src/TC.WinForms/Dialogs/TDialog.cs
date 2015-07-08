// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TC.WinForms.Forms;

namespace TC.WinForms.Dialogs
{
	/// <summary>Represents a dialog box.</summary>
	/// <typeparam name="TContentControl">The type of the content control.</typeparam>
	public class TDialog<TContentControl> : TForm where TContentControl : TDialogContentControl, new()
	{
		/// <summary>Initializes a new instance of the <see cref="TDialog{TContentControl}"/> class.</summary>
		public TDialog()
		{
			InitializeControls();
		}

		#region InitializeControls

		private void InitializeControls()
		{
			FlowLayoutPanel panelBottom = new FlowLayoutPanel();

			// SuspendLayout
			SuspendLayout();
			_contentControl.SuspendLayout();
			panelBottom.SuspendLayout();

			// fContentControl
			_contentControl.Dock = DockStyle.Fill;
			_contentControl.TabIndex = 0;
			Controls.Add(_contentControl);

			// lPanelBottom
			panelBottom.FlowDirection = FlowDirection.RightToLeft;
			panelBottom.WrapContents = false;
			panelBottom.AutoSize = true;
			panelBottom.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			panelBottom.Dock = DockStyle.Bottom;
			panelBottom.Padding = new Padding(3, 6, 3, 6);
			panelBottom.TabIndex = 1;
			Controls.Add(panelBottom);

			// TDialog
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			ControlBox = CancelButton != null;
			Text = _contentControl.Text;

			// move the dialog off-screen before displaying it and setting the final location and size
			Rectangle bounds = ScreenUtilities.CalculateTotalScreenBounds();
			Location = new Point(bounds.X - 10000, bounds.Y - 10000);

			// ResumeLayout
			panelBottom.ResumeLayout();
			_contentControl.ResumeLayout();
			ResumeLayout();
		}

		private void InitializeDialogResultButtons()
		{
			IList<DialogResultButton> buttons = _contentControl.DialogResultButtons;
			int count = buttons.Count;
			if (count > 0)
			{
				// find the bottom panel that should contain the buttons
				FlowLayoutPanel panel = null;
				foreach (Control control in Controls)
					if ((panel = control as FlowLayoutPanel) != null)
						break;

				if (panel == null) return;

				Size maxSize = Size.Empty;
				for (int i = count - 1; i >= 0; i--)
				{
					DialogResultButton button = buttons[i];
					Button buttonControl = button.CreateButton();
					buttonControl.TabIndex = i;
					panel.Controls.Add(buttonControl);

					Size size = buttonControl.GetPreferredSize(Size.Empty);
					if (size.Width > maxSize.Width) maxSize.Width = size.Width;
					if (size.Height > maxSize.Height) maxSize.Height = size.Height;

					switch (button.DialogResult)
					{
						case DialogResult.OK:
							if (AcceptButton == null)
								AcceptButton = buttonControl;
							ControlBox = true;
							FirstFocusControl = buttonControl;
							break;
						case DialogResult.Cancel:
							if (CancelButton == null)
								CancelButton = buttonControl;
							ControlBox = true;
							break;
					}
				}

				foreach (Control control in panel.Controls)
					control.Size = maxSize;

				MinimumSize = new Size(
					panel.Padding.Horizontal + ((maxSize.Width + 15) * count),
					maxSize.Height + panel.Padding.Vertical);
			}
		}

		#endregion

		private readonly TContentControl _contentControl = new TContentControl();

		/// <summary>Gets the control with the content of this dialog.</summary>
		/// <value>The control with the content of this dialog.</value>
		public TContentControl ContentControl { get { return _contentControl; } }

		/// <summary>Sets the bounds of this dialog.</summary>
		public void SetBounds()
		{
			Rectangle bounds = Bounds;
			Size preferredSize = GetPreferredSize(Size.Empty);
			SetBounds(
				bounds.X + ((bounds.Width - preferredSize.Width) / 2),
				bounds.Y + ((bounds.Height - preferredSize.Height) / 2),
				preferredSize.Width,
				preferredSize.Height);
		}

		/// <summary>Raises the <see cref="E:Load"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			InitializeDialogResultButtons();
			Size = GetPreferredSize(Size.Empty);

			base.OnLoad(e);
			
			SetBounds();
			
			if (Text.IsNullOrEmpty())
			{
				Form owner = Owner ?? Application.MainForm;
				string text = owner != null ? owner.Text : String.Empty;
				Text = text.IsNullOrEmpty() ? TApplication.Title : text;
			}
		}

		/// <summary>Raises the <see cref="E:Shown"/> event.</summary>
		/// <param name="e">A <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			if (!Modal) DialogResult = DialogResult.Cancel;
		}

		private bool _closeUnconditionally;

		/// <summary>Raises the <see cref="E:Closing"/> event.</summary>
		/// <param name="e">A <see cref="T:CancelEventArgs"/> that contains the event data.</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			if (e.Cancel || _closeUnconditionally) return;

			DialogResult currentResult = DialogResult;
			if (currentResult != DialogResult.Cancel)
			{
				e.Cancel = true;

				// check whether the controls are valid
				if (_contentControl.AreControlsValid())
				{
					SetUIEnabled(false);
					this.InvokeAsync(delegate
					{
						try
						{
							_contentControl.PerformDialogResultAction(
								currentResult,
								delegate(DialogResult result)
								{
									SetUIEnabled(true);
									if (result != DialogResult.None)
										_closeUnconditionally = true;
									DialogResult = result;
									if (!Modal) Close();
								});
						}
						catch (Exception exception)
						{
							SetUIEnabled(true);
							ShowError(exception);
						}
					});
				}
			}
		}

		private void SetUIEnabled(bool enabled)
		{
			ControlBox = enabled && CancelButton != null;
			foreach (Control control in Controls)
				control.Enabled = enabled;
			UseWaitCursor = !enabled;
		}

		/// <summary>Shows the dialog modally with the specified owner.</summary>
		/// <param name="owner">The <see cref="T:Control"/> that owns the dialog.</param>
		/// <returns>The <see cref="T:DialogResult"/> that represents the result of the dialog.</returns>
		public DialogResult ShowDialog(Control owner)
		{
			IWin32Window realOwner = GetRealOwner(owner);
			if (realOwner != null)
			{
				ShowInTaskbar = false;
				ShowIcon = false;
			}

			return base.ShowDialog(realOwner);
		}

		private static IWin32Window GetRealOwner(Control control)
		{
			return control != null
				? control.FindForm()
				: Form.ActiveForm;
		}
	}
}
