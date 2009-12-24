// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
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
	/// <summary>Represents a control that displays one of the system icons.</summary>
	[ToolboxBitmap(typeof(PictureBox)), DefaultProperty("SystemIcon")]
	public class TSystemIconBox : Control
	{
		/// <summary>Initializes a new instance of the <see cref="TSystemIconBox"/> class.</summary>
		public TSystemIconBox()
		{
			AutoSize = true;

			SetStyle(
				ControlStyles.FixedWidth
				| ControlStyles.FixedHeight
				| ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.SupportsTransparentBackColor
				| ControlStyles.ResizeRedraw,
				true);

			SetStyle(
				ControlStyles.Selectable
				| ControlStyles.Opaque,
				false);
		}

		#region overrides to define the default designer behavior

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>true if enabled; otherwise, false.</returns>
		[DefaultValue(true)]
		public override bool AutoSize
		{
			get { return base.AutoSize; }
			set { base.AutoSize = value; }
		}

		private static readonly Padding _defaultPadding = new Padding(5);

		/// <summary>Gets the internal spacing, in pixels, of the contents of a control.</summary>
		/// <returns>A <see cref="T:Padding"/> that represents the internal spacing of the contents of a control.</returns>
		protected override Padding DefaultPadding { get { return _defaultPadding; } }

		private static readonly Size _defaultMinimumSize = new Size(32, 32) + _defaultPadding.Size;

		/// <summary>Gets the length and height, in pixels, that is specified as the default minimum size of a control.</summary>
		/// <returns>A <see cref="T:Size"/> representing the size of the control.</returns>
		protected override Size DefaultMinimumSize { get { return _defaultMinimumSize; } }

		#endregion

		private SystemIcon _systemIcon;
		private Image _customImage, _image;

		/// <summary>Gets or sets the <see cref="T:SystemIcon"/> that should be displayed.</summary>
		/// <value>The <see cref="T:SystemIcon"/> that should be displayed.</value>
		[DefaultValue(typeof(SystemIcon), "None"), RefreshProperties(RefreshProperties.All)]
		[Category("Appearance"), Description("The system icon that is displayed.")]
		public SystemIcon SystemIcon
		{
			get
			{
				return _systemIcon;
			}

			set
			{
				if (_systemIcon != value)
				{
					_systemIcon = value;

					if (value != SystemIcon.Custom)
						_customImage = null;

					InitializeImage();
				}
			}
		}

		/// <summary>Gets or sets the custom image that should be displayed.</summary>
		/// <value>The custom image that should be displayed.</value>
		[RefreshProperties(RefreshProperties.All), Category("Appearance")]
		[Description("The custom image that is displayed.")]
		public Image CustomImage
		{
			get
			{
				return _customImage;
			}

			set
			{
				if (_customImage != value)
				{
					_customImage = value;

					if (value != null)
						_systemIcon = SystemIcon.Custom;
					else if (_systemIcon == SystemIcon.Custom)
						_systemIcon = SystemIcon.None;

					InitializeImage();
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeCustomImage()
		{
			return (_systemIcon == SystemIcon.Custom)
				== (_customImage == null);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		private void ResetCustomImage()
		{
			CustomImage = null;
		}

		internal void InitializeImage()
		{
			Size size = GetImageSize(_image);
			_image = CreateImage();

			if (size != GetImageSize(_image))
				Size = PreferredSize;

			Invalidate();
		}

		private static Size GetImageSize(Image image)
		{
			return image != null
				? image.Size
				: Size.Empty;
		}

		private Image CreateImage()
		{
			switch (_systemIcon)
			{
				case SystemIcon.Custom: return _customImage;
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
			Form form = FindForm();
			return form != null ? form.Icon.ToBitmap() : null;
		}

		/// <summary>Retrieves the size of a rectangular area into which a control can be fitted.</summary>
		/// <param name="proposedSize">The custom-sized area for a control.</param>
		/// <returns>An ordered pair of type <see cref="T:Size"/> representing the width and height of a rectangle.</returns>
		public override Size GetPreferredSize(Size proposedSize)
		{
			return _image != null
				? _image.Size + Padding.Size
				: new Size(32, 32);
		}

		/// <summary>Raises the <see cref="E:Paint"/> event.</summary>
		/// <param name="e">A <see cref="T:PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (_image != null)
			{
				Size imageSize = _image.Size;
				Padding padding = Padding;

				Rectangle destinationRectangle =
					new Rectangle(
						padding.Left,
						padding.Top,
						imageSize.Width,
						imageSize.Height);
				destinationRectangle.Intersect(e.ClipRectangle);
				if (!destinationRectangle.IsEmpty)
				{
					Rectangle sourceRectangle = destinationRectangle;
					sourceRectangle.Offset(-padding.Left, -padding.Top);

					e.Graphics.DrawImage(
						_image,
						destinationRectangle,
						sourceRectangle,
						GraphicsUnit.Pixel);
				}
			}

			base.OnPaint(e);
		}
	}
}
