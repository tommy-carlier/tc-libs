// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using SC = System.Collections;

namespace TC.WinForms
{
	/// <summary>Provides utilities that deal with list controls.</summary>
	public static class ListControlUtilities
	{
		/// <summary>Initializes the specified combo box control with the specified items.</summary>
		/// <typeparam name="T">The type of the items to add.</typeparam>
		/// <param name="comboBox">The combo box control to initialize.</param>
		/// <param name="itemsToAdd">The items to add to the list control.</param>
		public static void Initialize<T>(this ComboBox comboBox, IEnumerable<T> itemsToAdd)
		{
			if (comboBox == null) throw new ArgumentNullException("comboBox");
			if (itemsToAdd == null) throw new ArgumentNullException("itemsToAdd");

			ComboBox.ObjectCollection comboBoxItems = comboBox.Items;
			comboBox.BeginUpdate();
			try
			{
				comboBoxItems.Clear();
				foreach (T item in itemsToAdd)
					comboBoxItems.Add(item);
			}
			finally
			{
				comboBox.EndUpdate();

				if (comboBoxItems.Count > 0)
				{
					comboBox.Enabled = true;
					if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
						comboBox.SelectedIndex = 0;
					else comboBox.Text = string.Empty;
				}
				else comboBox.Enabled = false;
			}
		}

		/// <summary>Initializes the specified list box control with the specified items.</summary>
		/// <typeparam name="T">The type of the items to add.</typeparam>
		/// <param name="listBox">The list box control to initialize.</param>
		/// <param name="itemsToAdd">The items to add to the list control.</param>
		public static void Initialize<T>(this ListBox listBox, IEnumerable<T> itemsToAdd)
		{
			if (listBox == null) throw new ArgumentNullException("listBox");
			if (itemsToAdd == null) throw new ArgumentNullException("itemsToAdd");

			ListBox.ObjectCollection listBoxItems = listBox.Items;
			listBox.BeginUpdate();
			try
			{
				listBoxItems.Clear();
				foreach (T item in itemsToAdd)
					listBoxItems.Add(item);
			}
			finally
			{
				listBox.EndUpdate();

				if (listBoxItems.Count > 0)
				{
					listBox.Enabled = true;
					listBox.SelectedIndex = 0;
				}
				else listBox.Enabled = false;
			}
		}
	}
}