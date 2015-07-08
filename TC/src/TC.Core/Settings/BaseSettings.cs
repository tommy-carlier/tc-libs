// TC Core Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Xml;
using System.Xml.XPath;

namespace TC.Settings
{
	/// <summary>The abstract base class for any kind of settings.</summary>
	public abstract class BaseSettings
	{
		/// <summary>Initializes a new instance of the <see cref="BaseSettings"/> class.</summary>
		/// <param name="xmlElementName">The name of the XML-element that represents this instance.</param>
		protected BaseSettings(string xmlElementName)
		{
			if (xmlElementName == null) throw new ArgumentNullException("xmlElementName");
			if (xmlElementName.Length == 0)
				throw new ArgumentException("xmlElementName cannot be an empty string", "xmlElementName");

			_xmlElementName = xmlElementName;
		}

		private readonly string _xmlElementName;

		#region abstract members that have to be implemented by derived classes

		/// <summary>Loads the settings from the specified <see cref="T:XPathNavigator"/>.</summary>
		/// <param name="xml">The <see cref="T:XPathNavigator"/> to load the settings from.</param>
		protected abstract void LoadCore(XPathNavigator xml);

		/// <summary>Saves the settings to the specified <see cref="T:XmlWriter"/>.</summary>
		/// <param name="writer">The <see cref="T:XmlWriter"/> to save the settings to.</param>
		protected abstract void SaveCore(XmlWriter writer);

		#endregion

		#region public methods to load and save the settings

		/// <summary>Loads the settings from the specified <see cref="T:XPathNavigator"/>.</summary>
		/// <param name="xml">The <see cref="T:XPathNavigator"/> to load the settings from.</param>
		public void Load(XPathNavigator xml)
		{
			if (xml == null) throw new ArgumentNullException("xml");
			xml = xml.SelectSingleNode(_xmlElementName);
			if (xml != null) LoadCore(xml);
		}

		/// <summary>Saves the settings to the specified <see cref="T:XmlWriter"/>.</summary>
		/// <param name="writer">The <see cref="T:XmlWriter"/> to save the settings to.</param>
		public void Save(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");
			writer.WriteStartElement(_xmlElementName);
			SaveCore(writer);
			writer.WriteEndElement();
		}

		#endregion
	}
}
