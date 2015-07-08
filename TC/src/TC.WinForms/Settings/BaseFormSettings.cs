// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.XPath;
using TC.Settings;

namespace TC.WinForms.Settings
{
	/// <summary>The abstract base class for the settings of a <see cref="T:TForm"/>.</summary>
	public abstract class BaseFormSettings : BaseSettings
	{
		/// <summary>Initializes a new instance of the <see cref="BaseFormSettings"/> class.</summary>
		/// <param name="xmlElementName">The name of the XML-element that represents this instance.</param>
		protected BaseFormSettings(string xmlElementName)
			: base(xmlElementName)
		{
			X = -1;
			Y = -1;
			Width = -1;
			Height = -1;
		}

		/// <summary>Loads the settings from the specified <see cref="T:XPathNavigator"/>.</summary>
		/// <param name="xml">The <see cref="T:XPathNavigator"/> to load the settings from.</param>
		protected override void LoadCore(XPathNavigator xml)
		{
			X = xml.GetAttribute("x", String.Empty).ToInt32(-1);
			Y = xml.GetAttribute("y", String.Empty).ToInt32(-1);
			Width = xml.GetAttribute("width", String.Empty).ToInt32(-1);
			Height = xml.GetAttribute("height", String.Empty).ToInt32(-1);
			IsMaximized = xml.GetAttribute("maximized", String.Empty).ToBoolean();
		}

		/// <summary>Saves the settings to the specified <see cref="T:XmlWriter"/>.</summary>
		/// <param name="writer">The <see cref="T:XmlWriter"/> to save the settings to.</param>
		protected override void SaveCore(XmlWriter writer)
		{
			if (X >= 0) writer.WriteAttributeString("x", X.ToDataString());
			if (Y >= 0) writer.WriteAttributeString("y", Y.ToDataString());
			if (Width >= 0) writer.WriteAttributeString("width", Width.ToDataString());
			if (Height >= 0) writer.WriteAttributeString("height", Height.ToDataString());
			if (IsMaximized) writer.WriteAttributeString("maximized", true.ToDataString());
		}

		/// <summary>Gets or sets the X-coordinate of the location of the form.</summary>
		/// <value>The X-coordinate of the location of the form.</value>
		[SuppressMessage(
			"Microsoft.Naming",
			"CA1704:IdentifiersShouldBeSpelledCorrectly",
			MessageId = "X",
			Justification = "X is a meaningful name: it represents the X-coordinate.")]
		public int X { get; set; }

		/// <summary>Gets or sets the Y-coordinate of the location of the form.</summary>
		/// <value>The Y-coordinate of the location of the form.</value>
		[SuppressMessage(
			"Microsoft.Naming",
			"CA1704:IdentifiersShouldBeSpelledCorrectly",
			MessageId = "Y",
			Justification = "Y is a meaningful name: it represents the Y-coordinate.")]
		public int Y { get; set; }

		/// <summary>Gets or sets the width of the form.</summary>
		/// <value>The width of the form.</value>
		public int Width { get; set; }

		/// <summary>Gets or sets the height of the form.</summary>
		/// <value>The height of the form.</value>
		public int Height { get; set; }

		/// <summary>Gets or sets a value indicating whether the form is maximized.</summary>
		/// <value><c>true</c> if the form is maximized; otherwise, <c>false</c>.</value>
		public bool IsMaximized { get; set; }
	}
}
