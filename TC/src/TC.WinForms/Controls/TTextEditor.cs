// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.ComponentModel;
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
			_undoCommand = new SimpleActionCommand(Undo);
			_redoCommand = new SimpleActionCommand(Redo);
			_cutCommand = new SimpleActionCommand(Cut);
			_pasteCommand = new SimpleActionCommand(Paste);
			_deleteCommand = new SimpleActionCommand(delegate { SelectedText = String.Empty; });

			_undoCommand.CanExecute = false;
			_redoCommand.CanExecute = false;
			_cutCommand.CanExecute = false;
			_pasteCommand.CanExecute = true;
			_deleteCommand.CanExecute = false;

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

			_undoCommand.CanExecute = CanUndo;
			_redoCommand.CanExecute = CanRedo;
		}

		/// <summary>Raises the <see cref="E:SelectionChanged"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnSelectionChanged(EventArgs e)
		{
			base.OnSelectionChanged(e);

			bool hasSelection = SelectionLength > 0;
			_cutCommand.CanExecute = hasSelection;
			_deleteCommand.CanExecute = hasSelection;
		}

		/// <summary>Raises the <see cref="E:ReadOnlyChanged"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnReadOnlyChanged(EventArgs e)
		{
			base.OnReadOnlyChanged(e);

			_pasteCommand.CanExecute = !ReadOnly;
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
			string textToInsert = data.GetData(DataFormats.UnicodeText, true) as string;
			if (textToInsert.IsNotNullOrEmpty())
			{
				ReplaceSelectedText(textToInsert, true);
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
				int selectionStart = SelectionStart;
				SelectedText = newSelectedText;
				Select(selectionStart, newSelectedText.Length);
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
						_undoCommand.Execute();
						return true;

					case Keys.Control | Keys.Y:
						// Ctrl+Y => redo
						_redoCommand.Execute();
						return true;

					case Keys.Control | Keys.X:
					case Keys.Shift | Keys.Delete:
						// Ctrl+X or Shift+Delete => cut selected text
						_cutCommand.Execute();
						return true;

					case Keys.Control | Keys.V:
					case Keys.Shift | Keys.Insert:
						// Ctrl+V or Shift+Insert => paste
						_pasteCommand.Execute();
						return true;

					case Keys.Delete:
						// Delete => delete selected text
						_deleteCommand.Execute();
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
			var dataFormat = DataFormats.GetFormat(format);
			if (CanPaste(dataFormat))
			{
				Paste(dataFormat);
				return true;
			}
			else return false;
		}

		#region OverwriteMode members

		private bool _overwriteMode;

		/// <summary>Gets a value indicating whether the editor is in overwrite-mode.</summary>
		/// <value>If the editor is in overwrite-mode, true; otherwise, false.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool OverwriteMode
		{
			get
			{
				return _overwriteMode;
			}

			private set
			{
				if (_overwriteMode != value)
				{
					_overwriteMode = value;
					OnOverwriteModeChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _overwriteModeChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:OverwriteMode"/> property has changed.</summary>
		public event EventHandler OverwriteModeChanged
		{
			add { Events.AddHandler(_overwriteModeChanged, value); }
			remove { Events.RemoveHandler(_overwriteModeChanged, value); }
		}

		/// <summary>Raises the <see cref="E:OverwriteModeChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnOverwriteModeChanged(EventArgs e)
		{
			this.TriggerEvent(Events, _overwriteModeChanged, e);
        }

		#endregion

		#region commands

		private readonly SimpleActionCommand _undoCommand;

		/// <summary>Gets the command to undo changes.</summary>
		/// <value>The command to undo changes.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand UndoCommand { get { return _undoCommand; } }

		private readonly SimpleActionCommand _redoCommand;

		/// <summary>Gets the command to redo undone changes.</summary>
		/// <value>The command to redo undone changes.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand RedoCommand { get { return _redoCommand; } }

		private readonly SimpleActionCommand _cutCommand;

		/// <summary>Gets the command to cut the selected text to the clipboard.</summary>
		/// <value>The command to cut the selected text to the clipboard.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand CutCommand { get { return _cutCommand; } }

		private readonly SimpleActionCommand _pasteCommand;

		/// <summary>Gets the command to paste text from the clipboard.</summary>
		/// <value>The command to paste text from the clipboard.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand PasteCommand { get { return _pasteCommand; } }

		private readonly SimpleActionCommand _deleteCommand;

		/// <summary>Gets the command to delete the selected text.</summary>
		/// <value>The command to delete the selected text.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand DeleteCommand { get { return _deleteCommand; } }

		#endregion

		#region IProxyCommandsImplementation<EditCommand> Members

		/// <summary>Gets the <see cref="T:ICommand"/> that implements the specified <see cref="T:TCommandIdentifier"/>.</summary>
		/// <param name="identifier">The <see cref="T:TCommandIdentifier"/> to get the <see cref="T:ICommand"/> of.</param>
		/// <returns>The <see cref="T:ICommand"/> that implements the specified <see cref="T:TCommandIdentifier"/>.</returns>
		public ICommand GetCommand(EditCommand identifier)
		{
			switch (identifier)
			{
				case EditCommand.Undo: return _undoCommand;
				case EditCommand.Redo: return _redoCommand;
				case EditCommand.Cut: return _cutCommand;
				case EditCommand.Copy: return CopyCommand;
				case EditCommand.Paste: return _pasteCommand;
				case EditCommand.Delete: return _deleteCommand;
				case EditCommand.SelectAll: return SelectAllCommand;
				default: return null;
			}
		}

		#endregion
	}
}
