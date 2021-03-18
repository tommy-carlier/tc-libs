// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System.ComponentModel;

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

		private readonly ProxyCommand _undoCommand = new ProxyCommand();

		/// <summary>Gets the 'Undo'-command.</summary>
		/// <value>The 'Undo'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand UndoCommand { get { return _undoCommand; } }

		private readonly ProxyCommand _redoCommand = new ProxyCommand();

		/// <summary>Gets the 'Redo'-command.</summary>
		/// <value>The 'Redo'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand RedoCommand { get { return _redoCommand; } }

		private readonly ProxyCommand _cutCommand = new ProxyCommand();

		/// <summary>Gets the 'Cut'-command.</summary>
		/// <value>The 'Cut'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand CutCommand { get { return _cutCommand; } }

		private readonly ProxyCommand _copyCommand = new ProxyCommand();

		/// <summary>Gets the 'Copy'-command.</summary>
		/// <value>The 'Copy'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand CopyCommand { get { return _copyCommand; } }

		private readonly ProxyCommand _pasteCommand = new ProxyCommand();

		/// <summary>Gets the 'Paste'-command.</summary>
		/// <value>The 'Paste'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand PasteCommand { get { return _pasteCommand; } }

		private readonly ProxyCommand _deleteCommand = new ProxyCommand();

		/// <summary>Gets the 'Delete'-command.</summary>
		/// <value>The 'Delete'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand DeleteCommand { get { return _deleteCommand; } }

		private readonly ProxyCommand _selectAllCommand = new ProxyCommand();

		/// <summary>Gets the 'Select All'-command.</summary>
		/// <value>The 'Select All'-command.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ICommand SelectAllCommand { get { return _selectAllCommand; } }

		#endregion

		#region Implementation members

		private IProxyCommandsImplementation<EditCommand> _implementation;

		/// <summary>Gets or sets the implementation of the edit-commands.</summary>
		/// <value>The implementation of the edit-commands.</value>
		[Category("Behavior"), DefaultValue(null)]
		[Description("The implementation of the edit-commands.")]
		public IProxyCommandsImplementation<EditCommand> Implementation
		{
			get
			{
				return _implementation;
			}

			set
			{
				if (_implementation != value)
				{
					_implementation = value;
					SetImplementation(value ?? EmptyProxyCommandsImplementation<EditCommand>.Instance);
				}
			}
		}

		private void SetImplementation(IProxyCommandsImplementation<EditCommand> implementation)
		{
			_undoCommand.Implementation = implementation.GetCommand(EditCommand.Undo);
			_redoCommand.Implementation = implementation.GetCommand(EditCommand.Redo);
			_cutCommand.Implementation = implementation.GetCommand(EditCommand.Cut);
			_copyCommand.Implementation = implementation.GetCommand(EditCommand.Copy);
			_pasteCommand.Implementation = implementation.GetCommand(EditCommand.Paste);
			_deleteCommand.Implementation = implementation.GetCommand(EditCommand.Delete);
			_selectAllCommand.Implementation = implementation.GetCommand(EditCommand.SelectAll);
		}

		#endregion
	}
}
