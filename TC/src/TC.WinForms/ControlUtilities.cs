// TC WinForms Library
// Copyright © 2008-2010 Tommy Carlier
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
		/// <returns>A collection of all the descendants of the specified control that
		/// are of type <typeparamref name="T"/>.</returns>
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
			// if includeControl is true, the passed control is returned (if it is of type T)
			T current;
			if (includeControl && (current = control as T) != null)
				yield return current;

			// enumerate the descendants of each child control
			foreach (Control childControl in control.Controls)
				foreach (T descendant in EnumerateDescendantsCore<T>(childControl, true))
					yield return descendant;

			// enumerate the descendants of the ContextMenuStrip
			ContextMenuStrip contextMenu = control.ContextMenuStrip;
			if (contextMenu != null)
				foreach (T descendant in EnumerableDescendantsCore<T>(contextMenu.Items))
					yield return descendant;

			// enumerate the descendants that are not controls, but ToolStripItems
			ToolStrip toolStrip = control as ToolStrip;
			if (toolStrip != null)
				foreach (T descendant in EnumerableDescendantsCore<T>(toolStrip.Items))
					yield return descendant;
		}

		private static IEnumerable<T> EnumerableDescendantsCore<T>(ToolStripItemCollection items)
			where T : class
		{
			T current;

			foreach (ToolStripItem item in items)
			{
				if ((current = item as T) != null)
					yield return current;

				ToolStripDropDownItem dropDownItem = item as ToolStripDropDownItem;
				if (dropDownItem != null)
					foreach (T descendant in EnumerableDescendantsCore<T>(dropDownItem.DropDown.Items))
						yield return descendant;
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
			SetWindowThemeCore(control, applicationName, null);
		}

		/// <summary>Causes a control to use Windows Explorer's visual style information.</summary>
		/// <param name="control">The control whose visual style information is to be changed.</param>
		public static void SetExplorerWindowTheme(this Control control)
		{
			if (control == null) throw new ArgumentNullException("control");
			SetWindowThemeCore(control, "Explorer", null);
		}

		private static void SetWindowThemeCore(Control control, string applicationName, string idList)
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
