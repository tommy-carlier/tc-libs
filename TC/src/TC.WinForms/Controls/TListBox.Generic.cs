// TC WinForms Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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