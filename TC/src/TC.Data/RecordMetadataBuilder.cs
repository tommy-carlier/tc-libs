﻿// TC Data Library
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;

namespace TC.Data
{
	using KVP = KeyValuePair<string, string>;

	/// <summary>Provides a means to build a <see cref="T:IRecordMetadata"/> instance.</summary>
	public sealed class RecordMetadataBuilder
	{
		private readonly IDictionary<string, string>
			_properties = new Dictionary<string, string>();

		/// <summary>Removes all the properties.</summary>
		public void Clear()
		{
			_properties.Clear();
		}

		/// <summary>Gets or sets the value of the property with the specified key.</summary>
		/// <param name="key">The key of the property to get or set the value of.</param>
		/// <value>The value of the specified property, or null if no property exists with the specified key.</value>
		public string this[string key]
        {
            get => key == null ? throw new ArgumentNullException("key") : _properties.GetValue(key);
            set
            {
                if (key == null) throw new ArgumentNullException("key");
                if (value != null)
                    _properties[key] = value;
                else _properties.Remove(key);
            }
        }

        /// <summary>Removes the property with the specified key.</summary>
        /// <param name="key">The key of the property to remove.</param>
        /// <returns>If the property was successfully removed, true; otherwise, false.</returns>
        /// <remarks>Also returns false if no property exists with the specified key.</remarks>
        public bool Remove(string key)
			=> key == null 
				? throw new ArgumentNullException("key") 
				: _properties.Remove(key);

        /// <summary>Builds an <see cref="T:IRecordMetadata"/> instance.</summary>
        /// <returns>The created instance.</returns>
        public IRecordMetadata Build() => new Metadata(_properties);

        internal static IRecordMetadata Copy(IRecordMetadata metadata) => new Metadata(metadata);

        #region inner type Metadata

        private sealed class Metadata : IRecordMetadata
		{
			private readonly IDictionary<string, string> _properties;

			internal Metadata(IDictionary<string, string> properties)
			{
				_properties = new Dictionary<string, string>(properties);
			}

			internal Metadata(IRecordMetadata metadata)
			{
				_properties = new Dictionary<string, string>(metadata.Count);
				foreach (var property in metadata.Properties)
					_properties[property.Key] = property.Value;
			}

            #region IRecordMetadata Members

            string IRecordMetadata.this[string key]
				=> key == null 
					? throw new ArgumentNullException("key") 
					: _properties.GetValue(key);

            IEnumerable<string> IRecordMetadata.Keys => _properties.Keys;

            IEnumerable<KVP> IRecordMetadata.Properties => _properties;

            int IRecordMetadata.Count => _properties.Count;

            #endregion
        }

		#endregion
	}
}
