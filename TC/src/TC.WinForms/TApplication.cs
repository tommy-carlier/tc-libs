// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

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
			fAboutCommand = new SimpleActionCommand(ShowAboutDialog);
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
			ToolStripManager.Renderer = new TToolStripRenderer();

			fCurrent = new TApp();
			TMainForm lForm = new TMainForm();
			lForm.Show();
			Application.Run(fCurrent);
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

		private static TApplication fCurrent = new TApplication();

		/// <summary>Gets the current application.</summary>
		/// <value>The current application.</value>
		public static TApplication Current { get { return fCurrent; } }

		#region application information

		private static readonly Icon
			fIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

		/// <summary>Gets the icon of the current application.</summary>
		/// <value>The icon of the current application.</value>
		public static Icon Icon { get { return fIcon; } }

		/// <summary>Gets the title of the application.</summary>
		/// <value>The title of the application.</value>
		public static string Title
		{
			get
			{
				var lAttribute = GetEntryAssemblyFirstAttribute<AssemblyTitleAttribute>(false);
				return lAttribute != null ? lAttribute.Title : String.Empty;
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
				var lAttribute = GetEntryAssemblyFirstAttribute<AssemblyCopyrightAttribute>(false);
				return lAttribute != null ? lAttribute.Copyright : String.Empty;
			}
		}

		/// <summary>Gets the <see cref="T:Uri"/> of the official website of the application.</summary>
		/// <value>The <see cref="T:Uri"/> of the official website of the application.</value>
		public static Uri WebsiteUri
		{
			get
			{
				var lAttribute = GetEntryAssemblyFirstAttribute<ApplicationWebsiteAttribute>(false);
				return lAttribute != null ? lAttribute.Uri : null;
			}
		}

		/// <summary>Gets the title of address of the official website of the application.</summary>
		/// <value>The title of address of the official website of the application.</value>
		public static string WebsiteDisplayString
		{
			get
			{
				var lAttribute = GetEntryAssemblyFirstAttribute<ApplicationWebsiteAttribute>(false);
				return lAttribute != null ? lAttribute.Title : String.Empty;
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

		private readonly ICommand fAboutCommand;

		/// <summary>Gets the command to show information about the current application.</summary>
		/// <value>The command to show information about the current application.</value>
		public ICommand AboutCommand { get { return fAboutCommand; } }

		private void ShowAboutDialog()
		{
			using (var lDialog = new TDialog<TAboutDialogContentControl>())
				lDialog.ShowDialog(Form.ActiveForm);
		}
	}
}
