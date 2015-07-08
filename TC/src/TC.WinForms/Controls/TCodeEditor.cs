// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

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
	public class TCodeEditor : TPlainTextEditor, IDocumentContainer
	{
		/// <summary>Initializes a new instance of the <see cref="TCodeEditor"/> class.</summary>
		public TCodeEditor()
		{
			ResetFileDialogFilter();
		}

		/// <summary>Raises the <see cref="E:TextChanged"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			IsModified = UndoCommand.CanExecute;
		}

		/// <summary>Gets the <see cref="T:SystemFont"/> of the control.</summary>
		/// <value>The <see cref="T:SystemFont"/> of the control.</value>
		public override SystemFont SystemFont { get { return SystemFont.Monospace; } }

		#region IDocumentContainer Members

		#region FilePath members

		private string _filePath;

		/// <summary>Gets the full path of the document file.</summary>
		/// <value>The full path of the document file, or null if it hasn't been saved yet.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string FilePath
		{
			get
			{
				return _filePath;
			}

			private set
			{
				if (_filePath != value)
				{
					_filePath = value;
					OnFilePathChanged(EventArgs.Empty);
					if (value.IsNotNullOrEmpty())
						FileName = Path.GetFileName(value);
				}
			}
		}

		private static readonly object _filePathChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:FilePath"/> property has changed.</summary>
		public event EventHandler FilePathChanged
		{
			add { Events.AddHandler(_filePathChanged, value); }
			remove { Events.RemoveHandler(_filePathChanged, value); }
		}

		/// <summary>Raises the <see cref="E:FilePathChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnFilePathChanged(EventArgs e)
		{
			EventHandler handler = Events[_filePathChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region FileName members

		private string _fileName;

		/// <summary>Gets or sets the file name of the document file.</summary>
		/// <value>The file name of the document file.</value>
		[Category("Behavior"), Description("The file name of the document file."), DefaultValue(null)]
		public string FileName
		{
			get
			{
				return _fileName;
			}

			set
			{
				if (_fileName != value)
				{
					_fileName = value;
					OnFileNameChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _fileNameChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:FileName"/> property has changed.</summary>
		public event EventHandler FileNameChanged
		{
			add { Events.AddHandler(_fileNameChanged, value); }
			remove { Events.RemoveHandler(_fileNameChanged, value); }
		}

		/// <summary>Raises the <see cref="E:FileNameChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnFileNameChanged(EventArgs e)
		{
			EventHandler handler = Events[_fileNameChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region IsModified members

		private bool _isModified;

		/// <summary>Gets a value indicating whether the document is modified.</summary>
		/// <value><c>true</c> if the document is modified; otherwise, <c>false</c>.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsModified
		{
			get
			{
				return _isModified;
			}

			private set
			{
				if (_isModified != value)
				{
					_isModified = value;
					OnIsModifiedChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _isModifiedChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:IsModified"/> property has changed.</summary>
		public event EventHandler IsModifiedChanged
		{
			add { Events.AddHandler(_isModifiedChanged, value); }
			remove { Events.RemoveHandler(_isModifiedChanged, value); }
		}

		/// <summary>Raises the <see cref="E:IsModifiedChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnIsModifiedChanged(EventArgs e)
		{
			EventHandler handler = Events[_isModifiedChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region FileDialogFilter members

		/// <summary>Gets the default value of the <see cref="P:FileDialogFilter"/> property.</summary>
		/// <value>The default value of the <see cref="P:FileDialogFilter"/>.</value>
		protected virtual string DefaultFileDialogFilter { get { return "All Files (*.*)|*.*"; } }

		private string _fileDialogFilter;

		/// <summary>Gets or sets the filter that is used in the dialog to open or save the document.</summary>
		/// <value>The filter that is used in the dialog to open or save the document.</value>
		[Category("Behavior"), Description("The filter that is used in the dialog to open or save the document.")]
		public string FileDialogFilter
		{
			get
			{
				return _fileDialogFilter;
			}

			set
			{
				if (_fileDialogFilter != value)
				{
					_fileDialogFilter = value;
					OnFileDialogFilterChanged(EventArgs.Empty);
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeFileDialogFilter()
		{
			return _fileDialogFilter != DefaultFileDialogFilter;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		private void ResetFileDialogFilter()
		{
			_fileDialogFilter = DefaultFileDialogFilter;
		}

		private static readonly object _fileDialogFilterChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:FileDialogFilter"/> property has changed.</summary>
		public event EventHandler FileDialogFilterChanged
		{
			add { Events.AddHandler(_fileDialogFilterChanged, value); }
			remove { Events.RemoveHandler(_fileDialogFilterChanged, value); }
		}

		/// <summary>Raises the <see cref="E:FileDialogFilterChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnFileDialogFilterChanged(EventArgs e)
		{
			EventHandler handler = Events[_fileDialogFilterChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
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
