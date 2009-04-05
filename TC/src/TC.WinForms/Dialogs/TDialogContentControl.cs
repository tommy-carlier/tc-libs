// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TC.WinForms;
using TC.WinForms.Controls;
using TC.WinForms.Forms;

namespace TC.WinForms.Dialogs
{
	/// <summary>Represents the main content control of a <see cref="T:TDialog{TContentControl}"/>.</summary>
	public class TDialogContentControl : TUserControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:TDialogContentControl"/> class.</summary>
		public TDialogContentControl()
		{
			BackColor = SystemColors.Window;
			ForeColor = SystemColors.WindowText;
		}

		#region DialogResultButtons

		private readonly Collection<DialogResultButton> fDialogResultButtons = new Collection<DialogResultButton>();

		/// <summary>Gets the definition of the buttons that set the <see cref="T:DialogResult"/> of the dialog.</summary>
		/// <value>The definition of the buttons that set the <see cref="T:DialogResult"/> of the dialog.</value>
		[Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Description("The definition of the DialogResult-buttons at the bottom of the dialog.")]
		public Collection<DialogResultButton> DialogResultButtons { get { return fDialogResultButtons; } }

		#endregion

		#region Text

		/// <summary>Gets or sets the title of the dialog.</summary>
		/// <value>The title of the dialog.</value>
		/// <returns>The title of the dialog.</returns>
		[EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Category("Appearance"), Description("The title of the dialog.")]
		public override string Text
		{
			get { return base.Text; }
			set { base.Text = value; }
		}

		/// <summary>Raises the <see cref="E:TextChanged"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			Form lForm = FindForm();
			if (lForm != null)
				lForm.Text = Text;
		}

		#endregion

		#region BackColor, ForeColor, DefaultMargin, DefaultPadding, DefaultMinimumSize

		/// <summary>Gets or sets the background color for the control.</summary>
		/// <returns>A <see cref="T:Color"/> that represents the background color of the control.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

		/// <summary>Gets or sets the foreground color of the control.</summary>
		/// <returns>The foreground <see cref="T:Color"/> of the control.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}

		private static readonly Padding fDefaultMargin = Padding.Empty;

		/// <summary>Gets the space, in pixels, that is specified by default between controls.</summary>
		/// <returns>A <see cref="T:Padding"/> that represents the default space between controls.</returns>
		protected override Padding DefaultMargin { get { return fDefaultMargin; } }

		private static readonly Padding fDefaultPadding = new Padding(8);

		/// <summary>Gets the internal spacing, in pixels, of the contents of a control.</summary>
		/// <returns>A <see cref="T:Padding"/> that represents the internal spacing of the contents of a control.</returns>
		protected override Padding DefaultPadding { get { return fDefaultPadding; } }

		private static readonly Size fDefaultMinimumSize = new Size(100, 50);

		/// <summary>Gets the length and height, in pixels, that is specified as the default minimum size of a control.</summary>
		/// <returns>A <see cref="T:Size"/> representing the size of the control.</returns>
		protected override Size DefaultMinimumSize { get { return fDefaultMinimumSize; } }

		#endregion

		/// <summary>Paints the background of the control.</summary>
		/// <param name="e">A <see cref="T:PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			Size lSize = Size;
			using (Pen lPen = new Pen(Color.FromArgb(100, ForeColor)))
				e.Graphics.DrawLine(lPen, 0F, lSize.Height - 1, lSize.Width, lSize.Height - 1);
		}

		/// <summary>Raises the <see cref="E:Resize"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Invalidate();
		}

		#region validate controls

		internal bool AreControlsValid()
		{
			try
			{
				ValidateControls();
				return true;
			}
			catch (DialogValidationException lException)
			{
				if (lException.InvalidControl != null && lException.InvalidControl.CanSelect)
					lException.InvalidControl.Select();

				ShowError(lException);
				return false;
			}
		}

		/// <summary>Validates the controls of the dialog.</summary>
		/// <remarks>If one of the controls is invalid, a <see cref="T:DialogValidationException"/> has to be thrown.</remarks>
		protected virtual void ValidateControls()
		{
		}

		#endregion

		/// <summary>Performs the action that produces the dialog result.</summary>
		/// <param name="dialogResult">The <see cref="T:DialogResult"/> of the pressed dialog button.</param>
		/// <param name="callback">The callback function that has to be called when the action is performed.</param>
		public virtual void PerformDialogResultAction(DialogResult dialogResult, Action<DialogResult> callback)
		{
			if (callback != null)
				callback(dialogResult);
		}
	}
}
