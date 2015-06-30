// TC WinForms Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms
{
	/// <summary>Represents an object that can render items in list controls.</summary>
	/// <typeparam name="TItem">The type of the items to render.</typeparam>
	[SuppressMessage(
		"Microsoft.Naming",
		"CA1704:IdentifiersShouldBeSpelledCorrectly",
		MessageId = "Renderer",
		Justification = "Renderer is a term that describes an object that handles visual rendering.")]
	public class ListItemRenderer<TItem> : ListItemRenderer where TItem : class
	{
		/// <summary>Initializes a new instance of the <see cref="ListItemRenderer{TItem}"/> class.</summary>
		/// <param name="control">The control that owns this renderer.</param>
		/// <param name="defaultImage">The default image for each item.</param>
		public ListItemRenderer(ListControl control, Image defaultImage)
			: base(control, defaultImage) { }

		/// <summary>Gets the image to display for the specified item.</summary>
		/// <param name="item">The item to get the display image of.</param>
		/// <returns>The image to display for the specified item.</returns>
		protected override Image GetItemImage(object item)
		{
			TItem typedItem = item as TItem;
			return typedItem != null
				? GetItemImage(typedItem)
				: base.GetItemImage(item);
		}

		/// <summary>Gets the image to display for the specified item.</summary>
		/// <param name="item">The item to get the display image of.</param>
		/// <returns>The image to display for the specified item.</returns>
		protected virtual Image GetItemImage(TItem item)
		{
			return base.GetItemImage(item);
		}

		/// <summary>Gets the text to display for the specified item.</summary>
		/// <param name="item">The item to get the text representation of.</param>
		/// <returns>The text to display for the specified item.</returns>
		protected override string GetItemText(object item)
		{
			TItem typedItem = item as TItem;
			return typedItem != null
				? GetItemText(typedItem)
				: base.GetItemText(item);
		}

		/// <summary>Gets the text to display for the specified item.</summary>
		/// <param name="item">The item to get the text representation of.</param>
		/// <returns>The text to display for the specified item.</returns>
		protected virtual string GetItemText(TItem item)
		{
			return base.GetItemText(item);
		}
	}
}