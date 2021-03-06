﻿// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;

namespace TC.WinForms.Commands
{
	/// <summary>Represents a control (or component) that can execute commands.</summary>
	public interface ICommandControl : IDisposable
	{
		/// <summary>Notifies the control that the bound command is enabled or disabled.</summary>
		/// <param name="enabled">if set to <c>true</c> the control should be enabled, otherwise disabled.</param>
		void SetCommandEnabled(bool enabled);

		/// <summary>Occurs when the control is activated.</summary>
		/// <remarks>When this event occurs, the bound command will be executed.</remarks>
		event EventHandler Activated;

		/// <summary>Occurs when the control has been disposed.</summary>
		event EventHandler Disposed;
	}
}
