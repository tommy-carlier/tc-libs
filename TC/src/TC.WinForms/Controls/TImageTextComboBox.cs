// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a combo box control where each item is displayed as an image with text to the right.</summary>
	/// <typeparam name="TItem">The type if the items in the combo box control.</typeparam>
	[ToolboxBitmap(typeof(ComboBox))]
	public abstract class TImageTextComboBox<TItem> : ComboBox where TItem : class
	{
		/// <summary>Initializes a new instance of the <see cref="T:TImageTextComboBox{TItem}"/> class.</summary>
		protected TImageTextComboBox()
		{
			DropDownStyle = ComboBoxStyle.DropDownList;
			DrawMode = DrawMode.OwnerDrawFixed;
		}

		/// <summary>Gets or sets a value specifying the style of the combo box.</summary>
		/// <returns>One of the <see cref="T:ComboBoxStyle"/> values. The default is DropDownList.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public new ComboBoxStyle DropDownStyle
		{
			get { return base.DropDownStyle; }
			set { base.DropDownStyle = value; }
		}

		/// <summary>Gets or sets a value indicating whether your code or the operating system will handle drawing of elements in the list.</summary>
		/// <returns>One of the <see cref="T:DrawMode"/> enumeration values. The default is <see cref="F:DrawMode.OwnerDrawFixed"/>.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public new DrawMode DrawMode
		{
			get { return base.DrawMode; }
			set { base.DrawMode = value; }
		}

		/// <summary>Raises the <see cref="E:MeasureItem"/> event.</summary>
		/// <param name="e">The <see cref="T:MeasureItemEventArgs"/> that was raised.</param>
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			Image lImage;
			string lText;
			if (TryGetImageAndText(e.Index, out lImage, out lText))
			{
				if (lImage != null)
				{
					e.ItemWidth += lImage.Width;
					if (lImage.Height > e.ItemHeight)
						e.ItemHeight = lImage.Height;
				}

				if (!string.IsNullOrEmpty(lText))
				{
					Size lTextSize = TextRenderer.MeasureText(
						e.Graphics,
						lText, 
						Font,
						Size.Empty,
						ItemTextFormatFlags);

					e.ItemWidth += lTextSize.Width;
					if (lTextSize.Height > e.ItemHeight)
						e.ItemHeight = lTextSize.Height;
				}
			}
		}

		/// <summary>Raises the <see cref="E:DrawItem"/> event.</summary>
		/// <param name="e">A <see cref="T:DrawItemEventArgs"/> that contains the event data.</param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			e.DrawBackground();

			Image lImage;
			string lText;
			if (TryGetImageAndText(e.Index, out lImage, out lText))
			{
				Rectangle lBounds = e.Bounds;
				
				Rectangle lImageBounds 
					= lImage != null 
						? new Rectangle(Point.Empty, lImage.Size) 
						: Rectangle.Empty;

				lImageBounds.Location = new Point(
					lBounds.Left,
					lBounds.Top + ((lBounds.Height - lImageBounds.Height) / 2));

				Rectangle lTextBounds = new Rectangle(
					lImageBounds.Right,
					lBounds.Top,
					lBounds.Width - lImageBounds.Right,
					lBounds.Height);

				if (lImage != null)
					e.Graphics.DrawImage(lImage, lImageBounds);

				if (!string.IsNullOrEmpty(lText))
					TextRenderer.DrawText(
						e.Graphics,
						lText,
						e.Font,
						lTextBounds,
						e.ForeColor,
						ItemTextFormatFlags);
			}

			base.OnDrawItem(e);
			e.DrawFocusRectangle();
		}

		private const TextFormatFlags ItemTextFormatFlags
			= TextFormatFlags.Left | TextFormatFlags.VerticalCenter
			| TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine;

		private bool TryGetImageAndText(int index, out Image image, out string text)
		{
			if (index >= 0 && index < Items.Count)
			{
				TItem lItem = Items[index] as TItem;
				if (lItem != null)
				{
					GetImageAndText(lItem, out image, out text);
					return true;
				}
			}

			image = null;
			text = null;
			return false;
		}

		/// <summary>When overriden in a derived class, gets the image and text associated with the specified item.</summary>
		/// <param name="item">The item to get the image and text of.</param>
		/// <param name="image">When this method returns, contains the image associated with the specified item.</param>
		/// <param name="text">When this method returns, contains the text associated with the specified item.</param>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1021:AvoidOutParameters",
			Justification = "This is an easy way to return 2 values from a method that is only used by developers that create a derived class.")]
		protected abstract void GetImageAndText(TItem item, out Image image, out string text);

		/// <summary>Gets or sets currently selected item in the <see cref="T:ComboBox"/>.</summary>
		/// <returns>The object that is the currently selected item or null if there is no currently selected item.</returns>
		public virtual new TItem SelectedItem
		{
			get { return base.SelectedItem as TItem; }
			set { base.SelectedItem = value; }
		}

		/// <summary>Initializes the combo box with the specified items.</summary>
		/// <param name="items">The items to add to the combo box.</param>
		public void Initialize(IEnumerable<TItem> items)
		{
			if (items == null) throw new ArgumentNullException("items");

			BeginUpdate();
			Items.Clear();
			foreach (TItem lItem in items)
				Items.Add(lItem);
			EndUpdate();

			if (Items.Count > 0)
			{
				Enabled = true;
				SelectedIndex = 0;
			}
			else Enabled = false;
		}
	}
}
