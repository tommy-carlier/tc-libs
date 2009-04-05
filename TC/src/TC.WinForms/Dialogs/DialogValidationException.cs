// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Dialogs
{
	/// <summary>Represents an error when validating dialog controls.</summary>
	public class DialogValidationException : Exception
	{
		/// <summary>Initializes a new instance of the <see cref="DialogValidationException"/> class.</summary>
		/// <param name="invalidControl">The invalid control that caused the error.</param>
		/// <param name="message">The error message.</param>
		public DialogValidationException(Control invalidControl, string message)
			: base(message)
		{
			fInvalidControl = invalidControl;
		}

		private readonly Control fInvalidControl;

		/// <summary>Gets the invalid control that caused the error.</summary>
		/// <value>The invalid control.</value>
		public Control InvalidControl { get { return fInvalidControl; } }
	}
}
