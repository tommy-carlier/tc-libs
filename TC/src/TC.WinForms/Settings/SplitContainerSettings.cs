// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

using TC.Settings;

namespace TC.WinForms.Settings
{
	/// <summary>Contains the settings of a <see cref="T:SplitContainer"/>.</summary>
	public class SplitContainerSettings : BaseSettings
	{
		/// <summary>Initializes a new instance of the <see cref="T:SplitContainerSettings"/> class.</summary>
		/// <param name="xmlElementName">Name of the XML-element.</param>
		public SplitContainerSettings(string xmlElementName)
			: base(xmlElementName) { }

		/// <summary>Loads the settings from the specified <see cref="T:XPathNavigator"/>.</summary>
		/// <param name="xml">The <see cref="T:XPathNavigator"/> to load the settings from.</param>
		protected override void LoadCore(XPathNavigator xml)
		{
			CollapsedPanel = ToFixedPanel(xml.GetAttribute("collapsed-panel", String.Empty));
			int splitterDistance = xml.GetAttribute("splitter-distance", String.Empty).ToInt32();
			if (splitterDistance > 0) SplitterDistance = splitterDistance;
		}

		/// <summary>Saves the settings to the specified <see cref="T:XmlWriter"/>.</summary>
		/// <param name="writer">The <see cref="T:XmlWriter"/> to save the settings to.</param>
		protected override void SaveCore(XmlWriter writer)
		{
			switch (CollapsedPanel)
			{
				case FixedPanel.Panel1: writer.WriteAttributeString("collapsed-panel", "Panel1"); break;
				case FixedPanel.Panel2: writer.WriteAttributeString("collapsed-panel", "Panel2"); break;
			}

			if (SplitterDistance > 0)
				writer.WriteAttributeString("splitter-distance", SplitterDistance.ToDataString());
		}

		private static FixedPanel ToFixedPanel(string value)
		{
			switch (value)
			{
				case "Panel1": return FixedPanel.Panel1;
				case "Panel2": return FixedPanel.Panel2;
				default: return FixedPanel.None;
			}
		}

		/// <summary>Gets or sets the collapsed panel.</summary>
		/// <value>The collapsed panel.</value>
		public FixedPanel CollapsedPanel { get; set; }

		/// <summary>Gets or sets the splitter distance.</summary>
		/// <value>The splitter distance.</value>
		public int SplitterDistance { get; set; }
	}
}
