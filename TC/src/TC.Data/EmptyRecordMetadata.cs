// TC Data Library
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TC.Data
{
	using KVP = KeyValuePair<string, string>;

	/// <summary>Represents an empty instance of <see cref="T:IRecordMetadata"/>.</summary>
	public sealed class EmptyRecordMetadata : IRecordMetadata
	{
		private EmptyRecordMetadata() { }

        /// <summary>Gets the single instance of <see cref="T:EmptyRecordMetadata"/>.</summary>
        public static readonly EmptyRecordMetadata Instance = new EmptyRecordMetadata();

		#region IRecordMetadata Members

		/// <summary>Gets the value of the property with the specified key.</summary>
		/// <param name="key">The key of the property to get the value of.</param>
		/// <value>The value of the specified property, or null if no property exists with the specified key.</value>
		public string this[string key]
		{
			get
			{
				if (key == null) throw new ArgumentNullException("key");
				return null;
			}
		}

        /// <summary>Gets the keys of all the properties.</summary>
        /// <value>A collection with all the keys.</value>
        public IEnumerable<string> Keys => CollectionUtilities.CreateEmptyCollection<string>();

        /// <summary>Gets the properties.</summary>
        /// <value>A collection with all the properties.</value>
        public IEnumerable<KVP> Properties => CollectionUtilities.CreateEmptyCollection<KVP>();

        /// <summary>Gets the number of properties.</summary>
        /// <value>The number of properties.</value>
        public int Count => 0;

        #endregion
    }
}
