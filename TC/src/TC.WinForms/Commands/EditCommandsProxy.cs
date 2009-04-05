// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TC.WinForms.Commands
{
	/// <summary>Contains the standard edit-commands, whose implementation is pluggable.</summary>
	[DefaultProperty("Implementation")]
	public sealed class EditCommandsProxy : Component
	{
		/// <summary>Initializes a new instance of the <see cref="EditCommandsProxy"/> class.</summary>
		public EditCommandsProxy() { }

		/// <summary>Initializes a new instance of the <see cref="EditCommandsProxy"/> class.</summary>
		/// <param name="container">The container to add this instance to.</param>
		public EditCommandsProxy(IContainer container)
		{
			container.Add(this);
		}

		#region command properties

		private readonly ProxyCommand fUndoCommand = new ProxyCommand();

		/// <summary>Gets the 'Undo'-command.</summary>
		/// <value>The 'Undo'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand UndoCommand { get { return fUndoCommand; } }

		private readonly ProxyCommand fRedoCommand = new ProxyCommand();

		/// <summary>Gets the 'Redo'-command.</summary>
		/// <value>The 'Redo'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand RedoCommand { get { return fRedoCommand; } }

		private readonly ProxyCommand fCutCommand = new ProxyCommand();

		/// <summary>Gets the 'Cut'-command.</summary>
		/// <value>The 'Cut'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand CutCommand { get { return fCutCommand; } }

		private readonly ProxyCommand fCopyCommand = new ProxyCommand();

		/// <summary>Gets the 'Copy'-command.</summary>
		/// <value>The 'Copy'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand CopyCommand { get { return fCopyCommand; } }

		private readonly ProxyCommand fPasteCommand = new ProxyCommand();

		/// <summary>Gets the 'Paste'-command.</summary>
		/// <value>The 'Paste'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand PasteCommand { get { return fPasteCommand; } }

		private readonly ProxyCommand fDeleteCommand = new ProxyCommand();

		/// <summary>Gets the 'Delete'-command.</summary>
		/// <value>The 'Delete'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand DeleteCommand { get { return fDeleteCommand; } }

		private readonly ProxyCommand fSelectAllCommand = new ProxyCommand();

		/// <summary>Gets the 'Select All'-command.</summary>
		/// <value>The 'Select All'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand SelectAllCommand { get { return fSelectAllCommand; } }

		#endregion

		#region Implementation members

		private IProxyCommandsImplementation<EditCommand> fImplementation;

		/// <summary>Gets or sets the implementation of the edit-commands.</summary>
		/// <value>The implementation of the edit-commands.</value>
		[Category("Behavior"), DefaultValue(null)]
		[Description("The implementation of the edit-commands.")]
		public IProxyCommandsImplementation<EditCommand> Implementation
		{
			get { return fImplementation; }
			set
			{
				if (fImplementation != value)
				{
					fImplementation = value;
					SetImplementation(value ?? EmptyProxyCommandsImplementation<EditCommand>.Instance);
				}
			}
		}

		private void SetImplementation(IProxyCommandsImplementation<EditCommand> implementation)
		{
			fUndoCommand.Implementation = implementation.GetCommand(EditCommand.Undo);
			fRedoCommand.Implementation = implementation.GetCommand(EditCommand.Redo);
			fCutCommand.Implementation = implementation.GetCommand(EditCommand.Cut);
			fCopyCommand.Implementation = implementation.GetCommand(EditCommand.Copy);
			fPasteCommand.Implementation = implementation.GetCommand(EditCommand.Paste);
			fDeleteCommand.Implementation = implementation.GetCommand(EditCommand.Delete);
			fSelectAllCommand.Implementation = implementation.GetCommand(EditCommand.SelectAll);
		}

		#endregion
	}
}
