// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tccore
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tccore/license

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace TC.Settings
{
	/// <summary>The abstract base class for the application settings.</summary>
	public abstract class BaseApplicationSettings : BaseSettings
	{
		/// <summary>Initializes a new instance of the <see cref="BaseApplicationSettings"/> class.</summary>
		/// <param name="settingsFileName">The full path of the settings file.</param>
		protected BaseApplicationSettings(string settingsFileName)
		{
			if (settingsFileName == null) throw new ArgumentNullException("settingsFileName");
			if (settingsFileName.Length == 0)
				throw new ArgumentException("settingsFileName cannot be an empty string", "settingsFileName");

			fSettingsFileName = settingsFileName;
		}

		private readonly string fSettingsFileName;

		/// <summary>Gets the full path of the settings file.</summary>
		/// <value>The full path of the settings file.</value>
		public string SettingsFileName { get { return fSettingsFileName; } }

		private readonly List<BaseSettings> fChildSettings = new List<BaseSettings>();

		/// <summary>Registers settings of the specified type.</summary>
		/// <typeparam name="TSettings">The type of the settings to register.</typeparam>
		/// <returns>The registered settings.</returns>
		protected TSettings RegisterSettings<TSettings>() where TSettings : BaseSettings, new()
		{
			TSettings lSettings = new TSettings();
			fChildSettings.Add(lSettings);
			return lSettings;
		}

		/// <summary>Gets the name of the XML-element that represents the settings.</summary>
		/// <value>The name of the XML-element that represents the settings.</value>
		public override string XmlElementName { get { return "settings"; } }

		// the loading and saving is locked so only 1 thread can load or save the settings at a time
		private readonly object fLockLoadSave = new object();

		/// <summary>Loads the application settings.</summary>
		public void Load()
		{
			lock (fLockLoadSave)
				if (File.Exists(fSettingsFileName))
					using (XmlReader lReader = XmlReader.Create(fSettingsFileName))
						Load(new XPathDocument(lReader).CreateNavigator());
		}

		/// <summary>Saves the application settings.</summary>
		public void Save()
		{
			lock (fLockLoadSave)
				using (XmlWriter lWriter = XmlWriter.Create(
					fSettingsFileName,
					new XmlWriterSettings { Indent = true }))
				{
					lWriter.WriteStartDocument();
					Save(lWriter);
				}
		}

		/// <summary>Loads the settings from the specified <see cref="T:XPathNavigator"/>.</summary>
		/// <param name="xml">The <see cref="T:XPathNavigator"/> to load the settings from.</param>
		protected override void LoadCore(XPathNavigator xml)
		{
			foreach (var lSettings in fChildSettings)
				lSettings.Load(xml);
		}

		/// <summary>Saves the settings to the specified <see cref="T:XmlWriter"/>.</summary>
		/// <param name="writer">The <see cref="T:XmlWriter"/> to save the settings to.</param>
		protected override void SaveCore(XmlWriter writer)
		{
			foreach (var lSettings in fChildSettings)
				lSettings.Save(writer);
		}
	}
}
