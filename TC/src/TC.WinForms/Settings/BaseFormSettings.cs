// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
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
			X = ConvertString.ToInt32(xml.GetAttribute("x", String.Empty), -1);
			Y = ConvertString.ToInt32(xml.GetAttribute("y", String.Empty), -1);
			Width = ConvertString.ToInt32(xml.GetAttribute("width", String.Empty), -1);
			Height = ConvertString.ToInt32(xml.GetAttribute("height", String.Empty), -1);
			IsMaximized = ConvertString.ToBoolean(xml.GetAttribute("maximized", String.Empty));
		}

		/// <summary>Saves the settings to the specified <see cref="T:XmlWriter"/>.</summary>
		/// <param name="writer">The <see cref="T:XmlWriter"/> to save the settings to.</param>
		protected override void SaveCore(XmlWriter writer)
		{
			if (X >= 0) writer.WriteAttributeString("x", ConvertString.FromInt32(X));
			if (Y >= 0) writer.WriteAttributeString("y", ConvertString.FromInt32(Y));
			if (Width >= 0) writer.WriteAttributeString("width", ConvertString.FromInt32(Width));
			if (Height >= 0) writer.WriteAttributeString("height", ConvertString.FromInt32(Height));
			if (IsMaximized) writer.WriteAttributeString("maximized", ConvertString.FromBoolean(true));
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
