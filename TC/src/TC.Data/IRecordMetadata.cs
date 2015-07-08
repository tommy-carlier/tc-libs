// TC Data Library
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TC.Data
{
	/// <summary>Contains extra information about a <see cref="T:IRecord"/>.</summary>
	public interface IRecordMetadata
	{
		/// <summary>Gets the value of the property with the specified key.</summary>
		/// <param name="key">The key of the property to get the value of.</param>
		/// <value>The value of the specified property, or null if no property exists with the specified key.</value>
		string this[string key] { get; }

		/// <summary>Gets the keys of all the properties.</summary>
		/// <value>A collection with all the keys.</value>
		IEnumerable<string> Keys { get; }

		/// <summary>Gets the properties.</summary>
		/// <value>A collection with all the properties.</value>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1006:DoNotNestGenericTypesInMemberSignatures",
			Justification = "There is no other general way of returning a collection of properties.")]
		IEnumerable<KeyValuePair<string, string>> Properties { get; }

		/// <summary>Gets the number of properties.</summary>
		/// <value>The number of properties.</value>
		int Count { get; }
	}
}