// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

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

			string lTitle = TApplication.Title;
			Text = "About " + lTitle;

			LabelTitle.Text = LabelTitle.Text.Replace("{Title}", lTitle);
			LabelVersion.Text = LabelVersion.Text.Replace("{Version}", TApplication.Version.ToString());
			LabelCopyright.Text = LabelCopyright.Text.Replace("{Copyright}", TApplication.Copyright);
			Hyperlink.Text = Hyperlink.Text.Replace("{URL}", TApplication.WebsiteDisplayString);
			if (string.IsNullOrEmpty(Hyperlink.Text)) Hyperlink.Visible = false;

			ResumeLayout();
		}

		private void HandlerVisitWebsiteCommandExecuted(object sender, EventArgs e)
		{
			Uri lUri = TApplication.WebsiteUri;
			if (lUri != null)
				try { Process.Start(lUri.ToString()); }
				catch (Win32Exception lException) { ShowError(lException); }
				catch (ObjectDisposedException lException) { ShowError(lException); }
				catch (FileNotFoundException lException) { ShowError(lException); }
		}
	}
}
