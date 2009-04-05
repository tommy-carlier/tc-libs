// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

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
			fClearDocumentCommand = new SimpleActionCommand(ClearDocument);
			fLoadDocumentCommand = new SimpleActionCommand(LoadDocument);
			fSaveDocumentCommand = new SimpleActionCommand(SaveDocument);
			fSaveDocumentAsCommand = new SimpleActionCommand(SaveDocumentAs);
		}

		#region commands

		private readonly ICommand fClearDocumentCommand;

		/// <summary>Gets the command to clear the document.</summary>
		/// <value>The command to clear the document.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand ClearDocumentCommand { get { return fClearDocumentCommand; } }

		private readonly ICommand fLoadDocumentCommand;

		/// <summary>Gets the command to load a document.</summary>
		/// <value>The command to load a document.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand LoadDocumentCommand { get { return fLoadDocumentCommand; } }

		private readonly ICommand fSaveDocumentCommand;

		/// <summary>Gets the command to save the current document.</summary>
		/// <value>The command to save the current document.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand SaveDocumentCommand { get { return fSaveDocumentCommand; } }

		private readonly ICommand fSaveDocumentAsCommand;

		/// <summary>Gets the command to save the current document as a different file.</summary>
		/// <value>The command to save the current document as a different file.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand SaveDocumentAsCommand { get { return fSaveDocumentAsCommand; } }

		#endregion

		#region DocumentContainer members

		private IDocumentContainer fDocumentContainer;

		/// <summary>Gets or sets the component that contains the actual document.</summary>
		/// <value>The component that contains the actual document.</value>
		[Category("Behavior"), Description("The component that contains the document."), DefaultValue(null)]
		public IDocumentContainer DocumentContainer
		{
			get { return fDocumentContainer; }
			set
			{
				if (fDocumentContainer != value)
				{
					if (fDocumentContainer != null)
					{
						fDocumentContainer.FileNameChanged -= RaiseDocumentTitleChanged;
						fDocumentContainer.IsModifiedChanged -= RaiseDocumentTitleChanged;
					}

					if (value != null)
					{
						value.FileNameChanged += RaiseDocumentTitleChanged;
						value.IsModifiedChanged += RaiseDocumentTitleChanged;
					}

					fDocumentContainer = value;
					OnDocumentContainerChanged(EventArgs.Empty);
					RaiseDocumentTitleChanged(fDocumentContainer, EventArgs.Empty);
				}
			}
		}

		private static readonly object fEventDocumentContainerChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:DocumentContainer"/> property has changed.</summary>
		public event EventHandler DocumentContainerChanged
		{
			add { Events.AddHandler(fEventDocumentContainerChanged, value); }
			remove { Events.RemoveHandler(fEventDocumentContainerChanged, value); }
		}

		/// <summary>Raises the <see cref="E:DocumentContainerChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnDocumentContainerChanged(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventDocumentContainerChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
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
				return fDocumentContainer != null
					? (fDocumentContainer.FileName ?? String.Empty)
						+ (fDocumentContainer.IsModified ? "*" : String.Empty)
					: String.Empty;
			}
		}

		private static readonly object fEventDocumentTitleChanged = new object();

		/// <summary>Occurs when the value of the <see cref="P:DocumentTitle"/> property has changed.</summary>
		public event EventHandler DocumentTitleChanged
		{
			add { Events.AddHandler(fEventDocumentTitleChanged, value); }
			remove { Events.RemoveHandler(fEventDocumentTitleChanged, value); }
		}

		/// <summary>Raises the <see cref="E:DocumentTitleChanged"/> event.</summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected virtual void OnDocumentTitleChanged(EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventDocumentTitleChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
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
			if (fDocumentContainer == null)
				throw new InvalidOperationException("DocumentContainer is not initialized.");

			if (!fDocumentContainer.IsModified || AskToSaveDocument())
				fDocumentContainer.ClearDocument(String.Empty);
		}

		#endregion

		#region LoadDocument

		/// <summary>Loads the document from a file.</summary>
		public void LoadDocument()
		{
			if (fDocumentContainer == null)
				throw new InvalidOperationException("DocumentContainer is not initialized.");

			if (!fDocumentContainer.IsModified || AskToSaveDocument())
				using (OpenFileDialog lDialog = new OpenFileDialog())
				{
					try
					{
						if (!string.IsNullOrEmpty(fCurrentDocumentFolder))
							lDialog.InitialDirectory = fCurrentDocumentFolder;

						lDialog.Filter = fDocumentContainer.FileDialogFilter;
						lDialog.FilterIndex = fSelectedFilterIndex;

						if (lDialog.ShowDialog(this) == DialogResult.OK)
						{
							fSelectedFilterIndex = lDialog.FilterIndex;
							fCurrentDocumentFolder = Path.GetDirectoryName(lDialog.FileName);
							fDocumentContainer.LoadDocument(lDialog.FileName, fSelectedFilterIndex);
						}
					}
					catch (Exception lException)
					{
						if (lException.IsCritical()) throw;
						else ShowError(lException);
					}
				}
		}

		#endregion

		#region SaveDocument

		/// <summary>Saves the document to a file.</summary>
		public void SaveDocument()
		{
			if (fDocumentContainer == null)
				throw new InvalidOperationException("DocumentContainer is not initialized.");

			TryToSaveDocument();
		}

		/// <summary>Saves the document to a different file.</summary>
		public void SaveDocumentAs()
		{
			if (fDocumentContainer == null)
				throw new InvalidOperationException("DocumentContainer is not initialized.");

			try
			{
				SaveDocumentOrCancel(GetFilePathFromSaveFileDialog(fDocumentContainer.FilePath));
			}
			catch (Exception lException)
			{
				if (lException.IsCritical()) throw;
				else ShowError(lException);
			}
		}

		private bool TryToSaveDocument()
		{
			try
			{
				string lFilePath = fDocumentContainer.FilePath;
				if (string.IsNullOrEmpty(lFilePath))
					lFilePath = GetFilePathFromSaveFileDialog(fDocumentContainer.FileName);

				return SaveDocumentOrCancel(lFilePath);
			}
			catch (Exception lException)
			{
				if (lException.IsCritical()) throw;
				else ShowError(lException);
			}

			return false;
		}

		private string GetFilePathFromSaveFileDialog(string filePath)
		{
			using (SaveFileDialog lDialog = new SaveFileDialog())
			{
				lDialog.Filter = fDocumentContainer.FileDialogFilter;
				lDialog.FilterIndex = fSelectedFilterIndex;

				if (!string.IsNullOrEmpty(fCurrentDocumentFolder))
					lDialog.InitialDirectory = fCurrentDocumentFolder;

				if (!string.IsNullOrEmpty(filePath))
					lDialog.FileName = filePath;

				if (lDialog.ShowDialog(this) == DialogResult.OK)
				{
					fSelectedFilterIndex = lDialog.FilterIndex;
					fCurrentDocumentFolder = Path.GetDirectoryName(lDialog.FileName);
					return lDialog.FileName;
				}
			}

			return null;
		}

		private bool SaveDocumentOrCancel(string filePath)
		{
			if (!string.IsNullOrEmpty(filePath))
				try
				{
					fDocumentContainer.SaveDocument(filePath, fSelectedFilterIndex);
					return true;
				}
				catch (Exception lException)
				{
					if (lException.IsCritical()) throw;
					else ShowError(lException);
				}

			return false;
		}

		private bool AskToSaveDocument()
		{
			switch (AskYesNoCancel(
				"Save changes to \"{0}\"?".FormatInvariant(fDocumentContainer.FileName ?? String.Empty),
				false))
			{
				case DialogResult.Yes: return TryToSaveDocument();
				case DialogResult.No: return true;
				default: /* DialogResult.Cancel */ return false;
			}
		}

		private int fSelectedFilterIndex;
		private string fCurrentDocumentFolder;

		#endregion

		#region loading and saving settings

		/// <summary>Loads the base document form settings.</summary>
		/// <param name="settings">The settings to load.</param>
		protected void LoadBaseFormSettings(DocumentFormSettings settings)
		{
			base.LoadBaseFormSettings(settings);

			fCurrentDocumentFolder = settings.CurrentDocumentFolder;
			fSelectedFilterIndex = settings.SelectedFilterIndex;
		}

		/// <summary>Saves the base document form settings.</summary>
		/// <param name="settings">The settings to save.</param>
		protected void SaveBaseFormSettings(DocumentFormSettings settings)
		{
			base.SaveBaseFormSettings(settings);

			settings.CurrentDocumentFolder = fCurrentDocumentFolder;
			settings.SelectedFilterIndex = settings.SelectedFilterIndex;
		}

		#endregion

		/// <summary>Raises the <see cref="E:Closing"/> event.</summary>
		/// <param name="e">A <see cref="T:CancelEventArgs"/> that contains the event data.</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (!e.Cancel
				&& fDocumentContainer != null
				&& fDocumentContainer.IsModified
				&& !AskToSaveDocument())
			{
				e.Cancel = true;
			}
		}
	}
}
