// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a <see cref="T:UserControl"/> that contains a document.</summary>
	public class TDocumentUserControl : TUserControl, IDocumentContainer
	{
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
					if (value == this)
						throw new InvalidOperationException("You cannot assign an instance to its own DocumentContainer property.");

					if (_documentContainer != null)
					{
						_documentContainer.FilePathChanged -= HandlerDocumentContainerFilePathChanged;
						_documentContainer.FileNameChanged -= HandlerDocumentContainerFileNameChanged;
						_documentContainer.IsModifiedChanged -= HandlerDocumentContainerIsModifiedChanged;
					}

					if (value != null)
					{
						value.FilePathChanged += HandlerDocumentContainerFilePathChanged;
						value.FileNameChanged += HandlerDocumentContainerFileNameChanged;
						value.IsModifiedChanged += HandlerDocumentContainerIsModifiedChanged;
					}

					_documentContainer = value;
					OnDocumentContainerChanged(EventArgs.Empty);
					HandlerDocumentContainerFilePathChanged(_documentContainer, EventArgs.Empty);
					HandlerDocumentContainerFileNameChanged(_documentContainer, EventArgs.Empty);
					HandlerDocumentContainerIsModifiedChanged(_documentContainer, EventArgs.Empty);
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
			this.TriggerEvent(Events, _documentContainerChanged, e);
			(Events[_documentContainerChanged] as EventHandler)?.Invoke(this, e);
        }

        #endregion

        #region IDocumentContainer Members

        string IDocumentContainer.FilePath
		{
			get { return _documentContainer?.FilePath; }
		}

		private static readonly object _filePathChanged = new object();

        event EventHandler IDocumentContainer.FilePathChanged
		{
			add { Events.AddHandler(_filePathChanged, value); }
			remove { Events.RemoveHandler(_filePathChanged, value); }
		}

		private void HandlerDocumentContainerFilePathChanged(object sender, EventArgs e)
		{
			this.TriggerEvent(Events, _filePathChanged, e);
        }

        string IDocumentContainer.FileName
		{
			get { return _documentContainer?.FileName; }
		}

		private static readonly object _fileNameChanged = new object();

        event EventHandler IDocumentContainer.FileNameChanged
		{
			add { Events.AddHandler(_fileNameChanged, value); }
			remove { Events.RemoveHandler(_fileNameChanged, value); }
		}

		private void HandlerDocumentContainerFileNameChanged(object sender, EventArgs e)
		{
			this.TriggerEvent(Events, _fileNameChanged, e);
        }

        bool IDocumentContainer.IsModified
		{
			get { return _documentContainer != null && _documentContainer.IsModified; }
		}

		private static readonly object _isModifiedChanged = new object();

        event EventHandler IDocumentContainer.IsModifiedChanged
		{
			add { Events.AddHandler(_isModifiedChanged, value); }
			remove { Events.RemoveHandler(_isModifiedChanged, value); }
		}

		private void HandlerDocumentContainerIsModifiedChanged(object sender, EventArgs e)
		{
			this.TriggerEvent(Events, _isModifiedChanged, e);
		}

		string IDocumentContainer.FileDialogFilter
		{
			get
			{
				return _documentContainer?.FileDialogFilter;
			}
		}

        void IDocumentContainer.ClearDocument(string fileName)
		{
			if (_documentContainer != null)
				_documentContainer.ClearDocument(fileName);
		}

        void IDocumentContainer.LoadDocument(string filePath, int filterIndex)
		{
			if (_documentContainer != null)
				_documentContainer.LoadDocument(filePath, filterIndex);
		}

        void IDocumentContainer.SaveDocument(string filePath, int filterIndex)
		{
			if (_documentContainer != null)
				_documentContainer.SaveDocument(filePath, filterIndex);
		}

		#endregion
	}
}
