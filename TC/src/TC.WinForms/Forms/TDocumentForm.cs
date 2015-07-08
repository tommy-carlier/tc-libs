// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

using TC.WinForms.Commands;
using TC.WinForms.Settings;

namespace TC.WinForms.Forms
{
	/// <summary>Represents a window that contains a document.</summary>
	public class TDocumentForm : TForm
	{
		/// <summary>Initializes a new instance of the <see cref="TDocumentForm"/> class.</summary>
		public TDocumentForm()
		{
			_clearDocumentCommand = new SimpleActionCommand(ClearDocument);
			_loadDocumentCommand = new SimpleActionCommand(LoadDocument);
			_saveDocumentCommand = new SimpleActionCommand(SaveDocument);
			_saveDocumentAsCommand = new SimpleActionCommand(SaveDocumentAs);
		}

		#region commands

		private readonly ICommand _clearDocumentCommand;

		/// <summary>Gets the command to clear the document.</summary>
		/// <value>The command to clear the document.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand ClearDocumentCommand { get { return _clearDocumentCommand; } }

		private readonly ICommand _loadDocumentCommand;

		/// <summary>Gets the command to load a document.</summary>
		/// <value>The command to load a document.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand LoadDocumentCommand { get { return _loadDocumentCommand; } }

		private readonly ICommand _saveDocumentCommand;

		/// <summary>Gets the command to save the current document.</summary>
		/// <value>The command to save the current document.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand SaveDocumentCommand { get { return _saveDocumentCommand; } }

		private readonly ICommand _saveDocumentAsCommand;

		/// <summary>Gets the command to save the current document as a different file.</summary>
		/// <value>The command to save the current document as a different file.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand SaveDocumentAsCommand { get { return _saveDocumentAsCommand; } }

		#endregion

		#region DocumentContainer members

		private IDocumentContainer _documentContainer;

		/// <summary>Gets or sets the component that contains the actual document.</summary>
		/// <value>The component that contains the actual document.</value>
		[Category("Behavior"), Description("The component that contains the document."), DefaultValue(null)]
		public IDocumentContainer DocumentContainer
		{
			get
			{
				return _documentContainer;
			}

			set
			{
				if (_documentContainer != value)
				{
					if (_documentContainer != null)
					{
						_documentContainer.FileNameChanged -= RaiseDocumentTitleChanged;
						_documentContainer.IsModifiedChanged -= RaiseDocumentTitleChanged;
					}

					if (value != null)
					{
						value.FileNameChanged += RaiseDocumentTitleChanged;
						value.IsModifiedChanged += RaiseDocumentTitleChanged;
					}

					_documentContainer = value;
					OnDocumentContainerChanged(EventArgs.Empty);
					RaiseDocumentTitleChanged(_documentContainer, EventArgs.Empty);
				}
			}
		}

		private static readonly object _documentContainerChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:DocumentContainer"/> property has changed.</summary>
		public event EventHandler DocumentContainerChanged
		{
			add { Events.AddHandler(_documentContainerChanged, value); }
			remove { Events.RemoveHandler(_documentContainerChanged, value); }
		}

