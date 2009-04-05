// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tccore
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tccore/license

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace TC.Settings
{
	/// <summary>Represents settings that contain a list of strings.</summary>
	public class StringListSettings : BaseSettings
	{
		private readonly string fListElementName, fListItemElementName;
		private readonly bool fSorted;

		/// <summary>Initializes a new instance of the <see cref="StringListSettings"/> class.</summary>
		/// <param name="listElementName">Name of the list element.</param>
		/// <param name="listItemElementName">Name of the list-item elements.</param>
		/// <param name="sorted">Determines whether the items should be sorted.</param>
		public StringListSettings(string listElementName, string listItemElementName, bool sorted)
		{
			if (listElementName == null) throw new ArgumentNullException("listElementName");
			if (listItemElementName == null) throw new ArgumentNullException("listItemElementName");
			if (listElementName.Length == 0)
				throw new ArgumentException("listElementName cannot be an empty string", "listElementName");
			if (listItemElementName.Length == 0)
				throw new ArgumentException("listItemElementName cannot be an empty string", "listItemElementName");

			fListElementName = listElementName;
			fListItemElementName = listItemElementName;
			fSorted = sorted;
		}

		private readonly List<string> fItems = new List<string>();

		/// <summary>Gets the list of strings.</summary>
		/// <value>The list of strings.</value>
		public IList<string> Items { get { return fItems; } }

		/// <summary>Gets the name of the XML-element that represents the settings.</summary>
		/// <value>The name of the XML-element that represents the settings.</value>
		public override string XmlElementName { get { return fListElementName; } }

		/// <summary>Loads the settings from the specified <see cref="T:XPathNavigator"/>.</summary>
		/// <param name="xml">The <see cref="T:XPathNavigator"/> to load the settings from.</param>
		protected override void LoadCore(XPathNavigator xml)
		{
			fItems.Clear();
			foreach (XPathNavigator lNavigator in xml.Select(fListItemElementName))
				if (!string.IsNullOrEmpty(lNavigator.Value))
					fItems.Add(lNavigator.Value);

			if (fSorted) fItems.Sort();
		}

		/// <summary>Saves the settings to the specified <see cref="T:XmlWriter"/>.</summary>
		/// <param name="writer">The <see cref="T:XmlWriter"/> to save the settings to.</param>
		protected override void SaveCore(XmlWriter writer)
		{
			if (fSorted) fItems.Sort();

			foreach (string lItem in fItems.SkipEmpty())
				writer.WriteElementString(fListItemElementName, lItem);
		}

		/// <summary>Adds a unique item.</summary>
		/// <param name="item">The item to add.</param>
		/// <param name="ignoreCase">if set to <c>true</c>, the case of the string is ignored.</param>
		public void AddUniqueItem(string item, bool ignoreCase)
		{
			if (item == null) throw new ArgumentNullException("item");

			StringComparison lComparison 
				= ignoreCase
					? StringComparison.OrdinalIgnoreCase
					: StringComparison.Ordinal;

			foreach (string lItem in fItems)
				if (item.Equals(lItem, lComparison))
					return;

			fItems.Add(item);
		}
	}
}
