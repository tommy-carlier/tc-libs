// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a list box control.</summary>
	[ToolboxBitmap(typeof(ListBox))]
	public class TListBox : ListBox
	{
		/// <summary>Initializes a new instance of the <see cref="TListBox"/> class.</summary>
		public TListBox()
		{
			DrawMode = DrawMode.OwnerDrawVariable;
		}

		/// <summary>Gets or sets a value indicating whether your code or
		/// the operating system will handle drawing of elements in the list.</summary>
		/// <returns>One of the <see cref="T:DrawMode"/> enumeration values.
		/// The default is <see cref="F:DrawMode.OwnerDrawFixed"/>.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public new DrawMode DrawMode
		{
			get { return base.DrawMode; }
			set { base.DrawMode = value; }
		}

		#region measuring and drawing items

		/// <summary>Raises the <see cref="E:MeasureItem"/> event.</summary>
		/// <param name="e">The <see cref="T:MeasureItemEventArgs"/> that was raised.</param>
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			ItemRenderer.MeasureItem(e, GetItem(e.Index));
		}

		/// <summary>Raises the <see cref="E:DrawItem"/> event.</summary>
		/// <param name="e">A <see cref="T:DrawItemEventArgs"/> that contains the event data.</param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			ItemRenderer.DrawItem(e, GetItem(e.Index));
		}

		private object GetItem(int index)
		{
			return index >= 0 && index < Items.Count
				? Items[index]
				: null;
		}

		#endregion

		#region ItemRenderer and CreateItemRenderer

		private ListItemRenderer _itemRenderer;

		/// <summary>Gets the <see cref="T:ListItemRenderer"/> that renders the items of this control.</summary>
		/// <value>The <see cref="T:ListItemRenderer"/> that renders the items of this control.</value>
		[SuppressMessage(
			"Microsoft.Naming",
			"CA1704:IdentifiersShouldBeSpelledCorrectly",
			MessageId = "Renderer",
			Justification = "Renderer is a term that describes an object that handles visual rendering.")]
		protected ListItemRenderer ItemRenderer
		{
			get { return _itemRenderer ?? (_itemRenderer = CreateItemRenderer()); }
		}

		/// <summary>Creates a <see cref="T:ListItemRenderer"/> that will render the items of this control.</summary>
		/// <returns>The created <see cref="T:ListItemRenderer"/>.</returns>
		[SuppressMessage(
			"Microsoft.Naming",
			"CA1704:IdentifiersShouldBeSpelledCorrectly",
			MessageId = "Renderer",
			Justification = "Renderer is a term that describes an object that handles visual rendering.")]
		protected virtual ListItemRenderer CreateItemRenderer()
		{
			return new ListItemRenderer(this, null);
		}

		#endregion
	}
}
