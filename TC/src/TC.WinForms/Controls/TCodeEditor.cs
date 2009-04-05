// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using TC.WinForms.Commands;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a control for editing programming code.</summary>
	[ToolboxBitmap(typeof(RichTextBox))]
	public class TCodeEditor : RichTextBox, IHasSystemFont, IProxyCommandsImplementation<EditCommand>, IDocumentContainer
	{
		/// <summary>Initializes a new instance of the <see cref="TCodeEditor"/> class.</summary>
		public TCodeEditor()
		{
			fUndoCommand = new SimpleActionCommand(Undo);
			fRedoCommand = new SimpleActionCommand(Redo);
			fCutCommand = new SimpleActionCommand(Cut);
			fCopyCommand = new SimpleActionCommand(Copy);
			fPasteCommand = new SimpleActionCommand(delegate { Paste(DataFormats.GetFormat(DataFormats.Text)); });
			fDeleteCommand = new SimpleActionCommand(delegate { SelectedText = String.Empty; });
			fSelectAllCommand = new SimpleActionCommand(SelectAll);

			fUndoCommand.CanExecute = false;
			fRedoCommand.CanExecute = false;
			fCutCommand.CanExecute = false;
			fCopyCommand.CanExecute = false;
			fDeleteCommand.CanExecute = false;
			fSelectAllCommand.CanExecute = false;

			AcceptsTab = true;
			AllowDrop = true;
			HideSelection = false;
			Font = SystemFont.Monospace.ToFont();
			ResetFileDialogFilter();
		}

		/// <summary>Gets or sets a value indicating whether the control will enable drag-and-drop operations.</summary>
		/// <returns>true if drag-and-drop is enabled in the control; otherwise, false.</returns>
		[DefaultValue(true)]
		public override bool AllowDrop
		{
			get { return base.AllowDrop; }
			set { base.AllowDrop = value; }
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

		/// <summary>Raises the <see cref="E:TextChanged"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			fUndoCommand.CanExecute = CanUndo;
			fRedoCommand.CanExecute = CanRedo;
			fSelectAllCommand.CanExecute = TextLength > 0;
			IsModified = fUndoCommand.CanExecute;
		}

		/// <summary>Raises the <see cref="E:SelectionChanged"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnSelectionChanged(EventArgs e)
		{
			base.OnSelectionChanged(e);

			bool lHasSelection = SelectedText.Length > 0;
			fCutCommand.CanExecute = lHasSelection;
			fCopyCommand.CanExecute = lHasSelection;
			fDeleteCommand.CanExecute = lHasSelection;

			int lCharIndex = SelectionStart;
			CurrentLineNumber = GetLineFromCharIndex(lCharIndex) + 1;
			CurrentColumnNumber = lCharIndex - GetFirstCharIndexOfCurrentLine() + 1;
		}

		/// <summary>Raises the <see cref="E:DragOver"/> event.</summary>
		/// <param name="drgevent">A <see cref="T:DragEventArgs"/> that contains the event data.</param>
		protected override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver(drgevent);

			drgevent.Effect =
				drgevent.Data.GetDataPresent(DataFormats.Text)
					? DragDropEffects.Copy
					: DragDropEffects.None;
		}

		/// <summary>Raises the <see cref="E:DragDrop"/> event.</summary>
		/// <param name="drgevent">A <see cref="T:DragEventArgs"/> that contains the event data.</param>
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			base.OnDragDrop(drgevent);

			string lText = drgevent.Data.GetData(DataFormats.Text) as string;
			if (!string.IsNullOrEmpty(lText))
			{
				ReplaceSelectedText(lText, true);
				Select();
			}
		}

		/// <summary>Raises the <see cref="E:KeyDown"/> event.</summary>
		/// <param name="e">A <see cref="T:KeyEventArgs"/> that contains the event data.</param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (!e.Handled)
				e.SuppressKeyPress = HandleKey(e.KeyData);
		}

		/// <summary>Handles the specified keyboard key.</summary>
		/// <param name="keyData">The data of the pressed key.</param>
		/// <returns>If the key was handled, true; otherwise, false.</returns>
		protected virtual bool HandleKey(Keys keyData)
		{
			if (ReadOnly)
				switch (keyData)
				{
					case Keys.Tab: FindForm().SelectNextControl(); return true;
					case Keys.Shift | Keys.Tab: FindForm().SelectPreviousControl(); return true;
				}
			else
				switch (keyData)
				{
					case Keys.Insert: OverwriteMode = !OverwriteMode; return false;
					case Keys.Tab: IncreaseLineIndent(); return true;
					case Keys.Shift | Keys.Tab: DecreaseLineIndent(); return true;
					case Keys.Enter: HandleEnterKey(); return true;
				}

			return false;
		}

		private void ReplaceSelectedText(string newSelectedText, bool select)
		{
			if (select)
			{
				int lSelectionStart = SelectionStart;
				SelectedText = newSelectedText;
				Select(lSelectionStart, newSelectedText.Length);
			}
			else SelectedText = newSelectedText;
		}

		private void IncreaseLineIndent()
		{
			string lSelectedText = SelectedText;
			if (lSelectedText.Length == 0)
				ReplaceSelectedText("\t", false);
			else if (lSelectedText.Length > 1 && lSelectedText.EndsWith("\n", StringComparison.OrdinalIgnoreCase))
			{
				lSelectedText =
					lSelectedText
						.Substring(0, lSelectedText.Length - 1)
						.Replace("\n", "\n\t");

				ReplaceSelectedText("\t" + lSelectedText + "\n", true);
			}
			else ReplaceSelectedText("\t" + lSelectedText.Replace("\n", "\n\t"), true);
		}

		private void DecreaseLineIndent()
		{
			if (SelectionLength == 0)
			{
				int lSelectionStart = SelectionStart;
				if (lSelectionStart > 0 && Text[lSelectionStart - 1] == '\t')
					SendKeys.Send("{BS}");
			}
			else
			{
				string lSelectedText = SelectedText;
				if (lSelectedText.Length > 0 && lSelectedText[0] == '\t')
				{
					if (lSelectedText.Length > 1)
						lSelectedText = lSelectedText.Substring(1);
					else lSelectedText = String.Empty;
				}

				ReplaceSelectedText(lSelectedText.Replace("\n\t", "\n"), true);
			}
		}

		private void HandleEnterKey()
		{
			int lFirstCharIndex = GetFirstCharIndexOfCurrentLine();
			int lCurrentCharIndex = SelectionStart;

			string lNewText = "\n";
			if (lCurrentCharIndex > lFirstCharIndex)
				lNewText += GetIndentForNewLine(
					Text.Substring(lFirstCharIndex, lCurrentCharIndex - lFirstCharIndex));

			ReplaceSelectedText(lNewText, false);
		}

		private static readonly Regex
			fRegexPreviousLineIndent = new Regex(@"^(?<indent>[\t ]*).*$", RegexOptions.Compiled);

		/// <summary>Gets the indent to add to the new line after the user presses the Enter-key.</summary>
		/// <param name="previousLine">The previous line.</param>
		/// <returns>The indent to add to the new line.</returns>
		protected virtual string GetIndentForNewLine(string previousLine)
		{
			if (previousLine == null) throw new ArgumentNullException("previousLine");

			Match lMatch = fRegexPreviousLineIndent.Match(previousLine);
			return lMatch.Success ? lMatch.Groups["indent"].Value : String.Empty;
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

		#region CurrentLineNumber members

		private int fCurrentLineNumber;

		/// <summary>Gets the current line number.</summary>
		/// <value>The current line number.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CurrentLineNumber
		{
			get { return fCurrentLineNumber; }
			private set
			{
				if (fCurrentLineNumber != value)
				{
					fCurrentLineNumber = value;
					OnCurrentLineNumberChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object fEventCurrentLineNumberChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:CurrentLineNumber"/> property has changed.</summary>
		public event EventHandler CurrentLineNumberChanged
		{
			add { Events.AddHandler(fEventCurrentLineNumberChanged, value); }
			remove { Events.RemoveHandler(fEventCurrentLineNumberChanged, value); }
		}

		/// <summary>Raises the <see cref="E:CurrentLineNumberChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnCurrentLineNumberChanged(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventCurrentLineNumberChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		#endregion

		#region CurrentColumnNumber members

		private int fCurrentColumnNumber;

		/// <summary>Gets the current column number.</summary>
		/// <value>The current column number.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CurrentColumnNumber
		{
			get { return fCurrentColumnNumber; }
			private set
			{
				if (fCurrentColumnNumber != value)
				{
					fCurrentColumnNumber = value;
					OnCurrentColumnNumberChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object fEventCurrentColumnNumberChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:CurrentColumnNumber"/> property has changed.</summary>
		public event EventHandler CurrentColumnNumberChanged
		{
			add { Events.AddHandler(fEventCurrentColumnNumberChanged, value); }
			remove { Events.RemoveHandler(fEventCurrentColumnNumberChanged, value); }
		}

		/// <summary>Raises the <see cref="E:CurrentColumnNumberChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnCurrentColumnNumberChanged(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventCurrentColumnNumberChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		#endregion

		#region edit commands

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

		private readonly SimpleActionCommand fCopyCommand;

		/// <summary>Gets the command to copy the selected text to the clipboard.</summary>
		/// <value>The command to copy the selected text to the clipboard.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand CopyCommand { get { return fCopyCommand; } }

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

		private readonly SimpleActionCommand fSelectAllCommand;

		/// <summary>Gets the command to select all the text.</summary>
		/// <value>The command to select all the text.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand SelectAllCommand { get { return fSelectAllCommand; } }

		#endregion

		#region IHasSystemFont Members

		/// <summary>Gets the <see cref="T:SystemFont"/> of the control.</summary>
		/// <value>The <see cref="T:SystemFont"/> of the control.</value>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual SystemFont SystemFont { get { return SystemFont.Monospace; } }

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
				case EditCommand.Copy: return fCopyCommand;
				case EditCommand.Paste: return fPasteCommand;
				case EditCommand.Delete: return fDeleteCommand;
				case EditCommand.SelectAll: return fSelectAllCommand;
				default: return null;
			}
		}

		#endregion

		#region IDocumentContainer Members

		#region FilePath members

		private string fFilePath;

		/// <summary>Gets the full path of the document file.</summary>
		/// <value>The full path of the document file, or null if it hasn't been saved yet.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string FilePath
		{
			get { return fFilePath; }
			private set
			{
				if (fFilePath != value)
				{
					fFilePath = value;
					OnFilePathChanged(EventArgs.Empty);
					if (!string.IsNullOrEmpty(value))
						FileName = Path.GetFileName(value);
				}
			}
		}

		private static readonly object fEventFilePathChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:FilePath"/> property has changed.</summary>
		public event EventHandler FilePathChanged
		{
			add { Events.AddHandler(fEventFilePathChanged, value); }
			remove { Events.RemoveHandler(fEventFilePathChanged, value); }
		}

		/// <summary>Raises the <see cref="E:FilePathChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnFilePathChanged(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventFilePathChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		#endregion

		#region FileName members

		private string fFileName;

		/// <summary>Gets or sets the file name of the document file.</summary>
		/// <value>The file name of the document file.</value>
		[Category("Behavior"), Description("The file name of the document file."), DefaultValue(null)]
		public string FileName
		{
			get { return fFileName; }
			set
			{
				if (fFileName != value)
				{
					fFileName = value;
					OnFileNameChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object fEventFileNameChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:FileName"/> property has changed.</summary>
		public event EventHandler FileNameChanged
		{
			add { Events.AddHandler(fEventFileNameChanged, value); }
			remove { Events.RemoveHandler(fEventFileNameChanged, value); }
		}

		/// <summary>Raises the <see cref="E:FileNameChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnFileNameChanged(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventFileNameChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		#endregion

		#region IsModified members

		private bool fIsModified;

		/// <summary>Gets a value indicating whether the document is modified.</summary>
		/// <value><c>true</c> if the document is modified; otherwise, <c>false</c>.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsModified
		{
			get { return fIsModified; }
			private set
			{
				if (fIsModified != value)
				{
					fIsModified = value;
					OnIsModifiedChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object fEventIsModifiedChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:IsModified"/> property has changed.</summary>
		public event EventHandler IsModifiedChanged
		{
			add { Events.AddHandler(fEventIsModifiedChanged, value); }
			remove { Events.RemoveHandler(fEventIsModifiedChanged, value); }
		}

		/// <summary>Raises the <see cref="E:IsModifiedChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnIsModifiedChanged(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventIsModifiedChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		#endregion

		#region FileDialogFilter members

		/// <summary>Gets the default value of the <see cref="P:FileDialogFilter"/> property.</summary>
		/// <value>The default value of the <see cref="P:FileDialogFilter"/>.</value>
		protected virtual string DefaultFileDialogFilter { get { return "All Files (*.*)|*.*"; } }

		private string fFileDialogFilter;

		/// <summary>Gets or sets the filter that is used in the dialog to open or save the document.</summary>
		/// <value>The filter that is used in the dialog to open or save the document.</value>
		[Category("Behavior"), Description("The filter that is used in the dialog to open or save the document.")]
		public string FileDialogFilter
		{
			get { return fFileDialogFilter; }
			set
			{
				if (fFileDialogFilter != value)
				{
					fFileDialogFilter = value;
					OnFileDialogFilterChanged(EventArgs.Empty);
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeFileDialogFilter()
		{
			return fFileDialogFilter != DefaultFileDialogFilter;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		private void ResetFileDialogFilter()
		{
			fFileDialogFilter = DefaultFileDialogFilter;
		}

		private static readonly object fEventFileDialogFilterChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:FileDialogFilter"/> property has changed.</summary>
		public event EventHandler FileDialogFilterChanged
		{
			add { Events.AddHandler(fEventFileDialogFilterChanged, value); }
			remove { Events.RemoveHandler(fEventFileDialogFilterChanged, value); }
		}

		/// <summary>Raises the <see cref="E:FileDialogFilterChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnFileDialogFilterChanged(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventFileDialogFilterChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		#endregion

		/// <summary>Clears the document.</summary>
		/// <param name="fileName">The file name of the document.</param>
		public void ClearDocument(string fileName)
		{
			Text = String.Empty;
			FilePath = null;
			FileName = fileName;
			IsModified = false;
		}

		/// <summary>Loads the document from the specified file path.</summary>
		/// <param name="filePath">The full path of the document to load.</param>
		/// <param name="filterIndex">The index of the selected filter in the dialog.</param>
		public virtual void LoadDocument(string filePath, int filterIndex)
		{
			Text = File.ReadAllText(filePath);
			FilePath = filePath;
			IsModified = false;
		}

		/// <summary>Saves the document to specified file path.</summary>
		/// <param name="filePath">The full path of the file to save the document to.</param>
		/// <param name="filterIndex">The index of the selected filter in the dialog.</param>
		public void SaveDocument(string filePath, int filterIndex)
		{
			File.WriteAllText(filePath, Text, Encoding.UTF8);
			FilePath = filePath;
			IsModified = false;
		}

		#endregion
	}
}
