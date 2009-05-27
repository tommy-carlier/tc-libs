// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using TC.WinForms.Commands;

namespace TC.WinForms.Controls
{
	/// <summary>The base class for any control for viewing and/or editing multi-line text.</summary>
	[ToolboxBitmap(typeof(RichTextBox))]
	public abstract class TTextControl : RichTextBox
	{
		/// <summary>Initializes a new instance of the <see cref="TTextControl"/> class.</summary>
		protected TTextControl()
		{
			_copyCommand = new SimpleActionCommand(Copy);
			_selectAllCommand = new SimpleActionCommand(SelectAll);

			_copyCommand.CanExecute = false;
			_selectAllCommand.CanExecute = false;

			AcceptsTab = true;
			HideSelection = false;
		}

		/// <summary>Gets or sets a value indicating whether pressing the TAB key in a multiline text box control types a TAB character in the control instead of moving the focus to the next control in the tab order.</summary>
		/// <returns>true if users can enter tabs in a multiline text box using the TAB key; false if pressing the TAB key moves the focus. The default is true.</returns>
		[DefaultValue(true)]
		public new bool AcceptsTab
		{
			get { return base.AcceptsTab; }
			set { base.AcceptsTab = value; }
		}

		/// <summary>Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.</summary>
		/// <returns>true if the selected text does not appear highlighted when the text box control loses focus; false, if the selected text remains highlighted when the text box control loses focus. The default is false.</returns>
		[DefaultValue(false)]
		public new bool HideSelection
		{
			get { return base.HideSelection; }
			set { base.HideSelection = value; }
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows message to process.</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			switch (m.Msg)
			{
				case NativeMethods.WM_NCCALCSIZE:
					if (ShouldRenderWithVisualStyles())
						CalculateNonClientSize(ref m);
					break;
				case NativeMethods.WM_NCPAINT:
					if (ShouldRenderWithVisualStyles())
						PaintNonClientRegion(ref m);
					break;
				case NativeMethods.WM_THEMECHANGED:
					if (ShouldRenderWithVisualStyles())
						UpdateStyles();
					break;
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:CreateParams"/> representing the information needed when creating a control.</returns>
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;

				if (ShouldRenderWithVisualStyles())
					createParams.ExStyle &= ~NativeMethods.WS_EX_CLIENTEDGE;

				return createParams;
			}
		}

		#region non-client region painting

		private bool ShouldRenderWithVisualStyles()
		{
			return BorderStyle == BorderStyle.Fixed3D && Application.RenderWithVisualStyles;
		}

		private NativeMethods.RECT _borderRect;

		private void CalculateNonClientSize(ref Message m)
		{
			NativeMethods.NCCALCSIZE_PARAMS parameters;
			NativeMethods.RECT windowRect;

			ExtractParamsAndWindowRect(ref m, out parameters, out windowRect);
			var renderer = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.Normal);

			using (var deviceContext = new NativeMethods.DeviceContext(Handle))
			{
				var contentRect = new NativeMethods.RECT(
					renderer.GetBackgroundContentRectangle(deviceContext, windowRect.ToRectangle()));
				contentRect.Inflate(-1, -1);

				_borderRect = new NativeMethods.RECT(
					contentRect.Left - windowRect.Left,
					contentRect.Top - windowRect.Top,
					windowRect.Right - contentRect.Right,
					windowRect.Bottom - contentRect.Bottom);

				if (m.WParam == IntPtr.Zero)
					Marshal.StructureToPtr(contentRect, m.LParam, false);
				else
				{
					parameters.rgrc0 = contentRect;
					Marshal.StructureToPtr(parameters, m.LParam, false);
				}
			}

			m.Result = new IntPtr(NativeMethods.WVR_REDRAW);
		}

		private static void ExtractParamsAndWindowRect(
			ref Message m,
			out NativeMethods.NCCALCSIZE_PARAMS parameters,
			out NativeMethods.RECT windowRect)
		{
			if (m.WParam == IntPtr.Zero)
			{
				parameters = new NativeMethods.NCCALCSIZE_PARAMS();
				windowRect = NativeMethods.PtrToStruct<NativeMethods.RECT>(m.LParam);
			}
			else
			{
				parameters = NativeMethods.PtrToStruct<NativeMethods.NCCALCSIZE_PARAMS>(m.LParam);
				windowRect = parameters.rgrc0;
			}
		}

