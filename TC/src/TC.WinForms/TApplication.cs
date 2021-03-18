// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.Win32;

using TC.WinForms.Animation;
using TC.WinForms.Commands;
using TC.WinForms.Dialogs;
using TC.WinForms.Forms;

namespace TC.WinForms
{
	/// <summary>Represents the running application.</summary>
	public class TApplication : ApplicationContext
	{
		/// <summary>Initializes a new instance of the <see cref="TApplication"/> class.</summary>
		public TApplication()
		{
			AboutCommand = new SimpleActionCommand(ShowAboutDialog);
		}

        /// <summary>Runs the current application.</summary>
        /// <typeparam name="TApp">The type of the application.</typeparam>
        /// <typeparam name="TMainForm">The type of the main form.</typeparam>
        public static void Run<TApp, TMainForm>()
			where TApp : TApplication, new()
			where TMainForm : TForm, new()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Animator.Initialize();
			SetToolStripRenderer();
			SystemEvents.UserPreferenceChanged += delegate { SetToolStripRenderer(); };

			Current = new TApp();
			TMainForm form = new TMainForm();
			form.Show();
			Application.Run(Current);
		}

		private static void SetToolStripRenderer()
		{
			ToolStripManager.Renderer =
				SystemInformation.HighContrast
					? new ToolStripSystemRenderer()
					: new TToolStripRenderer() as ToolStripRenderer;
		}

        /// <summary>Runs the current application.</summary>
        /// <typeparam name="TMainForm">The type of the main form.</typeparam>
        public static void Run<TMainForm>() where TMainForm : TForm, new()
            => Run<TApplication, TMainForm>();

        /// <summary>Gets the current application.</summary>
        /// <value>The current application.</value>
        public static TApplication Current { get; private set; } = new TApplication();

        #region application information


        /// <summary>Gets the icon of the current application.</summary>
        /// <value>The icon of the current application.</value>
        public static Icon Icon { get; } = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

        /// <summary>Gets the title of the application.</summary>
        /// <value>The title of the application.</value>
        public static string Title => GetEntryAssemblyFirstAttribute<AssemblyTitleAttribute>(false)?.Title ?? String.Empty;

        /// <summary>Gets the version of the application.</summary>
        /// <value>The version of the application.</value>
        public static Version Version => Assembly.GetEntryAssembly().GetName().Version;

        /// <summary>Gets the copyright information of the application.</summary>
        /// <value>The copyright information of the application.</value>
        public static string Copyright => GetEntryAssemblyFirstAttribute<AssemblyCopyrightAttribute>(false)?.Copyright ?? String.Empty;

        /// <summary>Gets the <see cref="T:Uri"/> of the official website of the application.</summary>
        /// <value>The <see cref="T:Uri"/> of the official website of the application.</value>
        public static Uri WebsiteUri => GetEntryAssemblyFirstAttribute<ApplicationWebsiteAttribute>(false)?.Uri;

        /// <summary>Gets the title of address of the official website of the application.</summary>
        /// <value>The title of address of the official website of the application.</value>
        public static string WebsiteDisplayString => GetEntryAssemblyFirstAttribute<ApplicationWebsiteAttribute>(false)?.Title ?? String.Empty;

        private static TAttribute GetEntryAssemblyFirstAttribute<TAttribute>(bool inherit)
            where TAttribute : Attribute
			=> Assembly.GetEntryAssembly().GetFirstAttribute<TAttribute>(inherit);

        #endregion

        /// <summary>Gets the command to show information about the current application.</summary>
        /// <value>The command to show information about the current application.</value>
        public ICommand AboutCommand { get; }

        private void ShowAboutDialog()
		{
			using (var dialog = new TDialog<TAboutDialogContentControl>())
				dialog.ShowDialog(Form.ActiveForm);
		}
	}
}
