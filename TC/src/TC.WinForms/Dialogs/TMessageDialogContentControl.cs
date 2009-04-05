// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Dialogs
{
	/// <summary>Represents the content control of a <see cref="T:TMessageDialog"/>.</summary>
	public sealed partial class TMessageDialogContentControl : TDialogContentControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:TMessageDialogContentControl"/> class.</summary>
		public TMessageDialogContentControl()
		{
			InitializeComponent();
		}

		/// <summary>Gets or sets the message to display.</summary>
		/// <value>The message to display.</value>
		public string Message
		{
			get { return MessageLabel.Text; }
			set { MessageLabel.Text = value; }
		}

		/// <summary>Gets or sets the <see cref="T:DialogSideImage"/> to display.</summary>
		/// <value>The <see cref="T:DialogSideImage"/> to display.</value>
		public SystemIcon SideImage
		{
			get { return SideImageControl.SystemIcon; }
			set { SideImageControl.SystemIcon = value; }
		}

		/// <summary>Gets or sets the custom <see cref="T:Image"/> to display.</summary>
		/// <value>The custom <see cref="T:Image"/> to display.</value>
		public Image CustomSideImage
		{
			get { return SideImageControl.CustomImage; }
			set { SideImageControl.CustomImage = value; }
		}
	}
}
