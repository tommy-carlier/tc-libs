// TC Data Library
// Copyright © 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Data
{
	/// <summary>Describes a <see cref="T:IRecord"/>.</summary>
	public interface IRecordDescriptor
	{
		/// <summary>Gets the number of fields in the record.</summary>
		/// <value>The number of fields in the record.</value>
		int FieldCount { get; }

		/// <summary>Gets the name of the field at the specified index.</summary>
		/// <param name="ordinal">The zero-based index of the field.</param>
		/// <returns>The name of the field at the specified index.</returns>
		string GetFieldName(int ordinal);

		/// <summary>Gets the type of the field at the specified index.</summary>
		/// <param name="ordinal">The zero-based index of the field.</param>
		/// <returns>The type of the field at the specified index.</returns>
		Type GetFieldType(int ordinal);

		/// <summary>Gets the ordinal of the field with the specified name.</summary>
		/// <param name="name">The name of the field.</param>
		/// <returns>The zero-based index of the field in the record with the specified name;
		/// or -1, if no field can be found with the specified name.</returns>
		int GetFieldOrdinal(string name);
	}
}
