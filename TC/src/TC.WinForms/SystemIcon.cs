﻿// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.WinForms
{
	/// <summary>Represents one of the system icons.</summary>
	public enum SystemIcon
	{
		/// <summary>No icon (empty icon).</summary>
		None = 0,

		/// <summary>A custom image.</summary>
		Custom,

		/// <summary>The icon of the current form or dialog.</summary>
		FormIcon,

		/// <summary>The information icon.</summary>
		Information,

		/// <summary>The question icon.</summary>
		Question,

		/// <summary>The warning icon.</summary>
		Warning,

		/// <summary>The error icon.</summary>
		Error
	}
}