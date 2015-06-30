// TC WinForms Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Text;
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
			_aboutCommand = new SimpleActionCommand(ShowAboutDialog);
		}

		/// <summary>Runs the current application.</summary>
		/// <typeparam name="TApp">The type of the application.</typeparam>
		/// <typeparam name="TMainForm">The type of the main form.</typeparam>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The types TApp and TMainForm are important parameters and knowledge of generics is essential for using this function.")]
		public static void Run<TApp, TMainForm>()
			where TApp : TApplication, new()
			where TMainForm : TForm, new()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Animator.Initialize();
			SetToolStripRenderer();
			SystemEvents.UserPreferenceChanged += delegate { SetToolStripRenderer(); };

			_current = new TApp();
			TMainForm form = new TMainForm();
			form.Show();
			Application.Run(_current);
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
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type TMainForm is an important parameter and knowledge of generics is essential for using this function.")]
		public static void Run<TMainForm>() where TMainForm : TForm, new()
		{
			Run<TApplication, TMainForm>();
		}

		private static TApplication _current = new TApplication();

		/// <summary>Gets the current application.</summary>
		/// <value>The current application.</value>
		public static TApplication Current { get { return _current; } }

		#region application information

		private static readonly Icon
			_icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

		/// <summary>Gets the icon of the current application.</summary>
		/// <value>The icon of the current application.</value>
		public static Icon Icon { get { return _icon; } }

		/// <summary>Gets the title of the application.</summary>
		/// <value>The title of the application.</value>
		public static string Title
		{
			get
			{
				var attribute = GetEntryAssemblyFirstAttribute<AssemblyTitleAttribute>(false);
				return attribute != null ? attribute.Title : String.Empty;
			}
		}

		/// <summary>Gets the version of the application.</summary>
		/// <value>The version of the application.</value>
		public static Version Version
		{
			get { return Assembly.GetEntryAssembly().GetName().Version; }
		}

		/// <summary>Gets the copyright information of the application.</summary>
		/// <value>The copyright information of the application.</value>
		public static string Copyright
		{
			get
			{
				var attribute = GetEntryAssemblyFirstAttribute<AssemblyCopyrightAttribute>(false);
				return attribute != null ? attribute.Copyright : String.Empty;
			}
		}

		/// <summary>Gets the <see cref="T:Uri"/> of the official website of the application.</summary>
		/// <value>The <see cref="T:Uri"/> of the official website of the application.</value>
		public static Uri WebsiteUri
		{
			get
			{
				var attribute = GetEntryAssemblyFirstAttribute<ApplicationWebsiteAttribute>(false);
				return attribute != null ? attribute.Uri : null;
			}
		}

		/// <summary>Gets the title of address of the official website of the application.</summary>
		/// <value>The title of address of the official website of the application.</value>
		public static string WebsiteDisplayString
		{
			get
			{
				var attribute = GetEntryAssemblyFirstAttribute<ApplicationWebsiteAttribute>(false);
				return attribute != null ? attribute.Title : String.Empty;
			}
		}

		private static TAttribute GetEntryAssemblyFirstAttribute<TAttribute>(bool inherit)
			where TAttribute : Attribute
		{
			return Assembly
				.GetEntryAssembly()
				.GetFirstAttribute<TAttribute>(inherit);
		}

		#endregion

		private readonly ICommand _aboutCommand;

		/// <summary>Gets the command to show information about the current application.</summary>
		/// <value>The command to show information about the current application.</value>
		public ICommand AboutCommand { get { return _aboutCommand; } }

		private void ShowAboutDialog()
		{
			using (var dialog = new TDialog<TAboutDialogContentControl>())
				dialog.ShowDialog(Form.ActiveForm);
		}
	}
}
