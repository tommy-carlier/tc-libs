// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Dialogs
{
	/// <summary>Represents an error when validating dialog controls.</summary>
	[Serializable]
	public class DialogValidationException : Exception
	{
		/// <summary>Initializes a new instance of the <see cref="DialogValidationException"/> class.</summary>
		/// <param name="invalidControl">The invalid control that caused the error.</param>
		/// <param name="message">The error message.</param>
		public DialogValidationException(Control invalidControl, string message)
			: base(message)
		{
			_invalidControl = invalidControl;
		}

		private readonly Control _invalidControl;

		/// <summary>Gets the invalid control that caused the error.</summary>
		/// <value>The invalid control.</value>
		public Control InvalidControl { get { return _invalidControl; } }

		/// <summary>Initializes a new instance of the <see cref="T:DialogValidationException"/> class.</summary>
		public DialogValidationException() { }

		/// <summary>Initializes a new instance of the <see cref="DialogValidationException"/> class.</summary>
		/// <param name="message">The error message.</param>
		public DialogValidationException(string message) : base(message) { }

		/// <summary>Initializes a new instance of the <see cref="DialogValidationException"/> class.</summary>
		/// <param name="message">The error message.</param>
		/// <param name="inner">The exception that caused this exception to be thrown.</param>
		public DialogValidationException(string message, Exception inner)
			: base(message, inner) { }

		/// <summary>Initializes a new instance of the <see cref="DialogValidationException"/> class.</summary>
		/// <param name="info">The <see cref="T:SerializationInfo"/> that holds the serialized object data
		/// about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:StreamingContext"/> that contains contextual information 
		/// about the source or destination.</param>
		/// <exception cref="T:ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
		/// <exception cref="T:SerializationException">The class name is null or <see cref="P:HResult"/> is zero (0).</exception>
		protected DialogValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		/// <summary>When overridden in a derived class, sets the <see cref="T:SerializationInfo"/> 
		/// with information about the exception.</summary>
		/// <param name="info">The <see cref="T:SerializationInfo"/> that holds the serialized object data
		/// about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:StreamingContext"/> that contains contextual information
		/// about the source or destination.</param>
		/// <exception cref="T:ArgumentNullException">The <paramref name="info"/> parameter is a null reference 
		/// (Nothing in Visual Basic).</exception>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}
