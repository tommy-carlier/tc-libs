﻿// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Xml;
using System.Xml.XPath;

namespace TC.WinForms.Settings
{
	/// <summary>The abstract base class for the settings of a <see cref="T:TDocumentForm"/>.</summary>
	public abstract class DocumentFormSettings : BaseFormSettings
	{
		/// <summary>Initializes a new instance of the <see cref="DocumentFormSettings"/> class.</summary>
		/// <param name="xmlElementName">The name of the XML-element that represents this instance.</param>
		protected DocumentFormSettings(string xmlElementName)
			: base(xmlElementName) { }

		/// <summary>Loads the settings from the specified <see cref="T:XPathNavigator"/>.</summary>
		/// <param name="xml">The <see cref="T:XPathNavigator"/> to load the settings from.</param>
		protected override void LoadCore(XPathNavigator xml)
		{
			base.LoadCore(xml);
			CurrentDocumentFolder = xml.GetAttribute("folder", String.Empty);
			SelectedFilterIndex = xml.GetAttribute("filter", String.Empty).ToInt32();
		}

		/// <summary>Saves the settings to the specified <see cref="T:XmlWriter"/>.</summary>
		/// <param name="writer">The <see cref="T:XmlWriter"/> to save the settings to.</param>
		protected override void SaveCore(XmlWriter writer)
		{
			base.SaveCore(writer);

			if (CurrentDocumentFolder.IsNotNullOrEmpty())
				writer.WriteAttributeString("folder", CurrentDocumentFolder);
			
			if (SelectedFilterIndex > 0)
				writer.WriteAttributeString("filter", SelectedFilterIndex.ToDataString());
		}

		/// <summary>Gets or sets the initial folder in file dialogs.</summary>
		/// <value>The initial folder in file dialogs.</value>
		public string CurrentDocumentFolder { get; set; }

		/// <summary>Gets or sets the index of the selected filter in file dialogs.</summary>
		/// <value>The index of the selected filter in file dialogs.</value>
		public int SelectedFilterIndex { get; set; }
	}
}
