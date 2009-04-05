// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a <see cref="T:UserControl"/> that contains a document.</summary>
	public class TDocumentUserControl : TUserControl, IDocumentContainer
	{
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
					if (value == this)
						throw new InvalidOperationException("You cannot assign an instance to its own DocumentContainer property.");

					if (fDocumentContainer != null)
					{
						fDocumentContainer.FilePathChanged -= HandlerDocumentContainerFilePathChanged;
						fDocumentContainer.FileNameChanged -= HandlerDocumentContainerFileNameChanged;
						fDocumentContainer.IsModifiedChanged -= HandlerDocumentContainerIsModifiedChanged;
					}

					if (value != null)
					{
						value.FilePathChanged += HandlerDocumentContainerFilePathChanged;
						value.FileNameChanged += HandlerDocumentContainerFileNameChanged;
						value.IsModifiedChanged += HandlerDocumentContainerIsModifiedChanged;
					}

					fDocumentContainer = value;
					OnDocumentContainerChanged(EventArgs.Empty);
					HandlerDocumentContainerFilePathChanged(fDocumentContainer, EventArgs.Empty);
					HandlerDocumentContainerFileNameChanged(fDocumentContainer, EventArgs.Empty);
					HandlerDocumentContainerIsModifiedChanged(fDocumentContainer, EventArgs.Empty);
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

		#region IDocumentContainer Members

		string IDocumentContainer.FilePath
		{
			get { return fDocumentContainer != null ? fDocumentContainer.FilePath : null; }
		}

		private static readonly object fEventFilePathChanged = new object();
		event EventHandler IDocumentContainer.FilePathChanged
		{
			add { Events.AddHandler(fEventFilePathChanged, value); }
			remove { Events.RemoveHandler(fEventFilePathChanged, value); }
		}

		private void HandlerDocumentContainerFilePathChanged(object sender, EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventFilePathChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		string IDocumentContainer.FileName
		{
			get { return fDocumentContainer != null ? fDocumentContainer.FileName : null; }
		}

		private static readonly object fEventFileNameChanged = new object();
		event EventHandler IDocumentContainer.FileNameChanged
		{
			add { Events.AddHandler(fEventFileNameChanged, value); }
			remove { Events.RemoveHandler(fEventFileNameChanged, value); }
		}

		private void HandlerDocumentContainerFileNameChanged(object sender, EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventFileNameChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		bool IDocumentContainer.IsModified
		{
			get { return fDocumentContainer != null && fDocumentContainer.IsModified; }
		}

		private static readonly object fEventIsModifiedChanged = new object();
		event EventHandler IDocumentContainer.IsModifiedChanged
		{
			add { Events.AddHandler(fEventIsModifiedChanged, value); }
			remove { Events.RemoveHandler(fEventIsModifiedChanged, value); }
		}

		private void HandlerDocumentContainerIsModifiedChanged(object sender, EventArgs e)
		{
			EventHandler lEventHandler = Events[fEventIsModifiedChanged] as EventHandler;
			if (lEventHandler != null)
				lEventHandler(this, e);
		}

		string IDocumentContainer.FileDialogFilter { get { return fDocumentContainer != null ? fDocumentContainer.FileDialogFilter : null; } }

		void IDocumentContainer.ClearDocument(string fileName)
		{
			if (fDocumentContainer != null)
				fDocumentContainer.ClearDocument(fileName);
		}

		void IDocumentContainer.LoadDocument(string filePath, int filterIndex)
		{
			if (fDocumentContainer != null)
				fDocumentContainer.LoadDocument(filePath, filterIndex);
		}

		void IDocumentContainer.SaveDocument(string filePath, int filterIndex)
		{
			if (fDocumentContainer != null)
				fDocumentContainer.SaveDocument(filePath, filterIndex);
		}

		#endregion
	}
}