		private void PaintNonClientRegion(ref Message m)
		{
			IntPtr handle = Handle;

			NativeMethods.RECT windowRect;
			NativeMethods.GetWindowRect(handle, out windowRect);
			windowRect = new NativeMethods.RECT(
				0,
				0,
				windowRect.Right - windowRect.Left,
				windowRect.Bottom - windowRect.Top);

			using (var deviceContext = new NativeMethods.DeviceContext(handle))
			{
				var renderer = new VisualStyleRenderer(
					Enabled
						? ReadOnly
							? VisualStyleElement.TextBox.TextEdit.ReadOnly
							: VisualStyleElement.TextBox.TextEdit.Normal
						: VisualStyleElement.TextBox.TextEdit.Disabled);

				NativeMethods.ExcludeClipRect(
					deviceContext.GetHdc(),
					_borderRect.Left,
					_borderRect.Top,
					windowRect.Right - _borderRect.Right,
					windowRect.Bottom - _borderRect.Bottom);

				if (renderer.IsBackgroundPartiallyTransparent())
					renderer.DrawParentBackground(deviceContext, windowRect.ToRectangle(), this);

				renderer.DrawBackground(deviceContext, windowRect.ToRectangle());
			}

			m.Result = IntPtr.Zero;
		}

		#endregion

		/// <summary>Raises the <see cref="E:TextChanged"/> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			_selectAllCommand.CanExecute = TextLength > 0;
		}

		/// <summary>Raises the <see cref="E:SelectionChanged"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnSelectionChanged(EventArgs e)
		{
			base.OnSelectionChanged(e);

			_copyCommand.CanExecute = SelectionLength > 0;

			int charIndex = SelectionStart;
			CurrentLineNumber = GetLineFromCharIndex(charIndex) + 1;
			CurrentColumnNumber = charIndex - GetFirstCharIndexOfCurrentLine() + 1;
		}

		/// <summary>Raises the <see cref="E:KeyDown"/> event.</summary>
		/// <param name="e">A <see cref="T:KeyEventArgs"/> that contains the event data.</param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (!e.Handled)
				e.SuppressKeyPress = HandleKey(e.KeyData, ReadOnly);
		}

		/// <summary>Handles the specified keyboard key.</summary>
		/// <param name="keyData">The data of the pressed key.</param>
		/// <param name="inReadOnlyMode">Indicates whether the control is currently in read-only mode.</param>
		/// <returns>If the key was handled, true; otherwise, false.</returns>
		protected virtual bool HandleKey(Keys keyData, bool inReadOnlyMode)
		{
			if (inReadOnlyMode)
				switch (keyData)
				{
					case Keys.Tab:
						// TAB => go to next control
						FindForm().SelectNextControl();
						return true;

					case Keys.Shift | Keys.Tab:
						// Shift+TAB => go to previous control
						FindForm().SelectPreviousControl();
						return true;
				}

			switch (keyData)
			{
				case Keys.Control | Keys.C:
				case Keys.Control | Keys.Insert:
					// Ctrl+C or Ctrl+Insert => Copy
					_copyCommand.Execute();
					return true;

				case Keys.Control | Keys.A:
					// Ctrl+A => select all text
					_selectAllCommand.Execute();
					return true;
			}

			return false;
		}

		#region commands

		private readonly SimpleActionCommand _copyCommand;

		/// <summary>Gets the command to copy the selected text to the clipboard.</summary>
		/// <value>The command to copy the selected text to the clipboard.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand CopyCommand { get { return _copyCommand; } }

		private readonly SimpleActionCommand _selectAllCommand;

		/// <summary>Gets the command to select all the text.</summary>
		/// <value>The command to select all the text.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand SelectAllCommand { get { return _selectAllCommand; } }

		#endregion

		#region CurrentLineNumber members

		private int _currentLineNumber;

		/// <summary>Gets the current line number.</summary>
		/// <value>The current line number.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CurrentLineNumber
		{
			get { return _currentLineNumber; }
			private set
			{
				if (_currentLineNumber != value)
				{
					_currentLineNumber = value;
					OnCurrentLineNumberChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _currentLineNumberChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:CurrentLineNumber"/> property has changed.</summary>
		public event EventHandler CurrentLineNumberChanged
		{
			add { Events.AddHandler(_currentLineNumberChanged, value); }
			remove { Events.RemoveHandler(_currentLineNumberChanged, value); }
		}

		/// <summary>Raises the <see cref="E:CurrentLineNumberChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnCurrentLineNumberChanged(EventArgs e)
		{
			EventHandler handler = Events[_currentLineNumberChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region CurrentColumnNumber members

		private int _currentColumnNumber;

		/// <summary>Gets the current column number.</summary>
		/// <value>The current column number.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CurrentColumnNumber
		{
			get { return _currentColumnNumber; }
			private set
			{
				if (_currentColumnNumber != value)
				{
					_currentColumnNumber = value;
					OnCurrentColumnNumberChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _currentColumnNumberChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:CurrentColumnNumber"/> property has changed.</summary>
		public event EventHandler CurrentColumnNumberChanged
		{
			add { Events.AddHandler(_currentColumnNumberChanged, value); }
			remove { Events.RemoveHandler(_currentColumnNumberChanged, value); }
		}

		/// <summary>Raises the <see cref="E:CurrentColumnNumberChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnCurrentColumnNumberChanged(EventArgs e)
		{
			EventHandler handler = Events[_currentColumnNumberChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion
	}
}
