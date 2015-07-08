// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.WinForms
{
	/// <summary>Represents a component that contains and manages a document.</summary>
	public interface IDocumentContainer
	{
		/// <summary>Gets the full path of the document file.</summary>
		/// <value>The full path of the document file, or null if it hasn't been saved yet.</value>
		string FilePath { get; }

		/// <summary>Occurs when the value of the <see cref="P:FilePath"/> property has changed.</summary>
		event EventHandler FilePathChanged;

		/// <summary>Gets the file name of the document file.</summary>
		/// <value>The file name of the document file.</value>
		string FileName { get; }

		/// <summary>Occurs when the value of the <see cref="P:FileName"/> property has changed.</summary>
		event EventHandler FileNameChanged;

		/// <summary>Gets a value indicating whether the document is modified.</summary>
		/// <value><c>true</c> if the document is modified; otherwise, <c>false</c>.</value>
		bool IsModified { get; }

		/// <summary>Occurs when the value of the <see cref="P:IsModified"/> property has changed.</summary>
		event EventHandler IsModifiedChanged;

		/// <summary>Gets the filter that is used in the dialog to open or save the document.</summary>
		/// <value>The filter that is used in the dialog to open or save the document.</value>
		string FileDialogFilter { get; }

		/// <summary>Clears the document.</summary>
		/// <param name="fileName">The file name of the document.</param>
		void ClearDocument(string fileName);

		/// <summary>Loads the document from the specified file path.</summary>
		/// <param name="filePath">The full path of the document to load.</param>
		/// <param name="filterIndex">The index of the selected filter in the dialog.</param>
		void LoadDocument(string filePath, int filterIndex);

		/// <summary>Saves the document to specified file path.</summary>
		/// <param name="filePath">The full path of the file to save the document to.</param>
		/// <param name="filterIndex">The index of the selected filter in the dialog.</param>
		void SaveDocument(string filePath, int filterIndex);
	}
}
