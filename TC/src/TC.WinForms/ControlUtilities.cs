// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms
{
	/// <summary>Provides utilities that deal with controls.</summary>
	public static class ControlUtilities
	{
		#region EnumerateDescendants<T>

		/// <summary>Enumerates the descendants of the specified control that are of type <typeparamref name="T"/>.</summary>
		/// <typeparam name="T">The type of the descendants to enumerate.</typeparam>
		/// <param name="control">The control to enumerate descendants of.</param>
		/// <param name="includeControl">Indicates whether to include the specified control itself.</param>
		/// <returns>A collection of all the descendants of the specified control that are of type <typeparamref name="T"/>.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type T is an important parameter and knowledge of generics is essential for using this function.")]
		public static IEnumerable<T> EnumerateDescendants<T>(this Control control, bool includeControl) 
			where T : class
		{
			if (control == null) throw new ArgumentNullException("control");
			return EnumerateDescendantsCore<T>(control, includeControl);
		}

		private static IEnumerable<T> EnumerateDescendantsCore<T>(Control control, bool includeControl)
			where T : class
		{
			T lCurrent;
			if (includeControl && (lCurrent = control as T) != null)
				yield return lCurrent;

			foreach (Control lChildControl in control.Controls)
				foreach (T lDescendant in EnumerateDescendantsCore<T>(lChildControl, true))
					yield return lDescendant;

			ContextMenuStrip lContextMenu = control.ContextMenuStrip;
			if (lContextMenu != null)
				foreach (T lDescendant in EnumerableDescendantsCore<T>(lContextMenu.Items))
					yield return lDescendant;

			ToolStrip lToolStrip = control as ToolStrip;
			if (lToolStrip != null)
				foreach (T lDescendant in EnumerableDescendantsCore<T>(lToolStrip.Items))
					yield return lDescendant;
		}

		private static IEnumerable<T> EnumerableDescendantsCore<T>(ToolStripItemCollection items)
			where T : class
		{
			T lCurrent;

			foreach (ToolStripItem lItem in items)
			{
				if ((lCurrent = lItem as T) != null)
					yield return lCurrent;

				ToolStripDropDownItem lDropDownItem = lItem as ToolStripDropDownItem;
				if (lDropDownItem != null)
					foreach (T lDescendant in EnumerableDescendantsCore<T>(lDropDownItem.DropDown.Items))
						yield return lDescendant;
			}
		}

		#endregion

		#region SelectNextControl and SelectPreviousControl

		/// <summary>Selects the next control.</summary>
		/// <param name="form">The form to select the next control in.</param>
		public static void SelectNextControl(this Form form)
		{
			SelectControl(form, true);
		}

		/// <summary>Selects the previous control.</summary>
		/// <param name="form">The form to select the previous control in.</param>
		public static void SelectPreviousControl(this Form form)
		{
			SelectControl(form, false);
		}

		private static void SelectControl(Form form, bool next)
		{
			if (form != null)
				form.SelectNextControl(form.ActiveControl, next, true, true, true);
		}

		#endregion

		#region SetWindowTheme and SetExplorerWindowTheme

		/// <summary>Causes a control to use a different set of visual style information than its class normally uses.</summary>
		/// <param name="control">The control whose visual style information is to be changed.</param>
		/// <param name="applicationName">The application name to use in place of the calling application's name.</param>
		public static void SetWindowTheme(this Control control, string applicationName)
		{
			if (control == null) throw new ArgumentNullException("control");
			SetWindowThemeImpl(control, applicationName, null);
		}

		/// <summary>Causes a control to use Windows Explorer's visual style information.</summary>
		/// <param name="control">The control whose visual style information is to be changed.</param>
		public static void SetExplorerWindowTheme(this Control control)
		{
			if (control == null) throw new ArgumentNullException("control");
			SetWindowThemeImpl(control, "Explorer", null);
		}

		private static void SetWindowThemeImpl(Control control, string applicationName, string idList)
		{
			// only possible in Windows XP or later
			if (SystemUtilities.IsWindowsXPOrLater)
			{
				if (NativeMethods.SetWindowTheme(control.Handle, applicationName, idList) != 0)
					throw new Win32Exception();
			}
		}

		#endregion
	}
}
