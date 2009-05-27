// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

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
		private readonly string _listItemElementName;
		private readonly bool _sorted;

		/// <summary>Initializes a new instance of the <see cref="StringListSettings"/> class.</summary>
		/// <param name="listElementName">Name of the list element.</param>
		/// <param name="listItemElementName">Name of the list-item elements.</param>
		/// <param name="sorted">Determines whether the items should be sorted.</param>
		public StringListSettings(string listElementName, string listItemElementName, bool sorted)
			: base(listElementName)
		{
			if (listItemElementName == null) throw new ArgumentNullException("listItemElementName");
			if (listItemElementName.Length == 0)
				throw new ArgumentException("listItemElementName cannot be an empty string", "listItemElementName");

			_listItemElementName = listItemElementName;
			_sorted = sorted;
		}

		private readonly List<string> _items = new List<string>();

		/// <summary>Gets the list of strings.</summary>
		/// <value>The list of strings.</value>
		public IList<string> Items { get { return _items; } }

		/// <summary>Loads the settings from the specified <see cref="T:XPathNavigator"/>.</summary>
		/// <param name="xml">The <see cref="T:XPathNavigator"/> to load the settings from.</param>
		protected override void LoadCore(XPathNavigator xml)
		{
			_items.Clear();
			foreach (XPathNavigator navigator in xml.Select(_listItemElementName))
				if (!string.IsNullOrEmpty(navigator.Value))
					_items.Add(navigator.Value);

			if (_sorted) _items.Sort();
		}

		/// <summary>Saves the settings to the specified <see cref="T:XmlWriter"/>.</summary>
		/// <param name="writer">The <see cref="T:XmlWriter"/> to save the settings to.</param>
		protected override void SaveCore(XmlWriter writer)
		{
			if (_sorted) _items.Sort();

			foreach (string item in _items.SkipEmpty())
				writer.WriteElementString(_listItemElementName, item);
		}

		/// <summary>Adds a unique item.</summary>
		/// <param name="itemToAdd">The item to add.</param>
		/// <param name="ignoreCase">if set to <c>true</c>, the case of the string is ignored.</param>
		public void AddUniqueItem(string itemToAdd, bool ignoreCase)
		{
			if (itemToAdd == null) throw new ArgumentNullException("itemToAdd");

			StringComparison comparison
				= ignoreCase
					? StringComparison.OrdinalIgnoreCase
					: StringComparison.Ordinal;

			foreach (string item in _items)
				if (itemToAdd.Equals(item, comparison))
					return;

			_items.Add(itemToAdd);
		}
	}
}
