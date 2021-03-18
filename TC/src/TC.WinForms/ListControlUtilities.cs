// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TC.WinForms
{
	/// <summary>Provides utilities that deal with list controls.</summary>
	public static class ListControlUtilities
	{
		#region Initialize

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

				if (comboBoxItems.HasItems())
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

				if (listBoxItems.HasItems())
				{
					listBox.Enabled = true;
					listBox.SelectedIndex = 0;
				}
				else listBox.Enabled = false;
			}
		}

		#endregion

		#region IsEmpty and HasItems

		/// <summary>Determines whether the specified <see cref="T:ComboBox"/> is empty.</summary>
		/// <param name="comboBox">The <see cref="T:ComboBox"/> to check.</param>
		/// <returns>If the specified <see cref="T:ComboBox"/> is empty, <c>true</c>; otherwise, <c>false</c>.</returns>
		public static bool IsEmpty(this ComboBox comboBox)
		{
			if (comboBox == null) throw new ArgumentNullException("comboBox");
			return comboBox.Items.Count == 0;
		}

		/// <summary>Determines whether the specified <see cref="T:ListBox"/> is empty.</summary>
		/// <param name="listBox">The <see cref="T:ListBox"/> to check.</param>
		/// <returns>If the specified <see cref="T:ListBox"/> is empty, <c>true</c>; otherwise, <c>false</c>.</returns>
		public static bool IsEmpty(this ListBox listBox)
		{
			if (listBox == null) throw new ArgumentNullException("listBox");
			return listBox.Items.Count == 0;
		}

		/// <summary>Determines whether the specified <see cref="T:ComboBox"/> has items.</summary>
		/// <param name="comboBox">The <see cref="T:ComboBox"/> to check.</param>
		/// <returns>If the specified <see cref="T:ComboBox"/> has items, <c>true</c>; otherwise, <c>false</c>.</returns>
		public static bool HasItems(this ComboBox comboBox)
		{
			if (comboBox == null) throw new ArgumentNullException("comboBox");
			return comboBox.Items.Count > 0;
		}

		/// <summary>Determines whether the specified <see cref="T:ListBox"/> has items.</summary>
		/// <param name="listBox">The <see cref="T:ListBox"/> to check.</param>
		/// <returns>If the specified <see cref="T:ListBox"/> has items, <c>true</c>; otherwise, <c>false</c>.</returns>
		public static bool HasItems(this ListBox listBox)
		{
			if (listBox == null) throw new ArgumentNullException("listBox");
			return listBox.Items.Count > 0;
		}

		#endregion
	}
}