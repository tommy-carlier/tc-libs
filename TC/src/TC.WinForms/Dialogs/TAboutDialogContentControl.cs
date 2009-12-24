// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Dialogs
{
	/// <summary>Represents the content of the About-dialog.</summary>
	[SuppressMessage(
		"Microsoft.Performance",
		"CA1812:AvoidUninstantiatedInternalClasses",
		Justification = "This class is instantiated by TApplication.ShowAboutDialog")]
	internal partial class TAboutDialogContentControl : TDialogContentControl
	{
		/// <summary>Initializes a new instance of the <see cref="TAboutDialogContentControl"/> class.</summary>
		public TAboutDialogContentControl()
		{
			InitializeComponent();
		}

		/// <summary>Raises the <see cref="E:Load"/> event.</summary>
		/// <param name="e">An <see cref="T:EventArgs"/> that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			SuspendLayout();

			string title = TApplication.Title;
			Text = "About " + title;

			LabelTitle.Text = LabelTitle.Text.Replace("{Title}", title);
			LabelVersion.Text = LabelVersion.Text.Replace("{Version}", TApplication.Version.ToString());
			LabelCopyright.Text = LabelCopyright.Text.Replace("{Copyright}", TApplication.Copyright);
			Hyperlink.Text = Hyperlink.Text.Replace("{URL}", TApplication.WebsiteDisplayString);

			if (Hyperlink.Text.IsNullOrEmpty())
				Hyperlink.Visible = false;

			ResumeLayout();
		}

		private void HandlerVisitWebsiteCommandExecuted(object sender, EventArgs e)
		{
			Uri uri = TApplication.WebsiteUri;
			if (uri != null)
			{
				try
				{
					Process.Start(uri.ToString());
				}
				catch (Exception exception)
				{
					if ((exception is Win32Exception)
						|| (exception is ObjectDisposedException)
						|| (exception is FileNotFoundException))
					{
						ShowError(exception);
					}
					else throw;
				}
			}
		}
	}
}
