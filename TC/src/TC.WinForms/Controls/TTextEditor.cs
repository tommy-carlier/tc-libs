// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using TC.WinForms.Commands;

namespace TC.WinForms.Controls
{
	/// <summary>The base class for any control for editing multi-line text.</summary>
	public abstract class TTextEditor : TTextControl, IProxyCommandsImplementation<EditCommand>
	{
		/// <summary>Initializes a new instance of the <see cref="TTextEditor"/> class.</summary>
		protected TTextEditor()
		{
			fUndoCommand = new SimpleActionCommand(Undo);
			fRedoCommand = new SimpleActionCommand(Redo);
			fCutCommand = new SimpleActionCommand(Cut);
			fPasteCommand = new SimpleActionCommand(Paste);
			fDeleteCommand = new SimpleActionCommand(delegate { SelectedText = String.Empty; });

			fUndoCommand.CanExecute = false;
			fRedoCommand.CanExecute = false;
			fCutCommand.CanExecute = false;
			fPasteCommand.CanExecute = true;
			fDeleteCommand.CanExecute = false;

			AllowDrop = true;
		}

		/// <summary>Gets or sets a value indicating whether the control will enable drag-and-drop operations.</summary>
		/// <returns>true if drag-and-drop is enabled in the control; otherwise, false.</returns>
		[DefaultValue(true)]
		public override bool AllowDrop
		{
			get { return base.AllowDrop; }
			set { base.AllowDrop = value; }
		}

		/// <summary>Raises the <see cref="E:TextChanged"/> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			fUndoCommand.CanExecute = CanUndo;
			fRedoCommand.CanExecute = CanRedo;
		}

		/// <summary>Raises the <see cref="E:SelectionChanged"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnSelectionChanged(EventArgs e)
		{
			base.OnSelectionChanged(e);

			bool lHasSelection = SelectionLength > 0;
			fCutCommand.CanExecute = lHasSelection;
			fDeleteCommand.CanExecute = lHasSelection;
		}

		/// <summary>Raises the <see cref="E:ReadOnlyChanged"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnReadOnlyChanged(EventArgs e)
		{
			base.OnReadOnlyChanged(e);

			fPasteCommand.CanExecute = !ReadOnly;
		}

		/// <summary>Raises the <see cref="E:DragOver"/> event.</summary>
		/// <param name="drgevent">A <see cref="T:DragEventArgs"/> that contains the event data.</param>
		protected override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver(drgevent);

