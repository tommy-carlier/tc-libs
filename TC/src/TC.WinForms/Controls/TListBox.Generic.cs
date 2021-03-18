// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE


namespace TC.WinForms.Controls
{
	/// <summary>Represents a (strongly typed) list box control.</summary>
	/// <typeparam name="TItem">The type of the items.</typeparam>
	public class TListBox<TItem> : TListBox where TItem : class
	{
		/// <summary>Gets or sets currently selected item in the <see cref="T:TListBox{TItem}"/>.</summary>
		/// <value>The currently selected item.</value>
		/// <returns>The object that is the currently selected item or null if there is no currently selected item.</returns>
		public virtual new TItem SelectedItem
		{
			get { return base.SelectedItem as TItem; }
			set { base.SelectedItem = value; }
		}
	}
}