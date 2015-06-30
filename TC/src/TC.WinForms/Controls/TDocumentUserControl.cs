// TC WinForms Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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
			EventHandler handler = Events[_documentContainerChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		#endregion

		#region IDocumentContainer Members

		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "TDocumentUserControl forwards all of its calls to IDocumentContainer-members to fDocumentContainer.")]
		string IDocumentContainer.FilePath
		{
			get { return _documentContainer != null ? _documentContainer.FilePath : null; }
		}

		private static readonly object _filePathChanged = new object();

		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "TDocumentUserControl forwards all of its calls to IDocumentContainer-members to fDocumentContainer.")]
		event EventHandler IDocumentContainer.FilePathChanged
		{
			add { Events.AddHandler(_filePathChanged, value); }
			remove { Events.RemoveHandler(_filePathChanged, value); }
		}

		private void HandlerDocumentContainerFilePathChanged(object sender, EventArgs e)
		{
			EventHandler handler = Events[_filePathChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "TDocumentUserControl forwards all of its calls to IDocumentContainer-members to fDocumentContainer.")]
		string IDocumentContainer.FileName
		{
			get { return _documentContainer != null ? _documentContainer.FileName : null; }
		}

		private static readonly object _fileNameChanged = new object();

		[SuppressMessage(
		"Microsoft.Design",
		"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
		Justification = "TDocumentUserControl forwards all of its calls to IDocumentContainer-members to fDocumentContainer.")]
		event EventHandler IDocumentContainer.FileNameChanged
		{
			add { Events.AddHandler(_fileNameChanged, value); }
			remove { Events.RemoveHandler(_fileNameChanged, value); }
		}

		private void HandlerDocumentContainerFileNameChanged(object sender, EventArgs e)
		{
			EventHandler handler = Events[_fileNameChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "TDocumentUserControl forwards all of its calls to IDocumentContainer-members to fDocumentContainer.")]
		bool IDocumentContainer.IsModified
		{
			get { return _documentContainer != null && _documentContainer.IsModified; }
		}

		private static readonly object _isModifiedChanged = new object();

		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "TDocumentUserControl forwards all of its calls to IDocumentContainer-members to fDocumentContainer.")]
		event EventHandler IDocumentContainer.IsModifiedChanged
		{
			add { Events.AddHandler(_isModifiedChanged, value); }
			remove { Events.RemoveHandler(_isModifiedChanged, value); }
		}

		private void HandlerDocumentContainerIsModifiedChanged(object sender, EventArgs e)
		{
			EventHandler handler = Events[_isModifiedChanged] as EventHandler;
			if (handler != null)
				handler(this, e);
		}

		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "TDocumentUserControl forwards all of its calls to IDocumentContainer-members to fDocumentContainer.")]
		string IDocumentContainer.FileDialogFilter
		{
			get
			{
				return _documentContainer != null ? _documentContainer.FileDialogFilter : null;
			}
		}

		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "TDocumentUserControl forwards all of its calls to IDocumentContainer-members to fDocumentContainer.")]
		void IDocumentContainer.ClearDocument(string fileName)
		{
			if (_documentContainer != null)
				_documentContainer.ClearDocument(fileName);
		}

		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "TDocumentUserControl forwards all of its calls to IDocumentContainer-members to fDocumentContainer.")]
		void IDocumentContainer.LoadDocument(string filePath, int filterIndex)
		{
			if (_documentContainer != null)
				_documentContainer.LoadDocument(filePath, filterIndex);
		}

		[SuppressMessage(
			"Microsoft.Design",
			"CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "TDocumentUserControl forwards all of its calls to IDocumentContainer-members to fDocumentContainer.")]
		void IDocumentContainer.SaveDocument(string filePath, int filterIndex)
		{
			if (_documentContainer != null)
				_documentContainer.SaveDocument(filePath, filterIndex);
		}

		#endregion
	}
}
