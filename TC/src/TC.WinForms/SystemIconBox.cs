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

		/// <summary>Gets or sets the <see cref="T:SystemIcon"/> that should be displayed.</summary>
		/// <value>The <see cref="T:SystemIcon"/> that should be displayed.</value>
		[DefaultValue(typeof(SystemIcon), "None"), RefreshProperties(RefreshProperties.All)]
		[Category("Appearance"), Description("The system icon that is displayed.")]
		public SystemIcon SystemIcon
		{
			get { return _systemIcon; }
			set
			{
				if (_systemIcon != value)
				{
					_systemIcon = value;
					if (value != SystemIcon.Custom)
						_customImage = null;
					Image = CreateImage();
				}
			}
		}

		private Image _customImage;

		/// <summary>Gets or sets the custom image that should be displayed.</summary>
		/// <value>The custom image that should be displayed.</value>
		[RefreshProperties(RefreshProperties.All), Category("Appearance")]
		[Description("The custom image that is displayed.")]
		public Image CustomImage
		{
			get { return _customImage; }
			set
			{
				if (_customImage != value)
				{
					_customImage = value;
					if (value != null)
						_systemIcon = SystemIcon.Custom;
					else if (_systemIcon == SystemIcon.Custom)
						_systemIcon = SystemIcon.None;
					Image = CreateImage();
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

		internal void InitializeImage() { Image = CreateImage(); }

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
	}
}
