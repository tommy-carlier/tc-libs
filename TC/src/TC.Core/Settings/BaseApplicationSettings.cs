// TC Core Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

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
			: base("settings")
		{
			if (settingsFileName == null) throw new ArgumentNullException("settingsFileName");
			if (settingsFileName.Length == 0)
				throw new ArgumentException("settingsFileName cannot be an empty string", "settingsFileName");

			_settingsFileName = settingsFileName;
		}

		private readonly string _settingsFileName;

		/// <summary>Gets the full path of the settings file.</summary>
		/// <value>The full path of the settings file.</value>
		public string SettingsFileName { get { return _settingsFileName; } }

		private readonly List<BaseSettings> _childSettings = new List<BaseSettings>();

		/// <summary>Registers settings of the specified type.</summary>
		/// <typeparam name="TSettings">The type of the settings to register.</typeparam>
		/// <param name="settings">The settings to register.</param>
		/// <returns>The registered settings.</returns>
		protected TSettings RegisterSettings<TSettings>(TSettings settings) where TSettings : BaseSettings
		{
			if (settings == null) throw new ArgumentNullException("settings");

			_childSettings.Add(settings);
			return settings;
		}

		// the loading and saving is locked so only 1 thread can load or save the settings at a time
		private readonly object _lockLoadSave = new object();

		/// <summary>Loads the application settings.</summary>
		public void Load()
		{
			lock (_lockLoadSave)
				if (File.Exists(_settingsFileName))
					using (XmlReader reader = XmlReader.Create(_settingsFileName))
						Load(new XPathDocument(reader).CreateNavigator());
		}

		/// <summary>Saves the application settings.</summary>
		public void Save()
		{
			lock (_lockLoadSave)
				using (XmlWriter writer = XmlWriter.Create(
					_settingsFileName,
					new XmlWriterSettings { Indent = true }))
				{
					writer.WriteStartDocument();
					Save(writer);
				}
		}

		/// <summary>Loads the settings from the specified <see cref="T:XPathNavigator"/>.</summary>
		/// <param name="xml">The <see cref="T:XPathNavigator"/> to load the settings from.</param>
		protected override void LoadCore(XPathNavigator xml)
		{
			foreach (var settings in _childSettings)
				settings.Load(xml);
		}

		/// <summary>Saves the settings to the specified <see cref="T:XmlWriter"/>.</summary>
		/// <param name="writer">The <see cref="T:XmlWriter"/> to save the settings to.</param>
		protected override void SaveCore(XmlWriter writer)
		{
			foreach (var settings in _childSettings)
				settings.Save(writer);
		}
	}
}
