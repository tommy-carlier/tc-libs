// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

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
		{
			if (xmlElementName == null) throw new ArgumentNullException("xmlElementName");
			if (xmlElementName.Length == 0) throw new ArgumentException("xmlElementName cannot be an empty string.", "xmlElementName");
			
			fXmlElementName = xmlElementName;
		}

		private readonly string fXmlElementName;

		/// <summary>Gets the name of the XML-element that represents the settings.</summary>
		/// <value>The name of the XML-element that represents the settings.</value>
		public override string XmlElementName { get { return fXmlElementName; } }

		/// <summary>Loads the settings from the specified <see cref="T:XPathNavigator"/>.</summary>
		/// <param name="xml">The <see cref="T:XPathNavigator"/> to load the settings from.</param>
		protected override void LoadCore(XPathNavigator xml)
		{
			CollapsedPanel = ToFixedPanel(xml.GetAttribute("collapsed-panel", String.Empty));
			int lSplitterDistance = ConvertString.ToInt32(xml.GetAttribute("splitter-distance", String.Empty));
			if (lSplitterDistance > 0) SplitterDistance = lSplitterDistance;
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
				writer.WriteAttributeString("splitter-distance", ConvertString.FromInt32(SplitterDistance));
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