			drgevent.Effect =
				!ReadOnly && CanInsert(drgevent.Data)
					? DragDropEffects.Copy
					: DragDropEffects.None;
		}

		/// <summary>Determines whether the specified data can be inserted into this editor.</summary>
		/// <param name="data">The data to check.</param>
		/// <returns>If the specified data can be inserted into this editor, <c>true</c>; otherwise, <c>false</c>.</returns>
		protected virtual bool CanInsert(IDataObject data)
		{
			return data.GetDataPresent(DataFormats.UnicodeText)
				|| data.GetDataPresent(DataFormats.Text);
		}

		/// <summary>Raises the <see cref="E:DragDrop"/> event.</summary>
		/// <param name="drgevent">A <see cref="T:DragEventArgs"/> that contains the event data.</param>
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			base.OnDragDrop(drgevent);

			if (Insert(drgevent.Data))
				Select();
		}

		/// <summary>Inserts the specified data into this editor.</summary>
		/// <param name="data">The data to insert.</param>
		/// <returns>If the data could be inserted successfully, <c>true</c>; otherwise, <c>false</c>.</returns>
		protected virtual bool Insert(IDataObject data)
		{
			string lTextToInsert = data.GetData(DataFormats.UnicodeText, true) as string;
			if (!string.IsNullOrEmpty(lTextToInsert))
			{
				ReplaceSelectedText(lTextToInsert, true);
				return true;
			}
			else return false;
		}

		/// <summary>Replaces the currently selected text with the specified string.</summary>
		/// <param name="newSelectedText">The text to replace the currently selected text with.</param>
		/// <param name="select">if set to <c>true</c> the new text will be selected.</param>
		protected void ReplaceSelectedText(string newSelectedText, bool select)
		{
			if (newSelectedText == null) throw new ArgumentNullException("newSelectedText");

			if (select)
			{
				int lSelectionStart = SelectionStart;
				SelectedText = newSelectedText;
				Select(lSelectionStart, newSelectedText.Length);
			}
			else SelectedText = newSelectedText;
		}

		/// <summary>Handles the specified keyboard key.</summary>
		/// <param name="keyData">The data of the pressed key.</param>
		/// <param name="inReadOnlyMode">Indicates whether the control is currently in read-only mode.</param>
		/// <returns>If the key was handled, true; otherwise, false.</returns>
		protected override bool HandleKey(Keys keyData, bool inReadOnlyMode)
		{
			if (!inReadOnlyMode)
				switch (keyData)
				{
					case Keys.Insert:
						// INSERT => toggle the overwrite-mode flag
						OverwriteMode = !OverwriteMode;
						break;

					case Keys.Tab:
						// TAB => increase the line indent
						IncreaseLineIndent();
						return true;

					case Keys.Shift | Keys.Tab:
						// Shift+TAB => decrease the line indent
						DecreaseLineIndent();
						return true;

					case Keys.Control | Keys.Z:
						// Ctrl+Z => undo
						fUndoCommand.Execute();
						return true;

					case Keys.Control | Keys.Y:
						// Ctrl+Y => redo
						fRedoCommand.Execute();
						return true;

					case Keys.Control | Keys.X:
					case Keys.Shift | Keys.Delete:
						// Ctrl+X or Shift+Delete => cut selected text
						fCutCommand.Execute();
						return true;

					case Keys.Control | Keys.V:
					case Keys.Shift | Keys.Insert:
						// Ctrl+V or Shift+Insert => paste
						fPasteCommand.Execute();
						return true;

					case Keys.Delete:
						// Delete => delete selected text
						fDeleteCommand.Execute();
						return true;
				}

			return base.HandleKey(keyData, inReadOnlyMode);
		}

		/// <summary>Increases the line indent.</summary>
		public abstract void IncreaseLineIndent();

		/// <summary>Decreases the line indent.</summary>
		public abstract void DecreaseLineIndent();

		/// <summary>Replaces the current selection in the text box with the contents of the Clipboard.</summary>
		public virtual new void Paste()
		{
			if (!TryPaste(DataFormats.UnicodeText))
				TryPaste(DataFormats.Text);
		}

		private bool TryPaste(string format)
		{
			var lDataFormat = DataFormats.GetFormat(format);
			if (CanPaste(lDataFormat))
			{
				Paste(lDataFormat);
				return true;
			}
			else return false;
		}

		#region OverwriteMode members

		private bool fOverwriteMode;

		/// <summary>Gets a value indicating whether the editor is in overwrite-mode.</summary>
		/// <value>If the editor is in overwrite-mode, true; otherwise, false.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool OverwriteMode
		{
			get { return fOverwriteMode; }
			private set
			{
				if (fOverwriteMode != value)
				{
					fOverwriteMode = value;
					OnOverwriteModeChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object fEventOverwriteModeChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:OverwriteMode"/> property has changed.</summary>
		public event EventHandler OverwriteModeChanged
		{
			add { Events.AddHandler(fEventOverwriteModeChanged, value); }
			remove { Events.RemoveHandler(fEventOverwriteModeChanged, value); }
		}

		/// <summary>Raises the <see cref="E:OverwriteModeChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnOverwriteModeChanged(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventOverwriteModeChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		#endregion

		#region commands

		private readonly SimpleActionCommand fUndoCommand;

		/// <summary>Gets the command to undo changes.</summary>
		/// <value>The command to undo changes.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand UndoCommand { get { return fUndoCommand; } }

		private readonly SimpleActionCommand fRedoCommand;

		/// <summary>Gets the command to redo undone changes.</summary>
		/// <value>The command to redo undone changes.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand RedoCommand { get { return fRedoCommand; } }

		private readonly SimpleActionCommand fCutCommand;

		/// <summary>Gets the command to cut the selected text to the clipboard.</summary>
		/// <value>The command to cut the selected text to the clipboard.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand CutCommand { get { return fCutCommand; } }

		private readonly SimpleActionCommand fPasteCommand;

		/// <summary>Gets the command to paste text from the clipboard.</summary>
		/// <value>The command to paste text from the clipboard.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand PasteCommand { get { return fPasteCommand; } }

		private readonly SimpleActionCommand fDeleteCommand;

		/// <summary>Gets the command to delete the selected text.</summary>
		/// <value>The command to delete the selected text.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand DeleteCommand { get { return fDeleteCommand; } }

		#endregion

		#region IProxyCommandsImplementation<EditCommand> Members

		/// <summary>Gets the <see cref="T:ICommand"/> that implements the specified <see cref="T:TCommandIdentifier"/>.</summary>
		/// <param name="identifier">The <see cref="T:TCommandIdentifier"/> to get the <see cref="T:ICommand"/> of.</param>
		/// <returns>The <see cref="T:ICommand"/> that implements the specified <see cref="T:TCommandIdentifier"/>.</returns>
		public ICommand GetCommand(EditCommand identifier)
		{
			switch (identifier)
			{
				case EditCommand.Undo: return fUndoCommand;
				case EditCommand.Redo: return fRedoCommand;
				case EditCommand.Cut: return fCutCommand;
				case EditCommand.Copy: return CopyCommand;
				case EditCommand.Paste: return fPasteCommand;
				case EditCommand.Delete: return fDeleteCommand;
				case EditCommand.SelectAll: return SelectAllCommand;
				default: return null;
			}
		}

		#endregion
	}
}
