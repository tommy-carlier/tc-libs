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
			FlowLayoutPanel lPanelBottom = new FlowLayoutPanel();

			// SuspendLayout
			SuspendLayout();
			fContentControl.SuspendLayout();
			lPanelBottom.SuspendLayout();

			// fContentControl
			fContentControl.Dock = DockStyle.Fill;
			fContentControl.TabIndex = 0;
			Controls.Add(fContentControl);

			// lPanelBottom
			lPanelBottom.FlowDirection = FlowDirection.RightToLeft;
			lPanelBottom.WrapContents = false;
			lPanelBottom.AutoSize = true;
			lPanelBottom.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			lPanelBottom.Dock = DockStyle.Bottom;
			lPanelBottom.Padding = new Padding(3, 6, 3, 6);
			lPanelBottom.TabIndex = 1;
			Controls.Add(lPanelBottom);

			// TDialog
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			ControlBox = CancelButton != null;
			Text = fContentControl.Text;

			// move the dialog off-screen before displaying it and setting the final location and size
			Rectangle lBounds = ScreenUtilities.CalculateTotalScreenBounds();
			Location = new Point(lBounds.X - 10000, lBounds.Y - 10000);

			// ResumeLayout
			lPanelBottom.ResumeLayout();
			fContentControl.ResumeLayout();
			ResumeLayout();
		}

		private void InitializeDialogResultButtons()
		{
			IList<DialogResultButton> lDialogResultButtons = fContentControl.DialogResultButtons;
			int lCount = lDialogResultButtons.Count;
			if (lCount > 0)
			{
				// find the bottom panel that should contain the buttons
				FlowLayoutPanel lPanel = null;
				foreach (Control lControl in Controls)
					if ((lPanel = lControl as FlowLayoutPanel) != null)
						break;

				if (lPanel == null) return;

				Size lMaxSize = Size.Empty;
				for (int i = lCount - 1; i >= 0; i--)
				{
					DialogResultButton lButton = lDialogResultButtons[i];
					Button lButtonControl = lButton.CreateButton();
					lButtonControl.TabIndex = i;
					lPanel.Controls.Add(lButtonControl);

					Size lSize = lButtonControl.GetPreferredSize(Size.Empty);
					if (lSize.Width > lMaxSize.Width) lMaxSize.Width = lSize.Width;
					if (lSize.Height > lMaxSize.Height) lMaxSize.Height = lSize.Height;

					switch (lButton.DialogResult)
					{
						case DialogResult.OK:
							if (AcceptButton == null)
								AcceptButton = lButtonControl;
							ControlBox = true;
							FirstFocusControl = lButtonControl;
							break;
						case DialogResult.Cancel:
							if (CancelButton == null)
								CancelButton = lButtonControl;
							ControlBox = true;
							break;
					}
				}

				foreach (Control lControl in lPanel.Controls)
					lControl.Size = lMaxSize;

				MinimumSize = new Size(
					lPanel.Padding.Horizontal + ((lMaxSize.Width + 15) * lCount),
					lMaxSize.Height + lPanel.Padding.Vertical);
			}
		}

		#endregion

		private readonly TContentControl fContentControl = new TContentControl();

		/// <summary>Gets the control with the content of this dialog.</summary>
		/// <value>The control with the content of this dialog.</value>
		public TContentControl ContentControl { get { return fContentControl; } }

		/// <summary>Sets the bounds of this dialog.</summary>
		public void SetBounds()
		{
			Rectangle lBounds = Bounds;
			Size lPreferredSize = GetPreferredSize(Size.Empty);
			SetBounds(
				lBounds.X + ((lBounds.Width - lPreferredSize.Width) / 2),
				lBounds.Y + ((lBounds.Height - lPreferredSize.Height) / 2),
				lPreferredSize.Width,
				lPreferredSize.Height);
		}

		/// <summary>Raises the <see cref="E:Load"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			InitializeDialogResultButtons();
			SetBounds();
			if (string.IsNullOrEmpty(Text))
			{
				Form lOwner = Owner ?? Application.MainForm;
				string lText = lOwner != null ? lOwner.Text : String.Empty;
				Text = string.IsNullOrEmpty(lText) ? TApplication.Title : lText;
			}
		}

		/// <summary>Raises the <see cref="E:Shown"/> event.</summary>
		/// <param name="e">A <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			if (!Modal) DialogResult = DialogResult.Cancel;
		}

		private bool fCloseUnconditionally;

		/// <summary>Raises the <see cref="E:Closing"/> event.</summary>
		/// <param name="e">A <see cref="T:CancelEventArgs"/> that contains the event data.</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			if (e.Cancel || fCloseUnconditionally) return;

			DialogResult lResult = DialogResult;
			if (lResult != DialogResult.Cancel)
			{
				e.Cancel = true;

				// check whether the controls are valid
				if (fContentControl.AreControlsValid())
				{
					SetUIEnabled(false);
					this.InvokeAsync(delegate
					{
						try
						{
							fContentControl.PerformDialogResultAction(
								lResult,
								delegate(DialogResult result)
								{
									SetUIEnabled(true);
									if (result != DialogResult.None)
										fCloseUnconditionally = true;
									DialogResult = result;
									if (!Modal) Close();
								});
						}
						catch (Exception lException)
						{
							SetUIEnabled(true);
							ShowError(lException);
						}
					});
				}
			}
		}

		private void SetUIEnabled(bool enabled)
		{
			ControlBox = enabled && CancelButton != null;
			foreach (Control lControl in Controls)
				lControl.Enabled = enabled;
			UseWaitCursor = !enabled;
		}

		/// <summary>Shows the dialog modally with the specified owner.</summary>
		/// <param name="owner">The <see cref="T:Control"/> that owns the dialog.</param>
		/// <returns>The <see cref="T:DialogResult"/> that represents the result of the dialog.</returns>
		public DialogResult ShowDialog(Control owner)
		{
			IWin32Window lRealOwner = GetRealOwner(owner);
			if (lRealOwner != null)
			{
				ShowInTaskbar = false;
				ShowIcon = false;
			}

			return base.ShowDialog(lRealOwner);
		}

		private static IWin32Window GetRealOwner(Control control)
		{
			return control != null
				? control.FindForm()
				: Form.ActiveForm;
		}
	}
}