		/// <summary>Raises the <see cref="E:DocumentContainerChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnDocumentContainerChanged(EventArgs e)
		{
			EventHandler handler = Events[_documentContainerChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region DocumentTitle members

		/// <summary>Gets the document title.</summary>
		/// <value>The document title.</value>
		[Category("Appearance"), Description("The document title."), DefaultValue("")]
		public string DocumentTitle
		{
			get
			{
				return _documentContainer != null
					? (_documentContainer.FileName ?? String.Empty)
						+ (_documentContainer.IsModified ? "*" : String.Empty)
					: String.Empty;
			}
		}

		private static readonly object _documentTitleChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:DocumentTitle"/> property has changed.</summary>
		public event EventHandler DocumentTitleChanged
		{
			add { Events.AddHandler(_documentTitleChanged, value); }
			remove { Events.RemoveHandler(_documentTitleChanged, value); }
		}

		/// <summary>Raises the <see cref="E:DocumentTitleChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnDocumentTitleChanged(EventArgs e)
		{
			EventHandler handler = Events[_documentTitleChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		private void RaiseDocumentTitleChanged(object sender, EventArgs e)
		{
			OnDocumentTitleChanged(e);
		}

		#endregion

		#region ClearDocument

		/// <summary>Clears the document.</summary>
		public void ClearDocument()
		{
			if (_documentContainer == null)
				throw new InvalidOperationException("DocumentContainer is not initialized.");

			if (!_documentContainer.IsModified || AskToSaveDocument())
				_documentContainer.ClearDocument(String.Empty);
		}

		#endregion

		#region LoadDocument

		/// <summary>Loads the document from a file.</summary>
		public void LoadDocument()
		{
			if (_documentContainer == null)
				throw new InvalidOperationException("DocumentContainer is not initialized.");

			if (!_documentContainer.IsModified || AskToSaveDocument())
				using (OpenFileDialog dialog = new OpenFileDialog())
				{
					try
					{
						if (_currentDocumentFolder.IsNotNullOrEmpty())
							dialog.InitialDirectory = _currentDocumentFolder;

						dialog.Filter = _documentContainer.FileDialogFilter;
						dialog.FilterIndex = _selectedFilterIndex;

						if (dialog.ShowDialog(this) == DialogResult.OK)
						{
							_selectedFilterIndex = dialog.FilterIndex;
							_currentDocumentFolder = Path.GetDirectoryName(dialog.FileName);
							_documentContainer.LoadDocument(dialog.FileName, _selectedFilterIndex);
						}
					}
					catch (Exception exception)
					{
						if (exception.IsCritical()) throw;
						else ShowError(exception);
					}
				}
		}

		#endregion

		#region SaveDocument

		/// <summary>Saves the document to a file.</summary>
		public void SaveDocument()
		{
			if (_documentContainer == null)
				throw new InvalidOperationException("DocumentContainer is not initialized.");

			TryToSaveDocument();
		}

		/// <summary>Saves the document to a different file.</summary>
		public void SaveDocumentAs()
		{
			if (_documentContainer == null)
				throw new InvalidOperationException("DocumentContainer is not initialized.");

			try
			{
				SaveDocumentOrCancel(GetFilePathFromSaveFileDialog(_documentContainer.FilePath));
			}
			catch (Exception exception)
			{
				if (exception.IsCritical()) throw;
				else ShowError(exception);
			}
		}

		private bool TryToSaveDocument()
		{
			try
			{
				string filePath = _documentContainer.FilePath;
				if (filePath.IsNullOrEmpty())
					filePath = GetFilePathFromSaveFileDialog(_documentContainer.FileName);

				return SaveDocumentOrCancel(filePath);
			}
			catch (Exception exception)
			{
				if (exception.IsCritical()) throw;
				else ShowError(exception);
			}

			return false;
		}

		private string GetFilePathFromSaveFileDialog(string filePath)
		{
			using (SaveFileDialog dialog = new SaveFileDialog())
			{
				dialog.Filter = _documentContainer.FileDialogFilter;
				dialog.FilterIndex = _selectedFilterIndex;

				if (_currentDocumentFolder.IsNotNullOrEmpty())
					dialog.InitialDirectory = _currentDocumentFolder;

				if (filePath.IsNotNullOrEmpty())
					dialog.FileName = filePath;

				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					_selectedFilterIndex = dialog.FilterIndex;
					_currentDocumentFolder = Path.GetDirectoryName(dialog.FileName);
					return dialog.FileName;
				}
			}

			return null;
		}

		private bool SaveDocumentOrCancel(string filePath)
		{
			if (filePath.IsNotNullOrEmpty())
				try
				{
					_documentContainer.SaveDocument(filePath, _selectedFilterIndex);
					return true;
				}
				catch (Exception exception)
				{
					if (exception.IsCritical()) throw;
					else ShowError(exception);
				}

			return false;
		}

		private bool AskToSaveDocument()
		{
			switch (AskYesNoCancel(
				"Save changes to \"{0}\"?".FormatInvariant(_documentContainer.FileName ?? String.Empty),
				false))
			{
				case DialogResult.Yes: return TryToSaveDocument();
				case DialogResult.No: return true;
				default: /* DialogResult.Cancel */ return false;
			}
		}

		private int _selectedFilterIndex;
		private string _currentDocumentFolder;

		#endregion

		#region loading and saving settings

		/// <summary>Loads the base document form settings.</summary>
		/// <param name="settings">The settings to load.</param>
		protected void LoadBaseFormSettings(DocumentFormSettings settings)
		{
			base.LoadBaseFormSettings(settings);

			_currentDocumentFolder = settings.CurrentDocumentFolder;
			_selectedFilterIndex = settings.SelectedFilterIndex;
		}

		/// <summary>Saves the base document form settings.</summary>
		/// <param name="settings">The settings to save.</param>
		protected void SaveBaseFormSettings(DocumentFormSettings settings)
		{
			base.SaveBaseFormSettings(settings);

			settings.CurrentDocumentFolder = _currentDocumentFolder;
			settings.SelectedFilterIndex = settings.SelectedFilterIndex;
		}

		#endregion

		/// <summary>Raises the <see cref="E:Closing"/> event.</summary>
		/// <param name="e">A <see cref="T:CancelEventArgs"/> that contains the event data.</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (!e.Cancel
				&& _documentContainer != null
				&& _documentContainer.IsModified
				&& !AskToSaveDocument())
			{
				e.Cancel = true;
			}
		}
	}
}
