// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms
{
	/// <summary>Represents a control that displays one of the system icons.</summary>
	[ToolboxBitmap(typeof(PictureBox)), DefaultProperty("SystemIcon")]
	public class SystemIconBox : PictureBox
	{
		/// <summary>Initializes a new instance of the <see cref="SystemIconBox"/> class.</summary>
		public SystemIconBox()
		{
			SizeMode = PictureBoxSizeMode.AutoSize;
		}

		#region overrides to define the default designer behavior

		/// <summary>Gets or sets the image that is displayed by <see cref="T:PictureBox"/>.</summary>
		/// <returns>The <see cref="T:Image"/> to display.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image Image
		{
			get { return base.Image; }
			set { base.Image = value; }
		}

		/// <summary>Indicates how the image is displayed.</summary>
		/// <returns>One of the <see cref="T:PictureBoxSizeMode"/> values. The default is <see cref="F:PictureBoxSizeMode.AutoSize"/>.</returns>
		[DefaultValue(typeof(PictureBoxSizeMode), "AutoSize")]
		public new PictureBoxSizeMode SizeMode
		{
			get { return base.SizeMode; }
			set { base.SizeMode = value; }
		}

		private static readonly Padding fDefaultPadding = new Padding(5);

		/// <summary>Gets the internal spacing, in pixels, of the contents of a control.</summary>
		/// <returns>A <see cref="T:Padding"/> that represents the internal spacing of the contents of a control.</returns>
		protected override Padding DefaultPadding { get { return fDefaultPadding; } }

		private static readonly Size fDefaultMinimumSize = new Size(32, 32) + fDefaultPadding.Size;

		/// <summary>Gets the length and height, in pixels, that is specified as the default minimum size of a control.</summary>
		/// <returns>A <see cref="T:Size"/> representing the size of the control.</returns>
		protected override Size DefaultMinimumSize { get { return fDefaultMinimumSize; } }

		#endregion

		private SystemIcon fSystemIcon;

		/// <summary>Gets or sets the <see cref="T:SystemIcon"/> that should be displayed.</summary>
		/// <value>The <see cref="T:SystemIcon"/> that should be displayed.</value>
		[DefaultValue(typeof(SystemIcon), "None"), RefreshProperties(RefreshProperties.All)]
		[Category("Appearance"), Description("The system icon that is displayed.")]
		public SystemIcon SystemIcon
		{
			get { return fSystemIcon; }
			set
			{
				if (fSystemIcon != value)
				{
					fSystemIcon = value;
					if (value != SystemIcon.Custom)
						fCustomImage = null;
					Image = CreateImage();
				}
			}
		}

		private Image fCustomImage;

		/// <summary>Gets or sets the custom image that should be displayed.</summary>
		/// <value>The custom image that should be displayed.</value>
		[RefreshProperties(RefreshProperties.All), Category("Appearance")]
		[Description("The custom image that is displayed.")]
		public Image CustomImage
		{
			get { return fCustomImage; }
			set
			{
				if (fCustomImage != value)
				{
					fCustomImage = value;
					if (value != null)
						fSystemIcon = SystemIcon.Custom;
					else if (fSystemIcon == SystemIcon.Custom)
						fSystemIcon = SystemIcon.None;
					Image = CreateImage();
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeCustomImage()
		{
			return (fSystemIcon == SystemIcon.Custom)
				== (fCustomImage == null);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		private void ResetCustomImage()
		{
			CustomImage = null;
		}

		internal void InitializeImage() { Image = CreateImage(); }

		private Image CreateImage()
		{
			switch (fSystemIcon)
			{
				case SystemIcon.Custom: return fCustomImage;
				case SystemIcon.FormIcon: return CreateFormIconBitmap();
				case SystemIcon.Information: return SystemIcons.Information.ToBitmap();
				case SystemIcon.Question: return SystemIcons.Question.ToBitmap();
				case SystemIcon.Warning: return SystemIcons.Warning.ToBitmap();
				case SystemIcon.Error: return SystemIcons.Error.ToBitmap();
				default: return null;
			}
		}

		private Image CreateFormIconBitmap()
		{
			Form lForm = FindForm();
			return lForm != null ? lForm.Icon.ToBitmap() : null;
		}
	}
}
