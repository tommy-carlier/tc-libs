﻿// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

namespace TC.WinForms.Commands
{
	/// <summary>Identifies one of the standard edit-commands.</summary>
	public enum EditCommand
	{
		/// <summary>No command.</summary>
		None = 0,

		/// <summary>The 'Undo'-command.</summary>
		Undo,

		/// <summary>The 'Redo'-command.</summary>
		Redo,

		/// <summary>The 'Cut'-command.</summary>
		Cut,

		/// <summary>The 'Copy'-command.</summary>
		Copy,

		/// <summary>The 'Paste'-command.</summary>
		Paste,

		/// <summary>The 'Delete'-command.</summary>
		Delete,

		/// <summary>The 'Select All'-command.</summary>
		SelectAll
	}
}
